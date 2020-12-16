using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public class MvuCore<TComponent, TModel, TMsg, TArgs>
        where TComponent : MvuComponent<TModel, TMsg, TArgs>, new()
        where TModel : class
    {
        private static readonly MvuComponent<TModel, TMsg, TArgs> Component = new TComponent();

        private readonly IMediator _mediator;
        private volatile TModel _oldModel;
        private volatile TModel _model;
        public IMvuViewEngine<TModel, TMsg, TArgs> ViewEngine { get; set; }

        public MvuCore(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task InitAsync()
        {
            var (model, cmd) = Component.Init(ViewEngine.GetInitArgs());
            _model = _oldModel = model;
            if (cmd != null)
            {
                var msgQueue = new Queue<TMsg>();
                await cmd(_mediator, msgQueue.Enqueue, default);
                await MsgLoopAsync(msgQueue, default);
            }
        }

        public async Task DispatchAsync(TMsg msg, CancellationToken cancellationToken = default)
        {
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

        private async Task MsgLoopAsync(Queue<TMsg> msgQueue, CancellationToken cancellationToken)
        {
            DispatchHandler<TMsg> dispatchHandler = msgQueue.Enqueue;
            while (!(msgQueue.Count == 0 && cancellationToken.IsCancellationRequested))
            {
                var (model, cmd) = Component.Update(_model, msgQueue.Dequeue());
                _model = model;

                if (ViewEngine != null)
                {
                    await ViewEngine.RenderViewAsync(model);
                }

                if (cmd != null)
                {
                    await cmd(_mediator, dispatchHandler, cancellationToken);
                }
            }
        }
    }
}