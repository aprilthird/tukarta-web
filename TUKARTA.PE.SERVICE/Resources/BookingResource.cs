using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class BookingResource : PurchaseResource
    {
        public int PeopleAmount { get; set; }

        public DateTime DateTime { get; set; }
    }
}
