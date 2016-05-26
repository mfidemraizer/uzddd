using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Shared.Patterns.Repository;

namespace UserZoom.Shared.Patterns.UnitOfWork
{
    public sealed class EFUnitOfWork<TDomainObjectId, TDomainObject, TRepository, TContext> : IDomainUnitOfWork<TDomainObjectId, TDomainObject, TRepository>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
        where TRepository : IRepository<TDomainObjectId, TDomainObject>
        where TContext : DbContext, new()
    {
        public EFUnitOfWork(TContext context, TRepository repository)
        {
            DbContext = context;
            Repository = repository;
        }

        public TRepository Repository { get; }

        private TContext DbContext { get; }

        public Task CommitAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public Task RollbackAsync()
        {
            Dispose();

            return Task.FromResult(true);
        }
    }
}
