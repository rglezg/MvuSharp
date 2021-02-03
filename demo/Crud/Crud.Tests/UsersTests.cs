using System;
using System.Collections.Immutable;
using System.Linq;
using Crud.Core;
using Crud.Core.Models;
using MvuSharp;
using MvuSharp.Collections;
using MvuSharp.Testing;
using Xunit;
using static Crud.Core.Users;

namespace Crud.Tests
{
    public class UsersTests
    {
        private static readonly Model DefaultModel;

        static UsersTests()
        {
            var builder = ImmutableList<User>.Empty.ToBuilder();
            builder.Add(new User(Age: 20, Email: "a@email.com", Id: 1, Name: "Ana"));
            builder.Add(new User(Age: 28, Email: "pb@email.com", Id: 2, Name: "Pablo"));
            builder.Add(new User(Age: 12, Email: "fr@email.com", Id: 3, Name: "Francisco"));
            DefaultModel = new Model(builder.ToImmutable(), false);
        }

        [Fact(DisplayName = "'Add' adds a new user to 'Users' list and sets 'Adding' to 'false'.")]
        public void Add()
        {
            var user = new User(4,"Tester", "tester@email.com", 26);
            RecordList<User> list = DefaultModel.Users.Collection.Add(user);
            var model = DefaultModel with {Adding = true};
            new HandlerRegistrar()
                .Add(Handler.Create<Request.Add<User>>(_ => { }))
                .Add(Handler.Create<Request.SaveChanges, bool>(_ => true))
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Add(user)),
                    m => Assert.Equal(
                        model, m),
                    msg => Assert.Equal(new Msg.Set(model with{Users = list, Adding = false}), 
                        msg.Single()));
        }

        [Fact(DisplayName = "'Delete' removes a user by their Id.")]
        public void Delete()
        {
            const int id = 2;
            var model = DefaultModel;
            new HandlerRegistrar()
                .Add(Handler.Create<Request.Add<User>>(_ => { }))
                .Add(Handler.Create<Request.SaveChanges, bool>(_ => true))
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Delete(id)),
                    m => Assert.Equal(model, m),
                    msg => Assert.Equal(
                        new Msg.Set(model with {
                            Users = DefaultModel.Users.Collection.Where(u => u.Id != id).ToRecordList()}),
                        msg.Single()));
        }
    }
}