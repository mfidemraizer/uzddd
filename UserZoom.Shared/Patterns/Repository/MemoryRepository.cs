using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Shared.Patterns.Repository
{
    public class MemoryRepository<TDomainObjectId, TDomainObject> : IRepository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        public MemoryRepository(IIdGenerator<TDomainObjectId> idGenerator, IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
        {
            IdGenerator = idGenerator;
            Specs = specs;
        }

        public IIdGenerator<TDomainObjectId> IdGenerator { get; }

        private IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> Specs { get; }
        private HashSet<TDomainObject> Storage { get; } = new HashSet<TDomainObject>();

        public async Task AddOrUpdateAsync(TDomainObject domainObject)
        {
            if (domainObject.IsDirty)
            {
                var failedSpecs = await Specs.GetAddSpecs().RunSpecsAsync(domainObject);
                
                if (failedSpecs.Count() == 0)
                {
                    domainObject.Id = IdGenerator.Generate();
                    Contract.Assert(Storage.Add(domainObject));
                }
            }
            else
            {
                var failedSpecs = await Specs.GetUpdateSpecs().RunSpecsAsync(domainObject);

                if (failedSpecs.Count() == 0)
                {
                    Contract.Assert(Storage.Remove(domainObject));
                    Contract.Assert(Storage.Add(domainObject));
                }
            }
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
