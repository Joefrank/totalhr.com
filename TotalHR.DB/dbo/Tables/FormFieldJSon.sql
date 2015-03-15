CREATE TABLE [dbo].[FormFieldJSon] (
    [id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50)  NOT NULL,
    [DisplayName]     NVARCHAR (255) NOT NULL,
    [PlaceHolderText] NVARCHAR (255) NULL,
    [ToolTip]         NVARCHAR (255) NULL,
    [InitialValue]    NVARCHAR (255) NULL,
    [TypeName]        VARCHAR (20)   NOT NULL,
    [Created]         DATETIME       CONSTRAINT [DF_FormFieldJSon_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]       INT            NOT NULL,
    [LastUpdated]     DATETIME       NULL,
    [LastUpdatedBy]   INT            NULL,
    [obsolete]        BIT            CONSTRAINT [DF_FormFieldJSon_obsolete] DEFAULT ((0)) NOT NULL,
    [FormId]          INT            NOT NULL,
    CONSTRAINT [PK_FormFieldJSon] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_FormFieldJSon_Form] FOREIGN KEY ([FormId]) REFERENCES [dbo].[Form] ([Id])
);

