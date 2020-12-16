using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IRequest<TResponse> { }
    
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest command, CancellationToken cancellationToken);
    }
    
    public interface IRequest : IRequest<Unit> {}

    public interface IRequestHandler<in TRequest>
    {
        Task HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}