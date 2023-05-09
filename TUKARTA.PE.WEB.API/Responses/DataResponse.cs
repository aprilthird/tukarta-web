using System;

namespace TUKARTA.PE.WEB.API.Responses
{
    public class DataResponse : OkResponse
    {
        public object Data { get; set; }

        public DataResponse()
            : base()
        {
        }
    }
}
