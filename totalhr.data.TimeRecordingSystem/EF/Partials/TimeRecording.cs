using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.data.TimeRecordingSystem.EF
{
    public partial class TimeRecording
    {
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
            this.Audit = audit;
        }




    }
}
