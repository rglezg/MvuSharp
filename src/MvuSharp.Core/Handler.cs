using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public static class Handler
    {
        //No response
        public static Func<TRequest, TService, CancellationToken, Task<Unit>>
            Create<TRequest, TService>(
                Func<TRequest, TService, CancellationToken, Task> handler)
            where TRequest : IRequest<Unit>
            where TService : class =>
            async (request, service, token) =>
            {
                await handler(request, service, token);
                return Unit.Value;
            };

        //Sync
        public static Func<TRequest, TService, CancellationToken, Task<TResponse>>
            Create<TRequest, TResponse, TService>(
                Func<TRequest, TService, TResponse> handler)
            where TRequest : IRequest<TResponse>
            where TService : class =>
            (request, service, _) => Task.FromResult(handler(request, service));
        
        //No service
        public static Func<TRequest, object, CancellationToken, Task<TResult>>
            Create<TRequest, TResult>(
                Func<TRequest, CancellationToken, Task<TResult>> handler
            )
            where TRequest : IRequest<TResult> =>
            async (request, _, token) => await handler(request, token);

        //No service/No response
        public static Func<TRequest, object, CancellationToken, Task<Unit>>
            Create<TRequest>(
                Func<TRequest, CancellationToken, Task> handler
            )
            where TRequest : IRequest<Unit> =>
            async (request, _, token) =>
            {
                await handler(request, token);
                return Unit.Value;
            };

        //Sync/No response
        public static Func<TRequest, TService, CancellationToken, Task<Unit>>
            Create<TRequest, TService>(Action<TRequest, TService> handler)
            where TRequest : IRequest<Unit> where TService : class =>
            (request, service, _) =>
            {
                handler(request, service);
                return Unit.Task;
            };
        
        //Sync/No service
        public static Func<TRequest, object, CancellationToken, Task<TResult>>
            Create<TRequest, TResult>(
                Func<TRequest, TResult> handler
            )
            where TRequest : IRequest<TResult> =>
            (request, _, _) => Task.FromResult(handler(request));
        
        //Sync/No response/No service
        public static Func<TRequest, object, CancellationToken, Task<Unit>>
            Create<TRequest>(Action<TRequest> handler)
            where TRequest : IRequest<Unit> =>
            (request, _, _) =>
            {
                handler(request);
                return Unit.Task;
            };
    }
}