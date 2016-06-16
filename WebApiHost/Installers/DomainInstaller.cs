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

            IEnumerable<Type> dbContextTypes = allTypes.Where(t => t.Namespace.Contains(".Domain") && typeof(DbContext).IsAssignableFrom(t));

            IDictionary<Type, Type> dbSetHash = dbContextTypes.SelectMany
            (
                dbContexType => dbContexType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                                            .Where(property => property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            ).ToDictionary
            (
                    dbSet => dbSet.PropertyType.GetGenericArguments()[0],
                    dbSet => dbSet.DeclaringType
            );

            IEnumerable<Type> domainObjectTypes = allTypes.Where(t => t.Namespace.Contains(".Domain") && dbSetHash.ContainsKey(t) && typeof(DomainObject).IsAssignableFrom(t));

            Type repoType = typeof(IRepository<,>);
            Type unitOfWorkType = typeof(IDomainUnitOfWork<,,>);
            Type concreteRepoType = typeof(EFRepository<,,>);
            Type concreteUnitOfWork = typeof(EFUnitOfWork<,,,>);

            List<IRegistration> registrations = new List<IRegistration>();

            foreach (Type domainObjectType in domainObjectTypes)
            {
                Type domainRepo = repoType.MakeGenericType(typeof(Guid), domainObjectType);
                Type concreteDomainRepo = concreteRepoType.MakeGenericType(typeof(Guid), domainObjectType, dbSetHash[domainObjectType]);

                registrations.Add
                (
                    Component.For(domainRepo)
                                .ImplementedBy(concreteDomainRepo)
                                .LifestyleTransient()
                );

                registrations.Add
                (
                    Component.For(unitOfWorkType.MakeGenericType(typeof(Guid), domainObjectType, domainRepo))
                                .ImplementedBy(concreteUnitOfWork.MakeGenericType(typeof(Guid), domainObjectType, domainRepo, dbSetHash[domainObjectType]))
                                .LifestyleTransient()
                );
            }

            container.Register(registrations.ToArray());

            container.Register
            (
                Classes.FromAssembly(Assembly.Load("UserZoom.Domain"))
                        .Where
                        (
                            type => type.Namespace.Contains(".Domain") 
                                    && !type.Name.StartsWith("Fake") 
                                    && type.Name.EndsWith("Service")
                        ).WithServiceFirstInterface()
            );
        }
    }
}
