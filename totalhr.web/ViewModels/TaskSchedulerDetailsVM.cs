using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities= totalhr.data.EF;

namespace totalhr.web.ViewModels
{
    public class TaskSchedulerDetailsVM: TaskSchedulerVM
    {
       
        [DisplayName("Approval Needed")]
        public bool ApprovalNeeded { get; set; }
        
        public Nullable<bool> Approved { get; set; }
        
        [DisplayName("Task Dead line")]
        public Nullable<System.DateTime> ScheduledDateTime { get; set; }

        public string ScheduleLog { get; set; }
        public Nullable<int> ApprovedBy { get; set; }

        public string ErrorMessage { get; set; }
        public int UserId { get; set; }

        public List<UserVM> Users { get; set; }
        public List<DepartmentVM> Departments { get; set; }

        public TaskSchedulerDetailsVM() : base() { }

        public void SetupTaskScheduler(int userId, int departmentId, List<UserVM> users, List<DepartmentVM> departments )
        {
            this.DepartmentId = departmentId;
            this.ApprovalNeeded = false;
            if(this.AssignedBy == 0) AssignedBy = userId;
            if(this.AssignedTo ==0) AssignedTo = userId;
            this.UserId = userId;
            this.Users = users;
            this.Departments = departments;
        }


    }

    public class UserVM:SelectListItem
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public UserVM() { }

        public UserVM(Entities.User user)
        {
            Id = user.id;
            Name = user.firstname + " " + user.surname;
        }
        public static List<UserVM> ConvertUsersToList(List<Entities.User> entityUsers)
        {
            return entityUsers.ConvertAll(x => new UserVM(x));
        }
    }

    public class DepartmentVM: SelectListItem
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public DepartmentVM() { }
        public DepartmentVM(Entities.Department department)
        {
            Id = department.id;
            Name = department.Name;
        }

        public static List<DepartmentVM> ConvertDepartmentsToList(List<Entities.Department> entityDepartments)
        {
            return  entityDepartments.ConvertAll(x => new DepartmentVM(x));
        }
    }
}