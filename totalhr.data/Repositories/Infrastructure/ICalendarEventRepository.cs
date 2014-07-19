using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface ICalendarEventRepository : IGenericRepository<CalendarEvent>
    {
        List<CalendarEvent> GetCalendarMonthlyEventsByUser(int userid, int year, int month);

        List<CalendarEvent> GetCalendarMonthlyEventsByUserAndCalendar(int calendarId, int userid, int year, int month);
    }
}
