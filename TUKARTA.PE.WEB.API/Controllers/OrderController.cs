using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Helpers;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;
using TUKARTA.PE.SERVICE.Implementations;
using TUKARTA.PE.DATA.Extensions;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ordenes")]
    public class OrderController : BaseApiController
    {
        private readonly VisaNetService _visaNETService;

        public OrderController(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            VisaNetService visaNETService,
            ILogger<OrderController> logger)
            : base(context, userManager, logger)
        {
            _visaNETService = visaNETService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request, int? consumptionModality = null)
        {
            var user = await GetUserAsync();
            
            var query = _context.Purchases
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Type == ConstantHelpers.Service.Type.ORDER)
                .AsQueryable();

            if (consumptionModality.HasValue)
                query = query.Where(x => x.ConsumptionModality == consumptionModality.Value);

            query = query.Where(x => x.UserId == user.Id);

            query = query.Page(request);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new OrderResource
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
                        PeopleAmount = x.PeopleAmount.Value,
                        IssueType = x.IssueType.Value,
                        TicketName = x.TicketName,
                        RUC = x.RUC,
                        BusinessName = x.BusinessName,
                        ConsumptionModality = x.ConsumptionModality.Value,
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
        public async Task<IActionResult> Create([FromBody]OrderResource resource)
        {
            if(resource.Details == null || !resource.Details.Any())
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = $"Ningún detalle proporcionado"
                });
            }

            var user = await GetUserAsync();

            var order = new Purchase();
            order.Type = ConstantHelpers.Service.Type.ORDER;
            order.Status = ConstantHelpers.Service.Status.PAYMENT_IN_PROCESS;
            order.PaymentMethod = ConstantHelpers.Service.PaymentMethod.CARD;
            order.RestaurantId = resource.RestaurantId;
            order.UserId = user.Id;
            order.Annotations = resource.Annotations;
            order.Code = await _context.Purchases
                .CountAsync(x => x.RestaurantId == resource.RestaurantId 
                    && x.Type == ConstantHelpers.Service.Type.ORDER) + 1;
            order.PeopleAmount = resource.PeopleAmount;
            order.IssueType = resource.IssueType;
            order.ConsumptionModality = resource.ConsumptionModality;
            if (order.IssueType == ConstantHelpers.Service.IssueType.BILL)
            {
                order.RUC = resource.RUC;
                order.BusinessName = resource.BusinessName;
            }
            else
            {
                order.TicketName = resource.TicketName;
            }

            var details = new List<PurchaseDetail>();
            foreach(var item in resource.Details)
            {
                var purchaseDetail = new PurchaseDetail
                {
                    Purchase = order,
                    DishId = item.DishId,
                    PromotionId = item.PromotionId,
                    Quantity = item.Quantity
                };
                var dish = await _context.Dishes.FindAsync(item.DishId);
                purchaseDetail.Description = dish.Name;
                if(item.PromotionId.HasValue)
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
            await _context.Purchases.AddAsync(order);
            await _context.SaveChangesAsync();

            var response = new DataResponse
            {
                Data = new PurchaseGenerationResource
                {
                    Code = order.Code,
                    OperationNumber = order.OperationNumber,
                    PurchaseId = order.Id
                }
            }; 
            
            var securityToken = await _visaNETService.GetSecurityToken();
            if (string.IsNullOrEmpty(securityToken))
            {
                _context.PurchaseDetails.RemoveRange(details);
                _context.Purchases.Remove(order);
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

            return Ok(response);
        }

        [HttpPut("{orderId}/procesar-pago")]
        public async Task<IActionResult> PaymentProcessed([FromRoute] Guid orderId, [FromBody] PurchasePaymentResource resource)
        {
            var purchase = await _context.Purchases.FindAsync(orderId);
            if (purchase.PaymentMethod != ConstantHelpers.Service.PaymentMethod.CARD)
                return BadRequest(new ErrorResponse
                {
                    Message = $"La orden de Id '{orderId}' no presenta un método de pago por tarjeta."
                });
            if(purchase.Status != ConstantHelpers.Service.Status.PAYMENT_IN_PROCESS)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"La orden de Id '{orderId}' ya ha procesado el pago."
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

        [HttpPut("{orderId}/cancelar")]
        public async Task<IActionResult> Cancel([FromRoute]Guid orderId)
        {
            var purchase = await _context.Purchases.FindAsync(orderId);
            if (purchase.Status != ConstantHelpers.Service.Status.PENDING_APPROVAL)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"La orden de Id '{orderId}' ya no puede ser cancelada porque presenta un estado distinto a pendiente."
                });
            }
            purchase.Status = ConstantHelpers.Service.Status.CANCELLED;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}