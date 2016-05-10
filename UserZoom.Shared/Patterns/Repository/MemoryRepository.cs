using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Shared.Patterns.Repository
{
    public class MemoryRepository<TDomainObjectId, TDomainObject> : IRepository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        public MemoryRepository(IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
        {

        }

        private HashSet<TDomainObject> Storage { get; } = new HashSet<TDomainObject>();

        public Task AddOrUpdateAsync(TDomainObject domainObject)
        {
            if (domainObject.IsDirty)
            {
                // TODO: Asignar id
                // domainObject.Id = Guid.NewGuid();
                Contract.Assert(Storage.Add(domainObject));
            }
            else
            {
                Contract.Assert(Storage.Remove(domainObject));
                Contract.Assert(Storage.Add(domainObject));
            }

            return Task.FromResult(true);
        }

        public Task<TDomainObject> GetByIdAsync(TDomainObjectId id)
        {
            TDomainObject domainObject = Storage.SingleOrDefault(o => o.Id.Equals(id));
            Contract.Assert(domainObject != null);

            return Task.FromResult(domainObject);
        }

        public Task RemoveAsync(TDomainObject domainObject)
        {
            Contract.Assert(Storage.Remove(domainObject));

            return Task.FromResult(true);
        }
    }
}
