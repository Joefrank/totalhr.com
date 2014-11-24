using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using CompanyDocumentService.Infrastructure;
using totalhr.data.Repositories.Infrastructure;
using totalhr.Shared.Models;

namespace CompanyDocumentService.Implementation
{
    public class DocumentManager : IDocumentManager
    {
        ICompanyDocumentRepository _docRepos;

        public DocumentManager(ICompanyDocumentRepository docRepos)
        {
            _docRepos = docRepos;
        }

        #region Folders

        public int CreateFolder(string displayName, bool MakePublic, int createdBy)
        {
            CompanyFolder folder = new CompanyFolder();
            folder.DisplayName = displayName;
            folder.CreatedBy = createdBy;
            folder.Created = DateTime.Now;
            folder.OpenedPublic = MakePublic;

            return _docRepos.AddFolder(folder);
        }

        public int UpdateFolder(int folderId, string displayName, int updatedBy)
        {
            return 0;
        }

        public void DeleteFolder(int folderId, int deletedBy)
        {
            
        }

        public List<CompanyFolder> ListFolders()
        {
            return null;
        }

        public List<CompanyFolder> ListFoldersByUser(int userId)
        {
            return _docRepos.ListFoldersByUser(userId);
        }

        public List<CompanyDocument> ListDocumentAndFoldersByUser(int userId, int userDepartmentId)
        {
            return _docRepos.ListDocumentAndFoldersByUser(userId, userDepartmentId);
        }

        public CompanyFolder GetFolder(int folderId)
        {
            return _docRepos.GetFolder(folderId);
        }

        #endregion folders


        #region Documents

        public List<CompanyDocument> SearchDocument(DocumentSearchInfo info)
        {
            return _docRepos.FindBy(x =>
                (string.IsNullOrEmpty(info.Name) || x.FolderDisplayName.Contains(info.Name))
                &&
                (string.IsNullOrEmpty(info.FileMimeType) || x.FileMimeType == info.FileMimeType)
                &&
                (info.FolderId == 0 || info.FolderId == x.FolderId)
                &&
                (info.AuthorId == 0 || info.AuthorId == x.CreatedBy)
                &&
                (info.StartDate == DateTime.MinValue || x.Created >= info.StartDate)
                &&
                (info.EndDate == DateTime.MinValue || x.Created <= info.EndDate)
                ).ToList();
        }

        public int UpdateDocument(DocumentInfoUpdate info)
        {
            CompanyDocument doc = _docRepos.FindBy(x => x.Id == info.DocId).FirstOrDefault(); ;
            doc.DisplayName = info.DisplayName;
            doc.OriginalFileName = (info.File == null)? doc.OriginalFileName : info.File.FileName;
            doc.FolderId = (info.FolderId > 0) ? info.FolderId : 0;
            doc.FolderDisplayName = (info.FolderId > 0) ? _docRepos.GetFolder(info.FolderId).DisplayName : "";
            doc.LastUpdated = DateTime.Now;
            doc.LastUpdatedBy = info.LastUpdatedBy;
            doc.PermissionTypeId = info.PermissionSelection;
            doc.ReadableType = info.ReadableType;
            doc.ReadableSize = info.ReadableSize;
            doc.FileMimeType = info.FileMimeType;

            _docRepos.Save();
            return doc.Id;
        }

        public int CreateDocument(DocumentInfo info)
        {
            CompanyDocument doc = new CompanyDocument();
            doc.DisplayName = info.DisplayName;
            doc.OriginalFileName = info.File.FileName;
            doc.FileId = info.NewFileId;
            doc.FolderId = (info.FolderId > 0)? info.FolderId : 0;
            doc.FolderDisplayName = (info.FolderId > 0)? this.GetFolder(info.FolderId).DisplayName : "";
            doc.Created = DateTime.Now;
            doc.CreatedBy = info.CreatedBy;
            doc.PermissionTypeId = info.PermissionSelection;
            doc.ReadableType = info.ReadableType;
            doc.ReadableSize = info.ReadableSize;
            doc.FileMimeType = info.FileMimeType;

            _docRepos.Add(doc);
            _docRepos.Save();

            return doc.Id;
        }

