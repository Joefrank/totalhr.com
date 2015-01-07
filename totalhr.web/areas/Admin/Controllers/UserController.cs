using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using totalhr.data.EF;
using totalhr.services.Infrastructure;
using totalhr.Shared;
using totalhr.Shared.Models;
using totalhr.web.Areas.Admin.Models;
using totalhr.Resources;
using System.IO;
using FileManagementService.Infrastructure;


namespace totalhr.web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly ICompanyService _companyService;
        private readonly IGlossaryService _glossaryService;
        private IFileService _fileService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));

        public UserController(IGlossaryService glossaryService, IAccountService accountService, IProfileService profileService,
            ICompanyService companyService, IFileService fileService, IOAuthService authService) :
            base(authService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _companyService = companyService;
            _glossaryService = glossaryService;
            _fileService = fileService;
        }

        public ActionResult Index()
        {
            return View(DoSearch());
        }

        public ActionResult Page(int id)
        {
            return View("Index", DoSearch(id, Request.Form["SortColumn"], Request.Form["SortDirection"]));
        }

        public UserSearchResult DoSearch(int pageNumber = 1, string SearchColumn = "", string SearchDirection = "")
        {
            var searchInfo = new UserSearchInfo
            {
                PageNumber = pageNumber,
                PageSize = DefaultPageSize,
                UserList = _accountService.ListCompanyUsersSimple(CurrentUser.CompanyId),
                DepartmentList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId),
                HrefLocation = "/Admin/User/",
                LanguageId = CurrentUser.LanguageId,
                OrderColumn = SearchColumn,
                OrderDirection = SearchDirection
            };

            var searchResult = new UserSearchResult{
                FoundUsers = _accountService.SearchUserWithPaging(searchInfo),
                SearchInfo = searchInfo
            };

            return searchResult;
        }

        public ActionResult SortBy(int hdnPageNumber, string SortColumn, string SortDirection)
        {
            hdnPageNumber = hdnPageNumber < 1 ? 1 : hdnPageNumber;
            return View("Index", DoSearch(hdnPageNumber, SortColumn, SortDirection));
        }

        public ActionResult UserDetails(string uniqueid)
        {
            var uadminstruct = _accountService.GetUserDetailsForAdmin(uniqueid);

            return View(uadminstruct);
        }

        public ActionResult SearchForUser(UserSearchInfo info)
        {
            info.UserList = _accountService.ListCompanyUsersSimple(CurrentUser.CompanyId);
            info.DepartmentList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId);
            info.HrefLocation = "/Admin/User/SearchForUser/";
            info.LanguageId = CurrentUser.LanguageId;

            var result = new UserSearchResult
            {
                FoundUsers = _accountService.SearchUserWithPaging(info),
                SearchInfo = info
            };
            return View("SearchResult", result);
        }

        private void LoadGlossaries()
        {            
            ViewBag.CountryList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Country);
            ViewBag.GenderList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Gender);
            ViewBag.TitleList = _glossaryService.GetGlossary(this.ViewingLanguageId, Variables.GlossaryGroups.Title);           
        }

        public ActionResult Create()
        {
            LoadGlossaries();
            var languageList = _glossaryService.GetLanguageList(CurrentUser.LanguageId);
            var deptList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId);
            return View(new NewEmployeeInfo { 
                CompanyId = CurrentUser.CompanyId, 
                Password = "", PasswordConfirm = "", UserName = "",
                DepartmentList = deptList, TermsAccepted = true,
                CreatedBy = CurrentUser.UserId,
                LanguageList = languageList
            });
        }

        [HttpPost]
        public ActionResult Create(NewEmployeeInfo info)
        {
            if (info.DepartmentId < 1)
            {
                ModelState.AddModelError("Missing_Department", Error.Error_Missing_Department); 
            }

            if (ModelState.IsValid)
            {
                var userstruct = _accountService.CreateEmployee(info, WebsiteKernel);

                if (userstruct == null || userstruct.UserId < 1)
                {                   
                    ModelState.AddModelError("RegistrationFailed", userstruct.RegError);
                    Log.Debug("Employee Registration failed " + userstruct.RegError);
                }
                
                if (info.PictureFile == null)
                {
                    ModelState.AddModelError("PictureFile", Error.Error_Profile_Picture_Missing);
                }
                else if (info.PictureFile.ContentLength > 0 && userstruct != null && userstruct.UserId > 0)
                {

                    string fileExtension = Path.GetExtension(info.PictureFile.FileName).Replace(".", "");
                    bool isValidFile = Enum.GetNames(typeof(Variables.AllowedImageExtensions)).Contains(fileExtension, StringComparer.CurrentCultureIgnoreCase);

                    if (!isValidFile)
                    {
                        string allValidExtensions = string.Join(",", Enum.GetNames(typeof(Variables.AllowedImageExtensions)));
                        ModelState.AddModelError("PictureFile", Error.Error_File_Type + allValidExtensions);
                    }
                    else
                    {
                        info.CreatedBy = CurrentUser.UserId;

                        //upload picture the file
                        int newFileId = _fileService.Create(info.PictureFile, ProfilePicturePath, 
                                CurrentUser.UserId, (int)Variables.FileType.ProfilePicture);

                        var fileName = Path.GetFileNameWithoutExtension(info.PictureFile.FileName);

                        //if success then create document
                        if (newFileId > 0)
                        {
                            UserProfilePicture profilePicture = new UserProfilePicture();

                            profilePicture.Created = DateTime.Now;
                            profilePicture.CreatedBy = CurrentUser.UserId;
                            profilePicture.FileId = newFileId;
                            profilePicture.UserId = userstruct.UserId;                           

                            string picturePath = Path.Combine(ProfilePicturePath, newFileId + Path.GetExtension(info.PictureFile.FileName));

                            using (FileStream fs = new FileStream(picturePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var original = System.Drawing.Image.FromStream(fs))
                                {
                                    profilePicture.Width = original.Width;
                                    profilePicture.Height = original.Height;
                                }
                            }
                                                     

                            bool result = _accountService.SaveProfilePicture(profilePicture);


                            if (!result)
                            {
                                ModelState.AddModelError("Unable_To_save_profile_picture", Error.Error_Could_Not_Save_Picture);
                            }
                            else
                            {
                                return View("UserCreated", info);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Unable_To_save_profile_picture", Error.Error_Could_Not_Save_Picture);
                        }
                    }
                }
            }

            LoadGlossaries();
            var deptList = _companyService.GetDepartmentSimple(CurrentUser.CompanyId);
            var languageList = _glossaryService.GetLanguageList(CurrentUser.LanguageId);
            return View(new NewEmployeeInfo
            {
                CompanyId = CurrentUser.CompanyId,               
                DepartmentList = deptList,
                CreatedBy = CurrentUser.UserId,
                LanguageList = languageList
            });
        }
    }
}
