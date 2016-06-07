using Microsoft.Owin;
using System.Threading;
using System.Threading.Tasks;

namespace UserZoom.Shared.Http
{
    public abstract class MiddlewareBase : OwinMiddleware
    {
        private static bool _initialized = false;
        private static readonly AutoResetEvent _resetEvent = new AutoResetEvent(true);

        public MiddlewareBase(OwinMiddleware middleware)
            : base(middleware)
        {

        }

        protected virtual Task OnInitAsync(IOwinContext context)
        {
            return Task.FromResult(true);
        }

        protected virtual Task OnEnter(IOwinContext context)
        {
            return Task.FromResult(true);
        }

        protected virtual Task OnExit(IOwinContext context)
        {
            return Task.FromResult(true);
        }

        public async override Task Invoke(IOwinContext context)
        {
            if (!_initialized)
            {
                _resetEvent.WaitOne();

                try
                {
                    if (!_initialized)
                    {
                        await OnInitAsync(context);
                        _initialized = true;
                    }
                }
                finally
                {
                    _resetEvent.Set();
                }
            }

            await OnEnter(context);
            await Next.Invoke(context);
            await OnExit(context); 
        }
    }
}
