using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mime;
using System.IO;
using System.Net.Mail;
using totalhr.services.messaging.Infrastructure;
using totalhr.Shared;
using log4net;

namespace totalhr.services.messaging.Implementation
{
    public class EmailService : IEmailService
    {
        public string signature = "";
        public string html_signature = "";
        NetworkCredential SMTPUserInfo = null;
        SmtpClient emailClient = null;
        bool bSMTPInitialized = false;

        private static readonly ILog log = LogManager.GetLogger(typeof(EmailService));

        public EmailService(SMTPSettings settings)
        {
            SMTPUserInfo = new NetworkCredential(settings.UserName, settings.Password);
            emailClient = new SmtpClient(settings.SMTPServer);
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = this.SMTPUserInfo;
            bSMTPInitialized = true;
        }

        public void InitSMTP(string smtpServer, string username, string password)
        {
            SMTPUserInfo = new NetworkCredential(username, password);
            emailClient = new SmtpClient(smtpServer);
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = this.SMTPUserInfo;
            bSMTPInitialized = !string.IsNullOrEmpty(smtpServer) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }


        public bool SendEmailHTML(string toAddress, string toName, string fromAddress, string fromName, string subject, string body)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(fromAddress, toAddress, subject, body.Replace(Environment.NewLine, "<br/>"));
                message.IsBodyHtml = true;
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = SMTPUserInfo;
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("EmailService (SendEmailHTML)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmail(string toAddress, string toName, string fromAddress, string fromName, string subject, string body)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(fromAddress, toAddress, subject, body);
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("EmailService (SendEmail)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendMultiPartEmail(String sender, String recipients, String subject, String textBody, String htmlBody)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                // Initialize the message with the plain text body:
                MailMessage msg = new MailMessage(sender, recipients, subject, textBody);
                // Convert the html body to a memory stream:
                Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(htmlBody);
                MemoryStream htmlStream = new MemoryStream(bytes);
                // Add the HTML body to the message:
                AlternateView htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
                msg.AlternateViews.Add(htmlView);
                // Ship it!
                emailClient.Send(msg);
                htmlView.Dispose();
                htmlStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("EmailService (SendMultiPartEmail)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmailHTML(EmailStruct estruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(estruct.SenderEmail, estruct.ReceiverEmail, estruct.EmailTitle, estruct.EmailBody.Replace(Environment.NewLine, "<br/>"));
                message.IsBodyHtml = true;
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = SMTPUserInfo;
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("EmailService (SendEmailHTML) - email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendEmail(EmailStruct estruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                MailMessage message = new MailMessage(estruct.SenderEmail, estruct.ReceiverEmail, estruct.EmailTitle, estruct.EmailBody);
                emailClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("EmailService (SendEmail)- email send failed: " + ex.Message);
                return false;
            }
        }

        public bool SendMultiPartEmail(HTMLEmailStruct htmlEmailStruct)
        {
            try
            {
                if (!bSMTPInitialized) throw new Exception("Error: SMTP Not initialized");
                // Initialize the message with the plain text body:
                MailMessage msg = new MailMessage(htmlEmailStruct.SenderEmail, htmlEmailStruct.ReceiverEmail, htmlEmailStruct.EmailTitle, htmlEmailStruct.EmailBody);
                // Convert the html body to a memory stream:
                Byte[] bytes = System.Text.Encoding.UTF8.GetBytes(htmlEmailStruct.HTMLBody);
                MemoryStream htmlStream = new MemoryStream(bytes);
                // Add the HTML body to the message:
                AlternateView htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
                msg.AlternateViews.Add(htmlView);
                // Ship it!
                emailClient.Send(msg);
                htmlView.Dispose();
                htmlStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                log.Debug("EmailService (SendMultiPartEmail) - email send failed: " + ex.Message);
                return false;
            }
        }
    }
}
