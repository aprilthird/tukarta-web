using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;

namespace TUKARTA.PE.DATA.Seed
{
    public class DbSeeder
    {
        public static void Seed(TuKartaDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (roleManager.FindByNameAsync(ConstantHelpers.Roles.SUPERADMIN).Result == null)
            {
                roleManager.CreateAsync(new ApplicationRole(ConstantHelpers.Roles.SUPERADMIN)).Wait();
            }

            if (roleManager.FindByNameAsync(ConstantHelpers.Roles.BUSINESS).Result == null)
            {
                roleManager.CreateAsync(new ApplicationRole(ConstantHelpers.Roles.BUSINESS)).Wait();
            }

            if (roleManager.FindByNameAsync(ConstantHelpers.Roles.DINER).Result == null)
            {
                roleManager.CreateAsync(new ApplicationRole(ConstantHelpers.Roles.DINER)).Wait();
            }

            if (userManager.FindByEmailAsync("superadmin@tukarta.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "superadmin@tukarta.com",
                    Email = "superadmin@tukarta.com",
                    Name = "Superadmin",
                    Surname = "Superadmin",
                    BirthDate = DateTime.Parse("1996-03-04"),
                    IsEnabled = true,
                    PhoneNumber = "999999999"
                };

                var result = userManager.CreateAsync(user, "TuKarta.2020").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ConstantHelpers.Roles.SUPERADMIN).Wait();
                }
            }

