using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.CORE.Structs;
using TUKARTA.PE.WEB.API.Helpers;

namespace TUKARTA.PE.WEB.API.Requests
{
    public class DataSetRequest : PaginationParameters
    {
        public string Search { get; set; }

        public DataSetRequest() : base() 
        {
            Page = ConstantHelpers.Request.DEFAULT_PAGE;
            Limit = ConstantHelpers.Request.DEFAULT_LIMIT;
            TakeAll = ConstantHelpers.Request.DEFAULT_ALL;
        }
    }
}
