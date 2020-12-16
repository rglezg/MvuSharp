using System;
using System.Collections.Generic;

namespace MvuSharp.Testing
{
    public class MockBuilder
    {
        private readonly Dictionary<Type, dynamic> _services = new Dictionary<Type, dynamic>();
        private ServiceFactory ServiceFactory => (type) => _services[type];

        public MockBuilder Setup<TRequest, TResponse>(Func<TRequest, TResponse> handleFunc)
        where TRequest : IRequest<TResponse>
        {
            var handler = new MockRequestHandler<TRequest, TResponse>(handleFunc);
            var handlerType = typeof(IRequestHandler<TRequest, TResponse>);
            _services[handlerType] = handler;
            return this;
        }

        public IMediator BuildMediator() => new Mediator(ServiceFactory);
    }
}