using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class Review : BaseEntity
    {
        public Guid PurchaseId { get; set; }

        public Purchase Purchase { get; set; }

        public double Score { get; set; }

        public string Comment { get; set; }
    }
}
