using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Material.Blazor.Internal
{
    public partial class InternalSnackbar : ComponentFoundation
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("MaterialBlazor.MBSnackbar.open", SnackbarReference);
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private protected override async Task InstantiateMcwComponent() => await JsRuntime.InvokeVoidAsync("MaterialBlazor.MBSnackbar.init", SnackbarReference);


        /// <inheritdoc/>
        private protected override async Task DestroyMcwComponent() => await JsRuntime.InvokeVoidAsync("MaterialBlazor.MBSnackbar.destroy", SnackbarReference);
    }
}
