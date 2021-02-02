using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dummy.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvuSharp;

namespace Dummy.Blazor.Data
{
    public static class EntityHandlers
    {
        public static HandlerRegistrar AddEntityHandlers(this HandlerRegistrar registrar)
        {
            return registrar
                .Add(typeof(Add<>))
                .Add(typeof(Delete<>))
                .Add(typeof(Find<>))
                .Add(typeof(GetAll<>));
        }
        
        private class Add<TEntity> : VoidRequestHandler<Request.Add<TEntity>, AppDbContext> where TEntity : class
        {
            protected override async Task HandleAsync(Request.Add<TEntity> request, AppDbContext service,
                CancellationToken cancellationToken)
            {
                await service.AddAsync(request.Entity, cancellationToken);
            }
        }

        private class Delete<TEntity> : SyncVoidRequestHandler<Request.Delete<TEntity>, AppDbContext>
            where TEntity : class
        {
            protected override void Handle(Request.Delete<TEntity> request, AppDbContext service)
            {
                service.Remove(request.Entity);
            }
        }
        
        private class GetAll<TEntity> : RequestHandler<Request.GetAll<TEntity>, IEnumerable<TEntity>, AppDbContext>
        where TEntity : class
        {
            public override async Task<IEnumerable<TEntity>> HandleAsyncBase(Request.GetAll<TEntity> request,
                AppDbContext service, CancellationToken cancellationToken)
            {
                return await service.Set<TEntity>().ToListAsync(cancellationToken);
            }
        }
        
        private class Find<TEntity> : RequestHandler<Request.Find<TEntity>, TEntity, AppDbContext> where TEntity : class
        {
            public override async Task<TEntity> HandleAsyncBase(Request.Find<TEntity> request, AppDbContext service,
                CancellationToken cancellationToken)
            {
                return await service.Set<TEntity>().FindAsync(new[] {request.Id}, cancellationToken);
            }
        }
    }
}        