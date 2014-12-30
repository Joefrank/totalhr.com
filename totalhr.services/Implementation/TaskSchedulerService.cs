using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using totalhr.services.Infrastructure;

namespace totalhr.services.Implementation
{
    public class TaskSchedulerService : ITaskSchedulerService
    {
        private ITimeRecordingRepository _timeRecordingRepository { get; set; }
        private ITaskSchedulerRepository _taskSchedulerRepository { get; set; }
        private IAccountService _accountService { get; set; }

        public TaskSchedulerService(ITimeRecordingRepository timeRecordingRepository, ITaskSchedulerRepository taskSchedulerRepository, IAccountService accountService)
        {
            _taskSchedulerRepository = taskSchedulerRepository;
            _timeRecordingRepository = timeRecordingRepository;
            _accountService = accountService;
        }

        public bool AddTask()
        {

            return true;
        }
    }
}
