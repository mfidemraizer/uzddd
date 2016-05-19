using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Shared.Patterns.Repository
{
    public abstract class Repository<TDomainObjectId, TDomainObject> : IDataHandler<TDomainObject>, IRepository<TDomainObjectId, TDomainObject>
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

        public async Task<IBasicResult> AddOrUpdateAsync(TDomainObject domainObject)
        {
            if (domainObject.IsDirty)
            {
                var failedSpecs = await Specs.GetAddSpecs().RunSpecsAsync(domainObject);

                if (failedSpecs.Count() == 0)
                {
                    domainObject.Id = IdGenerator.Generate();

                    IHasTimestamp withTimestamp = domainObject as IHasTimestamp;

                    if (withTimestamp != null)
                    {
                        withTimestamp.DateAdded = DateTimeOffset.Now;
                    }

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

            return null;
        }

        protected abstract Task OnAddAsync(TDomainObject domainObject);
        protected abstract Task OnUpdateAsync(TDomainObject domainObject);

        public abstract Task<ISingleObjectResult<TDomainObject>> GetByIdAsync(TDomainObjectId id);
        public abstract Task<IBasicResult> RemoveAsync(TDomainObject domainObject);

        Task IDataHandler<TDomainObject>.OnAddAsync(TDomainObject domainObject)
        {
            return OnAddAsync(domainObject);
        }

        Task IDataHandler<TDomainObject>.OnUpdateAsync(TDomainObject domainObject)
        {
            return OnUpdateAsync(domainObject);
        }
    }
}
