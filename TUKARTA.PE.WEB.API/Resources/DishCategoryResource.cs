using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.API.Resources
{
    public class DishCategoryResource : BaseResource
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Uri PictureUrl { get; set; }
    }
}
