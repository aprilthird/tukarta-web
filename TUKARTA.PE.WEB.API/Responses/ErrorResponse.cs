using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.WEB.API.Helpers;

namespace TUKARTA.PE.WEB.API.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public string Message { get; set; }
        
        public ErrorResponse()
            : base(ConstantHelpers.Response.Status.ERROR)
        {
        }
    }
}
