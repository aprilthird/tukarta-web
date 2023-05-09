using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class MenuCategoryResource : BaseResource
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RestaurantId { get; set; }

        public RestaurantResource Restaurant { get; set; }

        public IEnumerable<DishResource> Dishes { get; set; }
    }
}
