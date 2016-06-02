using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.AccumulatedResult;

namespace UserZoom.Shared.Patterns.Repository
{
    //[ContractClass(typeof(IRepositoryContract<,>))]
    public interface IRepository<TDomainObjectId, TDomainObject>
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
    {
        IIdGenerator<TDomainObjectId> IdGenerator { get; }

        Task<ISingleObjectResult<TDomainObject>> GetByIdAsync(TDomainObjectId id);
        Task<IBasicResult> AddOrUpdateAsync(TDomainObject domainObject);
        Task<IBasicResult> RemoveAsync(TDomainObject domainObject);
        Task<IMultipleObjectResult<ICollection<TDomainObject>, TDomainObject>> GetByCriteria(Expression<Func<TDomainObject, bool>> criteriaExpr);
    }
}