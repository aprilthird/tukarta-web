using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class PromotionResource : BaseResource
    {
        public double NewPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public bool AvailableToCarryOut { get; set; }

        public bool AvailableToEatIn { get; set; }

        public bool AvailableForOrder { get; set; }

        public bool AvailableForBooking { get; set; }

        public bool AvailableForDelivery { get; set; }

        public Guid DishId { get; set; }

        public DishResource Dish { get; set; }
    }
}
