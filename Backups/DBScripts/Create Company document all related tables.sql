
Create table FFile(
	id int
)

Create table CompanyDocument(
	Id int identity(1,1),
	DisplayName nvarchar(250),
	OriginalFileName nvarchar(250),
	Created datetime,
	CreatedBy int,
	LastUpdated datetime,
	LastUpdatedBy int,
	FolderId int,
	FileId int,
	NoOfViews int default(0),
	NoOfDownloads int default(0),
	LastViewed datetime,
	LastViewedBy int,
	LastDownloaded datetime,
	LastDownloadedBy int
)

-- these is where we store file permissions, note that objectid will be null for some types e.g. whole company, myself
Create table CompanyDocumentPermissions(
	DocumentId int,
	PermissionTypeId int, --permissiontypeid vary from (Myself which is createdby, Wholecompany, a department or a user)
	ObjectId int,
	Created datetime,
	CreatedBy int
)

Create table CompanyFolder(
	Id int identity(1,1),
	DisplayName nvarchar(250),
	Created datetime,
	CreatedBy int,
	LastUpdated datetime,
	LastUpdatedBy int,
	NoOfFiles int default(0),
	NoOfOpenings int default(0)	
)

Create table CompanyFolderFile(
	DocumentId int,
	FolderId int,
	Created datetime,
	CreatedBy int	
)

create table CompanyDocumentViews(
	DocumentId int,
	UserId int,
	ViewDate datetime	
)

create table CompanyDocumentDownloads(
	DocumentId int,
	UserId int,
	DownloadDate datetime	
)

Create table CompanyDocumentShares(
	DocumentId int,
	UserId int,
	DateShared datetime,
	TypeofShare int  -- TypeofShare could be email, system notification
)