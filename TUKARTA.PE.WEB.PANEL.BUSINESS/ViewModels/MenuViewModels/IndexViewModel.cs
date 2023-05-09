using BlazorInputFile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;
using TUKARTA.PE.SERVICE.Services.Interfaces;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.MenuViewModels
{
    public class IndexViewModel : ViewModel
    {
        public List<MenuCategoryResource> Menus { get; set; }
        public List<DishCategoryResource> DishCategories { get; set; }
        public IBlobService BlobService { get; set; }

        public Guid RestaurantId { get; set; }

        public IndexViewModel(TuKartaDbContext context, IBlobService blobService) : base(context)
        {
            BlobService = blobService;
        }

        public async Task LoadAsync()
        {
            Menus = await DbContext.MenuCategories
                .Where(x => x.RestaurantId == RestaurantId)
                .Select(x => new MenuCategoryResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    Dishes = x.Dishes
                    .Select(y => new DishResource
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Description = y.Description,
                        Price = y.Price,
                        DishCategoryId = y.DishCategoryId,
                        MenuCategoryId = y.MenuCategoryId,
                        PictureUrl = y.PictureUrl,
                        DishCategory = new DishCategoryResource
                        {
                            Id = y.DishCategoryId,
                            Name = y.DishCategory.Name
                        }
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<bool> SaveMenuCategoryAsync(MenuCategoryResource resource)
        {
            var menuCategory = resource.Id.HasValue
                ? await DbContext.MenuCategories.FindAsync(resource.Id.Value)
                : new MenuCategory();
            if (await DbContext.MenuCategories.Where(x => x.RestaurantId == resource.RestaurantId).AnyAsync(x => x.Name == resource.Name && x.Id != resource.Id))
            {
                ErrorMessage = "El nombre de la categoría de la carta ya se encuentra registrado";
                return false;
            }
            menuCategory.Name = resource.Name;
            menuCategory.Description = resource.Description;
            menuCategory.RestaurantId = RestaurantId;
            try
            {
                if (!resource.Id.HasValue)
                    await DbContext.MenuCategories.AddAsync(menuCategory);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task<bool> DeleteMenuAsync(Guid id)
        {
            var menuCategory = await DbContext.MenuCategories.FindAsync(id);
            DbContext.MenuCategories.Remove(menuCategory);
            try
            {
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task<bool> SaveDishAsync(DishResource resource, IFileListEntry file, Stream fileStream)
        {
            var dish = resource.Id.HasValue
                ? await DbContext.Dishes.FindAsync(resource.Id.Value)
                : new Dish();
            //if (await DbContext.Dishes.AnyAsync(x => x.Name == resource.Name && x.Id != resource.Id))
            //{
            //    ErrorMessage = "El nombre del plato ya se encuentra registrado";
            //    return false;
            //}
            dish.Name = resource.Name;
            dish.DishCategoryId = resource.DishCategoryId;
            dish.Price = resource.Price;
            dish.Description = resource.Description;
            dish.MenuCategoryId = resource.MenuCategoryId;
            try
            {
                if (file != null)
                {
                    if (dish.PictureUrl != null)
                        await BlobService.DeleteBlobAsync(dish.PictureUrl);
                    dish.PictureUrl = await BlobService.UploadFileBlobAsync(fileStream, file.Name, ConstantHelpers.Storage.Containers.DISHES);
                }
                if (!resource.Id.HasValue)
                    await DbContext.Dishes.AddAsync(dish);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task<bool> DeleteDishAsync(Guid id)
        {
            var dish = await DbContext.Dishes.FindAsync(id);
            DbContext.Dishes.Remove(dish);
            try
            {
                if (dish.PictureUrl != null)
                    await BlobService.DeleteBlobAsync(dish.PictureUrl);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task LoadDishCategoriesAsync()
        {
            DishCategories = await DbContext.DishCategories.Select(x => new DishCategoryResource
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PictureUrl = x.PictureUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToListAsync();
        }
    }
}
