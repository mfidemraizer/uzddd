using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.AccumulatedResult
{
    public class MultipleObjectResult<TCollection, TObject> : BasicResult, IMultipleObjectResult<TCollection, TObject>
        where TCollection : ICollection<TObject>
    {
        public MultipleObjectResult(string description, TCollection objects)
            : base(description)
        {
            Objects = objects;
        }

        public TCollection Objects { get; }
    }
}
