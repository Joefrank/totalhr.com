using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Models.Enums
{
    public enum Roles
    {
        CompanyAdmin = 1,
        SiteAdmin = 2,
        Employee = 3
    }

    public enum Profiles
    {
        CalendarEdit = 1,
        CalendarView = 2,
        CalendarCreate = 3,
        CalendarCreateEvent = 4,
        CalendarEventPropageteToCompany = 5
    }
}
