using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class ContactResource : BaseResource
    {
        public string ResponsibleName { get; set; }

        public string BusinessName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
