using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class DeliveryResource : PurchaseResource
    {
        public int IssueType { get; set; }

        public string TicketName { get; set; }

        public string RUC { get; set; }

        public string BusinessName { get; set; }

        public int PaymentMethod { get; set; }

        public double? PaymentAmount { get; set; }

        [Required]
        public string Address { get; set; }

        public string Reference { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
