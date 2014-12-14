using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.data.TimeRecordingSystem.Models;
using totalhr.services.Infrastructure;
using totalhr.web.Areas.TimeRecording.ViewModels;

namespace totalhr.web.Areas.TimeRecording.Controllers
{
    public class TimeRecordingController : Controller
    {
        public ITimeRecordingServices _timeRecordingService { get; set; }
        public IAccountService _accountsService { get; set; }

        public TimeRecordingController(ITimeRecordingServices timeRecordingService, IAccountService accountService)
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
        public ActionResult RecordTime()
        {
            //Get User Id & Company Id
            var user = _accountsService.GetUser(59);
            var vm = new TimeRecordingVM() { 
                UserId = user.id, 
                CompanyId = user.CompanyId, 
                StartTime= DateTime.Now,
                EndTime = DateTime.Now
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult RecordTime(TimeRecordingVM vm)
        {
            if(ModelState.IsValid)
            {
                var isSuccess = _timeRecordingService.RecordTimeForUser(vm.UserId, vm.CompanyId, vm.StartTime, vm.EndTime,
                    new Audit() { AddedByUserId = vm.UserId, DateAdded = DateTime.Now });
                if (isSuccess)
                    return RedirectToAction("Index","TimeRecording");
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Search()
        {
            var vm = new SearchVM();
            vm.SetUpInitialSearch();
            var searchResults = _timeRecordingService.Search(vm.StartDate, vm.EndDate,0, vm.ResultsPerPage);
            vm.Results= TimeRecordingDetailsVM.Build(searchResults);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Search(SearchVM vm)
        {
            //service to search
            return View(vm);
        }



    }
}
