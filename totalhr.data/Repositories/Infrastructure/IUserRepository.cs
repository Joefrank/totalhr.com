using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface IUserRepository : IGenericRepository<User>
    {
        List<Profile> GetUserProfiles(int UserId);

        List<Role> GetUserRoles(int UserId);

        List<User> GetCompanyUsers(int companyid);

        IEnumerable<SimpleUser> GetCompanyUsers(int companyid, int excludedUserId);

        List<string> GetUserNamesByIds(List<int> ids);

        IEnumerable<ListItemStruct> GetUserProfile(int userId);

        IEnumerable<ListItemStruct> GetUserProfileByGuid(Guid uniqueid);

        void UpdateUserProfiles(int hdnUserId, string hdnSelectedProfileIds, int updatedByUserId);

        IEnumerable<ListItemStruct> GetUserRole(int userId);

        IEnumerable<ListItemStruct> GetUserRoleByGuid(Guid uniqueid);

        void UpdateUserRoles(int hdnUserId, string hdnSelectedRoleIds, int updatedByUserId);

        IEnumerable<GetUserListForAdmin_Result> GetUserListForAdmin(bool? bShowActive, int languageId);

        IEnumerable<GetUserListForAdmin_Result> SearchUser(UserSearchInfo searchInfo);

        IEnumerable<SearchUserWithPaging_Result> SearchUserWithPaging(UserSearchInfo info);

        bool SaveProfilePicture(UserProfilePicture profilePicture);

        UserProfilePicture GetProfilePicture(int userid);

        string GetProfilePicturePath(int userid);

        UserPersonalInfo GetProfileDetails(string email);
    }
}
