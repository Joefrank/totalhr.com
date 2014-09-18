using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.data.Repositories.Infrastructure.TimeRecording
{
    interface ITimeRecordingRepository
    {
        void StartTime(int userId, DateTime startTime, int typeId, string note, int? taskRefId);
    }
}
