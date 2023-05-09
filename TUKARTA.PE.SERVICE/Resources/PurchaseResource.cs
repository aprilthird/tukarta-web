using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class PurchaseResource : BaseResource
    {
        public int Code { get; set; }

        public int OperationNumber { get; set; }

        public Guid UserId { get; set; }

        public UserResource User { get; set; }

        public Guid RestaurantId { get; set; }

        public RestaurantResource Restaurant { get; set; }

        public int Status { get; set; }

        public string Annotations { get; set; }

        public string RejectionReason { get; set; }

        public IEnumerable<PurchaseDetailResource> Details { get; set; }

        public double Total => Details?.Sum(x => x.SubTotal) ?? 0;
    }
}
