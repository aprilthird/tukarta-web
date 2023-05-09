using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class UserResource : BaseResource
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string FullName => $"{Surname}, {Name}";

        public string RawFullName => $"{Name} {Surname}";

        public DateTime BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
