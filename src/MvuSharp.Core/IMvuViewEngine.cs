using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IMvuViewEngine<in TModel, in TMsg, out TArgs>
    where TModel : class
    {
        Task RenderViewAsync(TModel model);
        TArgs GetInitArgs();
        Task Dispatch(TMsg msg, CancellationToken cancellationToken);
        void NavigateTo(string route, IReadOnlyDictionary<string, string> parameters);
    }
}