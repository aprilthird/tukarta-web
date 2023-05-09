using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TUKARTA.PE.SERVICE.Resources
{
    public class DishCategoryResource : BaseResource
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Uri PictureUrl { get; set; }
    }
}
