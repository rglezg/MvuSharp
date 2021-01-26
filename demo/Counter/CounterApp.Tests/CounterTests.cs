using System;
using CounterApp.Core;
using MvuSharp.Testing;
using Xunit;
using System.Linq;
using MvuSharp;
using static CounterApp.Core.Counter;

namespace CounterApp.Tests
{
    public class CounterTests
    {
        private static readonly Model DefaultModel = new (0, false);
        
        [Fact(DisplayName = "'Increment' increases 'Count' by 1")]
        public void Increment()
        {
            var model = DefaultModel;
            Mock.DefaultMediator
                .TestMvuFunc(Update(model, new Msg.Increment()),
                    m => Assert.Equal(model with {Count = 1}, m));
        }
        
        [Fact(DisplayName = "'Decrement' decreases 'Count' by 1")]
        public void Decrement()
        {
            var model = DefaultModel;
            Mock.DefaultMediator
                .TestMvuFunc(Update(model, new Msg.Decrement()),
                    m => Assert.Equal(model with {Count = -1}, m));
        }

        [Fact(DisplayName = "'Random' sets Working to 'true' and dispatches 'Set' with a generated random number")]
        public void Random()
        {
            const int random = 5;
            var model = DefaultModel;
            new HandlerRegistrar()
                .Add<Request.RandomInt, int>(_ => random)
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Random()),
                    m => Assert.Equal(model with {Working = true}, m),
                    msg => Assert.Equal(new Msg.Set(random), msg.Single()));
        }

        [Fact(DisplayName = "'Set' sets 'Count' to the specified value and 'Working' to 'false'")]
        public void Set()
        {
            const int random = 5;
            var model = DefaultModel with {Working = true};
            Mock.DefaultMediator
                .TestMvuFunc(Update(model, new Msg.Set(random)),
                    m => Assert.Equal(model with {Count = random, Working = false}, m));
        }
    }
}
