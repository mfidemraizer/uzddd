//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UserZoom.Shared.Data;

//namespace UserZoom.Shared.Patterns.Repository
//{
//    [ContractClassFor(typeof(IRepository<,>))]
//    public abstract class IRepositoryContract<TDomainObjectId, TDomainObject> : IRepository<TDomainObjectId, TDomainObject>
//        where TDomainObjectId : IEquatable<TDomainObjectId>
//        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
//    {
//        public IIdGenerator<TDomainObjectId> IdGenerator
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public Task AddOrUpdateAsync(TDomainObject domainObject)
//        {
//            Contract.Requires(domainObject != null);
//            Contract.Ensures(Contract.Result<Task>() != null);

//            throw new NotImplementedException();
//        }

//        public Task<TDomainObject> GetByIdAsync(TDomainObjectId id)
//        {
//            Contract.Requires(id != null);
//            Contract.Ensures(Contract.Result<Task<TDomainObject>>() != null);

//            throw new NotImplementedException();
//        }

//        public Task RemoveAsync(TDomainObject domainObject)
//        {
//            Contract.Requires(domainObject != null);
//            Contract.Ensures(Contract.Result<Task>() != null);

//            throw new NotImplementedException();
//        }
//    }
//}
