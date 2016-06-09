using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.AccumulatedResult
{
    public interface ISingleObjectResult<out TObject> : IBasicResult
    {
        TObject Object { get; }
    }
}
