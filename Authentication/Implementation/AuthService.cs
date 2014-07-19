using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Authentication.Models;
using log4net;

namespace Authentication.Implementation
{   

    public class AuthService : IAuthService
    {
        private readonly ClientUser _user = new ClientUser();
        private FormsAuthenticationTicket _ticket;
        private HttpContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthService));

        public FormsAuthenticationTicket Ticket
        {
            get
            {
                if (_ticket == null)
                {
                    try
                    {
                        _context = HttpContext.Current;
                        var cookie = _context.Request.Cookies[FormsAuthentication.FormsCookieName];
                        
                        if(cookie != null)
                            _ticket = FormsAuthentication.Decrypt(cookie.Value);
                    }
                    catch
                    {

                    }
                }
                return _ticket;
            }

            set { _ticket = value; }
        }

        public AuthService()
        {
            _context = HttpContext.Current;
        }

        public string GetUserName()
        {
            return (Ticket != null)? Ticket.Name : "";
        }

        public string GetFullName()
        {
            try
            {
                return (Ticket == null)
                           ? "" : GetDataAtIndex(Ticket.UserData, 3);
            }
            catch (Exception ex)
            {
                log.Debug("GetFullName - Could not parse user full name" + ex.Message);
                return null;
            }
        }

        public int GetUserId()
        {
            try
            {
                return (Ticket == null)
                           ? -1
                           : GetIntDataAtIndex(Ticket.UserData,0);
            }
            catch (Exception ex)
            {
                log.Debug("GetUserId - Could not parse user data" + ex.Message);
                return -2;
            }
        }

        public int GetUserLanguageId()
        {
            try
            {
                return (Ticket == null)
                           ? -1
                           : GetIntDataAtIndex(Ticket.UserData, 5);
            }
            catch (Exception ex)
            {
                log.Debug("GetUserLanguageId - Could not parse user languageid" + ex.Message);
                return -2;
            }
        }

        public string[] GetUserProfiles()
        {
            try
            {
                return (Ticket == null)
                           ? null: GetDataAtIndex(Ticket.UserData,2).Split(',');
            }
            catch (Exception ex)
            {
                log.Debug("GetUserProfiles - Could not parse user data" + ex.Message);
                return null;
            }
        }

        public string[] GetUserRoles()
        {
            try
            {
                return (Ticket == null)
                           ? null
                           : GetDataAtIndex(Ticket.UserData,1).Split(',');
            }
            catch (Exception ex)
            {
                log.Debug("GetUserRoles - Could not parse user data" + ex.Message);
                return null;
            }
        }

        public string GetCulture()
        {
            try
            {
                return (Ticket == null)
                           ? "" : GetDataAtIndex(Ticket.UserData, 4);
            }
            catch (Exception ex)
            {
                log.Debug("GetCulture - Could not parse user culture" + ex.Message);
                return null;
            }
        }

        private string GetDataAtIndex(string oData, int index)
        {
            return string.IsNullOrEmpty(oData)? "" : oData.Split('|')[index].Split(':')[1];
        }

        private int GetIntDataAtIndex(string oData, int index)
        {
            return string.IsNullOrEmpty(oData) ? -1 : Int32.Parse(oData.Split('|')[index].Split(':')[1]);
        }

        public ClientUser GetUser()
        {
            try
            {
                var profiles = GetUserProfiles();//user may not have profiles

                return new ClientUser
                    {
                        Profiles = (profiles == null) ? new List<string>() : profiles.ToList(),
                        Roles = GetUserRoles().ToList(),//user must have some roles
                        UserId = GetUserId(),
                        UserName = GetUserName(),
                        FullName = GetFullName(),
                        Culture = GetCulture(),
                        LanguageId = GetUserLanguageId()
                    };
            }
            catch(Exception ex)
            {
                log.Debug("GetUser - Could not compile user data" + ex.Message);
            }

            return null;
        }

        public void PersistUser(ClientUser user, int noHours, int noMins, int noSecons)
        {
            try
            {
                var ticket = new FormsAuthenticationTicket(
                   1, //ticket version
                   user.UserName,
                   DateTime.Now,
                   DateTime.Now.Add(new TimeSpan(0, noHours, noMins, noSecons)), //timeout
                   true, //persistent cookies
                   user.Data,
                   FormsAuthentication.FormsCookiePath);

                string hashedTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashedTicket);

                _context.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                log.Debug("PersistUser - Failed to persist user data" + ex.Message);
            }
        }


        public void LogUserOut()
        {
            try
            {
                _context.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                _context.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);

                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                    FormsAuthentication.SignOut();
            }
            catch (Exception ex)
            {
                log.Debug("LogUserOut failed." + ex.Message);
            }
        }

    }
}