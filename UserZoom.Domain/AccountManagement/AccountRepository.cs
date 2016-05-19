using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Repository;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Domain.AccountManagement
{
    public sealed class AccountRepository : Repository<Guid, Account>, IAccountRepository
    {
        private IRepository<Guid, Account> Repository { get; }

        public AccountRepository(IIdGenerator<Guid> idGenerator, IEnumerable<ISpecification<Guid, Account>> specs, IRepository<Guid, Account> repository)
            : base(idGenerator, specs)
        {
            Repository = repository;
        }

        public override Task<ISingleObjectResult<Account>> GetByIdAsync(Guid id)
        {
            return Repository.GetByIdAsync(id);
        }

        public override Task<IBasicResult> RemoveAsync(Account domainObject)
        {
            return Repository.RemoveAsync(domainObject);
        }

        protected override Task OnAddAsync(Account domainObject)
        {
            IDataHandler<Account> dataHandler = Repository as IDataHandler<Account>;

            Contract.Assert(dataHandler != null);

            return dataHandler.OnAddAsync(domainObject);
        }

        protected override Task OnUpdateAsync(Account domainObject)
        {
            IDataHandler<Account> dataHandler = Repository as IDataHandler<Account>;

            Contract.Assert(dataHandler != null);

            return dataHandler.OnUpdateAsync(domainObject);
        }

        public Task<IMultipleObjectResult<IList<Account>, Account>> ListTopTen()
        {
            throw new NotImplementedException();
        }

        public Task<IMultipleObjectResult<IList<Account>, Account>> ListTopTenActiveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
