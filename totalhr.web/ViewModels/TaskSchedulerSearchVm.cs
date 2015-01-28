
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRS = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.web.ViewModels
{
    public class TaskSchedulerSearchVm
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? AssignedBy { get; set; }
        public int? AssignedTo { get; set; }
        
        public List<UserVM> Users { get; set; }
        public List<DepartmentVM> Departments { get; set; }


        public int PageSize { get; set; }
        public int Skip { get; set; }


        public List<TaskSchedulerVM> Results { get; set; }


        public TaskSchedulerSearchVm() { }

        public void SetDefaults()
        {
            this.PageSize = 20;
            this.Skip = 0;
            
        }

        public void BuildResults(List<TRS.TaskScheduler> tasks)
        {
            this.Departments.Add(new DepartmentVM() { Id = null, Name = "-", Text = "-", Value = null, Selected = true });
            this.Users.Add(new UserVM() { Id = null, Name = "-", Selected=true });
            this.Results = tasks.ConvertAll(x => new TaskSchedulerVM(x));
        }

    }
}