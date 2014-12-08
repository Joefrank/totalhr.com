CREATE TABLE [dbo].[FormControl] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (30)   NOT NULL,
    [Created]        DATETIME       CONSTRAINT [DF_FormControls_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [HtmlCode]       NVARCHAR (255) NULL,
    [LastModified]   DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    [obsolete]       BIT            CONSTRAINT [DF_FormControls_obsolete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FormControls] PRIMARY KEY CLUSTERED ([id] ASC)
);

