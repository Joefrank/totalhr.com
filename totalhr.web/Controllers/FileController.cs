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

        public ActionResult UploadFile()
        {
            var dicoHiddenValues = new Dictionary<string, string>();
            //provide this for photo gallery
            if (Request.QueryString["albumid"] != null)
            {
                dicoHiddenValues.Add("AlbumId", Request.QueryString["albumid"]);
            }
            //provide this for all uploads
            if (Request.QueryString["fileTypeId"] != null)
            {
                dicoHiddenValues.Add("FileTypeId", Request.QueryString["fileTypeId"]);
            }

            return View(dicoHiddenValues);
        }

        [HttpPost]
        public ActionResult UploadFile(int FileTypeId, int AlbumId)
        {
            //typegroup (profile image, avatar, companydocument, Gallery Image)
            //determine the type of file and pass it to correct handler to handle file. if there is no type passed then just upload file.
            
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                IFileHandlerService fileHandler;

                switch (FileTypeId)
                {
                    case (int)Variables.FileType.ProfilePicture:
                        fileHandler = new ProfilePictureFileHandler(_fileService, _accountService);break;
                    case (int)Variables.FileType.GalleryImage:
                        fileHandler = new PhotoGalleryHandler(_fileService, _galleryService, AlbumId);break;
                    default: fileHandler = new BaseFileHandler(_fileService); break;
                }

                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    fileHandler.HandleFileCreation(file, CurrentUser.UserId, FileTypeId);
                    fName = file.FileName;
                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
            return View("Index");
        }
    }
}
