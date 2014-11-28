CREATE TABLE [dbo].[ContractTemplateSection] (
    [id]            INT             IDENTITY (1, 1) NOT NULL,
    [SectionName]   NVARCHAR (100)  NOT NULL,
    [Heading]       NVARCHAR (2000) NULL,
    [Created]       DATETIME        CONSTRAINT [DF_TemplateSection_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT             NOT NULL,
    [TemplateId]    INT             NOT NULL,
    [LastUpdated]   DATETIME        NULL,
    [LastUpdatedBy] INT             NULL,
    CONSTRAINT [PK_TemplateSection] PRIMARY KEY CLUSTERED ([id] ASC)
);

