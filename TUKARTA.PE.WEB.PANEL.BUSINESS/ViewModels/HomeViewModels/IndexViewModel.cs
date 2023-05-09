using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.HomeViewModels
{
    public class IndexViewModel : ViewModel
    {
        public List<RestaurantResource> Restaurants { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync(Guid userId)
        {
            Restaurants = await DbContext.Restaurants
                .Where(x => x.UserId == userId)
                .Select(x => new RestaurantResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    RUC = x.RUC,
                    Address = x.Address,
                    Story = x.Story,
                    IsEnabled = x.IsEnabled,
                    LogoUrl = x.LogoUrl,
                    CreatedAt = x.CreatedAt,
                }).ToListAsync();
        }
    }
}
