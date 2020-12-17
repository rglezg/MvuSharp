using System;
using MvuSharp;

namespace Counter.Lib
{
    public static class Counter
    {
        public record Model(int Count);

        public record Msg
        {
            private Msg() {}
            public record Increment() : Msg();
            public record Decrement() : Msg();
            public record Random() : Msg();
            public record Set(int Value) : Msg();
        }
    
        public static (Model, CommandHandler<Msg>) Init(Unit unit) 
        {
            return (new Model(0), null);
        }

        public static (Model, CommandHandler<Msg>) Update(Model model, Msg msg)
        {
            switch (msg)
            {
                case Msg.Increment _:
                    return (model with {Count = model.Count + 1}, null);
                case Msg.Decrement _ :
                    return (model with {Count = model.Count - 1}, null);
                case Msg.Random _:
                    return (model, 
                        Command.MapResult(
                            new Request.RandomInt(), 
                            response => new Msg.Set(response)));
                case Msg.Set set:
                    return (model with {Count = set.Value}, null);
                default:
                    throw new InvalidOperationException(msg.GetType().FullName);
            }
        }

        public class Component : MvuComponent<Model, Msg, Unit>
        {
            public Component() : base(Counter.Init, Counter.Update)
            {
            }
        }
    }
}