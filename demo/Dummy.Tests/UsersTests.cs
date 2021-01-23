using System.Collections.Generic;
using System.Linq;
using Dummy.Core.Models;
using MvuSharp.Collections;
using MvuSharp.Testing;
using Xunit;
using static Dummy.Core.Users;

namespace Dummy.Tests
{
    public class UsersTests
    {
        private static readonly Model DefaultModel = new(new List<User>
        {
            new User{Age = 20, Email = "a@email.com", Id = 1, Name = "Ana"},
            new User{Age = 28, Email = "pb@email.com", Id = 2, Name = "Pablo"},
            new User{Age = 12, Email = "fr@email.com", Id = 3, Name = "Francisco"}
        }.ToRecordList(), false);

        [Fact(DisplayName = "'Add' adds a new user to 'Users' list and sets 'Adding' to 'false'.")]
        public void Add()
        {
            var user = new User();
            var list = DefaultModel.Users.ToList();
            list.Add(user);
            var model = DefaultModel with {Adding = true};
            new MockBuilder()
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Add(user)),
                    m => Assert.Equal(
                        model with{Users = list.ToRecordList(), Adding = false}, m));
        }

        [Fact(DisplayName = "'Delete' removes a user by their Id.")]
        public void Delete()
        {
            var list = DefaultModel.Users.ToList();
            var u = list.Find(user => user.Id == 2);
            list.Remove(u);
            var model = DefaultModel;
            new MockBuilder()
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Delete(2)),
                    m => Assert.Equal(
                        model with{Users = list.ToRecordList()}, m));
        }
    }
}