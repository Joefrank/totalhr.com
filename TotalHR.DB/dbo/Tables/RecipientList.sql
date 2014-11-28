CREATE TABLE [dbo].[RecipientList] (
    [id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (250) NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [Created]       DATETIME       CONSTRAINT [DF_RecipientList_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT            NOT NULL,
    [Lastupdated]   DATETIME       NULL,
    [LastUpdatedBy] INT            NULL,
    [EventId]       INT            NOT NULL,
    CONSTRAINT [PK_RecipientList] PRIMARY KEY CLUSTERED ([id] ASC)
);

