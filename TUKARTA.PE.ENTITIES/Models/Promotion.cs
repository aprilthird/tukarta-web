using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class Promotion : BaseEntity
    {
        public DateTime ExpirationDateTime { get; set; }

        public double NewPrice { get; set; }

        public bool AvailableToCarryOut { get; set; } = true;
        
        public bool AvailableToEatIn { get; set; } = true;

        public bool AvailableForOrder { get; set; } = true;

        public bool AvailableForBooking { get; set; } = true;

        public bool AvailableForDelivery { get; set; } = true;
        public bool IsEnabled { get; set; } = true;

        public Guid DishId { get; set; }

        public Dish Dish { get; set; }

        public IEnumerable<Dish> Dishes { get; set; }
    }
}
