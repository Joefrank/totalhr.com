using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared
{
    public class Variables
    {

        public enum Languages
        {
            English = 1,
            French = 2
        }

        public enum GlossaryGroups
            {
                Gender,
                Country,
                Language,
                Title,
                CalendarEventTarget
            }

        public enum NamedGlossaryIds
        {
            OtherTitle = 15
        }

        public enum EmailTemplateIds
        {
            NewUserWelcome,
            AdminNewUserNotify,
            PasswordReminder,
            AccountActivated,
            AdminAccountActivated
        }

        public enum Roles
        {            
            CompanyAdmin = 1,
            SiteAdmin =2,
            Employee = 3
        }

        public enum Profiles
        {
            CalendarEdit =1,
            CalendarView = 2,
            CalendarCreate = 3,
            CalendarCreateEvent = 4,
            CalendarEventPropageteToCompany = 5
        }

        public enum CalendarEventTarget
        {
            MyselfOnly = 254,
            Company= 251,
            Department =252,
            User =253
        }


        public enum RepeatType
        {
            OnDates = 1,
            DailyMonToFri = 2,
            OnDayOfTheWeek = 3,
            MonthlyOnDates = 4,
            YearlyOnSameDate = 5
        }

        public enum ReminderType
        {
            TimeBefore= 1,
            EveryXTime = 2
        }

        public enum EventFrequency
        {
            EvtHour=2,
            EvtMinute = 1,
            EvtDay = 3,
            EvtWeek = 4,
            EvtMonth = 5
        }

        public enum CalendarEventAssociationType
        {
            Attendee =1,
            Reminder = 2,
            Repeat = 3
        }
        
        public static string AdminEmailSignature
        {
            get
            {
                return "The TotalHR Team";
            }
        }        
    }
   
}
