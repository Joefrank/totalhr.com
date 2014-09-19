using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.Repositories.Infrastructure.TimeRecording;

namespace totalhr.data.Repositories.Implementation.TimeRecording
{
    public class TimeRecordingRepository: ITimeRecordingRepository
    {
        public void StartTime(int userId, DateTime startTime, int typeId, string note, int? taskRefId)
        {
            throw new NotImplementedException();
        }
        
    }
}
