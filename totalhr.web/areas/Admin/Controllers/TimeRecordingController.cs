﻿using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.TimeRecordingSystem.EF;
using totalhr.services.Infrastructure;
using totalhr.web.ViewModels;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class TimeRecordingController : AdminBaseController
    {
        public ITimeRecordingServices _timeRecordingService { get; set; }
        public IAccountService _accountsService { get; set; }

        public TimeRecordingController(ITimeRecordingServices timeRecordingService, IAccountService accountService,
            IOAuthService authService)
            : base(authService)
        {
            _timeRecordingService = timeRecordingService;
            _accountsService = accountService;
        }

        // GET: /TimeRecording/TimeRecording/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecordTime(TimeRecordingVM vm)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = false;
                if (vm.Id == 0)
                {
                    isSuccess = _timeRecordingService.RecordTimeForUser(vm.Id, vm.UserId, vm.StartTime, vm.EndTime, vm.TypeId, vm.TaskRef,
                       new Audit() { AddedBy = vm.UserId, AddedDate = DateTime.Now });
                    if (isSuccess)
                    {
                        if (vm.TaskRef != null) return RedirectToAction("Details", "Task", new { id = vm.TaskRef });
                        return RedirectToAction("Index", "TimeRecording");
                    }
                }
                else
                {
                    isSuccess = _timeRecordingService.RecordTimeForUser(vm.Id, vm.UserId, vm.StartTime, vm.EndTime, vm.TypeId, vm.TaskRef,
                       new Audit().UpdateAudit(vm.UserId, DateTime.Now));
                    if (isSuccess)
                    {
                        if (vm.TaskRef != null) return RedirectToAction("Details", "Task", new { id = vm.TaskRef });
                        return RedirectToAction("Details", "TimeRecording", new { id = vm.Id });
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Search()
        {
            var vm = new SearchVM();
            vm.SetUpInitialSearch();
            var searchResults = _timeRecordingService.Search(vm.StartDate, vm.EndDate, base.CurrentUser.UserId, 0, vm.ResultsPerPage);
            vm.Results = TimeRecordingDetailsVM.Build(searchResults);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Search(SearchVM vm)
        {
            var searchResults = _timeRecordingService.Search(vm.StartDate, vm.EndDate, base.CurrentUser.UserId, vm.PageNumber * vm.ResultsPerPage, vm.ResultsPerPage);
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
