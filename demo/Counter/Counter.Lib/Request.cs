using MvuSharp;

namespace Counter.Lib
{
    public static class Request
    {
        public record RandomInt() : IRequest<int>;
    }
}