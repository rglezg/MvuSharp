using System.Collections.Immutable;
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
            new MockBuilder()
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Add(user)),
                    m => Assert.Equal(
                        model with{Users = list, Adding = false}, m));
        }

        [Fact(DisplayName = "'Delete' removes a user by their Id.")]
        public void Delete()
        {
            const int id = 2;
            var model = DefaultModel;
            new MockBuilder()
                .BuildMediator()
                .TestMvuFunc(Update(model, new Msg.Delete(id)),
                    m => Assert.Equal(
                        model with {
                            Users = DefaultModel.Users.Collection.Where(u => u.Id != id).ToRecordList()}, 
                        m));
        }
    }
}