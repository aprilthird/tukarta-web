using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class MenuCategory : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public IEnumerable<Dish> Dishes { get; set; }
    }
}
