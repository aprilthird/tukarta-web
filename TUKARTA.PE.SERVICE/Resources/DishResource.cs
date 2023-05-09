using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class DishResource : BaseResource
    {
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
