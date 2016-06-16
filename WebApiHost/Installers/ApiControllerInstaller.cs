using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Reflection;
using System.Web.Http;

namespace WebApiHost.Installers
{
    public class ApiControllerInstaller : IWindsorInstaller
    {
        public string[] AssemblyNames { get; }

        public ApiControllerInstaller(params string[] assemblyNames)
        {
            AssemblyNames = assemblyNames;
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach(Assembly assembly in AssemblyNames.Select(name => Assembly.Load(name)))
            {
                container.Register
                (
                    assembly.GetTypes()
                                            .Where(t => typeof(ApiController).IsAssignableFrom(t))
                                            .Select(t => Component.For(t).LifestyleTransient())
                                            .ToArray()
                );
            }
        }
    }
}
