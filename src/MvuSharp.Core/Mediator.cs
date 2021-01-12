using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public class Mediator : IMediator
    {
        private readonly ServiceFactory _factory;

        private static readonly ConcurrentDictionary<Type, dynamic> Handlers = new();

        public Mediator(ServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            var requestType = request.GetType();
            var definition = Handlers.GetOrAdd(
                requestType,
                (_, requestTypeArg) =>
                    Activator.CreateInstance(
                        typeof(RequestHandlerImpl<,>).MakeGenericType(requestTypeArg, typeof(TResponse))),
                requestType);
            return await ((RequestHandlerWrapper<TResponse>) definition).RunAsync(request, _factory, cancellationToken);
        }

        public async Task SendAsync(IRequest request, CancellationToken cancellationToken)
        {
            await SendAsync<Unit>(request, cancellationToken);
        }

        private abstract class RequestHandlerWrapper<TResponse>
        {
            public abstract Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
                CancellationToken cancellationToken);
        }

        private class RequestHandlerImpl<TRequest, TResponse>
            : RequestHandlerWrapper<TResponse>
            where TRequest : IRequest<TResponse>
        {
            public override async Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
                CancellationToken cancellationToken)
            {
                var handler = factory.GetService<IRequestHandler<TRequest, TResponse>>();
                if (handler is IAggregateHandler aggregateRequestHandler)
                {
                    aggregateRequestHandler.MediatorInternal = factory.GetService<IMediator>();
                }
                return await handler.HandleAsync((TRequest) request, cancellationToken);
            }
        }
    }
}