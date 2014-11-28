CREATE TABLE [dbo].[CTemplateSectionLink] (
    [TemplateSectionId] INT      NOT NULL,
    [TemplateId]        INT      NOT NULL,
    [Created]           DATETIME NOT NULL,
    [CreatedBy]         INT      NOT NULL,
    CONSTRAINT [PK_TemplateSectionLink] PRIMARY KEY CLUSTERED ([TemplateSectionId] ASC, [TemplateId] ASC),
    CONSTRAINT [FK_CTemplateSectionLink_ContractTemplateSection] FOREIGN KEY ([TemplateSectionId]) REFERENCES [dbo].[ContractTemplateSection] ([id])
);

