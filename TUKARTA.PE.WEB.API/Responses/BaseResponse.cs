using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Responses
{
    public class BaseResponse
    {
        public string Status { get; private set; }

        public BaseResponse(string status)
        {
            this.Status = status;
        }
    }
}
