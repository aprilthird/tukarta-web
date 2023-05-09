using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.WEB.API.Extensions;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly TuKartaDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly ILogger<BaseApiController> _logger;

        public BaseApiController(TuKartaDbContext context)
        {
            _context = context;
        }

        public BaseApiController(TuKartaDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public BaseApiController(TuKartaDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public BaseApiController(TuKartaDbContext context,
            ILogger<BaseApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public BaseApiController(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<BaseApiController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public BaseApiController(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<BaseApiController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        protected virtual Task<ApplicationUser> GetUserAsync() => _userManager.GetUserAsync(User);
    }
}