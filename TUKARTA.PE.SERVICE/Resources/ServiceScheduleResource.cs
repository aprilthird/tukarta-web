using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class ServiceScheduleResource
    {
        public Guid RestaurantId { get; set; }

        public RestaurantResource Restaurant { get; set; }

        public int Day { get; set; }

        public DateTime OpeningTime { get; set; }

        public DateTime ClosingTime { get; set; }

        public bool IsClosed { get; set; }

        public DateTime? SecondOpeningTime { get; set; }

        public DateTime? SecondClosingTime { get; set; }
    }
}
