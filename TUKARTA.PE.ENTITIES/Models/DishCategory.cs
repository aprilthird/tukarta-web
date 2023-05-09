using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class DishCategory : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Uri PictureUrl { get; set; }
    }
}
