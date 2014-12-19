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
        private IAccountService _accountService { get; set; }

        public TimeRecordingServices(ITimeRecordingRepository timeRecordingRepository, IAccountService accountService)
        {
            _timeRecordingRepository = timeRecordingRepository;
            _accountService = accountService;
        }

        public bool RecordTimeForUser(long id, int userId, DateTime startTime, DateTime endTime, Audit audit)
        {
            //find if user exists
            if (_accountService.GetUser(userId) != null)
            {
                //record time
                if (id == 0)
                {
                    var timeRecording = new TimeRecording(userId, startTime, endTime, audit);
                    _timeRecordingRepository.Add(timeRecording);
                }
                else
                {
                    var timeRecording = this.GetById(id);
                    if (timeRecording != null)
                    {
                        timeRecording.Build(userId, startTime, endTime, audit);
                    }
                }
                _timeRecordingRepository.Save();
                return true;
            }
            return false;
        }

        public List<TimeRecording> Search(DateTime startDate, DateTime endDate, int skip, int take )
        {
            return _timeRecordingRepository.Search(startDate, endDate, skip, take).ToList();
             
        }

        public TimeRecording GetById(Int64 id)
        {
            var entities= _timeRecordingRepository.FindBy(x => x.Id == id);
            return entities.FirstOrDefault();
        }
    }
}
