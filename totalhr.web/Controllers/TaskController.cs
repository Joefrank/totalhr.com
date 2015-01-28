using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.services.Infrastructure;
using totalhr.web.ViewModels;

namespace totalhr.web.Controllers
{
    public class TaskController : BaseController
    {
        private ITaskSchedulerService _taskSchedulerService;
        private IAccountService _accountsService;

        public TaskController(ITaskSchedulerService taskSchedulerService, IAccountService accountsService, IOAuthService authService)
            : base(authService)
        {
            _taskSchedulerService = taskSchedulerService;
            _accountsService = accountsService;
        }
        //
        // GET: /Task/
        [HttpGet]
        public ActionResult Index()
        {
            var vm = new TaskSchedulerSearchVm();
            vm.SetDefaults();
            this.SearchTasks(vm);
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(TaskSchedulerSearchVm vm)
        {
            this.SearchTasks(vm);
            return View(vm);
        }

        private void SearchTasks(TaskSchedulerSearchVm vm)
        {
            vm.Users = this.GetUserVMList();
            vm.Departments = this.GetDepartmentVMList();
            var results = _taskSchedulerService.ListBySearch(vm.Id, vm.Name, vm.AssignedTo, vm.AssignedBy, vm.Skip, vm.PageSize);
            vm.BuildResults(results);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var vm = new TaskSchedulerDetailsVM();
            BuildTaskSchedulerVM(vm);
            return View(vm);

        }


        [HttpPost]
        public ActionResult Add(TaskSchedulerDetailsVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = _taskSchedulerService.AddTask(vm.Name, vm.Description, vm.DepartmentId, vm.ApprovalNeeded, vm.AssignedBy, vm.AssignedTo, vm.ScheduledDateTime,
                      new Audit(CurrentUser.UserId, DateTime.Now));
                }

            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }

            BuildTaskSchedulerVM(vm);
            return View(vm);
        }


        private void BuildTaskSchedulerVM(TaskSchedulerDetailsVM vm)
        {
            var usersVM = GetUserVMList();
            var departmentsVM = GetDepartmentVMList();
            vm.SetupTaskScheduler(this.CurrentUser.UserId, this.CurrentUser.DepartmentId, usersVM, departmentsVM);
            //return vm;
        }

        private List<DepartmentVM> GetDepartmentVMList()
        {
            var departments = _accountsService.GetCompanyDepartments(this.CurrentUser.CompanyId);
            var departmentsVM = DepartmentVM.ConvertDepartmentsToList(departments);

            return departmentsVM;
        }

        private List<UserVM> GetUserVMList()
        {
            var users = _accountsService.GetCompanyUsers(this.CurrentUser.CompanyId);
            var usersVM = UserVM.ConvertUsersToList(users);

            return usersVM;
        }


    }
}
