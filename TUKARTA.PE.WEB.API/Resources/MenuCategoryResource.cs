using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class MenuCategoryResource : BaseResource
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RestaurantId { get; set; }

        public RestaurantResource Restaurant { get; set; }
    }
}
