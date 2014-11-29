CREATE TABLE [dbo].[EmailTemplate] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50)   NOT NULL,
    [Description]   NVARCHAR (1000) NULL,
    [Subject]       NVARCHAR (250)  NULL,
    [Template]      NVARCHAR (MAX)  NOT NULL,
    [RootId]        INT             CONSTRAINT [DF_EmailTemplate_RootId] DEFAULT ((-1)) NOT NULL,
    [LanguageId]    INT             NOT NULL,
    [Obsolete]      BIT             NOT NULL,
    [Created]       DATETIME        CONSTRAINT [DF_EmailTemplate_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT             NOT NULL,
    [LastUpdatedBy] INT             NULL,
    [LastUpdated]   DATETIME        NULL,
    [Identifier]    VARCHAR (50)    NULL,
    CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

