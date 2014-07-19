using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.Security;
using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Models;
using CM;
using log4net;

namespace Authentication.Implementation
{
    public class OckAuthService : IOAuthService
    {
        private const string CkNameUserId = "UserId";
        private const string CkNameUserFullName = "UserName";
        private const string CkNameEmail = "Email";
        private const string CkNameCulture = "Culture";
        private const string CkNameLanguageId = "LanguageId";
        private const string CkNameCompanyId = "CompanyId";
        private const string CkNameProfiles = "Profiles";
        private const string CkNameRoles = "Roles";

        private readonly HttpContext _context;
        private readonly ClientUser _user = new ClientUser();
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthService));


        public string UserFullName
        {
            get { return ReadEncryptedCookie<string>(CkNameUserFullName); }
        }

        public int UserId
        {
            get { return ReadEncryptedCookie<int>(CkNameUserId); }
        }

        public int UserLanguageId
        {
            get { return ReadEncryptedCookie<int>(CkNameLanguageId); }
        }

        public  int UserCompanyId
        {
            get { return ReadEncryptedCookie<int>(CkNameCompanyId); }
        }
        public string UserProfiles
        {
            get { return ReadEncryptedCookie<string>(CkNameProfiles); }
        }

        public string UserRoles
        {
            get { return ReadEncryptedCookie<string>(CkNameRoles); }
        }

        public  string UserCulture
        {
            get { return ReadEncryptedCookie<string>(CkNameCulture); }
        }

        public string UserEmail
        {
            get { return ReadEncryptedCookie<string>(CkNameEmail); }
        }

        public OckAuthService()
        {
            _context = HttpContext.Current;
        }

        public HttpCookie GenerateCookie(string cookieName, string cookieValue, TimeSpan tmDuration)
        {
            var cookie = new HttpCookie(cookieName)
            {
                Value = !string.IsNullOrEmpty(cookieValue) ? cookieValue : "",
                Expires = DateTime.Now + tmDuration
            };

            return cookie;
        }

        public bool PlaceClientCookie(string cookieName, string ckValue, TimeSpan tmDuration)
        {
            var ck = GenerateCookie(cookieName, ckValue, tmDuration);
            HttpContext.Current.Response.Cookies.Add(ck);
            return true;
        }

        public bool DeleteClientCookie(string cookieName)
        {
            return PlaceClientCookie(cookieName, "", new TimeSpan(-1, 0, 0, 0));
        }

        /// <summary>
        /// Generates an encrypted cookie based on values supplied in parameters
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="tmDuration"></param>
        /// <returns></returns>
        public HttpCookie GenerateEncryptedCookie(string cookieName, string cookieValue, TimeSpan tmDuration)
        {
            var cookie = new HttpCookie(cookieName)
                {
                    Value = !string.IsNullOrEmpty(cookieValue) ? Security.Encrypt(cookieValue) : "",
                    Expires = DateTime.Now + tmDuration
                };

            return cookie;
        }

        /// <summary>
        /// Places an encrypted cookie to client.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="ckValue"></param>
        /// <param name="tmDuration"></param>
        /// <returns></returns>
        public bool PlaceClientEncryptedCookie(string cookieName, string ckValue, TimeSpan tmDuration)
        {
            var ck = GenerateEncryptedCookie(cookieName, ckValue, tmDuration);
            HttpContext.Current.Response.Cookies.Add(ck);
            return true;
        }

        /// <summary>
        /// Gets an encrypted cookie from client and returns type as requested
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public T ReadEncryptedCookie<T>(string cookieName)
        {
              
                try
                {
                    var cookie = HttpContext.Current.Request.Cookies[cookieName];
                    if (cookie == null)
                        return default(T);

                    var converter = TypeDescriptor.GetConverter(typeof(T));

                    if (converter.CanConvertFrom(typeof (string)))
                    {
                        string value = Security.Decrypt(cookie.Value);
                        return (T) converter.ConvertFromString(value);
                    }
                    else
                    {
                        return default(T);
                    }
                   
                }
                catch { return default(T); }
        }

        public ClientUser GetClientUser()
        {
            try
            {
                var lstProfiles = !string.IsNullOrEmpty(UserProfiles)? 
                    (UserProfiles.Split(',').Select(s => s)).ToList() : new List<string>();
                var lstRoles = UserRoles.Split(',').Select(s => s).ToList();

                return new ClientUser
                    {
                        Profiles = lstProfiles,
                        Roles = lstRoles,//user must have some roles
                        UserId = this.UserId,
                        UserName = this.UserEmail,
                        FullName = this.UserFullName,
                        Culture = this.UserCulture,
                        LanguageId = this.UserLanguageId,
                        CompanyId = this.UserCompanyId
                    };
            }
            catch(Exception ex)
            {
                Log.Debug("GetUser - Could not compile user data" + ex.Message);
            }

            return null;
        }

        public void PersistClientUser(ClientUser user)
        {
            try
            {
                var sProfiles = String.Join(",", user.Profiles.Select(x => x).ToArray());
                var sRoles = String.Join(",", user.Roles.Select(x => x).ToArray());

                PlaceClientEncryptedCookie(CkNameUserId, user.UserId.ToString(CultureInfo.InvariantCulture), user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameEmail, user.UserName, user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameUserFullName, user.FullName, user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameCulture, user.Culture, user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameLanguageId, user.LanguageId.ToString(CultureInfo.InvariantCulture), user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameCompanyId, user.CompanyId.ToString(), user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameProfiles, sProfiles, user.CookieDuration);
                PlaceClientEncryptedCookie(CkNameRoles,sRoles , user.CookieDuration);
            }
            catch (Exception ex)
            {
                Log.Debug("PersistUser - Failed to persist user data username " + user.UserName + " exception: " + Environment.NewLine + ex.Message);
            }
        }


        public void LogUserOut()
        {
            try
            {
                DeleteClientCookie(CkNameUserId);
                DeleteClientCookie(CkNameEmail);
                DeleteClientCookie(CkNameUserFullName);
                DeleteClientCookie(CkNameCulture);
                DeleteClientCookie(CkNameLanguageId);
                DeleteClientCookie(CkNameProfiles);
                DeleteClientCookie(CkNameRoles);
                DeleteClientCookie(CkNameCompanyId);
            }
            catch (Exception ex)
            {
                Log.Debug("LogUserOut failed." + ex.Message);
            }
        }

        public bool IsUserLoggedIn()
        {
            return UserId > 0 && UserCompanyId > 0 && !string.IsNullOrEmpty(UserRoles) && 
                !string.IsNullOrEmpty(UserEmail) && !string.IsNullOrEmpty(UserFullName);
        }
    }
}
