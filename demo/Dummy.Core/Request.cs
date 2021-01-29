using System.Collections.Generic;
using Dummy.Core.Models;
using MvuSharp;

namespace Dummy.Core
{
    public static class Request
    {
        public record AddUser(User UserToAdd) : IRequest;
        public record DeleteUser(User UserToRemove) : IRequest;
        public record FindUser(int Id) : IRequest<User>;

        public record GetAllUsers : IRequest<IEnumerable<User>>;
        public record SaveChanges : IRequest<bool>;
    }
}