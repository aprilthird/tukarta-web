using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class ServiceSchedule
    {
        public Guid RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public int Day { get; set; }

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public bool IsClosed { get; set; }

        public TimeSpan? SecondOpeningTime { get; set; }

        public TimeSpan? SecondClosingTime { get; set; }
    }
}
