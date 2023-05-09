using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.CORE.Structs
{
    public class PaginationParameters
    {
        public int Page { get; set; }

        public int Limit { get; set; }

        public bool TakeAll { get; set; }
    }
}
