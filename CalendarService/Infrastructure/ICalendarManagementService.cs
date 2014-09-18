using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;
using TEF = totalhr.data.EF;

namespace Calendar.Infrastructure
{
    public interface ICalendarManagementService
    {
        TEF.Calendar GetCalendar(int calendarid);

        List<TEF.Calendar> GetCompanyCalendars(int companyid);

        List<TEF.CalendarEvent> GetCalendarEvents(int calendarid);

        TEF.CalendarEvent GetEvent(int eventid);

        CalendarEventInfo GetEventInfo(int eventid);

        TEF.CalendarEvent CreateEvent(totalhr.Shared.Models.CalendarEventInfo info);

        List<TEF.Calendar> GetUserCalendars(int userid);
                
        List<CalendarEventCache> GetUserCalendarEvents(int userid, int year, int month, int calendarid = 0);

        List<TEF.CalendarAssociation> GetCalendarEventInvitees(int calendareventid);

        List<TEF.CalendarAssociation> GetCalendarEventReminders(int calendareventid);

        List<CalendarEventCache> GetUserDayCalendarEvents(int userid, DateTime date, int calendarid = 0);

        CalendarEvent SaveEvent(totalhr.Shared.Models.CalendarEventInfo info);
    }
}
