using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class ReviewResource : BaseResource
    {
        public Guid PurchaseId { get; set; }

        public PurchaseResource Purchase { get; set; }

        public double Score { get; set; }

        public string Comment { get; set; }
    }
}
