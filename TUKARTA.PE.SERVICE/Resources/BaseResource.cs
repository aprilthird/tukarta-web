using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class BaseResource
    {
        public Guid? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
