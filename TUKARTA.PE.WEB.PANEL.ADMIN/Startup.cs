using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TUKARTA.PE.WEB.PANEL.ADMIN.Areas.Identity;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.WEB.PANEL.ADMIN.Helpers;
using TUKARTA.PE.DATA.Seed;
using TUKARTA.PE.WEB.PANEL.ADMIN.Services;
using Azure.Storage.Blobs;
using FluentValidation;
using Radzen;
using Blazored.Toast;
using TUKARTA.PE.SERVICE.Implementations;
using TUKARTA.PE.SERVICE.Services.Interfaces;
using TUKARTA.PE.SERVICE.Services.Implementations;
using TUKARTA.PE.DATA.Factories;

namespace TUKARTA.PE.WEB.PANEL.ADMIN
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TuKartaDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.UseNetTopologySuite()), ServiceLifetime.Transient);
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<TuKartaDbContext>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsPrincipalFactory>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddSingleton<ViewDataService>();
            services.AddSingleton(x => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorageConnectionString")));
            services.AddSingleton<IBlobService, BlobService>();
            services.AddSingleton<IExcelService, ExcelService>();

            services.AddBlazoredToast();

            // ViewModels
            services.AddTransient<ViewModels.HomeViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.DishCategoriesViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.UsersViewModels.DinerViewModel>();
            services.AddTransient<ViewModels.UsersViewModels.BusinessViewModel>();
            services.AddTransient<ViewModels.OrdersViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.BookingsViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.DeliveriesViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.ProfileViewModels.EditViewModel>();
            services.AddTransient<ViewModels.ContactViewModels.IndexViewModel>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TuKartaDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (ConstantHelpers.Seed.ENABLED)
            {
                DbSeeder.Seed(context, userManager, roleManager);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
