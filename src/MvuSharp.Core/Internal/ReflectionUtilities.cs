using System;
using System.Collections.Generic;
using System.Linq;

namespace MvuSharp.Internal
{
    internal static class ReflectionUtilities
    {
        
        internal static IEnumerable<Type> GetHandlerInterfaces(this Type type)
        {
            var queue = new Queue<Type>();
            queue.Enqueue(type);
            while (queue.Count > 0)
            {
                type = queue.Dequeue();
                if (type.IsClass && type.BaseType != typeof(object))
                {
                    queue.Enqueue(type.BaseType);
                }
                foreach (var @interface in type.GetInterfaces())
                {
                    if (@interface.IsGenericType)
                    {
                        var genericTypeDefinition = @interface.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(IRequestHandler<,,>))
                        {
                            yield return @interface;
                        }
                    }
                    else
                    {
                        queue.Enqueue(@interface);
                    }
                }
            }
        }
    }
}