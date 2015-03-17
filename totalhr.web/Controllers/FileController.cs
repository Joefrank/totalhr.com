using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileManagementService.Implementation;
using FileManagementService.Infrastructure;
using totalhr.Shared;

namespace totalhr.web.Controllers
{
    public class FileController : Controller
    {
        private IFileService _fileService;
        
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        //create list of files uploaded by this user.
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(int FileTypeId)
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
                        fileHandler = new ProfilePictureFileHandler();break;
                }

                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];



                    //Save file content goes here
                    fName = file.FileName;

                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                    }

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
