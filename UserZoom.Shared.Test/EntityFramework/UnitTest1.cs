using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserZoom.Domain;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Shared.Patterns.Repository;
using Castle.Windsor;

namespace UserZoom.Shared.Test.EntityFramework
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            IWindsorContainer container = Container.Configure();

            UZTask createdTask;

            using (TaskContext taskContext = new TaskContext())
            {
                createdTask = new UZTask();
                createdTask.Id = Guid.NewGuid();
                createdTask.Title = "Test title";
                
                taskContext.Tasks.Add(createdTask);

                await taskContext.SaveChangesAsync();
            }

            using (IDomainUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>> unitOfWork = container.Resolve<IDomainUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>>>())
            {
                var task = await unitOfWork.Repository.GetByIdAsync(createdTask.Id);
             

                task.Object.Title = "Changed title";

                await unitOfWork.CommitAsync();
            }

            using (TaskContext taskContext = new TaskContext())
            {
                taskContext.Database.Log = t => Debug.WriteLine(t);

                List<string> items = new List<string> { "Changed title" };
                IQueryable<UZTask> tasks = taskContext.Tasks.Where(t => items.Contains(t.Title));
                


               /* UZTask task1 = tasks.First();
                task1.Title = "Change title 2";*/

                await taskContext.SaveChangesAsync();
            }
        }
    }
}
