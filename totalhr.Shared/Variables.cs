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
            AdminAccountActivated,
            CalendarEventNotification
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

        public enum DocumentPermissionType
        {
            Private = 1,
            SelectedUsers = 2,
            Department = 3,
            WholeCompany = 4
        }

        public enum ReminderType
        {
            TimeBefore= 1,
            EveryXTime = 2
        }

        public enum CalendarEventReminderValues
        {
            R5MinsBefore=1,
            R10MinsBefore=2,
            R15MinsBefore=3,
            R20MinsBefore=4,
            R25MinsBefore=5,
            R30MinsBefore=6,
            R45MinsBefore=7,
            R1HourBefore=8,
            R2HoursBefore=9,
            R4HoursBefore=10,
            R1DayBefore=11,
            R2DaysBefore=12,
            R1WeekBefore=13,
            R2WeeksBefore=14,
            R1MonthBefore=15,
            Customize=16
        }

        public enum CalendarEventNotificationType
        {
            ByEmail=255,
            ByTextPhone=256
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
        
        public enum CalendarViewType
        {
            YearView,
            MonthView,
            WeekView,
            DayView
        }

        public enum FileType
        {
            CompanyDocument = 1
        }

        public enum AllowedFileExtension
        {            
            doc,
            docx,
            txt,
            ppt,
            pptx,
            rtf,
            pdf,
            odt,
            log,
            tex,
            wps,
            wpd,
            mp3,
            mov,
            gif,
            jpg,
            png,
            psd,
            pspimage,
            tif,
            tiff,
            thm

        }

        public enum DocumentShareType
        {
            Link = 1,
            Attachment = 2
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
