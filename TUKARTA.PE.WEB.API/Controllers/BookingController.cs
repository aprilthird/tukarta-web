using System;
using System.Collections.Generic;
using System.Linq;
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
using TUKARTA.PE.DATA.Extensions;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("reservas")]
    public class BookingController : BaseApiController
    {
        public BookingController(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<BookingController> logger)
            : base(context, userManager, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request)
        {
            var user = await GetUserAsync();

            var query = _context.Purchases
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Type == ConstantHelpers.Service.Type.BOOKING)
                .AsQueryable();

            query = query.Where(x => x.UserId == user.Id);

            query = query.Page(request);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new BookingResource
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
                        DateTime = x.BookingDateTime.Value,
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
        public async Task<IActionResult> Create([FromBody]BookingResource resource)
        {
            if (resource.Details == null || !resource.Details.Any())
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = $"Ningún detalle proporcionado"
                });
            }

            var user = await GetUserAsync();

            var booking = new Purchase();
            booking.Type = ConstantHelpers.Service.Type.BOOKING;
            booking.Status = ConstantHelpers.Service.Status.PENDING_APPROVAL;
            booking.RestaurantId = resource.RestaurantId;
            booking.UserId = user.Id;
            booking.Annotations = resource.Annotations;
            booking.Code = await _context.Purchases
                .CountAsync(x => x.RestaurantId == resource.RestaurantId
                    && x.Type == ConstantHelpers.Service.Type.BOOKING) + 1;
            booking.PeopleAmount = resource.PeopleAmount;
            booking.BookingDateTime = resource.DateTime;

            var details = new List<PurchaseDetail>();
            foreach (var item in resource.Details)
            {
                var purchaseDetail = new PurchaseDetail
                {
                    Purchase = booking,
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
            await _context.Purchases.AddAsync(booking);
            await _context.SaveChangesAsync();

            var response = new DataResponse
            {
                Data = new PurchaseGenerationResource
                {
                    Code = booking.Code,
                    OperationNumber = booking.OperationNumber,
                    PurchaseId = booking.Id
                }
            };

            return Ok(response);
        }

        [HttpPut("{bookingId}/cancelar")]
        public async Task<IActionResult> Cancel([FromRoute]Guid bookingId)
        {
            var purchase = await _context.Purchases.FindAsync(bookingId);
            if (purchase.Status != ConstantHelpers.Service.Status.PENDING_APPROVAL)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"La reserva de Id '{bookingId}' ya no puede ser cancelada porque presenta un estado distinto a pendiente."
                });
            }
            purchase.Status = ConstantHelpers.Service.Status.CANCELLED;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}