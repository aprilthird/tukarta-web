using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class PurchaseGenerationResource
    {
        public int Code { get; set; }

        public Guid PurchaseId { get; set; }

        public int OperationNumber { get; set; }

        public string SecurityToken { get; set; }
    }
}
