﻿using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using CompanyDocumentService.Infrastructure;
using totalhr.data.Repositories.Infrastructure;

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

        public int CreateFolder(string displayName, int createdBy)
        {
            return 0;
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

        public List<CompanyFolder> ListFoldersByUser(int userId, int userDepartmentId)
        {
            return _docRepos.ListFoldersByUser(userId, userDepartmentId);
        }

        public CompanyFolder GetFolder()
        {
            return null;
        }

        #endregion folders


        #region Documents

        public int CreateDocument(string documentDisplayName, string originalFileName, IO.FileInfo file, int folderId, int createdBy)
        {
            return 0;
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

        public CompanyDocument GetDocument()
        {
            return null;
        }

        #endregion Documents

    }
}
