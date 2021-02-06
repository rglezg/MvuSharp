using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Internal
{
    internal class Mediator : IMediator
    {
        private readonly ServiceFactory _factory;
        private readonly CancellationToken _cancellationToken;
        private readonly HandlerRegistrar _handlers;

        public Mediator(ServiceFactory serviceFactory, CancellationToken cancellationToken)
        {
            _factory = serviceFactory;
            _cancellationToken = cancellationToken;
            _handlers = serviceFactory.GetService<HandlerRegistrar>();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return await ((RequestHandlerWrapper<TResponse>) _handlers[request.GetType()])
                .RunAsync(request, _factory, _cancellationToken);
        }

        public async Task SendAsync(IRequest request)
        {
            await SendAsync<Unit>(request);
        }
    }
}