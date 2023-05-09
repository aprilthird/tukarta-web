using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class UserResource : BaseResource
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string FullName => $"{Surname}, {Name}";

        public string RawFullName => $"{Name} {Surname}";

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsExternalLogin { get; set; }
    }
}
