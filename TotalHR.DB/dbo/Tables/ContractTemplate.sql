CREATE TABLE [dbo].[ContractTemplate] (
    [id]            INT             IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (30)   NOT NULL,
    [Description]   NVARCHAR (1000) NULL,
    [Created]       DATETIME        CONSTRAINT [DF_Template_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT             NOT NULL,
    [Lastupdated]   DATETIME        NULL,
    [LastUpdatedBy] INT             NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Template_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id])
);

