using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Testing
{
    internal class MockRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Func<TRequest, Task<TResponse>> _handleFunc;

        public MockRequestHandler(Func<TRequest, TResponse> handleFunc)
        {
            _handleFunc = (command) => Task.FromResult(handleFunc(command));
        }

        public MockRequestHandler(Func<TRequest, Task<TResponse>> handleFunc)
        {
            _handleFunc = handleFunc;
        }

        public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken) =>
            _handleFunc(request);
    }

    internal class MockRequestHandler<TRequest>
        : MockRequestHandler<TRequest, Unit>
        where TRequest : IRequest
    {
        public MockRequestHandler(Action<TRequest> handleAction) 
            : base(request =>
            {
                handleAction(request);
                return Unit.Value;
            })
        {
        }

        public MockRequestHandler(Func<TRequest, Task> handleFunc) 
            : base(async request =>
            {
                await handleFunc(request);
                return Unit.Value;
            })
        {
        }
    }
}