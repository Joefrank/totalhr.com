using FileManagementService.Infrastructure;
using ImageGallery.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using totalhr.Shared;

namespace FileManagementService.Implementation
{
    public class PhotoGalleryHandler : BaseFileHandler, IFileHandlerService
    {
        private readonly IGalleryService _galleryService;
        private int AlbumId { get; set; }

        public PhotoGalleryHandler(IFileService fileService, IGalleryService galleryService, int albumId)
            : base(fileService)
        {           
            _galleryService = galleryService;
            UploadPath = ConfigurationManager.AppSettings["GalleryImagePath"];
            base.FileTypeId = (int)Variables.FileType.GalleryImage;
            base.OverridePath(this.DirectoryPath);
            AlbumId = albumId;
        }

        public override BaseFileHandler.FileSaveResult HandleFileCreation(HttpPostedFileBase postedFile, int creatorId)
        {
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                var fileResult = base.HandleFileCreation(postedFile, creatorId);

                if (fileResult.FileId > 0)
                {
                    var photoId = _galleryService.AddPhoto(new totalhr.Shared.Models.GalleryPhotoInfo{
                         AlbumId = this.AlbumId,
                         FileId = fileResult.FileId,
                         UserId = creatorId
                    });

                    return fileResult;
                }
            }
            return new BaseFileHandler.FileSaveResult { FileId = -1 };
        }
    }
}
