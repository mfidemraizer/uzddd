using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Repository;

namespace UserZoom.Domain.AccountManagement
{
    public interface IAccountRepository : IRepository<Guid, Account>
    {
        Task<IMultipleObjectResult<ICollection<Account>, Account>> ListTopTen();
        Task<IMultipleObjectResult<ICollection<Account>, Account>> ListTopTenActiveAsync();
    }
}
