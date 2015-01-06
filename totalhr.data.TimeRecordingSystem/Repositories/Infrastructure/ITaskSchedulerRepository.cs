using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.Repositories.Infrastructure;
using TRS = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.data.TimeRecordingSystem.Repositories.Infrastructure
{
    public interface ITaskSchedulerRepository : IGenericRepository<TRS.TaskScheduler>
    {

    }
}
