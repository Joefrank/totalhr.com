using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TRS = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.web.ViewModels
{
    public class TaskSchedulerVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int DepartmentId { get; set; }
        public string Department { get; set; }
        
        public int AssignedBy { get; set; }
        public string AssignedByUser { get; set; }

        public int AssignedTo { get; set; }
        public string AssignedToUser { get; set; }

        public TaskSchedulerVM() { }

        public TaskSchedulerVM (TRS.TaskScheduler task)
        {
            this.Id = task.Id;
            this.Name = task.Name;
            this.Description = task.Description;
            this.DepartmentId = task.DepartmentId;
            this.Department = task.Department.Name;
            this.AssignedBy = task.AssignedBy;
            this.AssignedByUser = task.AssignedByUser.FullName;
            this.AssignedTo = task.AssignedTo;
            this.AssignedToUser = task.AssignedToUser.FullName;
        }

    }
}