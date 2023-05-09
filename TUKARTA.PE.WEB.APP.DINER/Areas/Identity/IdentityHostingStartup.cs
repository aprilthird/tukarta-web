using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;

[assembly: HostingStartup(typeof(TUKARTA.PE.WEB.APP.DINER.Areas.Identity.IdentityHostingStartup))]
namespace TUKARTA.PE.WEB.APP.DINER.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}