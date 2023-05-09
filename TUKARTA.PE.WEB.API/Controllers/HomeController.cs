using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TUKARTA.PE.WEB.API.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("~/")]
        public IActionResult Index()
        {
            var url = new Uri("../api/docs", UriKind.Relative);
            return Redirect(url.ToString());
        }
    }
}