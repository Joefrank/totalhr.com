using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FileManagementService.Infrastructure;
using totalhr.data.EF;
using totalhr.services.Infrastructure;

namespace FileManagementService.Implementation
{
    public class ProfilePictureFileHandler : IFileHandlerService
    {
        protected IFileService FileService ;
        protected IAccountService AccountService;

        public string DirectoryPath
        {
            get { return ConfigurationManager.AppSettings["ProfilePicturePath"]; }
        }

        public ProfilePictureFileHandler(IFileService fileService, IAccountService accountService)
        {
            FileService = fileService;
            AccountService = accountService;
        }

        public int HandleFileCreation(HttpPostedFileBase postedFile, int creatorId, int fileTypeId)
        {
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                var pathString = HttpContext.Current.Server.MapPath(DirectoryPath);
                
                var fileId = this.FileService.Create(postedFile, pathString, creatorId, fileTypeId);

                if (fileId > 0)
                {
                    var picturePath = Path.Combine(pathString, fileId + Path.GetExtension(postedFile.FileName));
                    var pictureWidth = 0;
                    var pictureHeight = 0;

                            using (var fs = new FileStream(picturePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var original = System.Drawing.Image.FromStream(fs))
                                {
                                    pictureWidth = original.Width;
                                    pictureHeight = original.Height;
                                }
                            }

                      var profilePicture = new UserProfilePicture
                          {
                              Created = DateTime.Now,
                              CreatedBy = creatorId,
                              FileId = fileId,
                              UserId = creatorId,
                              Width = pictureWidth,
                              Height = pictureHeight
                          };

                     var result = AccountService.SaveProfilePicture(profilePicture);
                    if (result)
                    {
                        return fileId;
                    }
                }
            }
            return -1;
        }
    }
}
