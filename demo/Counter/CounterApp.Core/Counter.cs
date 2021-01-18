using System;
using MvuSharp;

namespace CounterApp.Core
{
    public static class Counter
    {
        public record Model(int Count, bool Working);

        public record Msg
        {
            private Msg() {}
            public record Increment : Msg;
            public record Decrement : Msg;
            public record Random : Msg;
            public record Set(int Value) : Msg;
        }
    
        public static (Model, CommandHandler<Msg>) Init() 
        {
            return (new Model(0, false), null);
        }

        public static (Model, CommandHandler<Msg>) Update(Model model, Msg msg)
        {
            switch (msg)
            {
                case Msg.Increment _:
                    return (model with {Count = model.Count + 1}, null);
                case Msg.Decrement _:
                    return (model with {Count = model.Count - 1}, null);
                case Msg.Random _:
                    return (model with {Working = true}, 
                        Command.MapResult(
                            new Request.RandomInt(), 
                            response => new Msg.Set(response)));
                case Msg.Set set:
                    return (model with {Working = false, Count = set.Value}, null);
                default:
                    throw new InvalidOperationException(msg.GetType().FullName);
            }
        }

        public class Component : MvuComponent<Model, Msg>
        {
            public Component() : base(Counter.Init, Counter.Update)
            {
            }
        }
    }
}