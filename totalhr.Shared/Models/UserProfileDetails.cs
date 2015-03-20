using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Models
{
    public class UserProfileDetails
    {
        public int UserId { get; set; }

        public string Gender { get; set; }

        public string Title { get; set; }

        public string OtherTitle { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string MiddleNames { get; set; }

        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostCode { get; set; }

        public string Country { get; set; }

        public string PersonalPhone { get; set; }

        public string MobilePhone { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PreferedLanguage { get; set; }

        public int NoOfVisits { get; set; }

        public string LastVisit { get; set; }

        public string JoinDate { get; set; }

        public string Department { get; set; }

        public string LineManager { get; set; }

        public string DetailsLastUpdated { get; set; }

        public string DetailsLastUpdatedBy { get; set; }

        public string ChoosenCulture { get; set; }

        public List<CustomField> CustomFields { get; set; }

        public List<ProfilePicture> UserProfilePicture { get; set; }
        
        public class CustomField
        {
            public int CustomFieldId { get; set; }

            public string CustomFieldIdentifier { get; set; }

            public string CustomFieldValue { get; set; }
        }

        public class ProfilePicture
        {
            public string FileName { get; set; }

            public int FileId { get; set; }
           
            public int ImageWidth { get; set; }

            public int ImageHeight { get; set; }

            public int PictureTypeId { get; set; }
        }
              
    }
}
