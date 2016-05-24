using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.UnitOfWork;

namespace UserZoom.Shared.IO
{
    public sealed class FileSystemUnitOfWork : IUnitOfWork
    {
        private ConcurrentQueue<Action> ActionQueue { get; set; } = new ConcurrentQueue<Action>();

        public void WriteFile(string fileName, string contents)
        {
            ActionQueue.Enqueue(() => File.WriteAllText(fileName, contents));
        }

        public Task CommitAsync()
        {
            return Task.Run
            (
                () =>
                {
                    Action currentAction;
                    
                    while (ActionQueue.TryDequeue(out currentAction))
                    {
                        currentAction();
                    }
                }
            );
        }

        public void Dispose()
        {
            RollbackAsync().Wait();
        }

        public Task RollbackAsync()
        {
            ActionQueue = new ConcurrentQueue<Action>();

            return Task.FromResult(true);
        }
    }
}
