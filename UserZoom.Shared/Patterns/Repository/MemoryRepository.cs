using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
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

        public override Task<TDomainObject> GetByIdAsync(TDomainObjectId id)
        {
            return Task.FromResult(Storage.Single(o => o.Id.Equals(id)));
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
        
        public override Task RemoveAsync(TDomainObject domainObject)
        {
            Contract.Assert(Storage.Remove(domainObject));

            return Task.FromResult(true);
        }
    }
}
