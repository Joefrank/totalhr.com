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
        public ActionResult Search()
        {
            var vm = new TaskSchedulerSearchVm();
            vm.SetDefaults();
            this.SearchTasks(vm);
            return View(vm);
        }
        [HttpPost]
        public ActionResult Search(TaskSchedulerSearchVm vm)
        {
            this.SearchTasks(vm);
            return View(vm);
        }

        private void SearchTasks(TaskSchedulerSearchVm vm)
        {
            var results = _taskSchedulerService.ListBySearch(vm.Id, vm.Name, vm.AssignedTo, vm.AssignedBy, vm.Skip, vm.PageSize);
            vm.BuildResults(results,this.GetUserVMList(),this.GetDepartmentVMList());
        }

        [HttpGet]
        public ActionResult Record(int id=0)
        {
            var vm = new TaskSchedulerDetailsVM();

            if (id > 0)
            {
                var task = _taskSchedulerService.GetById(id);
                if (task != null)
                {
                    vm = new TaskSchedulerDetailsVM(task);
                }
            }
            BuildTaskSchedulerVM(vm);
            return View(vm);

        }


        [HttpPost]
        public ActionResult Record(TaskSchedulerDetailsVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = _taskSchedulerService.RecordTask(vm.Id,vm.Name, vm.Description, vm.DepartmentId, vm.ApprovalNeeded, vm.AssignedBy, vm.AssignedTo, vm.ScheduledDateTime,
                      new Audit(CurrentUser.UserId, DateTime.Now));
                    if (success) return RedirectToAction("index");
                }

            }
            catch (Exception ex)
            {
                vm.ErrorMessage = ex.Message;
            }

            BuildTaskSchedulerVM(vm);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var task = _taskSchedulerService.GetById(id);
            var vm = new TaskSchedulerDetailsVM(task);
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
