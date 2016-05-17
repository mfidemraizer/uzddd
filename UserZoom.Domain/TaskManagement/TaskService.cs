using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Collections.Generic;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Repository;

namespace UserZoom.Domain.TaskManagement
{
    public sealed class TaskService : ITaskService
    {
        public TaskService(IRepository<Guid, UZTask> repository)
        {
            Contract.Requires(repository != null);

            Repository = repository;
        }

        public IRepository<Guid, UZTask> Repository { get; }

        public Task<IBasicResult> AddAsync(UZTask task)
        {
            return Repository.AddOrUpdateAsync(task);
        }

        public async Task<IBasicResult> ChangeTitleAsync(Guid taskId, string title)
        {
            ISingleObjectResult<UZTask> getResult = (SingleObjectResult<UZTask>)await Repository.GetByIdAsync(taskId);
            getResult.Object.Title = title;

            if (getResult.IsSuccessful)
            {
                IBasicResult updateResult = await Repository.AddOrUpdateAsync(getResult.Object);

                updateResult.AffectedResources.AddRange(getResult.AffectedResources);
            }

            return getResult;
        }
    }
}
