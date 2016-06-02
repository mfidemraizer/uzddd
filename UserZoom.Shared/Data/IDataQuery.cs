using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.AccumulatedResult;

namespace UserZoom.Shared.Data
{
    public interface IDataQuery<TObject>
    {
        Task<IMultipleObjectResult<ICollection<TObject>, TObject>> GetByCriteria(Func<IQueryable<TObject>, IQueryable<TObject>> queryFunc);
    }
}
