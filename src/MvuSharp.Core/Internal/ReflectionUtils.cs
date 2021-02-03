﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MvuSharp.Internal
{
    internal static class ReflectionUtils
    {
        public static IEnumerable<Type> GetHandlerInterfaces(this Type type) =>
            type
                .GetInterfaces()
                .Where(@interface =>
                    @interface.IsGenericType
                    && @interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,,>));

        public static object CreateGenericHandlerInstance(Type handlerGenericTypeDefinition, Type requestType)
        {
            var handlerType = handlerGenericTypeDefinition.MakeGenericType(requestType.GenericTypeArguments);
            return Activator.CreateInstance(
                typeof(RequestHandlerImplementation<,,>)
                    .MakeGenericType(
                        handlerType
                            .GetHandlerInterfaces()
                            .Single(i => i.GenericTypeArguments[0] == requestType)
                            .GenericTypeArguments),
                Activator.CreateInstance(handlerType));
        }
    }
}