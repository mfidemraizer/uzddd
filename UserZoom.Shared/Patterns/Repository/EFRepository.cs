using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Shared.Patterns.Repository
{
    public sealed class EFRepository<TDomainObjectId, TDomainObject> : Repository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        public EFRepository(DbSet<TDomainObject> dbSet, IIdGenerator<TDomainObjectId> idGenerator, IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
            : base(idGenerator, specs)
        {
            DbSet = dbSet;
        }

        public DbSet<TDomainObject> DbSet { get; }

        public async override Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Expression<Func<TDomainObject, bool>> criteriaExpr)
        {
            return new MultipleObjectResult<ICollection<TDomainObject>, TDomainObject>
            (
                "OK",
                await DbSet.Where(criteriaExpr).ToListAsync()
            );
        }

        public async override Task<ISingleObjectResult<TDomainObject>> GetByIdAsync(TDomainObjectId id)
        {
            return new SingleObjectResult<TDomainObject>
            (
                "Ok",
                await DbSet.FindAsync(id)
            );
        }

        public override Task<IBasicResult> RemoveAsync(TDomainObject domainObject)
        {
            DbSet.Remove(domainObject);

            return Task.FromResult<IBasicResult>(new BasicResult("OK"));
        }

        protected override Task OnAddAsync(TDomainObject domainObject)
        {
            DbSet.Add(domainObject);

            return Task.FromResult<IBasicResult>(new BasicResult("OK"));
        }

        protected override Task OnUpdateAsync(TDomainObject domainObject)
        {
            return Task.FromResult(true);
        }
    }
}
