using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using ChatService.Infrastructure;
using totalhr.Shared.Models;
using totalhr.services.Infrastructure;
using totalhr.Shared;
using AbsencesService.Infrastructure;

namespace totalhr.web.Controllers
{
    public class AbsenceController : BaseController
    {
        private IOAuthService _authService;
        private IGlossaryService _glossaryService;
        private IAbsencesManager _absenceService;

        public AbsenceController(IOAuthService authservice, IGlossaryService glossaryService, IAbsencesManager absenceService)
            : base(authservice)
        {
            _authService = authservice;
            _glossaryService = glossaryService;
            _absenceService = absenceService;           

            ViewBag.AbsenceTypeList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.AbsenceType);
        }

        public ActionResult Index()
        {
            var absences = _absenceService.ListUserAbsences(CurrentUser.UserId);
            return View(absences);
        }

        public ActionResult CreateAbsence()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAbsence(AbsenceInfo info)
        {
            if (!ModelState.IsValid)
            {
                //ViewBag.AbsenceTypeList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.AbsenceType);
                return View();
            }

            info.CreatorId = CurrentUser.UserId;
            info.UserId = CurrentUser.UserId;
            info.StatusId = (int)Variables.AbsenceRequestStatus.NewAbsence;

            _absenceService.CreateAbsence(info);

            return RedirectToAction("Index");
        }
    }
}
