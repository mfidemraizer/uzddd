using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHost
{
    public sealed class Service
    {
        private IDisposable App { get; set; }
        public void OnStart()
        {
            App = WebApp.Start<Startup>("http://localhost:8080");
        }

        public void OnStop()
        {
            if (App != null)
                App.Dispose();
        }
    }
}
