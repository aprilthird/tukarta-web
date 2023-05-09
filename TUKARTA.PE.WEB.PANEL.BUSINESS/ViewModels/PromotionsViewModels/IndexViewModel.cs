using BlazorInputFile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Services.CloudStorage;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.PromotionsViewModels
{
    public class IndexViewModel : ViewModel
    {
        public Guid RestaurantId { get; set; }
        public List<PromotionResource> Promotions { get; set; }
        public List<DishResource> Dishes { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
        {
            Promotions = await DbContext.Promotions
                .Where(x=>x.Dish.MenuCategory.RestaurantId == RestaurantId)
                .Select(x=> new PromotionResource
                {
                    Id = x.Id,
                    NewPrice = x.NewPrice,
                    DishId = x.DishId,
                    ExpirationDateTime = x.ExpirationDateTime,
                    IsEnabled = x.IsEnabled,
                    Dish = new DishResource
                    {
                        Id = x.DishId,
                        Name = x.Dish.Name
                    }
                }).ToListAsync();
        }

        public async Task<bool> SaveAsync(PromotionResource resource)
        {
            var promotion = resource.Id.HasValue
                ? await DbContext.Promotions.FindAsync(resource.Id.Value)
                : new Promotion();
            //if (await DbContext.Promotions.AnyAsync(x => x.Dish.Name == resource.Dish.Name && x.Id != resource.Id))
            //{
            //    ErrorMessage = "El plato ya se encuentra en promoción";
            //    return false;
            //}
            promotion.DishId= resource.DishId;
            promotion.NewPrice = resource.NewPrice;
            promotion.ExpirationDateTime = resource.ExpirationDateTime;
            try
            {

                if (!resource.Id.HasValue)
                    await DbContext.Promotions.AddAsync(promotion);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task<bool> ChangeStatusAsync(Guid id)
        {
            try
            {
                var promotion = await DbContext.Promotions.FindAsync(id);
                promotion.IsEnabled = !promotion.IsEnabled;
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var promotion = await DbContext.Promotions.FindAsync(id);
            DbContext.Promotions.Remove(promotion);
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return true;
        }

        public async Task LoadDishesAsync()
        {
            Dishes = await DbContext.Dishes.Select(x => new DishResource
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price
            }).ToListAsync();
        }
    }
}
