using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public class Mediator : IMediator
    {
        private readonly ServiceFactory _factory;

        private static readonly ConcurrentDictionary<Type, dynamic> Handlers =
            new ConcurrentDictionary<Type, dynamic>();

        public Mediator(ServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();
            var definition = Handlers.GetOrAdd(
                requestType,
                (type, cmdType) =>
                    Activator.CreateInstance(typeof(RequestHandlerImpl<,>).MakeGenericType(cmdType, typeof(TResponse))),
                requestType);
            return await ((RequestHandlerWrapper<TResponse>) definition).RunAsync(request, _factory, cancellationToken);
        }

        public async Task SendAsync(IRequest request, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();
            var definition = Handlers.GetOrAdd(
                requestType,
                (type, cmdType) =>
                    Activator.CreateInstance(typeof(RequestHandlerImpl<>).MakeGenericType(cmdType)),
                requestType);
            await ((RequestHandlerWrapper<Unit>) definition).RunAsync(request, _factory, cancellationToken);
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
            public override Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
                CancellationToken cancellationToken) =>
                factory
                    .GetService<IRequestHandler<TRequest, TResponse>>()
                    .HandleAsync((TRequest) request, cancellationToken);
        }

        private class RequestHandlerImpl<TRequest>
            : RequestHandlerWrapper<Unit>
            where TRequest : IRequest
        {
            public override async Task<Unit> RunAsync(IRequest<Unit> request, ServiceFactory factory, 
                CancellationToken cancellationToken)
            {
                await factory
                    .GetService<IRequestHandler<TRequest>>()
                    .HandleAsync((TRequest) request, cancellationToken);
                return Unit.Value;
            }
        }
    }
}