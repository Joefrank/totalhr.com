using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calendar.Infrastructure;
using Calendar.Models;
using System.Globalization;
using log4net;
using TEF = totalhr.data.EF;
using Authentication.Infrastructure;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using Authentication.Models;
using totalhr.Shared.Models;
using totalhr.Shared;
using totalhr.Resources;
using totalhr.services.Infrastructure;
using System.Text;

namespace totalhr.web.Controllers
{
    /*
     * Calendar should be based on culture of registered user  
     * and registered user should only select language supported by company
     * Multiple language support is a feature to be purchased.
     * 
     * */

   
    public class CalendarController : BaseController
    {
        ICalendarService _calService;
        ICalendarManagementService _calMservice;
        ICalendarRepository _calRepos;
        ICalendarEventRepository _calEventRepos;
        private IOAuthService _authService;
        private readonly IGlossaryService _glossaryService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));
        
        public CalendarController(ICalendarService cservice, ICalendarManagementService calmservice, 
            IOAuthService authservice, ICalendarRepository calrepos, ICalendarEventRepository caleventRepos, IGlossaryService glossaryService) :  base(authservice)
        {
            _calService = cservice;
            _calMservice = calmservice;
            _calRepos = calrepos;
            _calEventRepos = caleventRepos;
            _authService = authservice;
            _glossaryService = glossaryService;
        }

        private string MakeClientJSForWeekDays()
        {
            StringBuilder sbtemp = new StringBuilder();            
            string[] weekdays = _calService.GetWeekDaysByName(CultureInfo.CreateSpecificCulture(CurrentUser.Culture));
            int len = weekdays.Length;

            sbtemp.Append(" var d = new Date();" + Environment.NewLine + " var weekday = new Array(7);" + Environment.NewLine);

            for (int i = 0; i < len; i++)
            {
                sbtemp.Append(string.Format(@"weekday[{0}]=  ""{1}"";" + Environment.NewLine,
                    i, HttpUtility.JavaScriptStringEncode(weekdays[i])));
            }
            return sbtemp.ToString();
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult Index()
        {
            var allCalendars = _calMservice.GetCompanyCalendars(CurrentUser.CompanyId);
            return View(allCalendars);
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult GenerateDefault(int id)
        {
            //always put calendarid to get correct calendar.
            return MonthView(DateTime.Now.Year, DateTime.Now.Month, id);
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult EditEvent(int id)
        {
            if (id == 0)
            {
                return View("EventEdit", new CalendarEventInfo());
            }
            else
            {
                ViewBag.EventTargets = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.CalendarEventTarget);
               CalendarEvent cevent = _calMservice.GetEvent(id);
               var ceinfo = new CalendarEventInfo
               {
                   EventId = id,
                   Title = cevent.Title,
                   Description = cevent.Description,
                   Location = cevent.Location,
                   StartDate = cevent.StartOfEvent,
                   EndDate = cevent.EndOfEvent
               };

               return View("EventEdit", ceinfo);
            }
        }
        
        [CustomAuthorize(Roles = "3", Profiles = "4", AccessDeniedMessage = "Work this out - FormMessages.Error_NoProfile_CreateCalendarEvent")]
        [HttpGet] 
        public ActionResult CreateEvent(int id)
         {
            totalhr.data.EF.Calendar calendar = _calMservice.GetCalendar(id);
            if (calendar == null)
            {
                return RedirectToAction("AccessDenied", "Error", new { ModelError = "Calendar not registered." });
            }
            else
            {
                ViewBag.WeekDaysJS = MakeClientJSForWeekDays();
                ViewBag.EventTargets = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.CalendarEventTarget);
                return View("EventEdit", "~/Views/Shared/_PopupLayout.cshtml", new CalendarEventInfo {CalendarId = calendar.id, CalendarName = calendar.Name });
            }
         }

        [CustomAuthorize(Roles = "3", Profiles = "4", AccessDeniedMessage = "Work this out - FormMessages.Error_NoProfile_CreateCalendarEvent")]
        [HttpPost]
        public ActionResult CreateEvent(CalendarEventInfo eventinfo)
        {
            if (!ModelState.IsValid)
            {
                return View("EventEdit", eventinfo);
            }

            eventinfo.CreatedBy = CurrentUser.UserId;

            if (eventinfo.EventId > 0)
            {
                //update event
            }
            else
            {
                CalendarEvent cevent = _calMservice.CreateEvent(eventinfo);
                
            }

            //return view to confirm creation
            return View("EventEdit", eventinfo);
            
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult Generate(int year, int month)
        {
            var rqStruct = new CalendarRequestStruct
            {
                Info = CultureInfo.CreateSpecificCulture("en-GB"),//read user culture here
                TableTemplate = @" border=""1"" class=""calendar"" ",
                Year = year,
                Month = month
            };

            return View("Generate", _calService.GenerateCalendarHTML(rqStruct));
        }

        [CustomAuthorize(Roles = "3")]
        private ActionResult MonthView(int year, int month, int calendarid =0)
        {

            var calEvents = calendarid == 0 ? _calMservice.GetUserCalendarEvents(CurrentUser.UserId,year,month) :
                _calMservice.GetUserCalendarEvents(calendarid, CurrentUser.UserId, year, month);

            var rqStruct = new CalendarRequestStruct
            {
                Info = CultureInfo.CreateSpecificCulture(CurrentUser.Culture),
                TableTemplate = @" border=""1"" class=""calendar"" ",
                Year = year,
                Month = month,
                RelatedEvents = calEvents,
                CalendarId = calendarid,
                ClientConfig = new ClientScriptConfig
                    {
                        PageClientId = 1,
                        ActiveTdClickCallBack = @" onclick=""ManageActiveDay(this);"" ",
                        EventClickCallBack = @" onclick=""ManageEvent(this);"" ",
                        JsArrayEventName = "ArrEvents"
                    }
                
            };
            return View("Generate",_calService.GenerateCalendarHTML(rqStruct));
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult GetCalendarMonthViewByUser(int year, int month)
        {           
            return  MonthView(year, month);
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult WeekView(int year, int month, int day)
        {
            
            try
            {
                var weekRequest = new CalendarWeekRequestStruct
                {
                    DateRequested = new DateTime(year, month, day),
                    Info = CultureInfo.CreateSpecificCulture("fr-FR"),//read user culture here
                    TableTemplate = @" border=""1"" class=""calendarweek"" ",
                    DayHeaderFormat = "ddd, MMM d",
                    CrossEdgeContent = " "
                };

                return View("Generate", _calService.GenerateWeekHTML(weekRequest));
            }
            catch (Exception ex)
            {
                Log.Debug(string.Format("Calendar week view params Year: {0} - Month {1} - Day {2} " + Environment.NewLine +
                    ex.Message, year, month, day));

                return View("Generate");
            }

            
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult DayView(int year, int month, int day)
        {
            Log.Debug(string.Format("Calendar day view params Year: {0} - Month {1} - Day {2} ", year, month, day));

            var weekRequest = new CalendarWeekRequestStruct
            {
                DateRequested = new DateTime(year, month, day),
                Info = CultureInfo.CreateSpecificCulture("fr-FR"),//read current user culture
                TableTemplate = @" border=""1"" class=""calendarweek day"" ",
                DayHeaderFormat = "dddd, MMM d",
                CrossEdgeContent = " "
            };

            return View("Generate", _calService.GenerateDayHTML(weekRequest));
        }

        [CustomAuthorize(Roles = "3")]
        public ActionResult ListPersonal()
        {
            List<TEF.Calendar> lstCalendars = _calMservice.GetUserCalendars(CurrentUser.UserId);
            return View("Index", lstCalendars);
        }

       
    }
}

/*http://mvc.daypilot.org/calendar/ */