using BlazorInputFile;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.SERVICE.Services.Interfaces;
using TUKARTA.PE.WEB.PANEL.ADMIN.Helpers;

namespace TUKARTA.PE.WEB.PANEL.ADMIN.ViewModels.DishCategoriesViewModels
{
    public class IndexViewModel : ViewModel
    {
        public IEnumerable<DishCategoryResource> DishCategories { get; set; }

        public IBlobService BlobService { get; set; }

        public IndexViewModel(TuKartaDbContext context, IBlobService blobService) : base(context)
        {
            BlobService = blobService;
        }

        public async Task LoadAsync()
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

        public async Task<bool> SaveAsync(DishCategoryResource resource, IFileListEntry file, Stream fileStream)
        {
            var dishCategory = resource.Id.HasValue
                ? await DbContext.DishCategories.FindAsync(resource.Id.Value)
                : new DishCategory();
            if(await DbContext.DishCategories.AnyAsync(x => x.Name == resource.Name && x.Id != resource.Id))
            {
                ErrorMessage = "El nombre de la categoría ya se encuentra registrado";
                return false;
            }
            dishCategory.Name = resource.Name;
            dishCategory.Description = resource.Description;
            try
            {
                if (file != null)
                {
                    if(dishCategory.PictureUrl != null)
                        await BlobService.DeleteBlobAsync(dishCategory.PictureUrl);
                    dishCategory.PictureUrl = await BlobService.UploadFileBlobAsync(fileStream, file.Name, ConstantHelpers.Storage.Containers.DISH_CATEGORIES);
                }
                if (!resource.Id.HasValue)
                    await DbContext.DishCategories.AddAsync(dishCategory);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var dishCategory = await DbContext.DishCategories.FindAsync(id);
            DbContext.DishCategories.Remove(dishCategory);
            try
            {
                if (dishCategory.PictureUrl != null)
                    await BlobService.DeleteBlobAsync(dishCategory.PictureUrl);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }
    }
}
