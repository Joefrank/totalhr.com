CREATE TABLE [dbo].[CompanyDocumentPermissions] (
    [DocumentId]       INT      NOT NULL,
    [PermissionTypeId] INT      NOT NULL,
    [ObjectId]         INT      NOT NULL,
    [Created]          DATETIME CONSTRAINT [DF_CompanyDocumentPermissions_Created] DEFAULT (getdate()) NULL,
    [CreatedBy]        INT      NULL,
    CONSTRAINT [PK_CompanyDocumentPermissions] PRIMARY KEY CLUSTERED ([DocumentId] ASC, [PermissionTypeId] ASC, [ObjectId] ASC),
    CONSTRAINT [FK_CompanyDocumentPermissions_CompanyDocument] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[CompanyDocument] ([Id]),
    CONSTRAINT [FK_CompanyDocumentPermissions_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id])
);

