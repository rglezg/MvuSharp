using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Internal
{
    internal class Mediator : IMediator
    {
        private readonly ServiceFactory _factory;
        private readonly NavigationHandler _navigationHandler;
        private readonly CancellationToken _cancellationToken;
        private readonly HandlerRegistrar _handlers;

        public Mediator(ServiceFactory serviceFactory, NavigationHandler navigationHandler, CancellationToken cancellationToken)
        {
            _factory = serviceFactory;
            _navigationHandler = navigationHandler;
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

        public void NavigateTo(string route, IReadOnlyDictionary<string, string> parameters)
        {
            if (_navigationHandler != null) _navigationHandler(route, parameters);
        }
    }
}