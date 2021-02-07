using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvuSharp;
using MvuSharp.Internal;
using Xunit;

namespace UnitTests
{
    public class ReflectionUtilsTests
    {
        private static class Samples
        {
            public record BasicRequest : IRequest;

            public class BasicHandler : VoidRequestHandler<BasicRequest>
            {
                protected override Task HandleAsync(BasicRequest request, in CancellationToken cancellationToken) =>
                    throw new InvalidOperationException();
            }

            public record GenericRequest<T> : IRequest;

            public class GenericHandler<T> : SyncVoidRequestHandler<GenericRequest<T>>
            {
                protected override void Handle(GenericRequest<T> request) => throw new InvalidOperationException();
            }
        }

        [Fact]
        public void GetHandlerInterfaceBasic()
        {
            var handlerType = typeof(Samples.BasicHandler);
            var expectedInterfaceType = typeof(IRequestHandler<Samples.BasicRequest, Unit, object>);
            var actualInterfaceType = handlerType.GetHandlerInterfaces().Single();
            Assert.Equal(expectedInterfaceType, actualInterfaceType);
        }

        [Fact]
        public void GetHandlerInterfaceGeneric()
        {
            var handlerType = typeof(Samples.GenericHandler<>);
            var expectedInterfaceType = typeof(IRequestHandler<,,>)
                .MakeGenericType(typeof(Samples.GenericRequest<>), typeof(Unit), typeof(object)).GetGenericTypeDefinition();
            var actualInterfaceType = handlerType.GetHandlerInterfaces().Single().GetGenericTypeDefinition();
            Assert.Equal(expectedInterfaceType, actualInterfaceType);
        }

        [Fact]
        public void CreateGenericInstance()
        {
            var expectedInstanceType = typeof(RequestHandlerImplementation<Samples.GenericRequest<int>, Unit, object>);
            var actualInstanceType = ReflectionUtils.CreateHandlerInstance(
                typeof(Samples.GenericHandler<>), typeof(Samples.GenericRequest<int>)).GetType();
            Assert.Equal(expectedInstanceType, actualInstanceType);
        }
    }
}