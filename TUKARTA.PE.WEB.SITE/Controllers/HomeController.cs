using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TUKARTA.PE.WEB.SITE.Models;

namespace TUKARTA.PE.WEB.SITE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("politicas")]
        public ActionResult Policies()
        {
            var filePath = "~/files/POLITICAS.pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=POLITICAS.pdf");
            return File(filePath, "application/pdf");
        }

        [HttpGet("clientes/terminos-y-condiciones")]
        public ActionResult CustomerTermsAndConditions()
        {
            var filePath = "~/files/TERMINOS_Y_CONDICIONES_RESTAURANTE.pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=TERMINOS_Y_CONDICIONES_RESTAURANTE.pdf");
            return File(filePath, "application/pdf");
        }

        [HttpGet("restaurantes/terminos-y-condiciones")]
        public ActionResult BusinessTermsAndConditions()
        {
            var filePath = "~/files/TERMINOS_Y_CONDICIONES_USUARIOS.pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=TERMINOS_Y_CONDICIONES_USUARIOS.pdf");
            return File(filePath, "application/pdf");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
