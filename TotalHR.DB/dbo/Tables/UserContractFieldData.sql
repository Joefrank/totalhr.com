CREATE TABLE [dbo].[UserContractFieldData] (
    [FieldId]    INT             NOT NULL,
    [Contractid] INT             NOT NULL,
    [FormId]     INT             NOT NULL,
    [Data]       NVARCHAR (1500) NOT NULL,
    [Created]    DATETIME        NOT NULL,
    [CreatedBy]  INT             NOT NULL,
    CONSTRAINT [PK_UserContractFieldData] PRIMARY KEY CLUSTERED ([FieldId] ASC, [Contractid] ASC)
);

