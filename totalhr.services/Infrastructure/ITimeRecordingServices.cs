using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.Models;

namespace totalhr.services.Infrastructure
{
    public interface ITimeRecordingServices
    {
        bool RecordTimeForUser(int userId, int companyId, DateTime startTime, DateTime endTime, Audit audit);
    }
}
