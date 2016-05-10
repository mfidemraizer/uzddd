using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.Repository;

namespace UserZoom.Shared.Test
{
    [TestClass]
    public class IRepositoryTest
    {
        [TestMethod]
        public async Task CanAddDomainObjects()
        {
            IRepository<Guid, UZTask> repo = new MemoryRepository<Guid, UZTask>();

            UZTask testTask = new UZTask();
            await repo.AddOrUpdateAsync(testTask);

            UZTask gotTask = await repo.GetByIdAsync(testTask.Id);

            Assert.IsFalse(gotTask.IsDirty);
            Assert.AreEqual(testTask, gotTask);
        }
    }
}
