using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Helpers
{
    public class ConstantHelpers : TUKARTA.PE.CORE.Helpers.ConstantHelpers
    {
        public static class Response
        {
            public static class Status
            {
                public const string OK = "ok";
                public const string ERROR = "error";
            }
        }

        public static class Request
        {
            public const int DEFAULT_LIMIT = 10;
            public const int DEFAULT_PAGE = 1;
            public const bool DEFAULT_ALL = true;
        }
    }
}
