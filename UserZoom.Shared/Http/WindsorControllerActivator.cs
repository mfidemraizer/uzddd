using Castle.Windsor;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace UserZoom.Shared.Http
{
    public sealed class WindsorControllerActivator : IHttpControllerActivator
    {
        public WindsorControllerActivator(IWindsorContainer container)
        {
            Container = container;
        }

        private IWindsorContainer Container { get; }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController)Container.Resolve(controllerType);
        }
    }
}
