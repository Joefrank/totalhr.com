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
            UploadPath = GetUploadPath(pictureType);
            base.FileTypeId = (int)Variables.FileType.ProfilePicture;
            base.OverridePath(this.DirectoryPath);
        }

        private string GetUploadPath(int picturetype)
        {
            var path = string.Empty;

            switch (picturetype)
            {
                case (int)Variables.ProfilePictureType.Portrait:
                    path = "ProfilePicturePath";
                    break;
                case (int)Variables.ProfilePictureType.Avatar:
                    path = "AvatarPicturePath";
                    break;
                case (int)Variables.ProfilePictureType.SmallAvatar:
                    path = "SmallAvatarPicturePath";
                    break;
            }
            return ConfigurationManager.AppSettings[path];
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
                var maxSize = GetSizeByPictureType(PictureType);
                var newsize = maxSize;

                if (maxSize.Width == pictureWidth && maxSize.Height == pictureHeight)
                {
                    newsize = new Size(pictureWidth ,pictureHeight);
                }else{
                    newsize = ResizeProfilePicture(fileResult.FullPath, maxSize);
                }                

                var profilePicture = new UserProfilePicture
                    {
                        Created = DateTime.Now,
                        CreatedBy = creatorId,
                        FileId = fileResult.FileId,
                        UserId = creatorId,
                        Width = newsize.Width,
                        Height = newsize.Height,
                        ProfilePictureTypeId = PictureType,
                        FileName = fileResult.FileId + Path.GetExtension(fileResult.FullPath)
                    };

                var result = AccountService.SaveProfilePicture(profilePicture);

                if (result)
                {
                    return fileResult;
                }
                
            }

            return new BaseFileHandler.FileSaveResult { FileId = -1 };
        }

        public static Size GetSizeByPictureType(int profilePictureType){

            switch (profilePictureType)
            {
                case (int)Variables.ProfilePictureType.Portrait:
                    return Variables.ProfilePictureMaxSize.PortraitSize;                   
                case (int)Variables.ProfilePictureType.Avatar:
                    return Variables.ProfilePictureMaxSize.AvatarSize;                   
                case (int)Variables.ProfilePictureType.SmallAvatar:
                    return Variables.ProfilePictureMaxSize.SmallAvatarSize;
            }
            return new Size { Width = 50, Height = 50 };
        }

        public Size ResizeProfilePicture(string picturePath, Size size)
        {
            var photoBytes = System.IO.File.ReadAllBytes(picturePath);

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

            return size;
        }

    }
}
