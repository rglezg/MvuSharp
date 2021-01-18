using System.Threading;
using System.Threading.Tasks;
using CounterApp.Core;
using CounterApp.Blazor.Services;
using MvuSharp;
using static System.Threading.Tasks.Task;

namespace CounterApp.Blazor.Handlers
{
    public class RandomIntHandler : RequestHandler<Request.RandomInt, int>
    {
        private readonly RandomGenerator _generator;

        public RandomIntHandler(RandomGenerator generator)
        {
            _generator = generator;
        }
        public override async Task<int> HandleAsync(Request.RandomInt request, CancellationToken cancellationToken)
        {
            var random = _generator.RandomInt(0, 100);
            await Delay(random * 20, cancellationToken);
            return random;
        }
    }
}