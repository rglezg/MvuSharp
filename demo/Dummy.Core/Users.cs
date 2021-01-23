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
            private Msg(){}

            public record Add(User UserToAdd) : Msg;
            public record Delete(int Id) : Msg;
            public record ShowAddView() : Msg;

        }

        public static (Model, CommandHandler<Msg>) Init()
        {
            return (new Model(RecordList<User>.Empty, false), null);
        }

        public static (Model, CommandHandler<Msg>) Update(Model model, Msg msg)
        {
            var list = model.Users.Collection.ToList();
            switch (msg)
            {
                case Msg.Add user:
                    list.Add(user.UserToAdd);
                    return (model with
                    {
                        Users = list.ToRecordList(), Adding = false
                    }, null);
                case Msg.Delete id:
                    var u = list.Find(user => user.Id == id.Id);
                    list.Remove(u);
                    return (model with
                    {
                        Users = list.ToRecordList()
                    }, null);
                case Msg.ShowAddView _:
                    return (model with {Adding = true}, null);
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