using System;
using MvuSharp;
using MvuSharp.Blazor;

namespace CustomGreeting.Core
{
    public static class Greeting
    {
        public record Model(string Name);

        public static (Model, CommandHandler<Unit>) Init(InitArgs<string> arg)
        {
            return (new Model(arg.QueryParams["name"]), null);
        }

        public static (Model, CommandHandler<Unit>) Update(Model model, Unit msg)
        {
            return (model, null);
        }
        
        public class Component : MvuComponent<Model, Unit, InitArgs<string>>
        {
            public Component() 
                : base(Greeting.Init, Greeting.Update)
            {
            }
        }
    }
}