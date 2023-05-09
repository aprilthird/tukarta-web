using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class BookingResource : PurchaseResource
    {
        public int PeopleAmount { get; set; }

        public DateTime DateTime { get; set; }
    }
}
