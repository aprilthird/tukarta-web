using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;

namespace TUKARTA.PE.WEB.APP.DINER.ViewModels
{
    public class ViewModel
    {
        protected TuKartaDbContext DbContext { get; set; }

        public string ErrorMessage { get; set; }

        protected ViewModel(TuKartaDbContext context)
        {
            DbContext = context;
        }
    }
}
