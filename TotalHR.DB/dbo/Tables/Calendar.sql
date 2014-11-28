CREATE TABLE [dbo].[Calendar] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (100) NOT NULL,
    [Description]    NVARCHAR (500) NULL,
    [Created]        DATETIME       CONSTRAINT [DF_Calendar_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [LastModified]   DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    [TemplateId]     INT            NULL,
    [OpenToAll]      BIT            CONSTRAINT [DF_Calendar_OpenToAll] DEFAULT ((0)) NOT NULL,
    [CompanyId]      INT            NOT NULL,
    CONSTRAINT [PK_Calendar] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Calendar_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([ID])
);

