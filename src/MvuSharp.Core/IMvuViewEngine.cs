using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
    public interface IMvuViewEngine<in TModel, in TMsg, out TArgs>
    where TModel : class
    {
        Task RenderViewAsync(TModel model);
        TArgs GetInitArgs();
        void Dispatch(TMsg msg);
        Task DispatchAsync(TMsg msg, CancellationToken cancellationToken);
    }
}