using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.services.Infrastructure;
using totalhr.web.Areas.TimeRecording.ViewModels;
using totalhr.web.Controllers;

namespace totalhr.web.Areas.TimeRecording.Controllers
{
    public class TimeRecordingController : BaseController
    {
        private ITimeRecordingServices _timeRecordingService;
        private IAccountService _accountsService;

        public TimeRecordingController(ITimeRecordingServices timeRecordingService, IAccountService accountService, IOAuthService authService):base(authService)
        {
            _timeRecordingService = timeRecordingService;
            _accountsService = accountService;
        }

        // GET: /TimeRecording/TimeRecording/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RecordTime(long id = 0)
        {
            var vm = new TimeRecordingVM();
            if (id == 0)
            {
                vm = new TimeRecordingVM()
                {
                    UserId = CurrentUser.UserId,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now
                };
            }
            else
            {
                vm = new TimeRecordingVM(_timeRecordingService.GetById(id));
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult RecordTime(TimeRecordingVM vm)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = false;
                if (vm.Id == 0)
                {
                    isSuccess = _timeRecordingService.RecordTimeForUser(vm.Id,vm.UserId, vm.StartTime, vm.EndTime,
                       new Audit() { AddedBy = vm.UserId, AddedDate = DateTime.Now });
                if (isSuccess)
                    return RedirectToAction("Index", "TimeRecording");
                }
                else
                {
                    isSuccess = _timeRecordingService.RecordTimeForUser(vm.Id,vm.UserId, vm.StartTime, vm.EndTime,
                       new Audit().UpdateAudit( vm.UserId, DateTime.Now ));
                    if (isSuccess)
                        return RedirectToAction("Details", "TimeRecording", new { id = vm.Id });
                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Search()
        {
            var vm = new SearchVM();
            vm.SetUpInitialSearch();
            var searchResults = _timeRecordingService.Search(vm.StartDate, vm.EndDate, 0, vm.ResultsPerPage);
            vm.Results = TimeRecordingDetailsVM.Build(searchResults);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Search(SearchVM vm)
        {
            var searchResults = _timeRecordingService.Search(vm.StartDate, vm.EndDate, vm.PageNumber * vm.ResultsPerPage, vm.ResultsPerPage);
            vm.Results = TimeRecordingDetailsVM.Build(searchResults);
            return View(vm);
        }


        public ActionResult Details(long id)
        {
            var vm = new TimeRecordingDetailsVM(_timeRecordingService.GetById(id));
            return View(vm);
        }

    }
}
