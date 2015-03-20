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

namespace totalhr.web.Controllers
{
    public class FileController : BaseController
    {
        private IFileService _fileService;
        private IAccountService _accountService;
        private IGalleryService _galleryService;
        
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
            IFileHandlerService fileHandler = new ProfilePictureFileHandler(_fileService, _accountService, id);
            var result = UploadTheFile(fileHandler, Request.Files);
            return Json(result.FileId > 0 ? new { Message = result.FullPath } : new { Message = "Error in saving file" });
        }

        public ActionResult GalleryPhotoUpload(int id)
        {
            return View("UploadGalleryPhoto");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">this is album id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadGalleryPhoto(int id)
        {
            IFileHandlerService fileHandler = new PhotoGalleryHandler(_fileService, _galleryService, id);
            var result = UploadTheFile(fileHandler, Request.Files);
            return Json(result.FileId > 0 ? new { Message = result.FullPath } : new { Message = "Error in saving file" });
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
        public ActionResult UploadFile(int FileTypeId)
        {
            //typegroup (profile image, avatar, companydocument, Gallery Image)
            //determine the type of file and pass it to correct handler to handle file. if there is no type passed then just upload file.
            
            //bool isSavedSuccessfully = true;
            //string fName = "";
            //try
            //{
            //    IFileHandlerService fileHandler;

            //    switch (FileTypeId)
            //    {
            //        case (int)Variables.FileType.ProfilePicture:
            //            fileHandler = new ProfilePictureFileHandler(_fileService, _accountService);break;
            //        case (int)Variables.FileType.GalleryImage:
            //            fileHandler = new PhotoGalleryHandler(_fileService, _galleryService, AlbumId);break;
            //        default: fileHandler = new BaseFileHandler(_fileService); break;
            //    }

            //    foreach (string fileName in Request.Files)
            //    {
            //        var file = Request.Files[fileName];
            //        fileHandler.HandleFileCreation(file, CurrentUser.UserId, FileTypeId);
            //        fName = file.FileName;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    isSavedSuccessfully = false;
            //}


            //if (isSavedSuccessfully)
            //{
            //    return Json(new { Message = fName });
            //}
            //else
            //{
            //    return Json(new { Message = "Error in saving file" });
            //}
            return View("Index");
        }
    }
}
