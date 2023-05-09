using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers
{
    public class ConstantHelpers : CORE.Helpers.ConstantHelpers
    {
        public static class Project
        {
            public const string TITLE = "TuKarta";
        }

        public static class Route
        {
            public const string LOGIN = "ingresar";
        }

        public static class Notification
        {
            public const int DEFAULT_DURATION = 3000;
        }

        public static class Storage
        {
            public static class Containers
            {
                public const string DISHES = "platos";
                public const string RESTAURANTS = "restaurantes";
            }
        }
    }
}
