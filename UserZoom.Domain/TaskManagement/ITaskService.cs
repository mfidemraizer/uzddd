using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.AccumulatedResult;
using UserZoom.Shared.Patterns.Repository;

namespace UserZoom.Domain.TaskManagement
{
    public interface ITaskService
    {
        Task<IBasicResult> AddAsync(UZTask task);
        Task<IBasicResult> ChangeTitleAsync(Guid taskId, string title);
    }
}
