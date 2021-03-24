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
        private readonly Dictionary<Type, Type> _genericHandlers = new();

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

            handler = _handlers[requestType] =
                ReflectionUtils.CreateHandlerInstance(
                    _genericHandlers[requestType.GetGenericTypeDefinition()]
                        .MakeGenericType(requestType.GenericTypeArguments), requestType);
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
                    _genericHandlers[interfaceType.GenericTypeArguments[0].GetGenericTypeDefinition()] =
                        handlerType.GetGenericTypeDefinition();
                }
            }
            else
            {
                foreach (var requestType in interfaces.Select(@interface => @interface.GenericTypeArguments[0]))
                {
                    _handlers[requestType] = ReflectionUtils.CreateHandlerInstance(handlerType, requestType);
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

        public IMediator BuildMediator(ServiceFactory serviceFactory = null, NavigationHandler navigationHandler = null)
        {
            serviceFactory ??= type =>
                type == typeof(HandlerRegistrar)
                    ? this
                    : throw new ArgumentException($"$Unexpected service type: {type.FullName}");

            return new Mediator(serviceFactory, navigationHandler,CancellationToken.None);
        }
    }
}