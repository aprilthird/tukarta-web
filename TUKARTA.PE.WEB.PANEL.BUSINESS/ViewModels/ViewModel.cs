using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels
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
