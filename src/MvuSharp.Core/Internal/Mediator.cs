using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Internal
{
    internal class Mediator : IMediator
    {
        private readonly ServiceFactory _factory;
        private readonly HandlerRegistrar _handlers;

        public Mediator(ServiceFactory serviceFactory)
        {
            _factory = serviceFactory;
            _handlers = serviceFactory.GetService<HandlerRegistrar>();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            return await ((RequestHandlerWrapper<TResponse>) _handlers[request.GetType()])
                .RunAsync(request, _factory, cancellationToken);
        }

        public async Task SendAsync(IRequest request, CancellationToken cancellationToken)
        {
            await SendAsync<Unit>(request, cancellationToken);
        }
    }
}