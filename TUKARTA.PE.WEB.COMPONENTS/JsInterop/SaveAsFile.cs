using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.COMPONENTS.JsInterop
{
    public class SaveAsFile : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public async Task SaveAsync(string fileName, string fileType, string byteBase64)
        {
            await JSRuntime.InvokeVoidAsync("TuKarta.saveAsFile", fileName, fileType, byteBase64);
        }
    }
}
