using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using MvuSharp;

namespace CustomGreeting.Core
{
    public static class Question
    {
        public record Model(string Name);

        public record Msg
        {
            private Msg() {}

            public record GoToGreeting(string Path, string Name) : Msg;
        }

        public static (Model, CommandHandler<Msg>) Init()
        {
            return (new Model(string.Empty), null);
        }

        public static (Model, CommandHandler<Msg>) Update(Model model, Msg msg)
        {
            switch (msg)
            {
                case Msg.GoToGreeting parameters:
                    var dict = new Dictionary<string, string>
                    {
                        ["name"] = parameters.Name
                    };
                    return (model with {Name = parameters.Name},
                        async (mediator, _) =>
                        {
                            await mediator.SendAsync(
                                new NavigationRequest.NavigateTo(parameters.Path, dict));
                        });
                default:
                    throw new InvalidOperationException(msg.GetType().FullName);
            }
        }
        public class Component : MvuComponent<Model, Msg>
        {
            public Component() : base(Question.Init, Question.Update)
            {
            }
        }
    }
}