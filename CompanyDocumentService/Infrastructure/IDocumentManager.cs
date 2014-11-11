using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace CompanyDocumentService.Infrastructure
{
    public interface IDocumentManager
    {
        
        int CreateFolder(string displayName, int createdBy);
        int UpdateFolder(int folderId, string displayName, int updatedBy);
        void DeleteFolder(int folderId, int deletedBy);
        List<CompanyFolder> ListFolders();
        List<CompanyFolder> ListFoldersByUser(int userId, int userDepartmentId);
        CompanyFolder GetFolder();

        int CreateDocument(string documentDisplayName, string originalFileName, int fileId, int folderId, int createdBy);
        int UpdateDocument(int documentId, string documentDisplayName, string originalFileName, int folderId, int updatedBy, IO.FileInfo file=null);
        void DeleteDocument(int documentId, int deletedBy);
        List<CompanyDocument> ListDocuments();
        List<CompanyDocument> ListDocumentsByUser(int userId);
        CompanyDocument GetDocument(int docid);

        List<CompanyDocument> ListDocumentAndFoldersByUser(int userId, int userDepartmentId);

        void ShareDocumentByEmail(int documentId, int shearerId);
        string GetDocumentLink(int documentId, int userId);

    }
}
