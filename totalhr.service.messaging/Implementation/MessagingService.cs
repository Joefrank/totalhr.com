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
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Shared.Models;
using totalhr.data.Repositories.Infrastructure;
using log4net;
using CM;

namespace totalhr.services.messaging.Implementation
{
    public class MessagingService : IMessagingService
    {
        private IEmailService _emailService;
        private IEmailRepository _mailRepos;
        private SMTPSettings _smtpsettings;
        private static readonly ILog log = LogManager.GetLogger(typeof(MessagingService));
        private readonly IUserRepository _userrepos;

        public MessagingService(IEmailService emailService, IEmailRepository mailRepos, IUserRepository userrepos)
        {
            _emailService = emailService;
            _mailRepos = mailRepos;
            _userrepos = userrepos;
        }

        public void ReadSMTPSettings(SMTPSettings smtpsettings)
        {
            _smtpsettings = smtpsettings;
            _emailService.InitSMTP(smtpsettings.SMTPServer, smtpsettings.UserName, smtpsettings.Password);
        }

        public void SendPasswordReminderEmail(AdminStruct adminstruct, User user)
        {
            EmailTemplate template = _mailRepos.FindBy(x => x.Identifier.Equals(Variables.EmailTemplateIds.PasswordReminder.ToString())).FirstOrDefault();

            NotifyUser(new EmailStruct
            {
                SenderName = adminstruct.AdminName,
                SenderEmail = adminstruct.AdminEmail,
                ReceiverName = user.firstname + " " + user.surname,
                ReceiverEmail = user.email,
                EmailTitle = template.Subject,
                EmailBody = string.Format(template.Template, user.firstname, user.username, user.email, CM.Security.Decrypt(user.password),
                    Variables.AdminEmailSignature)

            });
        }

        public void AcknowledgeNewUserRegistration(AdminStruct adminstruct, UserRegStruct userstruct)
        {
            //Send notification to both Admin and user instructions
            EmailTemplate template = _mailRepos.FindBy(x => x.Identifier.Equals(Variables.EmailTemplateIds.NewUserWelcome.ToString())).FirstOrDefault();
            EmailTemplate adminTemplate = _mailRepos.FindBy(x => x.Identifier.Equals(Variables.EmailTemplateIds.AdminNewUserNotify.ToString())).FirstOrDefault();
            
            //welcome user
            NotifyUser(new EmailStruct
            {
                SenderName = adminstruct.AdminName,
                SenderEmail = adminstruct.AdminEmail,
                ReceiverName = userstruct.Name + " " + userstruct.Surname,
                ReceiverEmail = userstruct.Email,
                EmailTitle = template.Subject,
                EmailBody = string.Format(template.Template, userstruct.Name, adminstruct.WebsiteName, userstruct.ActivationLink,
                    adminstruct.WebsiteName, Variables.AdminEmailSignature)

            });

            log.Debug("AcknowledgeNewUserRegistration - user notified ");

            //Notify admin
            NotifyUser(new EmailStruct
            {
                SenderName = adminstruct.AdminName,
                SenderEmail = adminstruct.AdminEmail,
                ReceiverName = adminstruct.AdminName,
                ReceiverEmail = adminstruct.AdminEmail,
                EmailTitle = template.Subject,
                EmailBody = string.Format(adminTemplate.Template, userstruct.Name + " " + userstruct.Surname,
                    userstruct.Email, userstruct.UserId, Variables.AdminEmailSignature)

            });

            log.Debug("AcknowledgeNewUserRegistration - Admin notified ");
        }

        public void AcknowledgeAccountActivation(AdminStruct adminstruct, User user)
        {
            EmailTemplate userTemplate = _mailRepos.FindBy(x => x.Identifier.Equals(Variables.EmailTemplateIds.AccountActivated.ToString())).FirstOrDefault();
            EmailTemplate adminTemplate = _mailRepos.FindBy(x => x.Identifier.Equals(Variables.EmailTemplateIds.AdminAccountActivated.ToString())).FirstOrDefault();

            //notify user
            NotifyUser(new EmailStruct
            {
                SenderName = adminstruct.AdminName,
                SenderEmail = adminstruct.AdminEmail,
                ReceiverName = user.firstname + " " + user.surname,
                ReceiverEmail = user.email,
                EmailTitle = userTemplate.Subject,
                EmailBody = string.Format(userTemplate.Template, user.firstname, adminstruct.WebsiteName, Variables.AdminEmailSignature)

            });

            log.Debug("AcknowledgeAccountActivation - user notified ");

            //Notify admin
            NotifyUser(new EmailStruct
            {
                SenderName = adminstruct.AdminName,
                SenderEmail = adminstruct.AdminEmail,
                ReceiverName = adminstruct.AdminName,
                ReceiverEmail = adminstruct.AdminEmail,
                EmailTitle = adminTemplate.Subject,
                EmailBody = string.Format(adminTemplate.Template, user.firstname + " " + user.surname,
                    user.email, user.id, Variables.AdminEmailSignature)

            });

            log.Debug("AcknowledgeAccountActivation - Admin notified ");
        }

        public bool NotifyUserOfCalendarEvent(AdminStruct adminstruct, CalendarEventInfo eventinfo)
        {
            var userTemplate = _mailRepos.FindBy(x => x.Identifier.Equals(Variables.EmailTemplateIds.CalendarEventNotification.ToString())).FirstOrDefault();
            var lstUsers = new List<User>();
           
            if (eventinfo.TargetAttendeeGroupId == (int) Variables.CalendarEventTarget.User)
            {
                List<int> lstUserids = eventinfo.InvitedUserIds.Split(',').Select(int.Parse).ToList();
                lstUsers = _userrepos.FindBy(x => lstUserids.Contains(x.id)).ToList();
            }
            else if(eventinfo.TargetAttendeeGroupId == (int) Variables.CalendarEventTarget.Department)
            {
                List<int> lstDepartments = eventinfo.InvitedDepartmentIds.Split(',').Select(int.Parse).ToList();
                lstUsers = _userrepos.FindBy(x => lstDepartments.Contains(x.departmentid)).ToList();
            }
            else if (eventinfo.TargetAttendeeGroupId == (int) Variables.CalendarEventTarget.MyselfOnly)
            {
                lstUsers = _userrepos.FindBy(x => x.id == eventinfo.CreatedBy).ToList();
            }
            else if (eventinfo.TargetAttendeeGroupId == (int) Variables.CalendarEventTarget.Company)
            {
                lstUsers = _userrepos.FindBy(x => x.CompanyId == eventinfo.CompanyId).ToList();
            }

            // try using multiple recipient emails
            foreach (var user in lstUsers)
            {
                NotifyUser(new EmailStruct
                    {
                        SenderName = adminstruct.AdminName,
                        SenderEmail = adminstruct.AdminEmail,
                        ReceiverName = user.firstname + " " + user.surname,
                        ReceiverEmail = user.email,
                        EmailTitle = string.Format(userTemplate.Subject, eventinfo.Title),
                        EmailBody =
                            string.Format(userTemplate.Template, user.firstname, eventinfo.Title,
                                          adminstruct.SiteRootURL, Variables.AdminEmailSignature)
                    });
            }          

            return true;
        }

        public bool NotifyUser(EmailStruct estruct)
        {
            return _emailService.SendEmailHTML(estruct.ReceiverEmail, estruct.ReceiverName, estruct.SenderEmail, estruct.SenderName, estruct.EmailTitle, estruct.EmailBody);
        }
       
    }
}
