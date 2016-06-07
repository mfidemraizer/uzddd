using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Http
{
    public sealed class SampleMiddleware2
    {
        public Task Invoke(IOwinContext context, Func<Task> next)
        {
            return next();
        }
    }
}
