using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FileManagementService.Infrastructure;
using ImageProcessor;
using totalhr.data.EF;
using totalhr.services.Infrastructure;
using totalhr.Shared;

namespace FileManagementService.Implementation
{
    public class ProfilePictureFileHandler : BaseFileHandler, IFileHandlerService
    {        
        protected IAccountService AccountService;
        
        public int PictureType { get; set; }

        public ProfilePictureFileHandler(IFileService fileService, IAccountService accountService, int pictureType)
            : base(fileService)
        {
            FileService = fileService;
            AccountService = accountService;
            PictureType = pictureType;
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

                //resize file to correct dimensions
                ResizeProfilePicture(fileResult.FullPath, PictureType);

                var profilePicture = new UserProfilePicture
                    {
                        Created = DateTime.Now,
                        CreatedBy = creatorId,
                        FileId = fileResult.FileId,
                        UserId = creatorId,
                        Width = pictureWidth,
                        Height = pictureHeight,
                        ProfilePictureTypeId = PictureType
                    };

                var result = AccountService.SaveProfilePicture(profilePicture);

                if (result)
                {
                    return fileResult;
                }
                
            }

            return new BaseFileHandler.FileSaveResult { FileId = -1 };
        }

        public void ResizeProfilePicture(string picturePath, int profilePictureTypeId)
        {
            var photoBytes = System.IO.File.ReadAllBytes(picturePath);

            var size = new Size(400,400);

            switch (profilePictureTypeId)
            {
                case (int)Variables.ProfilePictureType.Portrait:
                    size = Variables.ProfilePictureMaxSize.PortraitSize;
                    break;
                case (int)Variables.ProfilePictureType.Avatar:
                    size = Variables.ProfilePictureMaxSize.AvatarSize;
                    break;
                case (int)Variables.ProfilePictureType.SmallAvatar:
                    size = Variables.ProfilePictureMaxSize.SmallAvatarSize;
                    break;
            }

            // process image
            using (var inStream = new MemoryStream(photoBytes))
            {
                using (var imageFactory = new ImageFactory(true))
                {
                    imageFactory.Load(inStream)
                    .Resize(size)
                    .Save(picturePath);
                }
            }
          
        }

    }
}
