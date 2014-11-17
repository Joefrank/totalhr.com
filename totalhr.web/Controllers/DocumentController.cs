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
        private IFileService _fileService;
        private IAccountService _accountService;
        private ICompanyService _companyService;

        public string DocPath{
            get{return Path.Combine(Server.MapPath("~/CompanyDocuments/") , CurrentUser.CompanyId.ToString());}
        }

        public DocumentController(IOAuthService authservice, IDocumentManager docManager, IFileService fileService,
            IAccountService acctService, ICompanyService companyService)
            : base(authservice)
        {
            _authService = authservice;
            _docService = docManager;
            _fileService = fileService;
            _accountService = acctService;
            _companyService = companyService;
            ViewBag.CompanyId = (CurrentUser != null)? CurrentUser.CompanyId : 0;
        }

        public ActionResult Index()
        {
            ViewBag.CurrentUserId = CurrentUser.UserId;
            return View(_docService.ListDocumentAndFoldersByUser(CurrentUser.UserId, CurrentUser.CompanyId));
        }

        public ActionResult Create()
        {
            ViewBag.Folders = _docService.ListFoldersByUser(CurrentUser.UserId);
            return View(new DocumentInfo { DocId = 0 });
        }

        public ActionResult Details(int id)
        {
            CompanyDocument doc = _docService.GetDocument(id);
            string permissionSummary = string.Empty;

            //*** if user request a doc id that doesn't exist or he doesn't have access to redirect to no access page.

            if(doc.PermissionTypeId == (int)Variables.DocumentPermissionType.Private)
            {
                permissionSummary = Document.V_Permission_Private;
            }
            else if (doc.PermissionTypeId == (int)Variables.DocumentPermissionType.WholeCompany)
            {
                permissionSummary = Document.V_Permission_Company;
            }
            else if (doc.PermissionTypeId == (int)Variables.DocumentPermissionType.SelectedUsers)
            {
                var lstNames = _docService.GetPermissionObjectNames(doc.CompanyDocumentPermissions.ToList());
                permissionSummary = EnumExtensions.Description(Variables.DocumentPermissionType.SelectedUsers) + ": (" +
                    string.Join(",", lstNames.ToArray()) + ")";
            }
            else if (doc.PermissionTypeId == (int)Variables.DocumentPermissionType.Department)
            {
                var lstNames = _docService.GetPermissionObjectNames(doc.CompanyDocumentPermissions.ToList());
                permissionSummary = EnumExtensions.Description(Variables.DocumentPermissionType.Department) + ": (" +
                    string.Join(",", lstNames.ToArray()) + ")";
            }
            
            ViewBag.PermissonSummary = permissionSummary;
            return View(doc);
        }

        public ActionResult Edit(int id)
        {
            CompanyDocument doc = _docService.GetDocument(id);

            //*** if user request a doc id that doesn't exist or he doesn't have access to redirect to no access page.

            var docperms = doc.CompanyDocumentPermissions.ToList();
            ViewBag.DocumentPermission = docperms;
            ViewBag.Folders = _docService.ListFoldersByUser(CurrentUser.UserId);

            DocumentInfoUpdate info = new DocumentInfoUpdate
            {
                DocId = id,
                OldFileId = doc.FileId,
                DisplayName = doc.DisplayName,
                ExistingFileName = doc.OriginalFileName,
                FolderId = doc.FolderId.GetValueOrDefault(),
                PermissionSelection = doc.PermissionTypeId.GetValueOrDefault()
            };            

            //we need to get items (users or depts names to display them
            if (info.PermissionSelection == (int)Variables.DocumentPermissionType.Department)
            {
                List<int> lstDepartmentIds = docperms.Select(x => x.ObjectId).ToList();
                info.PermissionItemNames = _companyService.GetCompanyDepartmentsByIds(lstDepartmentIds);
                info.PermissionSelectionValue = string.Join(",", lstDepartmentIds.ToArray());               
            }
            else if (info.PermissionSelection == (int)Variables.DocumentPermissionType.SelectedUsers)
            {
                var lstUsers = docperms.Select(x => x.ObjectId).ToList();
                info.PermissionItemNames = _accountService.GetUserNamesByIds(lstUsers);
                info.PermissionSelectionValue = string.Join(",", lstUsers.ToArray());
            }
           

            return View("EditDocument", info);
        }

        public ActionResult CreateFolder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFolder(string DisplayName, int MakePublic)
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                ModelState.AddModelError("Error_Folder_DisplayName", Document.Error_DisplayName_Rq);
                return View("CreateFolder");
            }
            int folderid = _docService.CreateFolder(DisplayName, (MakePublic == 1), CurrentUser.UserId);
            return View("Index");
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
                        info.CreatedBy = CurrentUser.UserId;
                       
                        //upload the file
                        int newFileId = _fileService.Create(info.File, DocPath, CurrentUser.UserId, 
                            (int)Variables.FileType.CompanyDocument);

                        var fileName = Path.GetFileNameWithoutExtension(info.File.FileName);

                        //if success then create document
                        if (newFileId > 0)
                        {
                            info.NewFileId = newFileId;
                            int docid = _docService.CreateDocument(info);

                            if (docid < 1)
                            {
                                ModelState.AddModelError("Unable_To_create_doc", Document.Error_Could_Not_Create_Doc);
                            }
                            else
                            {
                                info.DocId = docid;

                                if (info.PermissionSelection > 0)
                                {
                                    _docService.CreateDocsPermission(info);
                                }
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Unable_To_Upload_doc", Document.Error_Could_Not_Upload_Doc);                            
                        }                        
                    }
                }
            }
            
            return View("Create");            

        }

        [HttpPost]
        public ActionResult UpdateDocument(DocumentInfoUpdate info)
        {
            //*** check if user request a is allowed to modify this doc rect to no access page.
            
            if (ModelState.ContainsKey("File"))
                ModelState["File"].Errors.Clear();

            if (ModelState.IsValid)
            {
                info.LastUpdatedBy = info.CreatedBy = CurrentUser.UserId;

                //if there is a change of file, save the update
                if (info.File != null && info.File.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(info.File.FileName).Replace(".", "");
                    bool isValidFile = Enum.GetNames(typeof(Variables.AllowedFileExtension)).Contains(fileExtension);


                    if (!isValidFile)
                    {
                        string allValidExtensions = string.Join(",", Enum.GetNames(typeof(Variables.AllowedFileExtension)));
                        ModelState.AddModelError("File", Document.Error_File_Type + allValidExtensions);
                    }
                    
                    //upload the file
                    _fileService.UpdateWith(info.OldFileId, info.File, Server.MapPath("~/CompanyDocuments/") + 
                        CurrentUser.CompanyId, CurrentUser.UserId, (int)Variables.FileType.CompanyDocument);
                }

                int result = _docService.UpdateDocument(info);

                if (result > 0)
                {                    
                    _docService.UpdateDocsPermission(info);
                    return RedirectToAction("Index");               

                }
                else
                {
                    ModelState.AddModelError("Unable_To_Update_doc", Document.Error_Could_Not_Update_Doc);
                }             

                
            }

            ModelState.AddModelError("Unable_To_Update_doc", Document.Error_Could_Not_Update_Doc);
            return RedirectToAction("Edit", "Document", new {id= info.DocId });

        }

        public FileResult Download(int id)
        {
            CompanyDocument doc = _docService.GetDocument(id);
            byte[] fileBytes = _fileService.ReadFileBytes(DocPath + "\\" + doc.FileId + Path.GetExtension(doc.OriginalFileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, doc.OriginalFileName);
        }

        public ActionResult Delete(int id)
        {
            //*** check that use has right to delete this doc otherwise no access page.
            return null;
        }
    }
}
