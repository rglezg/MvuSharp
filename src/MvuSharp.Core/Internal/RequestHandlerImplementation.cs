using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Internal
{
    internal class RequestHandlerImplementation<TRequest, TResponse, TService>
        : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
        where TService : class
    {
        private readonly Func<TRequest, TService, CancellationToken, Task<TResponse>> _handler;
        private readonly Func<ServiceFactory, TService> _resolveService;

        public RequestHandlerImplementation(Func<TRequest, TService, CancellationToken, Task<TResponse>> handler)
        {
            _handler = handler;
            _resolveService = typeof(TService) == typeof(object)
                ? _ => null
                : factory => factory.GetService<TService>();
        }

        public RequestHandlerImplementation(IRequestHandler<TRequest, TResponse, TService> handler)
            : this(handler.HandleAsyncBase)
        {
        }

        public override async Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
            CancellationToken cancellationToken)
        {
            return await _handler((TRequest) request, _resolveService(factory), cancellationToken);
        }
    }
}