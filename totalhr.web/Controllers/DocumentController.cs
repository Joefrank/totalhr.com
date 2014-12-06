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
using totalhr.services.messaging.Infrastructure;

namespace totalhr.web.Controllers
{
    public class DocumentController : BaseController
    {
        private IOAuthService _authService;
        private IDocumentManager _docService;
        private IFileService _fileService;
        private IAccountService _accountService;
        private ICompanyService _companyService;
        private readonly IMessagingService _messagingService;

        public string DocPath{
            get{return Path.Combine(Server.MapPath("~/CompanyDocuments/") , CurrentUser.CompanyId.ToString());}
        }

        public DocumentController(IOAuthService authservice, IDocumentManager docManager, IFileService fileService,
            IAccountService acctService, ICompanyService companyService, IMessagingService messageService)
            : base(authservice)
        {
            _authService = authservice;
            _docService = docManager;
            _fileService = fileService;
            _accountService = acctService;
            _companyService = companyService;
            _messagingService = messageService;
            ViewBag.CompanyId = (CurrentUser != null)? CurrentUser.CompanyId : 0;
        }

        public ActionResult Index()
        {
            ViewBag.CurrentUserId = CurrentUser.UserId;
            return View(_docService.ListDocumentAndFoldersByUser(CurrentUser.UserId, CurrentUser.DepartmentId));
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

        public ActionResult Edit(Guid id)
        {
            CompanyDocument doc = _docService.GetDocument(id);

            //don't allow non owner to edit somebody else's doc
            if (doc.CreatedBy != CurrentUser.UserId)
            {
                return RedirectToAction("AccessDenied", "Error", new { ModelError = Document.Error_Cant_Edit_Others_Doc });
            }
             
            var docperms = doc.CompanyDocumentPermissions.ToList();
            ViewBag.DocumentPermission = docperms;
            ViewBag.Folders = _docService.ListFoldersByUser(CurrentUser.UserId);

            DocumentInfoUpdate info = new DocumentInfoUpdate
            {
                DocId = doc.Id,
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
                            info.ReadableSize = _fileService.BytesToString(info.File.ContentLength);
                            info.ReadableType = Path.GetExtension(info.File.FileName).Replace(".", "");
                            info.FileMimeType = _fileService.GetMimeType(info.File.FileName);

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
            ViewBag.Folders = _docService.ListFoldersByUser(CurrentUser.UserId);
            return View("Create", info);           

        }

        [HttpPost]
        public ActionResult UpdateDocument(DocumentInfoUpdate info)
        {
            //don't allow non owner to edit somebody else's doc
            CompanyDocument doc = _docService.GetDocument(info.DocId);
            if (doc.CreatedBy != CurrentUser.UserId)
            {
                return RedirectToAction("AccessDenied", "Error", new { ModelError = Document.Error_Cant_Edit_Others_Doc });
            }
            
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

                    info.ReadableSize = _fileService.BytesToString(info.File.ContentLength);
                    info.ReadableType = Path.GetExtension(info.File.FileName).Replace(".", "");
                    info.FileMimeType = _fileService.GetMimeType(info.File.FileName);

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

        public FileResult OpenFile(Guid id)
        {
            CompanyDocument doc = _docService.GetDocumentWithViewCountUpdate(id, CurrentUser.UserId);
            FileResult fr =  File(DocPath + "\\" + doc.FileId + Path.GetExtension(doc.OriginalFileName), doc.FileMimeType);
            fr.FileDownloadName = doc.OriginalFileName;
            return fr;
        }

        public FileResult Download(Guid id)
        {
            CompanyDocument doc = _docService.GetDocumentWithDownloadCountUpdate(id, CurrentUser.UserId);
            byte[] fileBytes = _fileService.ReadFileBytes(DocPath + "\\" + doc.FileId + Path.GetExtension(doc.OriginalFileName));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, doc.OriginalFileName);
        }

        public ActionResult Delete(Guid id)
        {
            CompanyDocument doc = _docService.GetDocument(id);
            if (doc.CreatedBy != CurrentUser.UserId)
            {
                return RedirectToAction("AccessDenied", "Error", new { ModelError = Document.Error_Cant_Edit_Others_Doc });
            }
            _docService.Archive(id, CurrentUser.UserId);
            return RedirectToAction("Index");
        }

        public ActionResult Search(DocumentSearchInfo info)
        {
            info.SearchingUserSepartmentId = CurrentUser.DepartmentId;
            info.SearchingUserId = CurrentUser.UserId;
            ViewBag.CurrentUserId = CurrentUser.UserId;
            string SearchCriteria = string.Empty;

            SearchCriteria = string.IsNullOrEmpty(info.Name) ? "" : " <b>" + Document.V_With_Document_Name + ":</b> " + info.Name;
            SearchCriteria += (info.StartDate > DateTime.MinValue) ? " <b>" + Document.V_Created_After + ":</b> " + info.StartDate.ToShortDateString() : "";
            SearchCriteria += (info.EndDate > DateTime.MinValue) ? " <b>" + Document.V_Created_Before + ":</b> " + info.EndDate.ToShortDateString() : "";
            SearchCriteria += (info.AuthorId > 0) ? " <b>" + Document.V_Contributed_By + ":</b> " + info.ContributorName : "";
            SearchCriteria += (info.FolderId > 0) ? " <b>" + Document.V_In_Folder + ":</b> " + info.FolderName : "";
            SearchCriteria += (string.IsNullOrEmpty(info.FileMimeType) || info.FileMimeType  == "0") ? "" : " <b>" + Document.V_With_File_Type + ":</b> " + info.FileTypeName;

            // ** pass everything to the Model and handle it from there.
            ViewBag.SearchCriteria = !string.IsNullOrEmpty(SearchCriteria) ? Document.V_Your_Search + SearchCriteria : Document.V_No_Search_Criteria;

            ModelState["StartDate"].Errors.Clear();//get rid of empty date validation

            if (!ModelState.IsValid) {               
                return View("Index", _docService.ListDocumentAndFoldersByUser(CurrentUser.UserId, CurrentUser.DepartmentId));
            }

            var lstDocs = _docService.SearchDocument(info);
            return View("Index", lstDocs);
        }

        public ActionResult Share(Guid id)
        {
            var doc = _docService.GetDocument(id);

            DocumentShareInfo info = new DocumentShareInfo
            {
                DocumentId = doc.Id,
                DocumentName = doc.DisplayName,
                FileName = doc.OriginalFileName,
                DocumentLink = string.Format("{0}Document/OpenFile/{1}", WebsiteKernel.SiteRootURL, doc.Identifier)
            };

            ViewBag.UserList = _accountService.GetCompanyUsersSimple(CurrentUser.CompanyId, CurrentUser.UserId);
            return View(info);
        }

        [HttpPost]
        public ActionResult Share(DocumentShareInfo info)
        {
            ViewBag.UserList = _accountService.GetCompanyUsersSimple(CurrentUser.CompanyId, CurrentUser.UserId);

            if (!ModelState.IsValid)
            {                
                return View(info);
            }

            var recipientEmail = string.Empty;
            var recipientName = string.Empty;

            if(string.IsNullOrEmpty(info.WithUserEmail) && info.WithUserId < 1){
               ModelState.AddModelError("NoRecipient", "No Recipient has been specified");
            }
            else if(!string.IsNullOrEmpty(info.WithUserEmail))
            {
                recipientEmail = info.WithUserEmail; 
            }
            else if(info.WithUserId > 0)
            {
                var user = _accountService.GetUser(info.WithUserId);
                recipientEmail = user.email;
                recipientName = user.firstname + " " + user.surname;
            }

            if (!string.IsNullOrEmpty(recipientEmail))
            {
                _messagingService.ReadSMTPSettings(SiteMailSettings);

                var emailStruct = new HTMLEmailStruct
                {
                    SenderEmail = CurrentUser.UserName,
                    SenderName = CurrentUser.FullName,
                    ReceiverEmail = recipientEmail,
                    ReceiverName = "",
                    EmailBody = info.Message,
                    EmailTitle = string.Format(Document.V_Share_Doc_Email_Title, CurrentUser.FullName),
                    AttachmentPath = DocPath + "\\" + info.DocumentId + Path.GetExtension(info.FileName),
                    AttachmentName = info.FileName
                };
                
                bool result = _messagingService.EmailUserWithAttachment(emailStruct);  // .SendEmailWithAttachment(emailStruct);
                
                if (result)
                {
                    if (!string.IsNullOrEmpty(recipientName))
                        info.WithUserEmail = recipientName + " - (" + recipientEmail + ")";
                    return View("ShareComplete", info);
                }
            }
                        
            return View(info);
  
        }
    }
}
