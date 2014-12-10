using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.Models;

namespace totalhr.data.TimeRecordingSystem.EF
{
    public partial class TimeRecording
    {
        public TimeRecording() { }
        public TimeRecording(int userId, DateTime startTime, DateTime endTime, Audit audit)
        {
           UserId = userId;
                StartTime = startTime;
                EndTime = endTime;
                TypeId = 1;
                FillAudit(audit);
        }
        public void FillAudit(Audit audit)
        {
            AddedDate = audit.DateAdded;
            UpdatedDate = audit.DateUpdated;
            AddedById = audit.AddedByUserId;
            UpdatedById = audit.UpdatedByUserId;
        
        }
    }
}
