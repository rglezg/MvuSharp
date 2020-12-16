using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MvuSharp.Blazor
{
    public abstract class MvuPage<TComponent, TModel, TMsg, TArg> 
    : OwningComponentBase<MvuCore<TComponent, TModel, TMsg, TArg>>, 
        IMvuViewEngine<TModel, TMsg, TArg>
    where TComponent : MvuComponent<TModel, TMsg, TArg>, new()
    where TModel : class
    {
        private readonly Action _stateHasChanged;
        public TModel Model { get; private set; }

        public MvuPage()
        {
            _stateHasChanged = StateHasChanged;
        }

        public void Dispatch(TMsg msg)
        {
            Service.DispatchAsync(msg);
        }

        public async Task DispatchAsync(TMsg msg, CancellationToken cancellationToken = default)
        {
            await Service.DispatchAsync(msg, cancellationToken);
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public async Task RenderViewAsync(TModel model)
        {
            Model = model;
            await InvokeAsync(_stateHasChanged);
        }
        
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public virtual TArg GetInitArgs() => default;

        protected override void OnInitialized()
        {
            Service.ViewEngine = this;
        }

        protected override async Task OnParametersSetAsync()
        {
            await Service.InitAsync();
        }

        protected override bool ShouldRender() => Service.ModelHasChanged();

        public void Dispose()
        {
            Service.ViewEngine = null;
        }
    }
}