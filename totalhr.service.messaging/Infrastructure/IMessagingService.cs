using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Shared.Models;

namespace totalhr.services.messaging.Infrastructure
{
    public interface IMessagingService
    {
        bool NotifyUser(EmailStruct estruct);

        void AcknowledgeNewUserRegistration(AdminStruct adminstruct, UserRegStruct userstruct);

        void ReadSMTPSettings(SMTPSettings smtpsettings);

        void SendPasswordReminderEmail(AdminStruct adminstruct, User user);

        void AcknowledgeAccountActivation(AdminStruct adminstruct, User user);

        bool NotifyUserOfCalendarEvent(AdminStruct adminstruct, CalendarEventInfo eventinfo);

        bool EmailUserWithAttachment(HTMLEmailStruct estruct);
    }
}
