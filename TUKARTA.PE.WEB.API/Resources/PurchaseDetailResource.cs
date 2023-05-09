using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
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
    }
}
