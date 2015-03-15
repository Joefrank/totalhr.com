
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRS = totalhr.data.TimeRecordingSystem.EF;

namespace totalhr.web.ViewModels
{
    public class TaskSchedulerSearchVm
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? AssignedBy { get; set; }
        public int? AssignedTo { get; set; }

        public List<SelectListItem> UserList { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }

        public int PageSize { get; set; }
        public int Skip { get; set; }


        public List<TaskSchedulerVM> Results { get; set; }


        public TaskSchedulerSearchVm()
        {
            this.UserList = new List<SelectListItem>();
            this.DepartmentList = new List<SelectListItem>();
        }

        public void SetDefaults()
        {
            this.PageSize = 20;
            this.Skip = 0;

        }

        public void BuildResults(List<TRS.TaskScheduler> tasks, List<UserVM> users, List<DepartmentVM> departments)
        {
            departments.Add(new DepartmentVM() { Id = 0, Name = ""});
            departments.OrderBy(x => x.Id);
            users.Add(new UserVM() { Id = 0, Name = ""});
            users.OrderBy(x => x.Id);
            users.ForEach(x => UserList.Add(new SelectListItem() { Text = x.Name, Value = x.Id != 0 ? x.Id.ToString() : "", Selected = x.Id == 0 }));
            departments.ForEach(x => DepartmentList.Add(new SelectListItem() { Text = x.Name, Value = x.Id != 0 ? x.Id.ToString() : "", Selected = x.Id == 0 }));
            this.Results = tasks.ConvertAll(x => new TaskSchedulerVM(x));
        }

    }
}