using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using System;
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
                        .ImplementedBy<EFUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>, TaskContext>>(),

                //Component.For<TaskContext>().LifestyleTransient(),

                //Repositories
                Component.For<IRepository<Guid, UZTask>>()
                        .UsingFactoryMethod
                        (
                             () =>
                             {
                                 TaskContext dbContext = new TaskContext();

                                 EFRepository<Guid, UZTask> repo = new EFRepository<Guid, UZTask>
                                 (
                                     dbContext.Tasks,
                                     container.Resolve<IIdGenerator<Guid>>(),
                                     container.ResolveAll<ISpecification<Guid, UZTask>>()
                                   );

                                 return repo;
                             }
                        )
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
