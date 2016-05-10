using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.Specification
{
    public interface ISpecification<TId, TObject>
        where TId : IEquatable<TId>
        where TObject : ICanBeIdentifiable<TId>
    {
        IImmutableDictionary<string, string> BrokenRules { get; }
        Task<bool> IsSatisfiedByAsync(TObject someObject);
    }
}
