using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDocumentService.Implementation
{
    public class DocumentManager
    {
        public int CreateFolder(string name)
        {
            return 0;
        }

        public int UpdateFolder(string name)
        {
            return 0;
        }

        public void DeleteFolder(int folderId)
        {
            
        }

        public int CreateDocument(string documentDisplayName, string originalFileName, long fileSize)
        {
            return 0;
        }

        public int UpdateDocument(string documentDisplayName, string originalFileName, long fileSize)
        {
            return 0;
        }
        public void DeleteDocument(int documentId)
        {
            
        }

        public void ShareDocumentByEmail(int documentId)
        {
            
        }

        public string GetDocumentLink(int documentId)
        {
            return "";
        }
    }
}
