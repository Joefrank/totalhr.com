using Calendar.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEF = totalhr.data.EF;
using totalhr.data.Repositories.Implementation;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;


namespace Calendar.Implementation
{
    public class CalendarManagementService : ICalendarManagementService
    {
        private readonly ICalendarRepository _calrepos;
        private readonly ICalendarEventRepository _calEventRepos;

        public CalendarManagementService(ICalendarRepository calRepos, ICalendarEventRepository calEventRepos)
        {
            _calrepos = calRepos;
            _calEventRepos = calEventRepos;
        }

        public totalhr.data.EF.Calendar GetCalendar(int calendarid)
        {
            return _calrepos.FindBy(x => x.id == calendarid).FirstOrDefault();
        }

        public CalendarEvent CreateEvent(totalhr.Shared.Models.CalendarEventInfo info)
        {
            var cevent = new CalendarEvent();
            cevent.Title = info.Title;
            cevent.Description = info.Description;
            cevent.StartOfEvent = info.StartDate;
            cevent.EndOfEvent = info.EndDate;
            cevent.CreatedBy = info.CreatedBy;
            cevent.Location = info.Location;
            cevent.Created = DateTime.Now;
            cevent.CalendarId = info.CalendarId;

            _calEventRepos.Add(cevent);
            _calEventRepos.Save();

            return cevent;
        }

        public CalendarEvent GetEvent(int eventid)
        {
            return _calEventRepos.FindBy(x => x.id == eventid).FirstOrDefault();
        }

        public List<TEF.CalendarEvent> GetCalendarEvents(int calendarid)
        {
            return _calrepos.GetCalendarEvents(calendarid);
        }

        public List<TEF.Calendar> GetCompanyCalendars(int companyid)
        {
            return _calrepos.GetCompanyCalendar(companyid);
        } 

        public List<TEF.Calendar> GetUserCalendars(int userid)
        {
            
            return _calrepos.FindBy(x => x.CreatedBy == userid).ToList();
        }

        public List<TEF.CalendarEvent> GetUserCalendarEvents(int userid, int year, int month)
        {
            return _calEventRepos.GetCalendarMonthlyEventsByUser(userid, year, month);
        }

        public List<TEF.CalendarEvent> GetUserCalendarEvents(int calendarid, int userid, int year, int month)
        {
            return _calEventRepos.GetCalendarMonthlyEventsByUserAndCalendar(calendarid, userid, year, month);
        }

        public List<TEF.CalendarEventInvite> GetCalendarEventInvitees(int calendareventid)
        {
            return null;
        }

        public List<TEF.CalendarEventReminder> GetCalendarEventReminders(int calendareventid)
        {
            return null;
        }
    }
}
