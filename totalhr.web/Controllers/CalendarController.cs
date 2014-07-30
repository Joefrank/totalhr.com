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
       
        public ActionResult Index()
        {
            var allCalendars = _calMservice.GetCompanyCalendars(CurrentUser.CompanyId);
            return View(allCalendars);
        }
        
        public ActionResult GenerateDefault(int id)
        {
            //always put calendarid to get correct calendar.
            return MonthView(DateTime.Now.Year, DateTime.Now.Month, id);
        }

        public ActionResult EditEvent(int id)
        {
            if (id == 0)
            {
                return View("EventEdit", new CalendarEventInfo());
            }
            else
            {
                ViewBag.WeekDaysJS = MakeClientJSForWeekDays();
                return View("EventEdit", _calMservice.GetEventInfo(id));
            }
        }
       
        [ProfileCheck(Variables.Profiles.CalendarCreateEvent)]
        [HttpGet] 
        public ActionResult CreateEvent(int id)
         {
            var calendar = _calMservice.GetCalendar(id);
            if (calendar == null)
            {
                return RedirectToAction("AccessDenied", "Error", new { ModelError = "Calendar not registered." });
            }
            else
            {
                ViewBag.WeekDaysJS = MakeClientJSForWeekDays();
                //ViewBag.EventTargets = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.CalendarEventTarget);
                return View("EventEdit", "~/Views/Shared/_PopupLayout.cshtml", new CalendarEventInfo {CalendarId = calendar.id, CalendarName = calendar.Name });
            }
         }

        [ProfileCheck(Variables.Profiles.CalendarCreateEvent)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateEvent(CalendarEventInfo eventinfo)
        {
            if (!ModelState.IsValid)
            {
                return View("EventEdit", eventinfo);
            }

            eventinfo.CreatedBy = CurrentUser.UserId;           
            var cevent = _calMservice.CreateEvent(eventinfo);

            if (cevent.id > 0)
            {
                return RedirectToAction("GenerateDefault", "Calendar", new { id = eventinfo.CalendarId });
            }
            else
            {
                return RedirectToAction("CreateEvent", "Calendar", new {id = eventinfo.CalendarId });
            }
            
        }
       
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

        
        private ActionResult MonthView(int year, int month, int calendarid =0)
        {

            var calEvents = calendarid == 0 ? _calMservice.GetUserCalendarEvents(CurrentUser.UserId,year,month) :
                _calMservice.GetUserCalendarEvents(calendarid, CurrentUser.UserId, year, month);

            //***verify that current user is not viewing calendars they are not authorized to view
            var calendar = _calMservice.GetCalendar(calendarid);
            
            if (calendar != null)
            {
                ViewBag.CalendarName = calendar.Name;
            }

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

       
        public ActionResult GetCalendarMonthViewByUser(int year, int month, int day)
        {
            return MonthView(year, month);
        }

        public ActionResult GetCalendarMonth(int year, int month, int calendarid)
        {
            return MonthView(year, month, calendarid);
        }
       
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

        
        public ActionResult ListPersonal()
        {
            List<TEF.Calendar> lstCalendars = _calMservice.GetUserCalendars(CurrentUser.UserId);
            return View("Index", lstCalendars);
        }

       
    }
}

