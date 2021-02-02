using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IRequestHandler<TRequest, TResponse, TService>
        where TRequest : IRequest<TResponse>
        where TService : class
    {
        Task<TResponse> HandleAsyncBase(TRequest request, TService service, CancellationToken cancellationToken);
    }

    public abstract class RequestHandler<TRequest, TResponse, TService>
        : IRequestHandler<TRequest, TResponse, TService>
        where TRequest : IRequest<TResponse>
        where TService : class
    {
        public abstract Task<TResponse> HandleAsyncBase(TRequest request, TService service, CancellationToken cancellationToken);

        public RequestHandler<TRequest, TResponse, TService> Empty = new EmptyHandler();
        
        private class EmptyHandler : RequestHandler<TRequest, TResponse, TService>
        {
            public override Task<TResponse> HandleAsyncBase(TRequest request, TService service,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(default(TResponse));
            }
        }
    }

    public abstract class SyncRequestHandler<TRequest, TResponse, TService>
        : IRequestHandler<TRequest, TResponse, TService>
        where TRequest : IRequest<TResponse>
        where TService : class
    {
        public Task<TResponse> HandleAsyncBase(TRequest request, TService service, CancellationToken cancellationToken)
        {
            return Task.FromResult(Handle(request, service));
        }

        protected abstract TResponse Handle(TRequest request, TService service);
    }

    public abstract class SyncRequestHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse, object>
        where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> HandleAsyncBase(TRequest request, object service, CancellationToken cancellationToken)
        {
            return Task.FromResult(Handle(request));
        }

        protected abstract TResponse Handle(TRequest request);
    }
    
    public abstract class SyncVoidRequestHandler<TRequest, TService>
        : IRequestHandler<TRequest, Unit, TService>
        where TRequest : IRequest
        where TService : class
    {
        public Task<Unit> HandleAsyncBase(TRequest request, TService service, CancellationToken cancellationToken)
        {
            Handle(request, service);
            return Unit.Task;
        }

        protected abstract void Handle(TRequest request, TService service);
    }
    
    public abstract class SyncVoidRequestHandler<TRequest>
        : IRequestHandler<TRequest, Unit, object>
        where TRequest : IRequest
    {
        public Task<Unit> HandleAsyncBase(TRequest request, object service, CancellationToken cancellationToken)
        {
            Handle(request);
            return Unit.Task;
        }

        protected abstract void Handle(TRequest request);
    }

    public abstract class RequestHandler<TRequest, TResponse>
    :IRequestHandler<TRequest, TResponse, object>
    where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> HandleAsyncBase(TRequest request, object service, CancellationToken cancellationToken)
        {
            return HandleAsync(request, cancellationToken);
        }

        protected abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
    
    public abstract class VoidRequestHandler<TRequest, TService>
        :IRequestHandler<TRequest, Unit, TService>
        where TRequest : IRequest where TService : class
    {
        public async Task<Unit> HandleAsyncBase(TRequest request, TService service, CancellationToken cancellationToken)
        {
            await HandleAsync(request, service, cancellationToken);
            return Unit.Value;
        }

        protected abstract Task HandleAsync(TRequest request, TService service, CancellationToken cancellationToken);
    }
    
    public abstract class VoidRequestHandler<TRequest>
        :IRequestHandler<TRequest, Unit, object>
        where TRequest : IRequest
    {
        public async Task<Unit> HandleAsyncBase(TRequest request, object service, CancellationToken cancellationToken)
        {
            await HandleAsync(request, cancellationToken);
            return Unit.Value;
        }

        protected abstract Task HandleAsync(TRequest request, in CancellationToken cancellationToken);
    }
}