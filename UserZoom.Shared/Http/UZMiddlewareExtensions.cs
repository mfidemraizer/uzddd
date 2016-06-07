using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Http
{
    public static class UZMiddlewareExtensions
    {
        public static void UseSample(this IAppBuilder appBuilder)
        {
            appBuilder.Use<SampleMiddleware>();
        }

        public static void UseSample2(this IAppBuilder appBuilder)
        {
            SampleMiddleware2 sample2 = new SampleMiddleware2();
            
            appBuilder.Use(sample2.Invoke);
        }
    }
}
