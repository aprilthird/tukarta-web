using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class DishResource : BaseResource
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool IsCustomizable { get; set; } = false;

        public Uri PictureUrl { get; set; }

        public Guid DishCategoryId { get; set; }

        public DishCategoryResource DishCategory { get; set; }

        public Guid MenuCategoryId { get; set; }

        public MenuCategoryResource MenuCategory { get; set; }
    }
}
