﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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
        public IServiceProvider ServiceProvider { get; set; }

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
}