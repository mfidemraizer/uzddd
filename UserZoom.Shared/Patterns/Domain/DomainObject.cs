using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Patterns.Domain
{
    public abstract class DomainObject : ICanBeIdentifiable<Guid>, ICanPerformDirtyChecking
    {
        public virtual Guid Id { get; set; }

        public bool IsDirty => Id == Guid.Empty;

        public virtual bool Equals(ICanBeIdentifiable<Guid> other)
        {
            return other != null && other.Id == Id;
        }
    }
}
