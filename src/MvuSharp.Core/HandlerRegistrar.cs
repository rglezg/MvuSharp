using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvuSharp.Internal;

namespace MvuSharp
{
    public class HandlerRegistrar
    {
        private readonly Dictionary<Type, object> _handlers = new();

        public object this[Type requestType] => _handlers[requestType];

        public HandlerRegistrar Add<TRequest, TResponse, TService>(
            Func<TRequest, TService, CancellationToken, Task<TResponse>> handler)
            where TRequest : IRequest<TResponse>
            where TService : class
        {
            _handlers[typeof(TRequest)] = new RequestHandlerImplementation<TRequest, TResponse, TService>(handler);
            return this;
        }

        public IMediator BuildMediator(ServiceFactory serviceFactory = null)
        {
            serviceFactory ??= type =>
                type == typeof(HandlerRegistrar)
                    ? this
                    : throw new ArgumentException($"$Unexpected service type: {type.FullName}");

            return new Mediator(serviceFactory);
        }
    }
}