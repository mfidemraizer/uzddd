using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Collections.Generic;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Shared.Patterns.Repository;

namespace UserZoom.Domain.TaskManagement
{
    public sealed class TaskService : ITaskService
    {
        public TaskService(IDomainUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>> unitOfWork)
        {
            Contract.Requires(unitOfWork != null);

            UnitOfWork = unitOfWork;
        }

        public IDomainUnitOfWork<Guid, UZTask, IRepository<Guid, UZTask>> UnitOfWork { get; }

        public Task<IBasicResult> AddAsync(UZTask task)
        {
            return UnitOfWork.Repository.AddOrUpdateAsync(task);
        }

        public async Task<IBasicResult> ChangeTitleAsync(Guid taskId, string title)
        {
            ISingleObjectResult<UZTask> getResult = (SingleObjectResult<UZTask>)await UnitOfWork.Repository.GetByIdAsync(taskId);
            getResult.Object.Title = title;

            if (getResult.IsSuccessful)
            {
                IBasicResult updateResult = await UnitOfWork.Repository.AddOrUpdateAsync(getResult.Object);

                updateResult.AffectedResources.AddRange(getResult.AffectedResources);
            }

            return getResult;
        }
    }
}
