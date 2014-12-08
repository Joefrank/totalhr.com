CREATE TABLE [dbo].[ContractFormFieldValues] (
    [FormFieldid]   INT            NOT NULL,
    [Userid]        INT            NOT NULL,
    [Value]         NVARCHAR (MAX) NULL,
    [IntValue]      INT            NULL,
    [Created]       DATETIME       NOT NULL,
    [CreatedBy]     INT            NOT NULL,
    [Lastupdated]   DATETIME       NULL,
    [LastupdatedBy] INT            NULL,
    CONSTRAINT [PK_ContractFormFieldValues] PRIMARY KEY CLUSTERED ([FormFieldid] ASC, [Userid] ASC)
);

