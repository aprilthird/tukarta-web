using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.COMPONENTS.JsInterop
{
    public class Document : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string Title { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JSRuntime.InvokeVoidAsync("TuKarta.setDocumentTitle", Title + " | TuKarta");
        }
    }
}
