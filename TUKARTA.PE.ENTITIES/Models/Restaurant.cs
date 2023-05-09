using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class Restaurant : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string RUC { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int KilometersCoverage { get; set; }

        public double CommisionPrice { get; set; }

        public int CurrencyType { get; set; }

        public int EstimatedDeliveryMinutes { get; set; }

        public double MinimumAmount { get; set; }

        public double DeliveryCostPerKilometer { get; set; }

        public Uri WebSiteUrl { get; set; }

        public Point Location { get; set; }

        public string Address { get; set; }

        public string Story { get; set; }

        public Uri LogoUrl { get; set; }

        public bool IsOrderEnabled { get; set; }

        public bool IsBookingEnabled { get; set; }

        public bool IsDeliveryEnabled { get; set; }

        public bool IsEnabled { get; set; } = true;

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public IEnumerable<MenuCategory> MenuCategories { get; set; }

        public List<ServiceSchedule> ServiceSchedules { get; set; }
    }
}
