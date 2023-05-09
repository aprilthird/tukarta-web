using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.ENTITIES.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
