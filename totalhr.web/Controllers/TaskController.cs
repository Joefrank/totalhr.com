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

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            var vm = new TaskSchedulerVM();
            BuildTaskSchedulerVM(vm);
            return View(vm);

        }



        [HttpPost]
        public ActionResult Add(TaskSchedulerVM vm)
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


        private void BuildTaskSchedulerVM(TaskSchedulerVM vm)
        {
            var users = _accountsService.GetCompanyUsers(this.CurrentUser.CompanyId);
            var departments = _accountsService.GetCompanyDepartments(this.CurrentUser.DepartmentId);
            var usersVM = UserVM.ConvertUsersToList(users);
            var departmentsVM = DepartmentVM.ConvertDepartmentsToList(departments);
            vm.SetupTaskScheduler(this.CurrentUser.UserId, this.CurrentUser.DepartmentId, usersVM, departmentsVM);
            //return vm;
        }
    }
}
