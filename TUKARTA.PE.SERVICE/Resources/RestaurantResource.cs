using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class RestaurantResource : BaseResource
    {
        public string Name { get; set; }

        public string RUC { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int KilometersCoverage { get; set; }

        public double CommisionPrice { get; set; }

        public int CurrencyType { get; set; }

        public int EstimatedDeliveryMinutes { get; set; }

        public double MinimumAmount { get; set; }

        public double DeliveryCostPerKilometer { get; set; }

        public string WebSiteUrl { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Story { get; set; }

        public string Address { get; set; }

        public double? Distance { get; set; }

        public bool IsOrderEnabled { get; set; }

        public bool IsBookingEnabled { get; set; }

        public bool IsDeliveryEnabled { get; set; } 

        public bool IsEnabled { get; set; }

        public Uri LogoUrl { get; set; }

        public Guid UserId { get; set; }

        public UserResource User { get; set; }

        public List<ServiceScheduleResource> Schedules { get; set; }
    }
}
