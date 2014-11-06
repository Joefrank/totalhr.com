using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDocumentService.Infrastructure
{
    public interface IDocumentManager
    {
        
        int CreateFolder(string name);
        int UpdateFolder(string name);
        void DeleteFolder(int folderId);

        int CreateDocument(string documentDisplayName, string originalFileName, long fileSize);
        int UpdateDocument(string documentDisplayName, string originalFileName, long fileSize);
        void DeleteDocument(int documentId);

        void ShareDocumentByEmail(int documentId);
        string GetDocumentLink(int documentId);

    }
}
