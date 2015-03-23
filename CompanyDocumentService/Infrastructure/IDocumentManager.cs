using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Models;

namespace CompanyDocumentService.Infrastructure
{
    public interface IDocumentManager
    {
        
        int CreateFolder(string displayName, bool makePublic, int createdBy);
        int UpdateFolder(int folderId, string displayName, int updatedBy);
        void DeleteFolder(int folderId, int deletedBy);
        List<CompanyFolder> ListFolders();
        List<CompanyFolder> ListFoldersByUser(int userId);
        CompanyFolder GetFolder(int folderId);

        int CreateDocument(DocumentInfo info);
        void CreateDocsPermission(DocumentInfo info);
        int UpdateDocument(int documentId, string documentDisplayName, string originalFileName, int folderId, int updatedBy, IO.FileInfo file=null);
        int UpdateDocument(DocumentInfoUpdate info);
        void DeleteDocument(int documentId, int deletedBy);
        List<CompanyDocument> ListDocuments();
        List<CompanyDocument> ListDocumentsByUser(int userId);
        CompanyDocument GetDocument(int docid);
        CompanyDocument GetDocument(Guid docid);
        
        List<string> GetPermissionObjectNames(List<CompanyDocumentPermission> permissions);
        List<CompanyDocument> ListDocumentAndFoldersByUser(int userId, int userDepartmentId);
        void UpdateDocsPermission(DocumentInfoUpdate info);
        void UpdateDocViewCount(int docId, int increment, int userId);
        void UpdateDocDownloadCount(int docId, int increment, int userId);

        CompanyDocument GetDocumentWithViewCountUpdate(int docId, int userId);
        CompanyDocument GetDocumentWithViewCountUpdate(Guid docId, int userId);

        CompanyDocument GetDocumentWithDownloadCountUpdate(int docId, int userId);
        CompanyDocument GetDocumentWithDownloadCountUpdate(Guid docId, int userId);

        void Archive(int docId, int userId);
        void Archive(Guid docId, int userId);

        List<CompanyDocument> SearchDocument(DocumentSearchInfo info);

        void ShareDocumentByEmail(int documentId, int shearerId);
        string GetDocumentLink(int documentId, int userId);

    }
}
