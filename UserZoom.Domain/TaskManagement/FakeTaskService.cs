using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Shared.Patterns.AccumulatedResult;

namespace UserZoom.Domain.TaskManagement
{
    public class FakeTaskService : ITaskService
    {
        public Task<IBasicResult> AddAsync(UZTask task)
        {
            throw new NotImplementedException();
        }

        public Task<IBasicResult> ChangeTitleAsync(Guid taskId, string title)
        {
            throw new NotImplementedException();
        }
    }
}
