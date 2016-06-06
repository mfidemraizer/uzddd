using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
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
        public EFRepository(DbContext context, IIdGenerator<TDomainObjectId> idGenerator, IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
            : base(idGenerator, specs)
        {
            DbSet = context.Set<TDomainObject>();
        }

        public DbSet<TDomainObject> DbSet { get; }

        protected override async Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> queryFunc)
        {
            return new MultipleObjectResult<ICollection<TDomainObject>, TDomainObject>
            (
                "OK",
                 await queryFunc(DbSet).ToListAsync()
            );
        }

        public async override Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Expression<Func<TDomainObject, bool>> criteriaExpr, long from = 0, int count = 10)
        {
            Contract.Requires(count >= 10);

            return await GetByCriteria
            (
                queryable =>
                {
                    if (criteriaExpr != null)
                        queryable = queryable.Where(criteriaExpr);

                    if (from > 0)
                        queryable = queryable.Skip((int)from);

                    return queryable.Take(count);
                }
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
