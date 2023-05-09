using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TUKARTA.PE.WEB.COMPONENTS.JsInterop
{
    public class GeoLocation : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public struct Model
        {
            public double Latitude;
            public double Longitude;
            public bool IsEnabled;
            public bool HasError;
            public string ErrorMessage;
            public int ErrorCode;
        }

        public Model Response { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                Response = await JSRuntime.InvokeAsync<Model>("TuKarta.getCurrentLocation");
            }
        }
    }
}
