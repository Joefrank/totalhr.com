CREATE TABLE [dbo].[Feature] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)    NOT NULL,
    [Description]    NVARCHAR (1000) NOT NULL,
    [Created]        DATETIME        CONSTRAINT [DF_Feature_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT             NOT NULL,
    [LastModified]   DATETIME        NULL,
    [LastModifiedBy] INT             NULL,
    CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED ([id] ASC)
);

