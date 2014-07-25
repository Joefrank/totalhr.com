using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEF= totalhr.data.EF;

namespace Calendar.Infrastructure
{
    public interface ICalendarManagementService
    {
        TEF.Calendar GetCalendar(int calendarid);

        List<TEF.Calendar> GetCompanyCalendars(int companyid);

        List<TEF.CalendarEvent> GetCalendarEvents(int calendarid);

        TEF.CalendarEvent GetEvent(int eventid);

        TEF.CalendarEvent CreateEvent(totalhr.Shared.Models.CalendarEventInfo info);

        List<TEF.Calendar> GetUserCalendars(int userid);

        List<TEF.CalendarEvent> GetUserCalendarEvents(int userid, int year, int month);

        List<TEF.CalendarEvent> GetUserCalendarEvents(int calendarid, int userid, int year, int month);

        List<TEF.CalendarAssociation> GetCalendarEventInvitees(int calendareventid);

        List<TEF.CalendarAssociation> GetCalendarEventReminders(int calendareventid);
    }
}
