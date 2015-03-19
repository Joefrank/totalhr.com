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
using totalhr.Shared;

namespace FileManagementService.Implementation
{
    public class ProfilePictureFileHandler : BaseFileHandler, IFileHandlerService
    {        
        protected IAccountService AccountService;
        
        public ProfilePictureFileHandler(IFileService fileService, IAccountService accountService)
            : base(fileService)
        {
            FileService = fileService;
            AccountService = accountService;
            UploadPath = ConfigurationManager.AppSettings["ProfilePicturePath"];
            base.FileTypeId = (int)Variables.FileType.ProfilePicture;
            base.OverridePath(this.DirectoryPath);
        }

        public override BaseFileHandler.FileSaveResult HandleFileCreation(HttpPostedFileBase postedFile, int creatorId)
        {
            var fileResult = base.HandleFileCreation(postedFile, creatorId);

            if (fileResult.FileId > 0)
            {
                var pictureWidth = 0;
                var pictureHeight = 0;

                using (var fs = new FileStream(fileResult.FullPath, FileMode.Open, FileAccess.Read))
                {
                    using (var original = System.Drawing.Image.FromStream(fs))
                    {
                        pictureWidth = original.Width;
                        pictureHeight = original.Height;
                    }
                }

                //***apply resizing
                var profilePicture = new UserProfilePicture
                    {
                        Created = DateTime.Now,
                        CreatedBy = creatorId,
                        FileId = fileResult.FileId,
                        UserId = creatorId,
                        Width = pictureWidth,
                        Height = pictureHeight
                    };

                var result = AccountService.SaveProfilePicture(profilePicture);

                if (result)
                {
                    return fileResult;
                }
                
            }

            return new BaseFileHandler.FileSaveResult { FileId = -1 };
        }
    }
}
