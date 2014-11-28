CREATE TABLE [dbo].[Language] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50) NOT NULL,
    [CreatedBy]         INT           NOT NULL,
    [CreatedOn]         DATETIME      DEFAULT (getdate()) NOT NULL,
    [LastUpdatedBy]     INT           NULL,
    [LastUpdatedOn]     DATETIME      NULL,
    [RelatedGlossaryId] INT           NOT NULL,
    [Culture]           VARCHAR (10)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

