using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.Repository
{
    [ContractClass(typeof(IRepositoryContract<,>))]
    public interface IRepository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        Task<TDomainObject> GetByIdAsync(TDomainObjectId id);
        Task AddOrUpdateAsync(TDomainObject domainObject);
        Task RemoveAsync(TDomainObject domainObject);
    }
}