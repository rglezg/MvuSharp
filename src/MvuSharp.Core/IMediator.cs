using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IMediator
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
        Task SendAsync(IRequest request);
        void NavigateTo(string route, IReadOnlyDictionary<string, string> parameters);
    }
}