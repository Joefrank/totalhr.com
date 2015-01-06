using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using totalhr.services.Infrastructure;
using TRS = totalhr.data.TimeRecordingSystem.EF;

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

        public bool AddTask(string name, string description, int departmentId, bool needsApproval, int assignedBy, int assignedTo, DateTime? completeBy, Audit audit)
        {
            var task = new TRS.TaskScheduler()
            {
                Name = name,
                Description = description,
                DepartmentId = departmentId,
                ApprovalNeeded = needsApproval,
                AssignedBy = assignedBy,
                AssignedTo = assignedTo,
                ScheduledDateTime = completeBy,
                Audit = audit
            };

            _taskSchedulerRepository.Add(task);
            _taskSchedulerRepository.Save();
            return task.Id > 0;
        }
    }
}
