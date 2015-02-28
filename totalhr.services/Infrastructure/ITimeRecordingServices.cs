using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.services.Infrastructure
{
    public interface ITimeRecordingServices
    {
        bool RecordTimeForUser(long id,int userId, DateTime startTime, DateTime endTime, Int16 typeId, Int32? taskRef, Audit audit);
        List<TimeRecording> Search(DateTime startDate, DateTime endDate, int userId, int skip, int take);
        TimeRecording GetById(Int64 id);
    }
}
