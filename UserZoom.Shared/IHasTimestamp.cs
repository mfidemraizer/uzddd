using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared
{
    public interface IHasTimestamp
    {
        DateTimeOffset DateAdded { set; }
    }
}
