using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvuSharp.Internal;

namespace MvuSharp
{
    public class HandlerRegistrar
    {
        private readonly Dictionary<Type, object> _handlers = new();
        private readonly Dictionary<string, Type> _genericHandlers = new();

        public object this[Type requestType]
        {
            get
            {
                if (_handlers.TryGetValue(requestType, out var handler)
                || TryCreateGenericHandler(requestType, out handler)) return handler;
                throw new KeyNotFoundException($"Handler for request of type {requestType.FullName} not found.");
            }
        }

        private bool TryCreateGenericHandler(Type requestType, out object handler)
        {
            if (!requestType.IsGenericType)
            {
                handler = null;
                return false;
            }

            var handlerType = _genericHandlers[requestType.GetGenericTypeDefinition().ToString()]
                .MakeGenericType(requestType.GenericTypeArguments);
            var interfaceType = handlerType
                .GetHandlerInterfaces()
                .Single(i => i.GenericTypeArguments[0] == requestType);
            handler = typeof(RequestHandlerImplementation<,,>)
                .MakeGenericType(interfaceType.GenericTypeArguments)
                .GetConstructor(new[] {interfaceType})
                ?.Invoke(new [] {Activator.CreateInstance(handlerType)});
            _handlers[requestType] = handler;
            return true;
        }

        public HandlerRegistrar Add(Type handlerType)
        {
            var interfaces = handlerType
                .GetHandlerInterfaces()
                .ToList();
            if (interfaces.Count == 0)
                throw new ArgumentException($"{handlerType.FullName} is not a valid handler type.");
            if (handlerType.IsGenericType)
            {
                foreach (var interfaceType in interfaces)
                {
                    _genericHandlers[interfaceType.GenericTypeArguments[0].ToString()] =
                        handlerType.GetGenericTypeDefinition();
                }
            }
            else
            {
                var handlerInstance = Activator.CreateInstance(handlerType);
                foreach (var @interface in interfaces)
                {
                    _handlers[@interface.GenericTypeArguments[0]] = handlerInstance;
                }
            }

            return this;
        }

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