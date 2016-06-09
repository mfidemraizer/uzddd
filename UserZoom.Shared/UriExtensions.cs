using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared
{
    public static class UriExtensions
    {
        public static Uri ToUri(this string some) => new Uri(some);
    }
}
