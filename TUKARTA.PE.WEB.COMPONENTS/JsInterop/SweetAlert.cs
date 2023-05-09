using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TUKARTA.PE.WEB.COMPONENTS.JsInterop
{
    public class SweetAlert : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; } = "";

        [Parameter]
        public string ButtonText { get; set; } = "OK";

        [Parameter]
        public DialogType Type { get; set; }

        public async Task FireAsync(string overrideTitle = null)
        {
            if (!string.IsNullOrEmpty(overrideTitle))
                Title = overrideTitle;
            if (string.IsNullOrEmpty(Message))
                await JSRuntime.InvokeVoidAsync("TuKarta.showAlert", Title, null, Type.ToString());
            else 
                await JSRuntime.InvokeVoidAsync("TuKarta.showAlert", Title, Message, Type.ToString());
        }

        public async Task<bool> ConfirmAsync()
        {
            return await JSRuntime.InvokeAsync<bool>("TuKarta.confirm", Title, Message, ButtonText, Type.ToString());
        }

        public async Task<bool> ConfirmTaskAsync()
        {
            return await JSRuntime.InvokeAsync<bool>("TuKarta.confirmAsync", Title, Message, ButtonText, Type.ToString());
        }

        public enum DialogType
        {
            question, warning, error, success, info
        }
    }
}
