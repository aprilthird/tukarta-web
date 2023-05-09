using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class OrderResource : PurchaseResource
    {
        public int PeopleAmount { get; set; }

        public int IssueType { get; set; }

        public string TicketName { get; set; }

        public string RUC { get; set; }

        public string BusinessName { get; set; }

        public int ConsumptionModality { get; set; }
    }
}
