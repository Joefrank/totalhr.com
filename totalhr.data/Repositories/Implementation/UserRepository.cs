using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Implementation
{
    public class UserRepository : GenericRepository<TotalHREntities, User>, IUserRepository
    {
        public List<Profile> GetUserProfiles(int UserId)
        {
            return this.Context.Profiles.SqlQuery("dbo.GetUserProfiles @p0", UserId).ToList(); 
        }

        public List<Role> GetUserRoles(int UserId)
        {
            return this.Context.Roles.SqlQuery("dbo.GetUserRoles @p0", UserId).ToList(); 
        }

        public List<User> GetCompanyUsers(int companyid)
        {
            return this.FindBy(x => x.CompanyId == companyid).ToList();
        }

        public IEnumerable<SimpleUser> GetCompanyUsers(int companyid, int excludedUserId)
        {
            var qry = from user in this.Context.Users
                      where user.CompanyId == companyid && user.id != excludedUserId
                      orderby user.firstname
                      select new SimpleUser { Id = user.id, FullName = user.firstname + " " + user.surname };
            return qry;
        }

        public List<string> GetUserNamesByIds(List<int> ids)
        {
            return this.Context.Users.Where(x => ids.Contains(x.id)).
                Select(y => y.firstname + " " + y.surname).ToList();
        }

        public IEnumerable<ListItemStruct> GetUserProfile(int userId)
        {
            return from profile in this.Context.Profiles
                   join uprofile in Context.UserProfiles on profile.id equals uprofile.ProfileId
                   where uprofile.UserId == userId
                   select new ListItemStruct() { Id = profile.id, Name = profile.Name };
            
        }

        public IEnumerable<ListItemStruct> GetUserProfileByGuid(Guid uniqueid)
        {
            return from profile in this.Context.Profiles
                           join uprofile in Context.UserProfiles on profile.id equals uprofile.ProfileId
                           join user in Context.Users on uprofile.UserId equals user.id
                           where user.userguid == uniqueid
                           select new ListItemStruct() { Id = profile.id, Name = profile.Name };  


        }

        public void UpdateUserProfiles(int hdnUserId, string hdnSelectedProfileIds, int updatedByUser)
        {
            this.Context.UserProfiles.RemoveRange(this.Context.UserProfiles.Where(x => x.UserId == hdnUserId));
            List<int> selectedProfileIds = hdnSelectedProfileIds.Split(',').Select(int.Parse).ToList();
            foreach (var id in selectedProfileIds)
            {
                this.Context.UserProfiles.Add(new UserProfile { UserId = hdnUserId, ProfileId = id, Created = DateTime.Now, CreatedBy = updatedByUser });
            }
            this.Context.SaveChanges();
        }


        public IEnumerable<ListItemStruct> GetUserRole(int userId)
        {
            return from role in this.Context.Roles
                   join ur in Context.UserRoles on role.id equals ur.RoleId
                   where ur.UserId == userId
                   select new ListItemStruct() { Id = role.id, Name = role.Name };
        }

        public IEnumerable<ListItemStruct> GetUserRoleByGuid(Guid uniqueid)
        {
            return from role in this.Context.Roles
                   join ur in Context.UserRoles on role.id equals ur.RoleId
                   join user in Context.Users on ur.UserId equals user.id
                   where user.userguid == uniqueid
                   select new ListItemStruct() { Id = role.id, Name = role.Name };  

        }

        public void UpdateUserRoles(int hdnUserId, string hdnSelectedRoleIds, int updatedByUserId)
        {
            this.Context.UserRoles.RemoveRange(this.Context.UserRoles.Where(x => x.UserId == hdnUserId));

            List<int> selectedRoleIds = hdnSelectedRoleIds.Split(',').Select(int.Parse).ToList();

            foreach (var id in selectedRoleIds)
            {
                this.Context.UserRoles.Add(
                    new UserRole { UserId = hdnUserId, RoleId = id, Created = DateTime.Now, CreatedBy = updatedByUserId });
            }
            this.Context.SaveChanges();
        }

        public IEnumerable<GetUserListForAdmin_Result> GetUserListForAdmin(bool? bShowActive, int languageId)
        {
            return this.Context.GetUserListForAdmin(bShowActive,languageId) as IEnumerable<GetUserListForAdmin_Result>;
        }

        public IEnumerable<GetUserListForAdmin_Result> SearchUser(UserSearchInfo info)
        {
            return this.Context.SearchUser(info.UserId, info.Name, info.UserTypeId, info.DepartmentId, info.Email,
                info.PartialAddress, info.Town, info.County, info.PostCode, info.Phone, info.LanguageId) as IEnumerable<GetUserListForAdmin_Result>;
        }

        public IEnumerable<SearchUserWithPaging_Result> SearchUserWithPaging(UserSearchInfo info)
        {
            return this.Context.SearchUserWithPaging(info.UserId, info.Name, info.UserTypeId, info.DepartmentId, info.Email,
                info.PartialAddress, info.Town, info.County, info.PostCode, info.Phone, info.PageSize, 
                info.PageNumber, info.OrderColumn, info.OrderDirection, info.LanguageId);
        }

        public bool SaveProfilePicture(UserProfilePicture profilePicture)
        {
            var existingUserPictureProfile =
                this.Context.UserProfilePictures.FirstOrDefault(x => x.UserId == profilePicture.UserId && x.ProfilePictureTypeId == profilePicture.ProfilePictureTypeId);

            if (existingUserPictureProfile != null)
            {
                this.Context.UserProfilePictures.Remove(existingUserPictureProfile);
            }

            this.Context.UserProfilePictures.Add(profilePicture);
            this.Context.SaveChanges();
            return true;
        }

        public UserProfilePicture GetProfilePicture(int userid)
        {
            return this.Context.UserProfilePictures.FirstOrDefault(x => x.UserId == userid);
        }

        public string GetProfilePicturePath(int userid)
        {
            var result = (from upp in Context.UserProfilePictures                        
                         where upp.UserId == userid && upp.ProfilePictureTypeId == (int)Variables.ProfilePictureType.Portrait
                         select upp).FirstOrDefault();

            return result != null ? result.FileName : "";
        }

        public UserProfileDetails GetProfileDetails(int userId, string email)
        {
            var userprofile = this.Context.GetUserProfileDetails(userId, email).FirstOrDefault();

            if (userprofile != null)
            {
                var customfields = from ecf in this.Context.EmployeeCustomFields
                                   join cf in this.Context.CustomFields on ecf.CustomFieldId equals cf.Id
                                   where ecf.EmployeeId == userprofile.UserId
                                   select new UserProfileDetails.CustomField
                                   {
                                       CustomFieldId = ecf.CustomFieldId,
                                       CustomFieldIdentifier = cf.Identifier,
                                       CustomFieldValue = ecf.Value
                                   };

                var profilepictures = from pic in this.Context.UserProfilePictures
                                     join filex in this.Context.Files on pic.FileId equals  filex.id
                                     where pic.UserId == userprofile.UserId
                                     select new UserProfileDetails.ProfilePicture
                                     {
                                         FileName = pic.FileId + filex.extension,
                                         PictureTypeId = pic.ProfilePictureTypeId,
                                         FileId = pic.FileId,
                                         ImageHeight = pic.Height,
                                         ImageWidth = pic.Width
                                     };

                //EnumExtensions.ParseEnum<Variables.ProfilePictureType>(pic.ProfilePictureType)
                var personalInfo = new UserProfileDetails
                    {
                        UserId = userprofile.UserId,
                        Gender = userprofile.Gender,
                        Title = userprofile.Title,
                        OtherTitle = userprofile.OtherTitle,
                        Address1 = userprofile.Address1,
                        Address2 = userprofile.Address2,
                        Address3 = userprofile.Address3,
                        ChoosenCulture = userprofile.ChosenCulture,
                        City = userprofile.City,
                        Country = userprofile.Country,
                        Department = userprofile.Department,
                        DetailsLastUpdated = (userprofile.DetailsLastUpdated != null)? userprofile.DetailsLastUpdated .Value.ToShortDateString() : "",
                        DetailsLastUpdatedBy = userprofile.DetailsLastUpdatedBy,
                        Email = userprofile.Email,
                        FirstName = userprofile.firstname,
                        Surname = userprofile.Surname,
                        JoinDate = userprofile.JoinDate.ToShortDateString(),
                        LastVisit = (userprofile.LastVisit != null)? userprofile.LastVisit.Value.ToShortDateString() : "",
                        LineManager = userprofile.LineManager,
                        MiddleNames = userprofile.MiddleNames,
                        MobilePhone = userprofile.MobilePhone,
                        NoOfVisits = userprofile.NoOfVisits,
                        Password = userprofile.Password,
                        PersonalPhone = userprofile.PersonalPhone,
                        PostCode = userprofile.PostCode,
                        PreferedLanguage = userprofile.PreferedLanguage,
                        State = userprofile.State,
                        UserName = userprofile.UserName,
                        UserProfilePicture =  profilepictures.ToList(),
                        CustomFields = (customfields.Any()) ? customfields.ToList() : new List<UserProfileDetails.CustomField>()
                    };

                return personalInfo;
            }

            return null;
        }
    }
}
