using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;
using TRS = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.services.Infrastructure
{
    public interface ITaskSchedulerService
    {
        List<TRS.TaskScheduler> ListBySearch(int? id, string name, int? assignedTo, int? assignedBy, int skip, int take);
        bool AddTask(string name, string description, int departmentId, bool needsApproval, int assignedBy, int assignedTo, DateTime? completeBy, Audit audit);
    }
}
