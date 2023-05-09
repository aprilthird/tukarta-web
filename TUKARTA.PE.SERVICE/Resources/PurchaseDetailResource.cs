using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class PurchaseDetailResource : BaseResource
    {
        public Guid DishId { get; set; }

        public DishResource Dish { get; set; }

        public Guid PurchaseId { get; set; }

        public PurchaseResource Purchase { get; set; }

        public Guid? PromotionId { get; set; }

        public PromotionResource Promotion { get; set; }

        public double? OriginalPrice { get; set; }

        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount => OriginalPrice.HasValue ? ((OriginalPrice.Value - UnitPrice)*100)/OriginalPrice.Value : 0.00;

        public double SubTotal => UnitPrice * Quantity;
    }
}
