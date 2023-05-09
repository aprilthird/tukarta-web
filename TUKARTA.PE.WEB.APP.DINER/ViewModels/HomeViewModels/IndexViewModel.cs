using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.CORE.Structs;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.DATA.Extensions;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.WEB.APP.DINER.ViewModels.HomeViewModels
{
    public class IndexViewModel : ViewModel
    {
        private IEnumerable<PromotionResource> Promotions { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PromotionResource>> GetPromotionsAsync
            (PaginationParameters paginationParameters, bool refresh = false)
        {
            if (Promotions == null || refresh)
                await LoadAsync(paginationParameters);
            return Promotions;
        }

        private async Task LoadAsync(PaginationParameters paginationParameters)
        {
            Promotions = await DbContext.Promotions
                .Where(x => x.IsEnabled)
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
