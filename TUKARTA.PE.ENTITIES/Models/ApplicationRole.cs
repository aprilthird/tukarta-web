using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.ENTITIES.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }
    }
}
