using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.CORE.Structs;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.DATA.Extensions;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.WEB.APP.DINER.ViewModels.RestaurantsViewModels
{
    public class IndexViewModel : ViewModel
    {
        private IEnumerable<RestaurantResource> Restaurants { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RestaurantResource>> GetRestaurantsAsync
            (PaginationParameters paginationParameters, bool refresh = false)
        {
            if (Restaurants == null || refresh)
                await LoadAsync(paginationParameters);
            return Restaurants;
        }

        private async Task LoadAsync(PaginationParameters paginationParameters)
        {
            Restaurants = await DbContext.Restaurants
                .Where(x => x.IsEnabled)
                .Select(x => new RestaurantResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    RUC = x.RUC,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LogoUrl = x.LogoUrl,
                    CommisionPrice = x.CommisionPrice,
                    KilometersCoverage = x.KilometersCoverage,
                    CurrencyType = x.CurrencyType,
                    EstimatedDeliveryMinutes = x.EstimatedDeliveryMinutes,
                    Story = x.Story,
                    WebSiteUrl = x.WebSiteUrl.ToString(),
                    IsOrderEnabled = x.IsOrderEnabled,
                    IsBookingEnabled = x.IsBookingEnabled,
                    IsDeliveryEnabled = x.IsDeliveryEnabled,
                    Latitude = x.Location.X,
                    Longitude = x.Location.Y,
                    //Distance = geolocalization.Latitude.HasValue && geolocalization.Longitude.HasValue
                    //        ? x.Location.Distance(userLocation)
                    //        : (double?)null,
                    User = new UserResource
                    {
                        Name = x.User.Name,
                        Surname = x.User.Surname
                    },
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Page(paginationParameters).ToListAsync();
        }
    }
}
