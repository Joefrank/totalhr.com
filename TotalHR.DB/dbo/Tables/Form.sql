CREATE TABLE [dbo].[Form] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [FormSchema]    NVARCHAR (MAX) NULL,
    [Created]       DATETIME       CONSTRAINT [DF_Form_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT            NOT NULL,
    [LastUpdated]   DATE           NULL,
    [LastUpdatedBy] INT            NULL,
    [StatusId]      INT            NOT NULL,
    [FormTypeId]    INT            NOT NULL,
    [Name]          NVARCHAR (30)  NULL,
    [Description]   NVARCHAR (500) NULL,
    CONSTRAINT [PK_Form] PRIMARY KEY CLUSTERED ([Id] ASC)
);

