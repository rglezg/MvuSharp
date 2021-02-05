using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace MvuSharp
{
    public static class NavigationRequest
    {
        public record NavigateTo(string Path, IReadOnlyDictionary<string, string> Parameters) : IRequest;
    }
}