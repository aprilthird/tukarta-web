using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TUKARTA.PE.SERVICE.Services.CloudStorage
{
    public class BlobElementInfo
    {
        public Stream Content { get; set; }

        public string ContentType { get; set; }

        public BlobElementInfo(Stream Content, string ContentType)
        {
            this.Content = Content;
            this.ContentType = ContentType;
        }
    }
}
