using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class PromotionResource : BaseResource
    {
        public double Discount => (Dish?.Price ?? 0) - NewPrice;

        public double DiscountPercentage => Dish?.Price != 0 ? (Discount / Dish.Price) * 100 : 0;

        public double NewPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public bool AvailableToCarryOut { get; set; }

        public bool AvailableToEatIn { get; set; }

        public bool AvailableForOrder { get; set; }

        public bool AvailableForBooking { get; set; }

        public bool AvailableForDelivery { get; set; }
        public bool IsEnabled { get; set; }

        public Guid DishId { get; set; }

        public DishResource Dish { get; set; }
    }
}
