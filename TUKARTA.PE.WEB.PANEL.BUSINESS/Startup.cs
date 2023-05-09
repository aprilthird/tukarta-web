using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Areas.Identity;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using Azure.Storage.Blobs;
using Blazored.Toast;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;
using TUKARTA.PE.DATA.Seed;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Services;
using Blazored.SessionStorage;
using TUKARTA.PE.SERVICE.Implementations;
using Microsoft.AspNetCore.Identity.UI.Services;
using TUKARTA.PE.SERVICE.Services.Implementations;
using TUKARTA.PE.SERVICE.Services.Interfaces;
using TUKARTA.PE.DATA.Factories;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS
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
            //services.AddScoped<IEmailSender, EmailSenderService>();
            services.AddSingleton(x => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorageConnectionString")));
            services.AddSingleton<IBlobService, BlobService>();
            services.AddTransient<BusinessService>();

            services.AddBlazoredToast();
            services.AddBlazoredSessionStorage();

            services.AddTransient<ViewModels.HomeViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.DeliveriesViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.BookingsViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.OrdersViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.MenuViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.RestaurantViewModels.NewViewModel>();
            services.AddTransient<ViewModels.RestaurantViewModels.EditViewModel>();
            services.AddTransient<ViewModels.ProfileViewModels.EditViewModel>();
            services.AddTransient<ViewModels.PromotionsViewModels.IndexViewModel>();
            services.AddTransient<ViewModels.UserViewModels.RegisterViewModel>();
            services.AddTransient<ViewModels.UserViewModels.ExternalRegisterViewModel>();

            //google
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var googleAuthNsection = Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNsection["ClientId"];
                    options.ClientSecret = googleAuthNsection["ClientSecret"];
                });
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
