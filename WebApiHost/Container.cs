using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using System;
using System.Data.Entity;
using System.Web.Http;
using UserZoom.Domain;
using UserZoom.Domain.TaskManagement;
using UserZoom.Domain.TaskManagement.Specs;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Shared.Patterns.Repository;
using UserZoom.Shared.Patterns.Specification;
using UserZoom.Shared.Patterns.UnitOfWork;
using WebApiHost.Controllers;

namespace WebApiHost
{
    public static class Container
    {
        public static IWindsorContainer Configure()
        {
            WindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register
            (
                Component.For<EchoController>().ImplementedBy<EchoController>().LifestyleTransient(),

                Component.For<ITaskService>().ImplementedBy<TaskService>(),

                // Units of work
                Component.For<IDomainUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>>>()
                        .ImplementedBy<EFUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>, TaskContext>>()
                        .DependsOn(Dependency.OnComponent<DbContext, TaskContext>()),

                Component.For<TaskContext>().LifestyleBoundTo<ApiController>(),

                // DbContexts

                //Component.For<DbContext>()
                //        .ImplementedBy<TaskContext>()
                //        .LifestyleBoundTo<ApiController>(),

                //Repositories
                Component.For<IRepository<Guid, UZTask>>()
                            .ImplementedBy<EFRepository<Guid, UZTask>>()
                            .DependsOn(Dependency.OnComponent<DbContext, TaskContext>())
                            .LifestyleTransient(),

                // Id generators
                Component.For<IIdGenerator<Guid>>()
                        .ImplementedBy<GuidIdGenerator>(),

                // Specs
                Component.For<ISpecification<Guid, UZTask>>()
                         .ImplementedBy<AddOrUpdateTaskSpec>()
                         .LifestyleTransient()
            );

            return container;
        }
    }
}
