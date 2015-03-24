using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.data.Repositories.Implementation
{
    public class CalendarRepository : GenericRepository<TotalHREntities, Calendar>, ICalendarRepository
    {
        public IEnumerable<Calendar> GetCompanyCalendar(int companyid)
        {
            return this.Context.Calendars.Where(x => x.CompanyId == companyid);
        }
       
        public List<CalendarEvent> GetCalendarEvents(int calendarid)
        {
            var calendarevents = from ce in this.Context.CalendarEvents
                                 where ce.CalendarId == calendarid
                                 select ce;

            return calendarevents.ToList();

        } 
    }
}
