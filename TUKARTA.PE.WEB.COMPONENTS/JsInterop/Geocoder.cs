using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.COMPONENTS.JsInterop
{
    public class Geocoder : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public class Model
        {
            public bool IsEnabled { get; set; }
            public bool HasError { get; set; }
            public bool Found { get; set; }
            public string Address { get; set; }
            public string ErrorMessage { get; set; }
        }

        public Model Response { get; set; }

        public async Task RequestAsync(double Lat, double Lng)
        {
            Response = await JSRuntime.InvokeAsync<Model>("TuKarta.geocoderByLatLng", Lat, Lng);
        }
    }
}
