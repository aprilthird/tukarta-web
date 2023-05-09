using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class Purchase : BaseEntity
    {
        public int Code { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OperationNumber { get; set; }

        public int Type { get; set; }
        
        public int Status { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public string Annotations { get; set; }

        public string RejectionReason { get; set; }

        public int? PeopleAmount { get; set; }

        public int? IssueType { get; set; }

        public string TicketName { get; set; }

        public string RUC { get; set; }

        public string BusinessName { get; set; }

        public int? ConsumptionModality { get; set; }

        public int? PaymentMethod { get; set; }

        public double? PaymentAmount { get; set; }

        public DateTime? BookingDateTime { get; set; }

        public string DeliveryAddress { get; set; }

        public string DeliveryReference { get; set; }

        public Point DeliveryLocation { get; set; }

        public DateTime? DeliveryDateTime { get; set; }   

        public string PaymentTransactionId { get; set; }

        public string PaymentTraceNumber { get; set; }

        public string PaymentCurrency { get; set; }

        public string PaymentMaskedCardNumber { get; set; }

        public string PaymentCardBrand { get; set; }

        public IEnumerable<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
