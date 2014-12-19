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
            this.Build(userId, startTime, endTime, audit);  
        }

        public void Build(int userId, DateTime startTime, DateTime endTime, Audit audit)
        {
            UserId = userId;
            StartTime = startTime;
            EndTime = endTime;
            TypeId = 1;
            FillAudit(audit);
        }


        public void FillAudit(Audit audit)
        {
            if (Id == 0)
            {
                AddedDate = audit.DateAdded;
                AddedById = audit.AddedByUserId;
            }
            UpdatedDate = audit.DateUpdated;
            UpdatedById = audit.UpdatedByUserId;
        
        }


    }
}
