using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.AccumulatedResult
{
    public interface IMultipleObjectResult<TCollection, TObject> : IBasicResult
        where TCollection : ICollection<TObject>
    {
        TCollection Objects { get; }
    }
}
