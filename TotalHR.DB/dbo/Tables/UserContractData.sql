CREATE TABLE [dbo].[UserContractData] (
    [UserId]        INT            NOT NULL,
    [ContractId]    INT            NOT NULL,
    [Data]          NVARCHAR (MAX) NOT NULL,
    [Created]       DATETIME       NOT NULL,
    [CreatedBy]     INT            NOT NULL,
    [LastUpdated]   DATETIME       NULL,
    [LastUpdatedBy] INT            NULL,
    CONSTRAINT [PK_UserContractData] PRIMARY KEY CLUSTERED ([UserId] ASC, [ContractId] ASC),
    CONSTRAINT [FK_UserContractData_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_UserContractData_User1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_UserContractData_User2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_UserContractData_UserContract] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[UserContract] ([id])
);

