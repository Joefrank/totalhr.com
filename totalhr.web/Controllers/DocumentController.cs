using Authentication.Infrastructure;
using CompanyDocumentService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;
using totalhr.Shared.Models;
using totalhr.Shared;
using totalhr.Resources;
using System.IO;
using FileManagementService.Infrastructure;
using totalhr.data.EF;

namespace totalhr.web.Controllers
{
    public class DocumentController : BaseController
    {
        private IOAuthService _authService;
        private IDocumentManager _docService;
        private IFileUploadService _fileService;

        public DocumentController(IOAuthService authservice, IDocumentManager docManager, IFileUploadService fileService)
            : base(authservice)
        {
            _authService = authservice;
            _docService = docManager;
            _fileService = fileService;
            ViewBag.CompanyId = CurrentUser.CompanyId;
        }

        public ActionResult Index()
        {
            return View(_docService.ListDocumentAndFoldersByUser(CurrentUser.UserId, CurrentUser.CompanyId));
        }

        public ActionResult Create()
        {            
            return View();
        }

        public ActionResult Details(int id)
        {
            CompanyDocument doc = _docService.GetDocument(id);

            DocumentInfoUpdate info = new DocumentInfoUpdate
            {
               Id = id,
               DisplayName = doc.DisplayName,
               ExistingFileName = doc.OriginalFileName,
               FolderId = doc.FolderId.GetValueOrDefault()
            };
            return View(info);
        }

        [HttpPost]
        public JsonResult CreateFolderJSon(string DisplayName)
        {
            int folderid = _docService.CreateFolder(DisplayName, CurrentUser.UserId);
            return Json(new { FolderId = folderid, FolderName = DisplayName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateDocument(DocumentInfo info)
        {
            if (ModelState.IsValid)
            {
                if (info.File == null)
                {
                    ModelState.AddModelError("File", Document.Error_Upload_File);
                }
                else if (info.File.ContentLength > 0)
                {

                    string fileExtension = Path.GetExtension(info.File.FileName).Replace(".", "");
                    bool isValidFile = Enum.GetNames(typeof(Variables.AllowedFileExtension)).Contains(fileExtension);
                    

                    if (!isValidFile)
                    {
                        string allValidExtensions = string.Join(",", Enum.GetNames(typeof(Variables.AllowedFileExtension)));
                        ModelState.AddModelError("File", Document.Error_File_Type + allValidExtensions);
                    }
                    else
                    {
                        //upload the file
                        int newFileId = _fileService.Create(info.File, Server.MapPath("~/CompanyDocuments/") + CurrentUser.CompanyId,
                                CurrentUser.UserId, (int)Variables.FileType.CompanyDocument);

                        var fileName = Path.GetFileNameWithoutExtension(info.File.FileName);

                        //if success then create document
                        if (newFileId > 0)
                        {
                            int docid = _docService.CreateDocument(info.DisplayName, fileName, newFileId, info.FolderId, CurrentUser.UserId);
                            if (docid < 1)
                                ViewBag.Message = Document.Error_Could_Not_Create_Doc;
                            return View("Index");
                        }
                        else
                        {
                            ViewBag.Message = Document.Error_Could_Not_Upload_Doc;
                            return View("Create");
                        }                        
                    }
                }
            }
            
            return View("Create");            

        }



    }
}
