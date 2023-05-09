using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.ENTITIES.Base;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [NotMapped]
        public string FullName => $"{Surname}, {Name}";

        [NotMapped]
        public string RawFullName => $"{Name} {Surname}";

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsEnabled { get; set; } = false;

        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }

        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}
