using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using log4net;
using totalhr.Shared;
using totalhr.data.Repositories.Infrastructure;
using System.Data.Entity.Validation;

namespace totalhr.data.Repositories.Implementation
{
    public class CompanyDocumentRepository : GenericRepository<TotalHREntities, CompanyDocument>, ICompanyDocumentRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CompanyDocumentRepository));

        public int AddFolder(CompanyFolder folder)
        {
            try
            {
                Context.CompanyFolders.Add(folder);
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Log.Debug(ex.Message);
                throw;                
            }
        }

        public void CreateDocumentPermissions(List<int> objids, int userId, int permissionType, int documentId)
        {
            try
            {
                foreach (int id in objids)
                {
                    Context.CompanyDocumentPermissions.Add(new CompanyDocumentPermission
                    {
                        DocumentId = documentId,
                        PermissionTypeId = permissionType,
                        ObjectId = id,
                        Created = DateTime.Now,
                        CreatedBy = userId
                    });
                }

                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Log.Debug(ex.Message);
                throw;
            }
        }

        public CompanyFolder GetFolder(int folderId)
        {
            return Context.CompanyFolders.FirstOrDefault(x => x.Id == folderId);
        }

        public void RemoveDocPermissions(int docId)
        {
            Context.CompanyDocumentPermissions.RemoveRange(
                Context.CompanyDocumentPermissions.Where(x => x.DocumentId == docId));
        }

        public void CreateDocumentPermissions(int objid, int userId, int permissionType, int documentId)
        {
            try
            {
                
                Context.CompanyDocumentPermissions.Add(new CompanyDocumentPermission
                {
                    DocumentId = documentId,
                    PermissionTypeId = permissionType,
                    ObjectId = objid,
                    Created = DateTime.Now,
                    CreatedBy = userId
                });                

                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Log.Debug(ex.Message);
                throw;
            }
        }

        public List<CompanyFolder> ListFoldersByUser(int userId)
        {
           return Context.CompanyFolders.Where(x => x.OpenedPublic || x.CreatedBy == userId).ToList()       ;
        
        }

        public List<CompanyDocument> ListDocumentAndFoldersByUser(int userId, int departmentId)
        {
            return
                Context.CompanyDocuments.Where(x => 
                    (x.CreatedBy == userId)
                    ||
                    (x.CompanyDocumentPermissions.Any
                     (
                        p => p.DocumentId == x.Id && 
                        (
                            (p.PermissionTypeId == (int)Variables.DocumentPermissionType.SelectedUsers && p.ObjectId == userId)
                            ||
                            (p.PermissionTypeId == (int)Variables.DocumentPermissionType.Department && p.ObjectId == departmentId)
                            ||
                            (p.PermissionTypeId == (int)Variables.DocumentPermissionType.WholeCompany)
                        )
                     )
                   )

                ).ToList(); 
        }

        public List<string> GetPermissionObjectNames(List<CompanyDocumentPermission> permissions)
        {
            List<int> lstIds = permissions.Select(x => x.ObjectId).Distinct().ToList();
            if (permissions.FirstOrDefault().PermissionTypeId == (int)Variables.DocumentPermissionType.SelectedUsers)
            {
                return Context.Users.Where(x => lstIds.Contains(x.id)).
                    Select(y => y.firstname + " " + y.surname).ToList();
            }
            else if (permissions.FirstOrDefault().PermissionTypeId == (int)Variables.DocumentPermissionType.Department)
            {
                return Context.Departments.Where(x => lstIds.Contains(x.id)).
                    Select(y => y.Name).ToList();
            }
            
            return null;
        }
    }
}
