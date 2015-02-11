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

        public List<TRS.TaskScheduler> ListBySearch(int? id, string name, int? assignedTo, int? assignedBy, int skip, int take)
        {
            var tasks = _taskSchedulerRepository.GetAll();
            if (id != null) tasks = tasks.Where(x => x.Id == id);
            if (!string.IsNullOrEmpty(name)) tasks = tasks.Where(x => x.Name.Contains(name));
            if (assignedTo != null) tasks = tasks.Where(x => x.AssignedTo == assignedTo);
            if (assignedBy != null) tasks = tasks.Where(x => x.AssignedBy == assignedBy);
            return tasks.Any()? tasks.OrderBy(x => x.Id).Skip(skip).Take(take).ToList(): new List<TRS.TaskScheduler>();
        }

        public bool RecordTask(int id, string name, string description, int departmentId, bool needsApproval, int assignedBy, int assignedTo, DateTime? completeBy, Audit audit)
        {
            var task = new TRS.TaskScheduler();
            
            if (id > 0)
            {
               var taskEntity = this.GetById(id);
               if (taskEntity != null)
               {
                   task = taskEntity;
                   taskEntity.Audit.UpdateAudit(audit.AddedBy, audit.AddedDate);
                   audit = taskEntity.Audit;
               }
            }
            task.Build(name, description, departmentId, needsApproval, assignedBy, assignedTo, completeBy, audit);

            if(id==0) _taskSchedulerRepository.Add(task);
            _taskSchedulerRepository.Save();
            return task.Id > 0;
        }
        
        public  TRS.TaskScheduler GetById(int id)
        {
            var trs = _taskSchedulerRepository.FindBy(x => x.Id == id);
            return trs.Any() ? trs.FirstOrDefault() : null;
        }

    }
}
