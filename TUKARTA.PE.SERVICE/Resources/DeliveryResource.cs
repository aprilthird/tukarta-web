using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class DeliveryResource : PurchaseResource
    {
        public int IssueType { get; set; }

        public string TicketName { get; set; }

        public string RUC { get; set; }

        public string BusinessName { get; set; }

        public int PaymentMethod { get; set; }

        public double? PaymentAmount { get; set; }

        public string Address { get; set; }

        public string Reference { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
