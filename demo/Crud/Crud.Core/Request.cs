using System.Collections.Generic;
using Crud.Core.Models;
using MvuSharp;

namespace Crud.Core
{
    public static class Request
    {
        public record Add<TEntity>(TEntity Entity) : IRequest where TEntity : class;
        public record Delete<TEntity>(TEntity Entity) : IRequest where TEntity : class;
        public record Find<TEntity>(int Id) : IRequest<TEntity> where TEntity : class;

        public record GetAll<TEntity> : IRequest<IEnumerable<TEntity>>;
        public record SaveChanges : IRequest<bool>;
    }
}