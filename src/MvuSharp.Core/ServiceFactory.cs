using System;

namespace MvuSharp
{
    public delegate object ServiceFactory(Type type);

    internal static class ServiceFactoryExtensions
    {
        public static T GetService<T>(this ServiceFactory factory)
            where T : class
        {
            try
            {
                return factory(typeof(T)) as T ?? throw new NullReferenceException();
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Unable to construct instance of type {typeof(T)}. Register your CommandHandler Definitions with the container",
                    exception);
            }
        }
    }
}