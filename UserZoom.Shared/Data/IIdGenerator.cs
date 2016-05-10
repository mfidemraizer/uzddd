using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Data
{
    public interface IIdGenerator<TId>
        where TId : IEquatable<TId>
    {
        TId Generate();
    }
}