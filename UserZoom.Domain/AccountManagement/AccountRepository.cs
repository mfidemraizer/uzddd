using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
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

        public override Task<IMultipleObjectResult<ICollection<Account>, Account>> GetByCriteria(Expression<Func<Account, bool>> criteriaExpr, long from = 0, int count = 10)
        {
            return Repository.GetByCriteria(criteriaExpr, from, count);
        }

        protected override Task<IMultipleObjectResult<ICollection<Account>, Account>> GetByCriteria(Func<IQueryable<Account>, IQueryable<Account>> queryFunc)
        {
            IDataQuery<Account> dataQuery = Repository as IDataQuery<Account>;

            Contract.Assert(dataQuery != null);

            return dataQuery.GetByCriteria(queryFunc: queryFunc);
        }
    }
}
