using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
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

        public Uri WebSiteUrl { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Story { get; set; }

        public double? Distance { get; set; }

        public Uri LogoUrl { get; set; }

        public Guid? UserId { get; set; }

        public UserResource User { get; set; }
    }
}
