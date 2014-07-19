using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.services.Infrastructure;
using System.Web;
using System.Web.Security;
using totalhr.data.EF;

namespace totalhr.services.Implementation
{
    public class AuthService : IAuthService
    {

        public bool LogInUser(User user, int duration)
        {   
            var tkt = new FormsAuthenticationTicket(1, user.email, DateTime.Now,
               DateTime.Now.AddMinutes(duration), false, user.id.ToString());

            string sCookieVal = FormsAuthentication.Encrypt(tkt);
            var ckCookie = new HttpCookie(FormsAuthentication.FormsCookieName, sCookieVal);
            ckCookie.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Add(ckCookie);
            return true;
        }

        public bool LogInUser(totalhr.data.Models.UserDetailsStruct userstruct, int duration)
        {
            //pick up roles and profiles.
            var tkt = new FormsAuthenticationTicket(1, userstruct.UserBasicDetails.email, DateTime.Now,
               DateTime.Now.AddMinutes(duration), false, userstruct.UserBasicDetails.id.ToString());

            string sCookieVal = FormsAuthentication.Encrypt(tkt);
            var ckCookie = new HttpCookie(FormsAuthentication.FormsCookieName, sCookieVal);
            ckCookie.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Add(ckCookie);
            return true;
        }

        public void LogUserOut()
        {
            if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                FormsAuthentication.SignOut();
        }
    }
}
