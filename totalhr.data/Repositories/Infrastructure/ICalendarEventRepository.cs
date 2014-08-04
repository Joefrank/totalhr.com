using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface ICalendarEventRepository : IGenericRepository<CalendarEvent>
    {        
        List<CalendarEventCache> GetMonthlyCalendarEvents(int userid, int year, int month , int calendarId=0);

        CalendarAssociation CreateEventAssociation(CalendarAssociation assoc);

        List<CalendarAssociation> GetCalendarEventAssociations(int eventid);

        List<CalendarEventCache> GetCalendarDailyEventsByUser(int userid, DateTime date, int calendarid = 0);
    }
}
