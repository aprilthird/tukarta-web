using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    [Route("cartas")]
    [Route("restaurantes/{restaurantId}/cartas")]
    public class MenuCategoryController : BaseApiController
    {
        public MenuCategoryController(TuKartaDbContext context,
            ILogger<MenuCategoryController> logger)
            : base(context, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request, Guid? restaurantId = null)
        {
            var query = _context.MenuCategories.AsQueryable();

            if (restaurantId.HasValue)
                query = query.Where(x => x.RestaurantId == restaurantId.Value);

            query = query.FilterAndPage(request, x => x.Name);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new MenuCategoryResource
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
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

        // PUT: api/MenuCategory/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuCategory(Guid id, MenuCategory menuCategory)
        {
            if (id != menuCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(menuCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuCategoryExists(id))
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

        // POST: api/MenuCategory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MenuCategory>> PostMenuCategory(MenuCategory menuCategory)
        {
            _context.MenuCategories.Add(menuCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenuCategory", new { id = menuCategory.Id }, menuCategory);
        }

        // DELETE: api/MenuCategory/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MenuCategory>> DeleteMenuCategory(Guid id)
        {
            var menuCategory = await _context.MenuCategories.FindAsync(id);
            if (menuCategory == null)
            {
                return NotFound();
            }

            _context.MenuCategories.Remove(menuCategory);
            await _context.SaveChangesAsync();

            return menuCategory;
        }

        private bool MenuCategoryExists(Guid id)
        {
            return _context.MenuCategories.Any(e => e.Id == id);
        }
    }
}
