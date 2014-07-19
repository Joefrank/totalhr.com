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
        public List<Calendar> GetCompanyCalendar(int companyid)
        {
            var calendar = from ce in this.Context.Calendars
                           join us in this.Context.Users
                           on ce.CreatedBy equals us.id
                           where us.CompanyId == companyid
                           select ce;

            return calendar.ToList();
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
