using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Shared.Patterns.Repository
{
    public class MemoryRepository<TDomainObjectId, TDomainObject> : Repository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        public MemoryRepository(IIdGenerator<TDomainObjectId> idGenerator, IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
            : base(idGenerator, specs)
        {
        }
        
        private HashSet<TDomainObject> Storage { get; } = new HashSet<TDomainObject>();

        public override Task<ISingleObjectResult<TDomainObject>> GetByIdAsync(TDomainObjectId id)
        {
            SingleObjectResult<TDomainObject> result = new SingleObjectResult<TDomainObject>
            (
                "Operation done",
                Storage.Single(o => o.Id.Equals(id))
            );

            return Task.FromResult<ISingleObjectResult<TDomainObject>>(result);
        }

        protected override Task OnAddAsync(TDomainObject domainObject)
        {
            Contract.Assert(Storage.Add(domainObject));

            return Task.FromResult(true);
        }

        protected override Task OnUpdateAsync(TDomainObject domainObject)
        {
            Contract.Assert(Storage.Remove(domainObject));
            Contract.Assert(Storage.Add(domainObject));

            return Task.FromResult(true);
        }
        
        public override Task<IBasicResult> RemoveAsync(TDomainObject domainObject)
        {
            Contract.Assert(Storage.Remove(domainObject));

            return Task.FromResult<IBasicResult>(new BasicResult("Ok"));
        }

        public override Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Expression<Func<TDomainObject, bool>> criteriaExpr, long from = 0, int count = 10)
        {
            throw new NotImplementedException();
        }

        protected override Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> queryFunc)
        {
            throw new NotImplementedException();
        }
    }
}
