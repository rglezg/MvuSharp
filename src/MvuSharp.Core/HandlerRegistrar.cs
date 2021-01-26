using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public class HandlerRegistrar
    {
        private readonly Dictionary<Type, object> _handlers = new();

        public object this[Type requestType] => _handlers[requestType];

        public HandlerRegistrar Add<TRequest, TResponse, TService>(
            RequestHandler<TRequest, TResponse, TService> handler)
            where TRequest : IRequest<TResponse>
            where TService : class
        {
            _handlers[typeof(TRequest)] = new RequestHandlerImplementation<TRequest, TResponse, TService>(handler);
            return this;
        }

        public HandlerRegistrar Add<TRequest, TService>(
            Func<TRequest, TService, Task> handler)
            where TRequest : IRequest
            where TService : class
        {
            return Add<TRequest, Unit, TService>(async (request, service, _) =>
            {
                await handler(request, service);
                return Unit.Value;
            });
        }

        public HandlerRegistrar Add<TRequest, TResponse, TService>(
            Func<TRequest, TService, TResponse> handler)
            where TRequest : IRequest<TResponse>
            where TService : class
        {
            return Add((TRequest request, TService service, CancellationToken _) =>
                Task.FromResult(handler(request, service)));
        }

        public HandlerRegistrar Add<TRequest, TService>(
            Action<TRequest, TService> handler)
            where TRequest : IRequest
            where TService : class
        {
            return Add((TRequest request, TService service, CancellationToken _) =>
            {
                handler(request, service);
                return Unit.Task;
            });
        }

        public HandlerRegistrar Add<TRequest, TResponse>(
            Func<TRequest, Task<TResponse>> handler)
            where TRequest : IRequest<TResponse>
        {
            return Add<TRequest, TResponse, object>((request, _, _) => handler(request));
        }

        public HandlerRegistrar Add<TRequest, TResponse>(
            Func<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            return Add<TRequest, TResponse, object>((request, _, _) => Task.FromResult(handler(request)));
        }

        public HandlerRegistrar Add<TRequest>(
            Action<TRequest> handler)
            where TRequest : IRequest
        {
            return Add<TRequest, Unit>(request =>
            {
                handler(request);
                return Unit.Task;
            });
        }
    }
}