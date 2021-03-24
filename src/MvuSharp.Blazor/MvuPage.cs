using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace MvuSharp.Blazor
{
    public abstract class MvuPage<TComponent, TModel, TMsg, TArg> : 
        ComponentBase,
        IMvuViewEngine<TModel, TMsg, TArg>
    where TComponent : MvuComponent<TModel, TMsg, TArg>, new()
    where TModel : class
    {
        private MvuProgram<TComponent, TModel, TMsg, TArg> _mvuProgram;
        protected TModel Model { get; private set; }
        
        [Inject]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public IServiceProvider ServiceProvider { get; init; }
        
        [Inject]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public NavigationManager NavigationManager { get; init; }

        /// <summary>
        /// Puts a message in the message queue and starts the MVU loop.
        /// </summary>
        /// <param name="msg">The message to start the loop.</param>
        /// <param name="cancellationToken">A token for cancelling the task.</param>
        /// <returns></returns>
        public async Task Dispatch(TMsg msg, CancellationToken cancellationToken = default)
        {
            await _mvuProgram.DispatchAsync(msg, cancellationToken);
        }

        public void NavigateTo(string route, IReadOnlyDictionary<string, string> parameters)
        {
            var queryString = parameters.Count > 0
                ? parameters.ToImmutableDictionary()
                : ImmutableDictionary<string, string>.Empty;
            NavigationManager.NavigateTo(QueryHelpers.AddQueryString(route, queryString));
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public async Task RenderViewAsync(TModel model)
        {
            Model = model;
            await InvokeAsync(StateHasChanged);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public abstract TArg GetInitArgs();

        /// <inheritdoc/>
        /// <remarks>In MVU#, this method is responsible of initializing the MVU core loop.</remarks>
        protected override void OnInitialized()
        {
            _mvuProgram = new MvuProgram<TComponent, TModel, TMsg, TArg>(ServiceProvider, this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await _mvuProgram.InitAsync();
        }

        protected override bool ShouldRender() => _mvuProgram.ModelHasChanged();
    }

    public class MvuPage<TComponent, TModel, TMsg> : MvuPage<TComponent, TModel, TMsg, Unit>
        where TComponent : MvuComponent<TModel, TMsg, Unit>, new()
        where TModel : class
    {
        public override Unit GetInitArgs() => Unit.Value;
    }

    public record InitArgs<TArg>(TArg RouteParam, IReadOnlyDictionary<string, StringValues> QueryParams);
    public class MvuParamsPage<TComponent, TModel, TMsg, TArg> : MvuPage<TComponent, TModel, TMsg, InitArgs<TArg>>
        where TComponent : MvuComponent<TModel, TMsg, InitArgs<TArg>>, new() 
        where TModel : class
    {
        public override InitArgs<TArg> GetInitArgs()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            return new InitArgs<TArg>(GetRouteParameter(), QueryHelpers.ParseQuery(uri.Query));
        }
        public virtual TArg GetRouteParameter() => default;
    }
}