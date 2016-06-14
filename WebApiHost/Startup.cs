using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using UserZoom.Shared.Http;
using WebApiHost.Controllers;

namespace WebApiHost
{
    public sealed class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            IWindsorContainer container = Container.Configure();

            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            // Filters
            //config.Filters.Add(new CustomFilterAttribute());

            ModelStateFilterAttribute.Config modelStateConfig = new ModelStateFilterAttribute.Config();
            modelStateConfig.ActionWhiteList.Add
            (
                ctrl => ((TaskController)ctrl).CreateAsync(null)
            );
            config.Filters.Add(new ModelStateFilterAttribute(modelStateConfig));

            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            config.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(container));

            // MIDDLEWARE
            appBuilder.UseSample();
            appBuilder.UseErrorPage();
            appBuilder.UseWebApi(config);
        }
    }
}
