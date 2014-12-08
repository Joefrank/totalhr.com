using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;

namespace totalhr.data.Repositories.Infrastructure
{
    public interface ICompanyDocumentRepository : IGenericRepository<totalhr.data.EF.CompanyDocument>
    {
        int AddFolder(CompanyFolder folder);

        List<CompanyFolder> ListFoldersByUser(int userId);

        List<CompanyDocument> ListDocumentAndFoldersByUser(int userId, int departmentId);

        CompanyFolder GetFolder(int folderId);

        void CreateDocumentPermissions(List<int> objids, int userId, int permissionType, int documentId);

        void CreateDocumentPermissions(int objid, int userId, int permissionType, int documentId);

        List<string> GetPermissionObjectNames(List<CompanyDocumentPermission> permissions);

        void RemoveDocPermissions(int docId);
    }
}
