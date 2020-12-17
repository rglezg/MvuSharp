using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IMediator
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
        Task SendAsync(IRequest request, CancellationToken cancellationToken);
    }
}