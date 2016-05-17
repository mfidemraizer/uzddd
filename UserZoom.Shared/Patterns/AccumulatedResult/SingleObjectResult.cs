using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.AccumulatedResult
{
    public class SingleObjectResult<TObject> : BasicResult, ISingleObjectResult<TObject>
    {
        public SingleObjectResult(string description, TObject someObject)
            : base(description)
        {
            Object = someObject;
        }

        public TObject Object { get; }
    }
}
