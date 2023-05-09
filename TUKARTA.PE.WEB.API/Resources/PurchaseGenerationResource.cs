using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class PurchaseGenerationResource
    {
        public int Code { get; set; }

        public Guid PurchaseId { get; set; }

        public int OperationNumber { get; set; }

        public string SecurityToken { get; set; }
    }
}
