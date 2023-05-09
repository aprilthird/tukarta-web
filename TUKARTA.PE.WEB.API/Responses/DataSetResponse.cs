using System;
using System.Collections.Generic;
using System.Linq;

namespace TUKARTA.PE.WEB.API.Responses
{
    public class DataSetResponse : OkResponse
    {
        public int TotalResults => Data?.Count() ?? 0;

        public IEnumerable<object> Data { get; set; }

        public DataSetResponse()
            : base()
        {
        }
    }
}
