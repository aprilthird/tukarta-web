using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController] 
    [Route("restaurantes")]
    public class RestaurantController : BaseApiController
    {
        public RestaurantController(TuKartaDbContext context,
            ILogger<RestaurantController> logger)
            : base(context, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request, [FromQuery]GeoRequest geolocalization, 
            [FromQuery]Guid? dishCategoryId = null)
        {
            var query = _context.Restaurants.AsQueryable();

            if (dishCategoryId.HasValue)
                query = query.Where(x => x.MenuCategories.Any(m => m.Dishes.Any(d => d.DishCategoryId == dishCategoryId.Value)));

            var userLocation = new Point(0, 0);
            if (geolocalization.Latitude.HasValue && geolocalization.Longitude.HasValue)
            {
                userLocation = new Point(geolocalization.Latitude.Value, geolocalization.Longitude.Value) { SRID = ConstantHelpers.Geometry.DEFAULT_COORD_SYSTEM};
                query.OrderBy(x => x.Location.Distance(userLocation));
            }

            query = query.FilterAndPage(request, x => x.Name);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new RestaurantResource
                    {
                        Id = x.Id,
                        Name = x.Name,
                        RUC = x.RUC,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        LogoUrl = x.LogoUrl,
                        CommisionPrice = x.CommisionPrice,
                        KilometersCoverage = x.KilometersCoverage,
                        CurrencyType = x.CurrencyType,
                        EstimatedDeliveryMinutes = x.EstimatedDeliveryMinutes,
                        Story = x.Story,
                        WebSiteUrl = x.WebSiteUrl.ToString(),
                        IsOrderEnabled = x.IsOrderEnabled,
                        IsBookingEnabled = x.IsBookingEnabled,
                        IsDeliveryEnabled = x.IsDeliveryEnabled,
                        Latitude = x.Location.X,
                        Longitude = x.Location.Y,
                        Distance = geolocalization.Latitude.HasValue && geolocalization.Longitude.HasValue
                            ? x.Location.Distance(userLocation)
                            : (double?)null,
                        User = new UserResource
                        {
                            Name = x.User.Name,
                            Surname = x.User.Surname
                        },
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }).ToListAsync()
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Restaurants
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return NotFound(new ErrorResponse()
                {
                    Message = $"Restaurante con Id '{id}' no encontrado"
                });

            var response = new DataResponse()
            {
                Data = new RestaurantResource
                {
                    Id = data.Id,
                    Name = data.Name,
                    RUC = data.RUC,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    LogoUrl = data.LogoUrl,
                    CommisionPrice = data.CommisionPrice,
                    KilometersCoverage = data.KilometersCoverage,
                    CurrencyType = data.CurrencyType,
                    EstimatedDeliveryMinutes = data.EstimatedDeliveryMinutes,
                    Story = data.Story,
                    WebSiteUrl = data.WebSiteUrl.ToString(),
                    IsOrderEnabled = data.IsOrderEnabled,
                    IsBookingEnabled = data.IsBookingEnabled,
                    IsDeliveryEnabled = data.IsDeliveryEnabled,
                    Latitude = data.Location.X,
                    Longitude = data.Location.Y,
                    User = new UserResource
                    {
                        Name = data.User.Name,
                        Surname = data.User.Surname
                    },
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt
                }
            };

            return Ok(response);
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(Guid id, Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return BadRequest();
            }

            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestaurant", new { id = restaurant.Id }, restaurant);
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(Guid id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        private bool RestaurantExists(Guid id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
