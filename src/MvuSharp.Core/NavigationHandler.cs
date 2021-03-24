using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvuSharp
{
    public delegate void NavigationHandler(string route, IReadOnlyDictionary<string, string> parameters);
}