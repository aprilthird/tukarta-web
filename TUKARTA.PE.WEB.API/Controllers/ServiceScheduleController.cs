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
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    [Route("restaurantes/{restaurantId}/horarios")]
    public class ServiceScheduleController : BaseApiController
    {
        public ServiceScheduleController(TuKartaDbContext context,
            ILogger<ServiceScheduleController> logger)
            : base(context, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute]Guid? restaurantId = null)
        {
            var query = _context.ServiceSchedules.AsQueryable();

            if (restaurantId.HasValue)
                query = query.Where(x => x.RestaurantId == restaurantId.Value);

            var response = new DataSetResponse()
            {
                Data = await query
                    .Select(x => new ServiceScheduleResource
                    {

                    }).ToListAsync()
            };

            return Ok(response);
        }
    }
}