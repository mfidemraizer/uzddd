using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.AccumulatedResult
{
    public interface IBasicResult
    {
        bool IsSuccessful { get; }
        string Description { get; }
        IDictionary<string, string> AffectedResources { get; }
    }
}
