using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.data.TimeRecordingSystem.EF
{
    public partial class TimeRecording
    {
        public TimeRecording(int userId, DateTime startTime, DateTime endTime, Int16 typeId, Int32? taskRef, Audit audit)
        {
            this.Build(userId, startTime, endTime,typeId,taskRef, audit);  
        }

        public void Build(int userId, DateTime startTime, DateTime endTime, Int16 typeId, Int32? taskRef, Audit audit)
        {
            UserId = userId;
            StartTime = startTime;
            EndTime = endTime;
            TypeId = typeId;
            TaskRefId = taskRef;
            this.Audit = audit;
        }




    }
}
