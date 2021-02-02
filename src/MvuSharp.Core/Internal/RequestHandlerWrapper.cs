using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Internal
{
    internal abstract class RequestHandlerWrapper<TResponse>
    {
        public abstract Task<TResponse> RunAsync(IRequest<TResponse> request, ServiceFactory factory,
            CancellationToken cancellationToken);
    }
}