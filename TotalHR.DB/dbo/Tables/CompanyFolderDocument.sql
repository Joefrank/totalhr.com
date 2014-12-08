CREATE TABLE [dbo].[CompanyFolderDocument] (
    [DocumentId] INT      NOT NULL,
    [FolderId]   INT      NOT NULL,
    [Created]    DATETIME CONSTRAINT [DF_CompanyFolderDocument_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]  INT      NOT NULL,
    CONSTRAINT [PK_CompanyFolderDocument] PRIMARY KEY CLUSTERED ([DocumentId] ASC, [FolderId] ASC),
    CONSTRAINT [FK_CompanyFolderDocument_CompanyDocument] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[CompanyDocument] ([Id]),
    CONSTRAINT [FK_CompanyFolderDocument_CompanyFolder] FOREIGN KEY ([FolderId]) REFERENCES [dbo].[CompanyFolder] ([Id])
);

