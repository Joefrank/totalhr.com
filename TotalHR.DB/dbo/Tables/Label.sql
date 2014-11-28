CREATE TABLE [dbo].[Label] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Term]          NVARCHAR (1000) NOT NULL,
    [RootId]        INT             DEFAULT ((-1)) NOT NULL,
    [LanguageId]    INT             DEFAULT ((1)) NOT NULL,
    [CreatedBy]     INT             NOT NULL,
    [CreatedOn]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [LastUpdatedOn] DATETIME        NULL,
    [LastUpdatedBy] INT             NULL,
    [Obsolete]      BIT             DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

