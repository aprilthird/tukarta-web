using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class ReviewResource : BaseResource
    {
        public Guid PurchaseId { get; set; }

        public PurchaseResource Purchase { get; set; }

        public double Score { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
