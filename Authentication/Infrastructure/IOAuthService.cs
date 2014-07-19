using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Authentication.Models;

namespace Authentication.Infrastructure
{
    public interface IOAuthService
    {
        HttpCookie GenerateEncryptedCookie(string cookieName, string cookieValue, TimeSpan tmDuration);

        bool PlaceClientEncryptedCookie(string cookieName, string ckValue, TimeSpan tmDuration);

        T ReadEncryptedCookie<T>(string cookieName);

        ClientUser GetClientUser();

        void PersistClientUser(ClientUser user);

        void LogUserOut();

        bool IsUserLoggedIn();
    }
}
