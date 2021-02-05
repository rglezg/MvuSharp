using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MvuSharp.Collections;

namespace MvuSharp.Blazor
{
    public static class NavigationHandler
    {
        public static HandlerRegistrar AddNavigationHandler(this HandlerRegistrar registrar)
        {
            return registrar
                .Add(typeof(NavigateTo));
        }
        
        private class NavigateTo : SyncVoidRequestHandler<NavigationRequest.NavigateTo, NavigationManager>
        {
            protected override void Handle(NavigationRequest.NavigateTo request, NavigationManager service)
            {
                var (path, dictionary) = request;
                service.NavigateTo(QueryHelpers.AddQueryString(path, dictionary.ToImmutableDictionary()));
            }
        }
    }
}