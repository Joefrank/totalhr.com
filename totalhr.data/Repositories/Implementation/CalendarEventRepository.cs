using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
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

        public CalendarAssociation CreateEventAssociation(CalendarAssociation assoc)
        {
            this.Context.CalendarAssociations.Add(assoc);
            this.Context.SaveChanges();
            return assoc;
        }

        public List<CalendarAssociation> GetCalendarEventAssociations(int eventid)
        {
            return this.Context.CalendarAssociations.Where(x => x.EventId == eventid).ToList();
        }

        public List<CalendarEvent> GetCalendarDailyEventsByUser(int userid, DateTime date, int calendarid=0){
            return FindBy(x =>
                (DbFunctions.TruncateTime(x.StartOfEvent) == DbFunctions.TruncateTime(date) || 
                    DbFunctions.TruncateTime(x.EndOfEvent) == DbFunctions.TruncateTime(date))
                && (calendarid == 0 || x.CalendarId == calendarid)
                ).ToList(); 
        }

    }
}
