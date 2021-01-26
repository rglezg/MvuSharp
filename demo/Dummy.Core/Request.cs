using Dummy.Core.Models;
using MvuSharp;

namespace Dummy.Core
{
    public static class Request
    {
        public record AddUser(User UserToAdd) : IRequest<bool>;

        public record DeleteUser(int Id) : IRequest<bool>;
    }
}