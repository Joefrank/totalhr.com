using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class CalendarEventRepository : GenericRepository<TotalHREntities, CalendarEvent>, ICalendarEventRepository
    {
        //add filtering by user
        public List<CalendarEvent> GetCalendarMonthlyEventsByUser(int userid, int year, int month)
        {

            return FindBy(x =>
                (x.StartOfEvent.Month == month || x.EndOfEvent.Month == month)
                &&
                (x.StartOfEvent.Year == year || x.EndOfEvent.Year == year)
                ).ToList();                

        }

        //add filtering by user
        public List<CalendarEvent> GetCalendarMonthlyEventsByUserAndCalendar(int calendarId, int userid, int year, int month)
        {
            return FindBy(x => x.CalendarId == calendarId &&
                (x.StartOfEvent.Month == month || x.EndOfEvent.Month == month)
                &&
                (x.StartOfEvent.Year == year || x.EndOfEvent.Year == year)
                ).ToList();

        }
    }
}
