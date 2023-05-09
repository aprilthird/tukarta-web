using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Helpers;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;
using TUKARTA.PE.SERVICE.Implementations;
using TUKARTA.PE.DATA.Extensions;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("entregas")]
    public class DeliveryController : BaseApiController
    {
        private readonly VisaNetService _visaNETService;

        public DeliveryController(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            VisaNetService visaNETService,
            ILogger<DeliveryController> logger)
            : base(context, userManager, logger)
        {
            _visaNETService = visaNETService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request)
        {
            var user = await GetUserAsync();

            var query = _context.Purchases
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Type == ConstantHelpers.Service.Type.DELIVERY)
                .AsQueryable();

            query = query.Where(x => x.UserId == user.Id);

            query = query.Page(request);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new DeliveryResource
                    {
                        Id = x.Id,
                        Code = x.Code,
                        OperationNumber = x.OperationNumber,
                        Status = x.Status,
                        Annotations = x.Annotations,
                        RejectionReason = x.RejectionReason,
                        RestaurantId = x.RestaurantId,
                        Restaurant = new RestaurantResource
                        {
                            Name = x.Restaurant.Name
                        },
                        IssueType = x.IssueType.Value,
                        TicketName = x.TicketName,
                        RUC = x.RUC,
                        BusinessName = x.BusinessName,
                        PaymentMethod = x.PaymentMethod.Value,
                        PaymentAmount = x.PaymentAmount,
                        Address = x.DeliveryAddress,
                        Reference = x.DeliveryReference,
                        Latitude = x.DeliveryLocation.X,
                        Longitude = x.DeliveryLocation.Y,
                        DateTime = x.DeliveryDateTime,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Details = x.PurchaseDetails
                            .Select(p => new PurchaseDetailResource
                            {
                                Id = p.Id,
                                DishId = p.DishId,
                                PromotionId = p.PromotionId,
                                Description = p.Description,
                                OriginalPrice = p.OriginalPrice,
                                UnitPrice = p.UnitPrice,
                                Quantity = p.Quantity
                            }).ToList()
                    }).ToListAsync()
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]DeliveryResource resource)
        {
            if (resource.Details == null || !resource.Details.Any())
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = $"Ningún detalle proporcionado"
                });
            }

            var user = await GetUserAsync();

            var delivery = new Purchase();
            delivery.Type = ConstantHelpers.Service.Type.DELIVERY;
            delivery.Status = resource.PaymentMethod == ConstantHelpers.Service.PaymentMethod.EFFECTIVE
                ? ConstantHelpers.Service.Status.PENDING_APPROVAL
                : ConstantHelpers.Service.Status.PAYMENT_IN_PROCESS;
            delivery.RestaurantId = resource.RestaurantId;
            delivery.UserId = user.Id;
            delivery.Annotations = resource.Annotations;
            delivery.Code = await _context.Purchases
                .CountAsync(x => x.RestaurantId == resource.RestaurantId
                    && x.Type == ConstantHelpers.Service.Type.DELIVERY) + 1;
            delivery.IssueType = resource.IssueType;
            delivery.PaymentMethod = resource.PaymentMethod;
            if (delivery.IssueType == ConstantHelpers.Service.IssueType.BILL)
            {
                delivery.RUC = resource.RUC;
                delivery.BusinessName = resource.BusinessName;
            }
            else
            {
                delivery.TicketName = resource.TicketName;
            }
            if (delivery.PaymentMethod == ConstantHelpers.Service.PaymentMethod.EFFECTIVE)
            {
                delivery.PaymentAmount = resource.PaymentAmount;
            }
            delivery.DeliveryAddress = resource.Address;
            delivery.DeliveryReference = resource.Reference;
            delivery.DeliveryLocation = new Point(resource.Latitude, resource.Longitude)
                { SRID = ConstantHelpers.Geometry.DEFAULT_COORD_SYSTEM };

            var details = new List<PurchaseDetail>();
            foreach (var item in resource.Details)
            {
                var purchaseDetail = new PurchaseDetail
                {
                    Purchase = delivery,
                    DishId = item.DishId,
                    PromotionId = item.PromotionId,
                    Quantity = item.Quantity
                };
                var dish = await _context.Dishes.FindAsync(item.DishId);
                purchaseDetail.Description = dish.Name;
                if (item.PromotionId.HasValue)
                {
                    var promotion = await _context.Promotions.FindAsync(item.PromotionId.Value);
                    purchaseDetail.UnitPrice = promotion.NewPrice;
                    purchaseDetail.OriginalPrice = dish.Price;
                }
                else
                {
                    purchaseDetail.UnitPrice = dish.Price;
                }
                details.Add(purchaseDetail);
            }
            await _context.PurchaseDetails.AddRangeAsync(details);
            await _context.Purchases.AddAsync(delivery);
            await _context.SaveChangesAsync();

            var response = new DataResponse
            {
                Data = new PurchaseGenerationResource
                {
                    Code = delivery.Code,
                    OperationNumber = delivery.OperationNumber,
                    PurchaseId = delivery.Id
                }
            };

            if (resource.PaymentMethod == ConstantHelpers.Service.PaymentMethod.CARD)
            {
                var securityToken = await _visaNETService.GetSecurityToken();
                if (string.IsNullOrEmpty(securityToken))
                {
                    _context.PurchaseDetails.RemoveRange(details);
                    _context.Purchases.Remove(delivery);
                    await _context.SaveChangesAsync();
                    return BadRequest(new ErrorResponse
                    {
                        Message = "Ocurrió un error al comunicarse con la pasarela de pagos."
                    });
                }
                else
                {
                    ((PurchaseGenerationResource)response.Data).SecurityToken = securityToken;
                }
            }

            return Ok(response);
        }

        [HttpPut("{deliveryId}/procesar-pago")]
        public async Task<IActionResult> PaymentProcessed([FromRoute] Guid deliveryId, [FromBody] PurchasePaymentResource resource)
        {
            var purchase = await _context.Purchases.FindAsync(deliveryId);
            if (purchase.PaymentMethod != ConstantHelpers.Service.PaymentMethod.CARD)
                return BadRequest(new ErrorResponse
                {
                    Message = $"El delivery de Id '{deliveryId}' no presenta un método de pago por tarjeta."
                });
            if (purchase.Status != ConstantHelpers.Service.Status.PAYMENT_IN_PROCESS)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"El delivery de Id '{deliveryId}' ya ha procesado el pago."
                });
            }
            purchase.Status = ConstantHelpers.Service.Status.PENDING_APPROVAL;
            purchase.PaymentTransactionId = resource.TransactionId;
            purchase.PaymentTraceNumber = resource.TransactionTraceNumber;
            purchase.PaymentCurrency = resource.Currency;
            purchase.PaymentMaskedCardNumber = resource.MaskedCardNumber;
            purchase.PaymentCardBrand = resource.CardBrand;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{deliveryId}/cancelar")]
        public async Task<IActionResult> Cancel([FromRoute]Guid deliveryId)
        {
            var purchase = await _context.Purchases.FindAsync(deliveryId);
            if (purchase.Status != ConstantHelpers.Service.Status.PENDING_APPROVAL)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"El delivery de Id '{deliveryId}' ya no puede ser cancelado porque presenta un estado distinto a pendiente."
                });
            }
            purchase.Status = ConstantHelpers.Service.Status.CANCELLED;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}