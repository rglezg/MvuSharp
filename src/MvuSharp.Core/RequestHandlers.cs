using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IRequest<TResponse> { }
    
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    internal interface IAggregateHandler
    {
        internal IMediator MediatorInternal { get; set; }
    }
    
    public abstract class RequestHandler<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    {
        public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
    
    public interface IRequest : IRequest<Unit> {}

    public abstract class RequestHandler<TRequest>
        : IRequestHandler<TRequest, Unit>
    {
        async Task<Unit> IRequestHandler<TRequest, Unit>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            await HandleAsync(request, cancellationToken);
            return Unit.Value;
        }

        protected abstract Task HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class SyncRequestHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
    {
        public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken) => 
            Task.FromResult(Handle(request));

        protected abstract TResponse Handle(TRequest request);
    }

    public abstract class SyncRequestHandler<TRequest>
        : IRequestHandler<TRequest, Unit>
    {
        public Task<Unit> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            Handle(request);
            return Unit.Task;
        }

        protected abstract void Handle(TRequest request);
    }

    public abstract class AggregateRequestHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>, IAggregateHandler
    {
        IMediator IAggregateHandler.MediatorInternal { get; set; }
        protected IMediator Mediator => (this as IAggregateHandler).MediatorInternal;
    
        public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class AggregateRequestHandler<TRequest>
        : IRequestHandler<TRequest, Unit>, IAggregateHandler
    {
        IMediator IAggregateHandler.MediatorInternal { get; set; }
        protected IMediator Mediator => (this as IAggregateHandler).MediatorInternal;
        async Task<Unit> IRequestHandler<TRequest, Unit>.HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            await HandleAsync(request, cancellationToken);
            return Unit.Value;
        }

        protected abstract Task HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}