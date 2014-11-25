using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared;

namespace totalhr.services.messaging.Infrastructure
{
    public interface IEmailService
    {
        void InitSMTP(string smtpServer, string username, string password);

        bool SendEmailHTML(string toAddress, string toName, string fromAddress, string fromName, string subject, string body);

        bool SendEmail(string toAddress, string toName, string fromAddress, string fromName, string subject, string body);

        bool SendMultiPartEmail(String sender, String recipients, String subject, String textBody, String htmlBody);

        bool SendEmailWithAttachment(HTMLEmailStruct htmlEmailStruct);

    }
}
