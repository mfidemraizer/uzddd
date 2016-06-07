using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Http
{
    internal class SampleMiddleware : MiddlewareBase
    {
        private static bool _initialized = false;
        private static readonly object _syncLock = new object();
        private readonly Stopwatch _clock = new Stopwatch();

        public SampleMiddleware(OwinMiddleware middleware)
            : base(middleware)
        {

        }

        protected override Task OnInitAsync(IOwinContext context)
        {
            return base.OnInitAsync(context);
        }

        protected override Task OnEnter(IOwinContext context)
        {
            context.Environment.Add("Stopwatch", true);
            _clock.Start();

            return base.OnEnter(context);
        }

        protected override Task OnExit(IOwinContext context)
        {
            _clock.Stop();

            context.Response.Headers.Add("X-Request-Duration", new[] { _clock.ElapsedMilliseconds.ToString() });

            return base.OnExit(context);
        }
    }
}
