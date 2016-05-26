using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.Repository;
using UserZoom.Shared.Patterns.UnitOfWork;

namespace UserZoom.Shared.Patterns.Domain
{
    public interface IDomainUnitOfWork<TDomainObjectId, TDomainObject, TRepository> : IUnitOfWork
        where TDomainObjectId : IEquatable<TDomainObjectId>
        where TDomainObject : class, ICanBeIdentifiable<TDomainObjectId>, ICanPerformDirtyChecking
        where TRepository : IRepository<TDomainObjectId, TDomainObject>
    {
        TRepository Repository { get; }
    }
}
