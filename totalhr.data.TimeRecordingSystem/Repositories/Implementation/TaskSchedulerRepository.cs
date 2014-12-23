using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.Repositories.Implementation;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using TRS = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.data.TimeRecordingSystem.Repositories.Implementation
{
    public class TaskSchedulerRepository: GenericRepository<TimeRecordingEntities, TRS.TaskScheduler>, ITaskSchedulerRepository   
    {
    }
}
