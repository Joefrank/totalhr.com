using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Models;

namespace Authentication.Infrastructure
{
    public interface IAuthService
    {
        string GetUserName();

        int GetUserId();

        int GetUserLanguageId();

        string[] GetUserProfiles();

        string[] GetUserRoles();

        string GetCulture();

        ClientUser GetUser();

        void PersistUser(ClientUser user, int noHours, int noMins, int noSecons);

        void LogUserOut();
    }
}
