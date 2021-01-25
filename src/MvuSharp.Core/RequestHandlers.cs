using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IRequest<TResponse>
    {
    }

    public interface IRequest : IRequest<Unit>
    {
    }

    public abstract class RequestHandlerWrapper<TResponse>
    {
        public abstract Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
            CancellationToken cancellationToken);
    }

    public delegate Task<TResponse> RequestHandler<in TRequest, TResponse, in TService>(
        TRequest request,
        TService service,
        CancellationToken token)
        where TRequest : IRequest<TResponse>
        where TService : class;

    public class RequestHandlerImplementation<TRequest, TResponse, TService>
        : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
        where TService : class
    {
        private readonly RequestHandler<TRequest, TResponse, TService> _handler;

        public RequestHandlerImplementation(RequestHandler<TRequest, TResponse, TService> handler)
        {
            _handler = handler;
        }

        public override async Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
            CancellationToken cancellationToken)
        {
            var service = typeof(TService) == typeof(object) ? null : factory.GetService<TService>();
            return await _handler((TRequest) request, service, cancellationToken);
        }
    }
}