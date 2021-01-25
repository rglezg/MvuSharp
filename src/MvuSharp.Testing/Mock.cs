using System;
using System.Collections.Generic;

namespace MvuSharp.Testing
{
    public static class Mock
    {
        public static readonly Mediator DefaultMediator = 
            new(type => type == typeof(HandlerRegistrar) 
                ? new HandlerRegistrar() 
                : throw new InvalidOperationException());
        
        public static IMediator BuildMediator(this HandlerRegistrar registrar)
        {
            return new Mediator(type =>
            {
                if (type == typeof(HandlerRegistrar))
                {
                    return registrar;
                }

                throw new ArgumentException($"$Unexpected service type: {type.FullName}");
            });
        }
    }
}