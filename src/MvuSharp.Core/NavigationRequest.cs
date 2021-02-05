using System.Collections.Generic;

namespace MvuSharp
{
    public static class NavigationRequest
    {
        public record NavigateTo(string Path, IReadOnlyDictionary<string, string> Parameters) : IRequest;
    }
}