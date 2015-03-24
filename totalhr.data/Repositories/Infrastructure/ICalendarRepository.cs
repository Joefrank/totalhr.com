using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEF = totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface ICalendarRepository : IGenericRepository<totalhr.data.EF.Calendar>
    {
        IEnumerable<TEF.Calendar> GetCompanyCalendar(int companyid);

        List<TEF.CalendarEvent> GetCalendarEvents(int calendarid);
    }
}
