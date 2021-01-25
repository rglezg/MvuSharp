using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public class Mediator : IMediator
    {
        private readonly ServiceFactory _factory;
        private readonly HandlerRegistrar _handlers;

        private static readonly ConcurrentDictionary<Type, dynamic> Handlers = new();

        public Mediator(ServiceFactory serviceFactory)
        {
            _factory = serviceFactory;
            _handlers = serviceFactory.GetService<HandlerRegistrar>();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            var requestType = request.GetType();
            var definition = Handlers.GetOrAdd(
                requestType,
                (_, requestTypeArg) => _handlers[requestTypeArg],
                requestType);
            return await ((RequestHandlerWrapper<TResponse>) definition).RunAsync(request, _factory, cancellationToken);
        }

        public async Task SendAsync(IRequest request, CancellationToken cancellationToken)
        {
            await SendAsync<Unit>(request, cancellationToken);
        }
    }
}