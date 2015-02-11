using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.data.TimeRecordingSystem.EF
{
    public partial class TaskScheduler
    {

       public void Build(string name, string description, int departmentId, bool needsApproval, int assignedBy, int assignedTo, DateTime? completeBy, Audit audit)
        {
            this.Name = name;
            this.Description = description;
            this.DepartmentId = departmentId;
            this.ApprovalNeeded = needsApproval;
            this.AssignedBy = assignedBy;
            this.AssignedTo = assignedTo;
            this.ScheduledDateTime = completeBy;
            this.Audit = audit;
        }

    }
}
