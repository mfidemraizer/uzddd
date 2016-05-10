using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Data
{
    public sealed class GuidIdGenerator : IIdGenerator<Guid>
    {
        public Guid Generate() => Guid.NewGuid();
    }
}
