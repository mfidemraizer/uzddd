using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Domain;
using UserZoom.Domain.TaskManagement.Specs;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.Repository;
using UserZoom.Shared.Patterns.Specification;

namespace UserZoom.Shared.Test
{
    [TestClass]
    public class IRepositoryTest
    {
        [TestMethod]
        public async Task CanAddDomainObjects()
        {
            WindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register
            (
                // Repositories
                Component.For<IRepository<Guid, UZTask>>()
                        .ImplementedBy<MemoryRepository<Guid, UZTask>>()
                        .LifestyleTransient(),

                // Id generators
                Component.For<IIdGenerator<Guid>>()
                        .ImplementedBy<GuidIdGenerator>(),
                
                // Specs
                Component.For<ISpecification<Guid, UZTask>>()
                         .ImplementedBy<AddOrUpdateTaskSpec>()
                         .LifestyleTransient()
            );
            
            IRepository<Guid, UZTask> repo2 = container.Resolve<IRepository<Guid, UZTask>>();

               List <ISpecification<Guid, UZTask>> specs = new List<ISpecification<Guid, UZTask>>();
            specs.Add(new AddOrUpdateTaskSpec());

            IRepository<Guid, UZTask> repo =
                new MemoryRepository<Guid, UZTask>(new GuidIdGenerator(), specs);

            UZTask testTask = new UZTask();
            await repo.AddOrUpdateAsync(testTask);

            UZTask gotTask = await repo.GetByIdAsync(testTask.Id);

            Assert.IsFalse(gotTask.IsDirty);
            Assert.AreEqual(testTask, gotTask);
        }
    }
}
