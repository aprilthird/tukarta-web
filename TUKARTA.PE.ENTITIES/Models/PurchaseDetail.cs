using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class PurchaseDetail : BaseEntity
    {
        public Guid DishId { get; set; }

        public Dish Dish { get; set; }

        public Guid? PurchaseId { get; set; }

        public Purchase Purchase { get; set; }

        public Guid? PromotionId { get; set; }

        public Promotion Promotion { get; set; }

        public double? OriginalPrice { get; set; }

        [Required]
        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
