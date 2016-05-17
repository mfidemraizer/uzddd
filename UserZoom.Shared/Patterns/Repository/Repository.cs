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
    public abstract class Repository<TDomainObjectId, TDomainObject> : IRepository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        public Repository(IIdGenerator<TDomainObjectId> idGenerator, IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> specs)
        {
            IdGenerator = idGenerator;
            Specs = specs;
        }

        public IIdGenerator<TDomainObjectId> IdGenerator { get; }

        private IEnumerable<ISpecification<TDomainObjectId, TDomainObject>> Specs { get; }

        public async Task AddOrUpdateAsync(TDomainObject domainObject)
        {
            if (domainObject.IsDirty)
            {
                var failedSpecs = await Specs.GetAddSpecs().RunSpecsAsync(domainObject);

                if (failedSpecs.Count() == 0)
                {
                    domainObject.Id = IdGenerator.Generate();
                    await OnAddAsync(domainObject);
                }
            }
            else
            {
                var failedSpecs = await Specs.GetUpdateSpecs().RunSpecsAsync(domainObject);

                if (failedSpecs.Count() == 0)
                {
                    await OnUpdateAsync(domainObject);
                }
            }
        }

        protected abstract Task OnAddAsync(TDomainObject domainObject);
        protected abstract Task OnUpdateAsync(TDomainObject domainObject);

        public abstract Task<TDomainObject> GetByIdAsync(TDomainObjectId id);
        public abstract Task RemoveAsync(TDomainObject domainObject);
    }
}
