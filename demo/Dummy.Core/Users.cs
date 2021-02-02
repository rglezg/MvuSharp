using System;
using System.Linq;
using Dummy.Core.Models;
using MvuSharp;
using MvuSharp.Collections;

namespace Dummy.Core
{
    public static class Users
    {
        public record Model (RecordList<User> Users, bool Adding);

        public record Msg
        {
            private Msg()
            {
            }

            public record Add(User UserToAdd) : Msg;

            public record Delete(int Id) : Msg;

            public record ShowAddView() : Msg;
            public record Set(Model NewModel) : Msg;
        }

        public static (Model, CommandHandler<Msg>) Init()
        {
            var initModel = new Model(RecordList<User>.Empty, false);
            return (initModel,
                async (mediator, dispatch, token) =>
                {
                    var list = (await mediator.SendAsync(new Request.GetAll<User>(), token)).ToRecordList();
                    if (list.Count != 0)
                    {
                        dispatch(new Msg.Set(initModel with {Users = list}));
                    }
                });
        }

        public static (Model, CommandHandler<Msg>) Update(Model model, Msg msg)
        {
            var list = model.Users.Collection;
            switch (msg)
            {
                case Msg.Add addMsg:
                    return (model,
                        async (mediator, dispatch, token) =>
                        {
                            await mediator.SendAsync(new Request.Add<User>(addMsg.UserToAdd), token);
                            if (await mediator.SendAsync(new Request.SaveChanges(), token))
                            {
                                dispatch(new Msg.Set(model with {
                                    Users = list.Add(addMsg.UserToAdd),
                                    Adding = false
                                }));
                            }
                        });
                case Msg.Delete id:
                    var user = list.Find(u => u.Id == id.Id);
                    var request = new Request.Delete<User>(user);
                    return (model,
                        async (mediator, dispatch, token) =>
                        {
                            await mediator.SendAsync(request, token);
                            if (await mediator.SendAsync(new Request.SaveChanges(), token))
                            {
                                dispatch(new Msg.Set(model with {Users = list.Remove(user)}));
                            }
                        });
                case Msg.ShowAddView _:
                    return (model with {Adding = true}, null);
                case Msg.Set newModel:
                    return (newModel.NewModel, null);
                default:
                    throw new InvalidOperationException(msg.GetType().FullName);
            }
        }

        public class Component : MvuComponent<Model, Msg>
        {
            public Component() : base(Users.Init, Users.Update)
            {
            }
        }
    }
}