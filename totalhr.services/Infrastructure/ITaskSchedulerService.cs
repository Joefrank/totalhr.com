using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.services.Infrastructure
{
    public interface ITaskSchedulerService
    {
        bool AddTask(string name, string description, int departmentId, bool needsApproval, int assignedBy, int assignedTo, DateTime? completeBy, Audit audit);
    }
}
