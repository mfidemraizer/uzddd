using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserZoom.Domain;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace UserZoom.Shared.Test.EntityFramework
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            UZTask createdTask;

            using (TaskContext taskContext = new TaskContext())
            {
                createdTask = new UZTask();
                createdTask.Id = Guid.NewGuid();
                createdTask.Title = "Test title";
                
                taskContext.Tasks.Add(createdTask);

                await taskContext.SaveChangesAsync();
            }

            using (TaskContext taskContext = new TaskContext())
            {
                UZTask task = await taskContext.Tasks.FindAsync(Guid.NewGuid());
                task.Title = "Changed title";

                await taskContext.SaveChangesAsync();
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
