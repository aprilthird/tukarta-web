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
    public class DetailsViewModel : ViewModel
    {
        public RestaurantResource Restaurant { get; set; }

        private IEnumerable<PromotionResource> Promotions { get; set; }


        public DetailsViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadRestaurantAsync(Guid id)
        {
            Restaurant = await DbContext.Restaurants
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
                    Address = x.Address,
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
                }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PromotionResource>> GetPromotionsAsync
            (PaginationParameters paginationParameters, bool refresh = false)
        {
            if (Promotions == null || refresh)
                await LoadPromotionsAsync(paginationParameters);
            return Promotions;
        }

        private async Task LoadPromotionsAsync(PaginationParameters paginationParameters)
        {
            Promotions = await DbContext.Promotions
                .Where(x => x.IsEnabled)
                .Where(x => x.Dish.MenuCategory.RestaurantId == Restaurant.Id)
                .Select(x => new PromotionResource
                {
                    Id = x.Id,
                    ExpirationDateTime = x.ExpirationDateTime,
                    NewPrice = x.NewPrice,
                    AvailableForOrder = x.AvailableForOrder,
                    AvailableForBooking = x.AvailableForBooking,
                    AvailableForDelivery = x.AvailableForDelivery,
                    AvailableToCarryOut = x.AvailableToCarryOut,
                    AvailableToEatIn = x.AvailableToEatIn,
                    Dish = new DishResource
                    {
                        Name = x.Dish.Name,
                        Price = x.Dish.Price,
                        PictureUrl = x.Dish.PictureUrl
                    },
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).Page(paginationParameters).ToListAsync();
        }
    }
}
