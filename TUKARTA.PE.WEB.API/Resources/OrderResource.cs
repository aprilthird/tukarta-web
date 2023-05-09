using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class OrderResource : PurchaseResource
    {
        public int PeopleAmount { get; set; }

        public int IssueType { get; set; }

        public string TicketName { get; set; }

        public string RUC { get; set; }

        public string BusinessName { get; set; }

        public int ConsumptionModality { get; set; }

        public int PaymentMethod { get; set; }

        public double? PaymentAmount { get; set; }
    }
}
