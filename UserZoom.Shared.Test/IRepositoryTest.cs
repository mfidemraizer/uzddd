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
            List<ISpecification<Guid, UZTask>> specs = new List<ISpecification<Guid, UZTask>>();
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
