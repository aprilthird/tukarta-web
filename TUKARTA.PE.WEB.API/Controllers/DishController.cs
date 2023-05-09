using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    [Route("platos")]
    [Route("restaurantes/{restaurantId}/platos")]
    [Route("restaurantes/{restaurantId}/cartas/{menuCategoryId}/platos")]
    public class DishController : BaseApiController
    {
        public DishController(TuKartaDbContext context,
            ILogger<DishController> logger)
            : base(context, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request, [FromRoute]Guid? restaurantId = null, [FromRoute]Guid? menuCategoryId = null)
        {
            var query = _context.Dishes.AsQueryable();

            if (restaurantId.HasValue)
                query = query.Where(x => x.MenuCategory.RestaurantId == restaurantId.Value);
            if (menuCategoryId.HasValue)
                query = query.Where(x => x.MenuCategoryId == menuCategoryId.Value);

            query = query.FilterAndPage(request, x => x.Name);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new DishResource
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        IsCustomizable = x.IsCustomizable,
                        PictureUrl = x.PictureUrl,
                        MenuCategoryId = x.MenuCategoryId,
                        MenuCategory = new MenuCategoryResource
                        {
                            Name = x.MenuCategory.Name
                        },
                        DishCategoryId = x.DishCategoryId,
                        DishCategory = new DishCategoryResource
                        {
                            Name = x.DishCategory.Name
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
            var data = await _context.MenuCategories.FindAsync(id);

            if (data == null)
                return NotFound(new ErrorResponse()
                {
                    Message = $"Carta con Id '{id}' no encontrado"
                });

            var response = new DataResponse()
            {
                Data = new MenuCategoryResource
                {
                    Name = data.Name,
                    Description = data.Description,
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt
                }
            };

            return Ok(response);
        }

        // PUT: api/Dish/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(Guid id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // POST: api/Dish
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Dish>> PostDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dish/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dish>> DeleteDish(Guid id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return dish;
        }

        private bool DishExists(Guid id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
