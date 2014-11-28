CREATE TABLE [dbo].[CompanyFolder] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [DisplayName]   NVARCHAR (250) NOT NULL,
    [Created]       DATETIME       CONSTRAINT [DF_CompanyFolder_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT            NOT NULL,
    [LastUpdated]   DATETIME       NULL,
    [LastUpdatedBy] INT            NULL,
    [NoOfFiles]     INT            CONSTRAINT [DF__CompanyFo__NoOfF__41EDCAC5] DEFAULT ((0)) NOT NULL,
    [NoOfOpenings]  INT            CONSTRAINT [DF__CompanyFo__NoOfO__42E1EEFE] DEFAULT ((0)) NOT NULL,
    [OpenedPublic]  BIT            CONSTRAINT [DF_CompanyFolder_Public] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompanyFolder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyFolder_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id])
);

