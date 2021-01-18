using MvuSharp;

namespace CounterApp.Core
{
    public static class Request
    {
        public record RandomInt : IRequest<int>;
    }
}