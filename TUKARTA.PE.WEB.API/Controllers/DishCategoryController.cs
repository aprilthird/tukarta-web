using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Extensions;
using TUKARTA.PE.WEB.API.Requests;
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    [Route("categorias-de-platos")]
    public class DishCategoryController : BaseApiController
    {
        public DishCategoryController(TuKartaDbContext context)
            : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]DataSetRequest request)
        {
            var query = _context.DishCategories
                .FilterAndPage(request, x => x.Name)
                .AsQueryable();

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new DishCategoryResource
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        PictureUrl = x.PictureUrl,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }).ToListAsync()
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.DishCategories.FindAsync(id);

            if (data == null)
                return NotFound(new ErrorResponse()
                {
                    Message = $"Categoría de con '{id}' no encontrado"
                });

            var response = new DataResponse()
            {
                Data = new DishCategoryResource
                {
                    Name = data.Name,
                    Description = data.Description,
                    PictureUrl = data.PictureUrl,
                    CreatedAt = data.CreatedAt,
                    UpdatedAt = data.UpdatedAt
                }
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDishCategory(Guid id, DishCategory dishCategory)
        {
            if (id != dishCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(dishCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishCategoryExists(id))
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

        [HttpPost]
        public async Task<ActionResult<DishCategory>> PostDishCategory(DishCategory dishCategory)
        {
            _context.DishCategories.Add(dishCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDishCategory", new { id = dishCategory.Id }, dishCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DishCategory>> DeleteDishCategory(Guid id)
        {
            var dishCategory = await _context.DishCategories.FindAsync(id);
            if (dishCategory == null)
            {
                return NotFound();
            }

            _context.DishCategories.Remove(dishCategory);
            await _context.SaveChangesAsync();

            return dishCategory;
        }

        private bool DishCategoryExists(Guid id)
        {
            return _context.DishCategories.Any(e => e.Id == id);
        }
    }
}
