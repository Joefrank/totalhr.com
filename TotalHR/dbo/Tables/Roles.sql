CREATE TABLE [dbo].[Roles] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [Description]    VARCHAR (255) NULL,
    [Created]        DATETIME      CONSTRAINT [DF_Roles_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT           NOT NULL,
    [LastModified]   DATETIME      NULL,
    [LastModifiedBy] INT           NULL,
    [Identifier]     VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([id] ASC)
);

