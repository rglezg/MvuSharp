using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MvuSharp.Internal;

namespace MvuSharp
{
    public class MvuProgram<TComponent, TModel, TMsg, TArgs>
        where TComponent : MvuComponent<TModel, TMsg, TArgs>, new()
        where TModel : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMvuViewEngine<TModel, TMsg, TArgs> _viewEngine;
        private static readonly MvuComponent<TModel, TMsg, TArgs> Component = new TComponent();

        private volatile TModel _oldModel;
        private volatile TModel _model;

        public MvuProgram(
            IServiceProvider serviceProvider,
            IMvuViewEngine<TModel, TMsg, TArgs> viewEngine)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
        }

        private IMediator CreateMediator(CancellationToken cancellationToken)
        {
            var scope = _serviceProvider.CreateScope();
            return new Mediator(scope.ServiceProvider.GetService, _viewEngine.NavigateTo, cancellationToken);
        }

        public async Task InitAsync()
        {
            if (_model != null)
            {
                throw new InvalidOperationException("The component is already initialized.");
            }

            var (model, cmd) = Component.Init(_viewEngine.GetInitArgs());
            _model = _oldModel = model;
            if (cmd == null)
            {
                await _viewEngine.RenderViewAsync(model);
                return;
            }

            var mediator = CreateMediator(CancellationToken.None);
            var msgQueue = new Queue<TMsg>();
            await cmd(mediator, msgQueue.Enqueue);
            await MsgLoopAsync(msgQueue, default, mediator);
        }

        public async Task DispatchAsync(TMsg msg, CancellationToken cancellationToken = default)
        {
            if (_model == null)
            {
                throw new InvalidOperationException(
                    $"The component has not been initialized. " +
                    $"Call {nameof(InitAsync)} once before dispatching any message.");
            }

            var msgQueue = new Queue<TMsg>();
            msgQueue.Enqueue(msg);
            await MsgLoopAsync(msgQueue, cancellationToken);
        }

        public bool ModelHasChanged()
        {
            if (ReferenceEquals(_model, _oldModel)) return false;
            _oldModel = _model;
            return true;
        }

        private async Task MsgLoopAsync(Queue<TMsg> msgQueue, CancellationToken cancellationToken,
            IMediator mediator = null)
        {
            DispatchHandler<TMsg> dispatchHandler = msgQueue.Enqueue;
            while (msgQueue.Count != 0 && !cancellationToken.IsCancellationRequested)
            {
                var (model, cmd) = Component.Update(_model, msgQueue.Dequeue());
                _model = model;

                await _viewEngine.RenderViewAsync(model);

                if (cmd == null) continue;
                mediator ??= CreateMediator(cancellationToken);
                try
                {
                    await cmd(mediator, dispatchHandler);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}