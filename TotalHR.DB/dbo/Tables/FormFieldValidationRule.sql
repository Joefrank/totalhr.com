CREATE TABLE [dbo].[FormFieldValidationRule] (
    [FormFieldId]      INT            NOT NULL,
    [ValidationRuleId] INT            NOT NULL,
    [Created]          DATETIME       NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [ErrorMessage]     NVARCHAR (500) NULL,
    [SetValue]         NVARCHAR (150) NULL,
    [FormId]           INT            NOT NULL,
    CONSTRAINT [PK_FormFieldValidationRule] PRIMARY KEY CLUSTERED ([FormFieldId] ASC, [ValidationRuleId] ASC),
    CONSTRAINT [FK_FormFieldValidationRule_FormFieldJSon] FOREIGN KEY ([FormFieldId]) REFERENCES [dbo].[FormFieldJSon] ([id]),
    CONSTRAINT [FK_FormFieldValidationRule_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id])
);

