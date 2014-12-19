using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.data.TimeRecordingSystem.Models;

namespace totalhr.services.Infrastructure
{
    public interface ITimeRecordingServices
    {
        bool RecordTimeForUser(long id,int userId, DateTime startTime, DateTime endTime, Audit audit);
        List<TimeRecording> Search(DateTime startDate, DateTime endDate, int skip, int take);
        TimeRecording GetById(Int64 id);
    }
}
