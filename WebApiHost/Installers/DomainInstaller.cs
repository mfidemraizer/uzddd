using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Reflection;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Shared.Patterns.Repository;
using UserZoom.Shared.Patterns.UnitOfWork;
using System.Data.Entity;

namespace WebApiHost.Installers
{
    public class DomainInstaller : IWindsorInstaller
    {
        public string[] AssemblyNames { get; }

        public DomainInstaller(params string[] assemblyNames)
        {
            AssemblyNames = assemblyNames;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IEnumerable<Type> allTypes = AssemblyNames.Select(name => Assembly.Load(name))
                                                        .SelectMany(assembly => assembly.GetTypes());

            IEnumerable<Type> domainObjectTypes = allTypes.Where(t => t.Namespace.Contains(".Domain") && typeof(DomainObject).IsAssignableFrom(t));

            IEnumerable<Type> dbContexts = allTypes.Where
            (
                domainObjectType => typeof(DbContext).IsAssignableFrom(domainObjectType)
                && domainObjectType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Any
                (
                    contextType => contextType.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) 
                                    && contextType.PropertyType.GetGenericArguments()[0] == domainObjectType
                )
            );

            Type repoType = typeof(IRepository<,>);
            Type unitOfWorkType = typeof(IDomainUnitOfWork<,,>);
            Type concreteRepoType = typeof(EFRepository<,>);
            Type concreteUnitOfWork = typeof(EFUnitOfWork<,,,>);

            List<IRegistration> registrations = new List<IRegistration>();

            foreach (Type domainObjectType in domainObjectTypes)
            {
                Type domainRepo = repoType.MakeGenericType(typeof(Guid), domainObjectType);
                Type concreteDomainRepo = concreteRepoType.MakeGenericType(typeof(Guid), domainObjectType);

                registrations.Add
                (
                    Component.For(domainRepo)
                                .ImplementedBy(concreteRepoType.MakeGenericType(typeof(Guid), domainObjectType))
                                .LifestyleTransient()
                );

                registrations.Add
                (
                    Component.For(unitOfWorkType.MakeGenericType(typeof(Guid), domainObjectType, domainRepo))
                                .ImplementedBy(concreteUnitOfWork.MakeGenericType(typeof(Guid), domainObjectType, domainRepo, null))
                                .LifestyleTransient()
                );
            }
        }
    }
}