            if (userManager.FindByEmailAsync("negocio@tukarta.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "negocio@tukarta.com",
                    Email = "negocio@tukarta.com",
                    Name = "Negocio",
                    Surname = "Negocio",
                    BirthDate = DateTime.Parse("1995-10-10"),
                    IsEnabled = true,
                    PhoneNumber = "999777888"
                };

                var result = userManager.CreateAsync(user, "TuKarta.2020").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ConstantHelpers.Roles.BUSINESS).Wait();
                }
            }

            if (userManager.FindByEmailAsync("comensal@tukarta.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "comensal@tukarta.com",
                    Email = "comensal@tukarta.com",
                    Name = "Comensal",
                    Surname = "Comensal",
                    BirthDate = DateTime.Parse("1996-05-25"),
                    IsEnabled = true,
                    PhoneNumber = "988969959"
                };

                var result = userManager.CreateAsync(user, "TuKarta.2020").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ConstantHelpers.Roles.DINER).Wait();
                }
            }

            if (!context.DishCategories.Any())
            {
                context.DishCategories.AddRange(new List<DishCategory>()
                {
                    new DishCategory { Name = "Mariscos", Description = "Comida marítima, pescados y mariscos.", PictureUrl = new Uri("https://okdiario.com/img/2014/07/05/el-verano-es-buena-temporada-para-comer-marisco-655x368.jpg") },
                    new DishCategory { Name = "Criolla", Description = "Comida peruana.", PictureUrl = new Uri("https://blog.mesa247.pe/wp-content/uploads/2017/12/25286143_10155775153371438_418126503_o-1.jpg") },
                    new DishCategory { Name = "Amazónica", Description = "Comida de la selva peruana.", PictureUrl = new Uri("https://comidastipicas.co/wp-content/uploads/2017/09/Patarasca-1.jpg") },
                    new DishCategory { Name = "Postres", Description = "Deliciosos postres.", PictureUrl = new Uri("https://sifu.unileversolutions.com/image/es-ES/recipe-topvisual/2/1260-709/variedad-de-mini-postres-para-compartir-50267133.jpg") },
                    new DishCategory { Name = "Brasas y Parrillas", Description = "Comidas hechas a la brasa o parrilla.", PictureUrl = new Uri("https://parrilladasargentinas.com/wp-content/uploads/2018/09/Fakemeat-RFP-021218-1240x826-1030x686.jpg") },
                    new DishCategory { Name = "Bebidas", Description = "Refrescantes bebidas.", PictureUrl = new Uri("https://www.muyinteresante.com.mx/wp-content/uploads/2018/05/httpstved-prod.adobecqms.netcontentdamtbgmexicomuyinteresantemxsalud-y-bienestarmente-y-cerebro171124bebidas-alcoholicas.imgo_.jpg") }
                });
                context.SaveChanges();
            }

            if (!context.Restaurants.Any())
            {
                var businessUser = context.Users.FirstOrDefault(x => x.UserName == "negocio@tukarta.com");
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                context.Restaurants.AddRange(new List<Restaurant>()
                {
                    new Restaurant { Name = "Restaurante 1", RUC = "R1", PhoneNumber = "999888777", Email = "restaurante1@tukarta.com", KilometersCoverage = 10, CommisionPrice = 5, CurrencyType = 1, EstimatedDeliveryMinutes = 30, Location = geometryFactory.CreatePoint(new Coordinate(-12.0610534, -77.0331532)), WebSiteUrl = new Uri("https://www.google.com"), Story = "Un hermoso restaurante variado.", UserId = businessUser.Id, LogoUrl = new Uri("https://cdn6.f-cdn.com/contestentries/870686/1193270/5808c97a8c690_thumb900.jpg") },
                    new Restaurant { Name = "Restaurante 2", RUC = "R2", PhoneNumber = "999888666", Email = "restaurante2@tukarta.com", KilometersCoverage = 15, CommisionPrice = 8, CurrencyType = 1, EstimatedDeliveryMinutes = 20, Location = geometryFactory.CreatePoint(new Coordinate(-12.1023418, -77.0370475)), WebSiteUrl = new Uri("https://www.facebook.com"), Story = "Un hogareño restaurante limeño.", UserId = businessUser.Id, LogoUrl = new Uri("https://img.freepik.com/vector-gratis/logo-restaurante-retro-tenedor_23-2148456243.jpg") },
                    new Restaurant { Name = "Restaurante 3", RUC = "R3", PhoneNumber = "999888555", Email = "restaurante3@tukarta.com", KilometersCoverage = 20, CommisionPrice = 4, CurrencyType = 1, EstimatedDeliveryMinutes = 25, Location = geometryFactory.CreatePoint(new Coordinate(-12.0673831, -77.060701)), WebSiteUrl = new Uri("https://www.twitter.com"), Story = "Un restaurante moderno.", UserId = businessUser.Id, LogoUrl = new Uri("https://cdn.pixabay.com/photo/2017/09/23/21/21/label-2780146_960_720.png") },
                }); 
                context.SaveChanges();
            }

            if(!context.MenuCategories.Any())
            {
                var restaurants = context.Restaurants.ToList();
                context.MenuCategories.AddRange(new List<MenuCategory>()
                {
                    new MenuCategory { Name = "Platos de la Casa", Description = "Platos de la Casa", RestaurantId = restaurants[0].Id },
                    new MenuCategory { Name = "Entradas", Description = "Entradas", RestaurantId = restaurants[0].Id },
                    new MenuCategory { Name = "Postres", Description = "Postres", RestaurantId = restaurants[0].Id },

                    new MenuCategory { Name = "Erspecialidades", Description = "Especialidades", RestaurantId = restaurants[1].Id },
                    new MenuCategory { Name = "Parrillas", Description = "Parrillas", RestaurantId = restaurants[1].Id },
                    new MenuCategory { Name = "Pollos a la Brasa", Description = "Pollos a la Brasa", RestaurantId = restaurants[1].Id },
                    
                    new MenuCategory { Name = "Platos a la Carta", Description = "Platos a la Carta", RestaurantId = restaurants[2].Id },
                    new MenuCategory { Name = "Menú Diario", Description = "Menú Diario", RestaurantId = restaurants[2].Id },
                    new MenuCategory { Name = "Bebidas", Description = "Bebidas", RestaurantId = restaurants[2].Id },
                });
                context.SaveChanges();
            }

            if(!context.Dishes.Any())
            {
                var menus = context.MenuCategories.ToList();
                var categories = context.DishCategories.ToList();
                context.Dishes.AddRange(new List<Dish>()
                {
                    new Dish { Name = "Jalea Mixta", Description = "Jalea que viene con Ceviche", Price = 45, DishCategoryId = categories[0].Id, MenuCategoryId = menus[0].Id, PictureUrl = new Uri("https://elchimbotano.cl/wp-content/uploads/2018/08/Ceviche-Categoria-1170x679.jpg") },
                    new Dish { Name = "Chupe de Camarones", Description = "Delicioso chupe de camarones.", Price = 48.50, DishCategoryId = categories[0].Id, MenuCategoryId = menus[0].Id, PictureUrl = new Uri("https://ojo.pe/resizer/uWSAwI-zLl8N41ILSRlObN-0tL0=/980x528/smart/arc-anglerfish-arc2-prod-elcomercio.s3.amazonaws.com/public/UOBSTTN47FEHHOHCVMSGH5OO34.jpg") },
                    new Dish { Name = "Papa a la Huancaina", Description = "Papas acompañadas de crema la huancaina.", Price = 12, DishCategoryId = categories[1].Id, MenuCategoryId = menus[1].Id, PictureUrl = new Uri("https://i2.wp.com/decomidaperuana.com/wp-content/uploads/2017/08/papa-a-la-huancaina-receta-preparacion.jpg") },
                    new Dish { Name = "Postre de Chocolate", Description = "Delicioso postre bañado de chocolate.", Price = 10, DishCategoryId = categories[3].Id, MenuCategoryId = menus[2].Id, PictureUrl = new Uri("https://www.johaprato.com/files/styles/flexslider_full/public/budino_cioccolato_cocco2.png") },

                    new Dish { Name = "Costillas con Salsa BBQ", Description = "Costillas de res acompañados de salsa barbacoa.", Price = 60, DishCategoryId = categories[4].Id, MenuCategoryId = menus[3].Id, PictureUrl = new Uri("https://cdn.kiwilimon.com/recetaimagen/26648/th5-640x426-23963.jpg") },
                    new Dish { Name = "Bistec a la Parrilla", Description = "Bistec con papas y verduras a la parrilla.", Price = 35, DishCategoryId = categories[4].Id, MenuCategoryId = menus[4].Id, PictureUrl = new Uri("https://assets.kraftfoods.com/recipe_images/opendeploy/94128_640x428.jpg") },
                    new Dish { Name = "Pollo a la Brasa + Papas", Description = "Pollo a la brasa completo más una porción de papas.", Price = 52.50, DishCategoryId = categories[4].Id, MenuCategoryId = menus[5].Id, PictureUrl = new Uri("https://caretas.pe/wp-content/uploads/2019/07/cropped-cuarto-de-pollo-con-papas-1.jpg") },

                    new Dish { Name = "Arroz con Pollo con Crema a la Huancaina", Description = "Arroz con Pollo acompañado de crema a la Huancaina.", Price = 25, DishCategoryId = categories[1].Id, MenuCategoryId = menus[6].Id, PictureUrl = new Uri("https://i.pinimg.com/originals/b0/b4/91/b0b4917f6f40fa5b8a3d7b6e8f9de9e6.jpg") },
                    new Dish { Name = "Estofado de Pollo", Description = "Estofado con pollo.", Price = 8, DishCategoryId = categories[1].Id, MenuCategoryId = menus[7].Id, PictureUrl = new Uri("https://comidaperuana.life/wp-content/uploads/2018/10/estofado-de-pollo_700x465.jpg") },
                    new Dish { Name = "Pisco Sour", Description = "Refrescante bebida símbolo de la gastronomía peruana.", Price = 12.50, DishCategoryId = categories[5].Id, MenuCategoryId = menus[8].Id, PictureUrl = new Uri("https://cde.peru.com//ima/0/1/5/7/2/1572016/380x300/pisco-sour.jpg") },
                });
                context.SaveChanges();
            }

            if(!context.Promotions.Any())
            {
                var dishes = context.Dishes.ToList();
                context.Promotions.AddRange(new List<Promotion>
                {
                    new Promotion { NewPrice = 35.0, ExpirationDateTime = DateTime.Parse("2020-07-31"), DishId = dishes[0].Id },
                    new Promotion { NewPrice = 45.0, ExpirationDateTime = DateTime.Parse("2020-07-30"), DishId = dishes[4].Id },
                    new Promotion { NewPrice = 40.0, ExpirationDateTime = DateTime.Parse("2020-07-30"), DishId = dishes[6].Id },
                    new Promotion { NewPrice = 9.50, ExpirationDateTime = DateTime.Parse("2020-07-28"), DishId = dishes[9].Id },
                });
                context.SaveChanges();
            }
            if(!context.Purchases.Any())
            {
                var comensalUser = context.Users.FirstOrDefault(x => x.UserName == "comensal@tukarta.com");
                var restaurants = context.Restaurants.ToList();
               
                context.Purchases.AddRange(new List<Purchase>
                {
                    new Purchase{ Code = 1,UserId= comensalUser.Id,RestaurantId = restaurants[0].Id,CreatedAt=DateTime.Parse("2020-07-31"),PaymentAmount= 78,Type=ConstantHelpers.Service.Type.ORDER},
                    
                });
                context.SaveChanges();
            }
            if (!context.PurchaseDetails.Any())
            {
                var dishEstofado = context.Dishes.FirstOrDefault(x => x.Name == "Estofado de Pollo"); //sin promocion
                var dishBistec = context.Dishes.FirstOrDefault(x => x.Name == "Bistec a la parrilla"); //sin promocion
                var dishesPromotionJalea = context.Dishes.FirstOrDefault(X => X.Name == "Jalea Mixta");//con promocion {NewPrice = 35, OriginalPrice = 45]
                var promotionJalea = context.Promotions.FirstOrDefault(X => X.Dish.Name == "Jalea Mixta"); //consulta para llamar a la promocion que contiene la Jalea (uy la jalea madre mia)
                var purchases = context.Purchases.ToList();

                context.PurchaseDetails.AddRange(new List<PurchaseDetail>
                { 

                    new PurchaseDetail{ CreatedAt=DateTime.Parse("2020-07-31"),UpdatedAt=DateTime.Parse("2020-07-31"),PurchaseId = purchases[0].Id,DishId = dishEstofado.Id,OriginalPrice = 8,Description = "Estofado con pollo.",UnitPrice= 8, Quantity = 1},
                    new PurchaseDetail{ CreatedAt=DateTime.Parse("2020-07-31"),UpdatedAt=DateTime.Parse("2020-07-31"),PurchaseId = purchases[0].Id,DishId = dishBistec.Id,OriginalPrice = 35,Description = "Bistec con papas y verduras a la parrilla.",UnitPrice= 35, Quantity = 1},
                    new PurchaseDetail{ CreatedAt=DateTime.Parse("2020-07-31"),UpdatedAt=DateTime.Parse("2020-07-31"),PurchaseId = purchases[0].Id,DishId = dishesPromotionJalea.Id,PromotionId = promotionJalea.Id,OriginalPrice = 45,Description = "Jalea que viene con Ceviche",UnitPrice= 35, Quantity = 1},
                }
                    );
                context.SaveChanges();
            }
        }
    }
}
