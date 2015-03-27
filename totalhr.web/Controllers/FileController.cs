using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileManagementService.Implementation;
using FileManagementService.Infrastructure;
using totalhr.Shared;
using totalhr.services.Infrastructure;
using Authentication.Infrastructure;
using ImageGallery.Infrastructure;
using totalhr.Resources;
using System.Configuration;

namespace totalhr.web.Controllers
{
    public class FileController : BaseController
    {
        private IFileService _fileService;
        private IAccountService _accountService;
        private IGalleryService _galleryService;
        private IFileHandlerService fileHandler;

        public FileController(IFileService fileService, IAccountService accountService, IGalleryService galleryService, IOAuthService authService)
            : base(authService)
        {
            _fileService = fileService;
            _accountService = accountService;
            _galleryService = galleryService;
        }

        //create list of files uploaded by this user.
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadProfilePicture()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">this is the type of profile picture (avatar, portrait or small avatar)</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadProfilePicture(int id)
        {
            fileHandler = new ProfilePictureFileHandler(_fileService, _accountService, id);
            var result = UploadTheFile(fileHandler, Request.Files);
            return Json(result.FileId > 0 ? new { Message = result.FullPath } : new { Message = Account.Error_Saving_Picture });
        }

        public ActionResult UploadAvatar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadAvatar(int id)
        {
            fileHandler = new ProfilePictureFileHandler(_fileService, _accountService, id);
            var result = UploadTheFile(fileHandler, Request.Files);
            return Json(result.FileId > 0 ? new { Message = result.FullPath } : new { Message = Account.Error_Saving_Picture });
        }

        public ActionResult UploadSmallAvatar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadSmallAvatar(int id)
        {
            fileHandler = new ProfilePictureFileHandler(_fileService, _accountService, id);
            var result = UploadTheFile(fileHandler, Request.Files);
            return Json(result.FileId > 0 ? new { Message = result.FullPath } : new { Message = Account.Error_Saving_Picture });
        }

        public ActionResult GalleryPhotoUpload(int id)
        {
            ViewBag.AlbumId = id;
            return View("UploadGalleryPhoto");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">this is album id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadGalleryPhoto(int? id = null)
        {
            var albumId = 0;
            Int32.TryParse(Request.Form["albumId"], out albumId);
            IFileHandlerService fileHandler = new PhotoGalleryHandler(_fileService, _galleryService, albumId);
            var result = UploadTheFile(fileHandler, Request.Files);
            return Json(result.FileId > 0 ? new { Message = result.FullPath } : new { Message = Account.Error_Saving_Picture });
        }

        private BaseFileHandler.FileSaveResult UploadTheFile(IFileHandlerService fileHandler, HttpFileCollectionBase files)
        {
            try
            {
                BaseFileHandler.FileSaveResult result = null;

                foreach (var file in from string fileName in files select files[fileName])
                {
                    result = fileHandler.HandleFileCreation(file, CurrentUser.UserId);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new BaseFileHandler.FileSaveResult {FileId = -1};
            }

        }
        
        [HttpPost]
        public JsonResult DeleteGalleryPhoto(int id)
        {
            var photo = _galleryService.GetPhoto(id);
            var result = false;

            if (photo != null)
            {
                result = _galleryService.DeletePhoto(photo.Id);

                //remove the related files
                if (result)
                {
                    var filesToDelete = new string[]{
                        ConfigurationManager.AppSettings["GalleryImagePath"] + "\\Large\\" + photo.FileName ,
                        ConfigurationManager.AppSettings["GalleryImagePath"] + "\\Thumbnail\\" + photo.FileName 
                    };

                    _fileService.RemovePhotoFiles(filesToDelete, photo.FileId);
                }
            }

            return Json(result ? new { Message = Gallery.V_Photo_Deleted } : new { Message = Gallery.V_Photo_Not_Deleted });
        }

    }
}
