using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class PurchasePaymentResource
    {
        public string TransactionNumber { get; set; }

        public string TransactionTraceNumber { get; set; }

        public string MaskedCardNumber { get; set; }

        public string CardBrand { get; set; }

        public string Currency { get; set; }
    }
}
