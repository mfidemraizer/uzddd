using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.AccumulatedResult
{
    public class BasicResult : IBasicResult
    {
        public BasicResult(string description)
        {
            Description = description;
        }

        public IDictionary<string, string> AffectedResources { get; } = new Dictionary<string, string>();

        public string Description { get; }

        public bool IsSuccessful => AffectedResources.Count == 0;
    }
}
