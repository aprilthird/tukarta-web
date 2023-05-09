using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;

namespace TUKARTA.PE.WEB.PANEL.ADMIN.ViewModels.HomeViewModels
{
    public class IndexViewModel : ViewModel
    {
        public int UserCount { get; set; }
        public int BusinessCount { get; set; }
        public int DishCount { get; set; }
        public int PromotionCount { get; set; }
        public int PurchaseCount { get; set; }
        public int ReviewCount { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
        {
            UserCount = await DbContext.Users.CountAsync();
            BusinessCount = await DbContext.Restaurants.CountAsync();
            DishCount = await DbContext.Dishes.CountAsync();
            PromotionCount = await DbContext.Promotions.CountAsync();
            PurchaseCount = await DbContext.Purchases.CountAsync();
            ReviewCount = await DbContext.Reviews.CountAsync();
        }
    }
}
