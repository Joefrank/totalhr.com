CREATE TABLE [dbo].[CTSectionFieldLink] (
    [TemplateSectionId] INT      NOT NULL,
    [FormFieldId]       INT      NOT NULL,
    [Created]           DATETIME CONSTRAINT [DF_CTSectionFieldLink_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]         INT      NOT NULL,
    CONSTRAINT [PK_CTSectionFieldLink] PRIMARY KEY CLUSTERED ([TemplateSectionId] ASC, [FormFieldId] ASC),
    CONSTRAINT [FK_CTSectionFieldLink_ContractTemplateSection] FOREIGN KEY ([TemplateSectionId]) REFERENCES [dbo].[ContractTemplateSection] ([id]),
    CONSTRAINT [FK_CTSectionFieldLink_FormField] FOREIGN KEY ([FormFieldId]) REFERENCES [dbo].[FormField] ([id]),
    CONSTRAINT [FK_CTSectionFieldLink_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id])
);

