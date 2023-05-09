using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.DATA.Extensions;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    [Route("valoraciones")]
    [Route("restaurantes/{restaurantId}/valoraciones")]
    public class ReviewController : BaseApiController
    {
        public ReviewController(TuKartaDbContext context,
            ILogger<PromotionController> logger)
            : base(context, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request, [FromRoute]Guid? restaurantId = null)
        {
            var query = _context.Reviews.AsQueryable();

            if (restaurantId.HasValue)
                query = query.Where(x => x.Purchase.RestaurantId == restaurantId.Value);

            query = query.Page(request);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new ReviewResource
                    {
                        Id = x.Id,
                        Comment = x.Comment,
                        Score = x.Score,
                        Purchase = new PurchaseResource
                        {
                            User = new UserResource
                            {
                                Name = x.Purchase.User.Name,
                                Surname = x.Purchase.User.Surname
                            }
                        },
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }).ToListAsync()
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ReviewResource resource)
        {
            var review = new Review();
            review.Score = resource.Score;
            review.Comment = resource.Comment;
            review.PurchaseId = resource.PurchaseId;

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
