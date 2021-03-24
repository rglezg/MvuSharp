using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace MvuSharp.Blazor
{
    public static class NavigationHandler
    {
        public static HandlerRegistrar AddNavigationHandler(this HandlerRegistrar registrar)
        {
            return registrar
                .Add(Handler.Create<NavigationRequest.NavigateTo, NavigationManager>(
                    (request, service) =>
                    {
                        var (path, queryParameters) = request;
                        var queryString = queryParameters.Count > 0
                            ? queryParameters.ToImmutableDictionary()
                            : ImmutableDictionary<string, string>.Empty;
                        service.NavigateTo(QueryHelpers.AddQueryString(path, queryString));
                    }));
        }
    }
}