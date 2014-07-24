using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class ClientUser
    {
        public List<string> Profiles { get; set; }

        public List<string> Roles { get; set; }

        public string FullName { get; set; }
       
        public string UserName { get; set; }

        public string Data { get; set; }

        public int UserId { get; set; }

        public int LanguageId { get; set; }

        public int CompanyId { get; set; }

        public string Culture { get; set; }

        public TimeSpan CookieDuration { get; set; }

        public bool IsInRole(List<string> roles)
        {
            return !roles.Except(this.Roles).Any();
        }

        public bool IsInProfile(List<string> profiles)
        {
            return !profiles.Except(this.Profiles).Any();
        }

        public bool IsInProfile(IEnumerable<int> profiles)
        {
            return !profiles.Except(this.Profiles.Cast<int>()).Any();
        }

        public bool IsLogged()
        {
            return (UserId > 0 && LanguageId > 0 && CompanyId > 0 && Roles != null && Roles.Count > 0);
        }
    }
}
