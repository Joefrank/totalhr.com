CREATE Proc [dbo].[GetCompanyFoldersByUser]
	@userid int,
	@departmentid int
as
Begin


select cd.Id as documentId, cd.DisplayName, cd.OriginalFileName,
cd.Created, cd.CreatedBy, cd.FileId, cd.NoOfViews, cd.NoOfDownloads,
cf.Id as folderid, cf.DisplayName as FolderDisplayName
from CompanyDocument cd
left join CompanyFolder cf on cf.Id = cd.FolderId
where cd.CreatedBy = @userid 
union
select cd.Id as documentId, cd.DisplayName, cd.OriginalFileName,
cd.Created, cd.CreatedBy, cd.FileId, cd.NoOfViews, cd.NoOfDownloads,
cf.Id as folderid, cf.DisplayName as FolderDisplayName
from CompanyDocument cd
inner join CompanyDocumentPermissions cdp on cdp.documentid = cd.id
left join CompanyFolder cf on cf.Id = cd.FolderId
where 
(cdp.PermissionTypeId = 3 and cdp.Objectid = @departmentid)
or
(cdp.permissionTypeId = 2 and cdp.objectid = @userid)
or
(cdp.PermissionTypeId = 1)

End