using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class Dish : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool IsCustomizable { get; set; } = false;

        public Uri PictureUrl { get; set; }

        public Guid DishCategoryId { get; set; }

        public DishCategory DishCategory { get; set; }

        public Guid MenuCategoryId { get; set; }

        public MenuCategory MenuCategory { get; set; }
    }
}