        public void UpdateDocViewCount(int docId, int increment, int userId)
        {
            CompanyDocument doc = _docRepos.FindBy(x => x.Id == docId).FirstOrDefault();
            if (doc != null)
            {
                doc.NoOfViews += increment;
                doc.LastUpdatedBy = userId;
                doc.LastUpdated = DateTime.Now;
                _docRepos.Save();
            }
        }

        public void UpdateDocDownloadCount(int docId, int increment, int userId)
        {
            CompanyDocument doc = _docRepos.FindBy(x => x.Id == docId).FirstOrDefault();
            if (doc != null)
            {

                doc.LastUpdatedBy = userId;
                doc.LastUpdated = DateTime.Now;
                doc.NoOfDownloads += increment;
                _docRepos.Save();
            }
        }

        public void CreateDocsPermission(DocumentInfo info)
        {
            if (info.PermissionSelectionValue == null)
                return;

            if (info.PermissionSelectionValue.Contains(','))
            {
                List<int> objids = info.PermissionSelectionValue.Split(',').Select(int.Parse).ToList();

                if (objids != null)
                {
                    _docRepos.CreateDocumentPermissions(objids, info.CreatedBy, info.PermissionSelection, info.DocId);
                }
            }
            else
            {
                _docRepos.CreateDocumentPermissions(Convert.ToInt32(info.PermissionSelectionValue), info.CreatedBy, info.PermissionSelection, info.DocId);
            }
        }

        public void UpdateDocsPermission(DocumentInfoUpdate info)
        {
            _docRepos.RemoveDocPermissions(info.DocId);
            CreateDocsPermission(info);           
        }

        public List<string> GetPermissionObjectNames(List<CompanyDocumentPermission> permissions)
        {
            return _docRepos.GetPermissionObjectNames(permissions);
        }

        public int UpdateDocument(int documentId, string documentDisplayName, string originalFileName, int folderId, int updatedBy, IO.FileInfo file = null)
        {
            return 0;
        }

        public void DeleteDocument(int documentId, int deletedBy)
        {
            
        }

        public void ShareDocumentByEmail(int documentId, int shearerId)
        {
            
        }

        public string GetDocumentLink(int documentId, int userId)
        {
            return "";
        }

        public List<CompanyDocument> ListDocuments()
        {
            return null;
        }

        public List<CompanyDocument> ListDocumentsByUser(int userId)
        {
            return null;
        }

        public CompanyDocument GetDocument(int docId)
        {
            return _docRepos.FindBy(x => x.Id == docId).FirstOrDefault();
        }

        public CompanyDocument GetDocumentWithViewCountUpdate(int docId, int userId)
        {
            CompanyDocument doc  = _docRepos.FindBy(x => x.Id == docId).FirstOrDefault();
            if (doc != null)
            {
                doc.NoOfViews++;
                _docRepos.Save();
            }
            return doc;
        }

        public CompanyDocument GetDocumentWithDownloadCountUpdate(int docId, int userId)
        {
            CompanyDocument doc = _docRepos.FindBy(x => x.Id == docId).FirstOrDefault();
            if (doc != null)
            {
                doc.NoOfDownloads++;
                _docRepos.Save();
                doc.LastUpdated = DateTime.Now;
                doc.LastUpdatedBy = userId;
            }
            return doc;
        }

        public void Archive(int docId, int userId)
        {
            CompanyDocument doc = _docRepos.FindBy(x => x.Id == docId).FirstOrDefault();
            doc.Archived = true;
            doc.LastUpdated = DateTime.Now;
            doc.LastUpdatedBy = userId;
            _docRepos.Save();
        }

        #endregion Documents

    }
}
