using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.data.TimeRecordingSystem.Models;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using totalhr.services.Infrastructure;

namespace totalhr.services.Implementation
{
    public class TimeRecordingServices: ITimeRecordingServices
    {
        private ITimeRecordingRepository _timeRecordingRepository { get; set; }

        public TimeRecordingServices(ITimeRecordingRepository timeRecordingRepository)
        {
            _timeRecordingRepository = timeRecordingRepository;
        }

        public bool RecordTimeForUser(int userId, DateTime startTime, DateTime endTime, Audit audit)
        {
            var timeRecording = new TimeRecording(userId,startTime,endTime,audit);
            _timeRecordingRepository.Add(timeRecording);
            return true;
        }
    }
}
