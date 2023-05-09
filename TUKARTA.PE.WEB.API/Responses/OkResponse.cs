using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.WEB.API.Helpers;

namespace TUKARTA.PE.WEB.API.Responses
{
    public class OkResponse : BaseResponse
    {
        public OkResponse()
            : base(ConstantHelpers.Response.Status.OK)
        {
        }
    }
}
