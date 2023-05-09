using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.WEB.APP.DINER.ViewModels.PromotionsViewModels
{
    public class IndexViewModel : ViewModel
    {
        public IEnumerable<PromotionResource> Promotions { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
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
                        Price = x.Dish.Price
                    },
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();
        }
    }
}
