using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    [Route("promociones")]
    [Route("restaurantes/{restaurantId}/promociones")]
    public class PromotionController : BaseApiController
    {
        public PromotionController(TuKartaDbContext context,
            ILogger<PromotionController> logger)
            : base(context, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request, [FromRoute]Guid? restaurantId = null)
        {
            var query = _context.Promotions.AsQueryable();

            if (restaurantId.HasValue)
                query = query.Where(x => x.Dish.MenuCategory.RestaurantId == restaurantId.Value);
            
            query = query.FilterAndPage(request, x => x.Dish.Name);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new PromotionResource
                    {
                        Id = x.Id,
                        NewPrice = x.NewPrice,
                        ExpirationDateTime = x.ExpirationDateTime,
                        AvailableToCarryOut = x.AvailableToCarryOut,
                        AvailableToEatIn = x.AvailableToEatIn,
                        AvailableForOrder = x.AvailableForOrder,
                        AvailableForBooking = x.AvailableForBooking,
                        AvailableForDelivery = x.AvailableForDelivery,
                        DishId = x.DishId,
                        Dish = new DishResource
                        {
                            Id = x.DishId,
                            Name = x.Dish.Name,
                            Price = x.Dish.Price,
                            Description = x.Dish.Description,
                            DishCategoryId = x.Dish.DishCategoryId,
                            DishCategory = new DishCategoryResource
                            {
                                Name = x.Dish.DishCategory.Name
                            },
                            MenuCategory = new MenuCategoryResource
                            {
                                Name = x.Dish.MenuCategory.Name,
                                RestaurantId = x.Dish.MenuCategory.RestaurantId
                            },
                            IsCustomizable = x.Dish.IsCustomizable,
                            PictureUrl = x.Dish.PictureUrl
                        },
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }).ToListAsync()
            };

            return Ok(response);
        }
    }
}