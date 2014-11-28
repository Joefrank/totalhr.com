CREATE TABLE [dbo].[UserContract] (
    [id]            INT      IDENTITY (1, 1) NOT NULL,
    [Userid]        INT      NOT NULL,
    [TemplateId]    INT      NOT NULL,
    [Created]       DATETIME CONSTRAINT [DF_ContractTemplate_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     INT      NOT NULL,
    [Lastupdated]   DATETIME NULL,
    [LastUpdatedBy] INT      NULL,
    [Views]         INT      CONSTRAINT [DF_UserContract_Views] DEFAULT ((0)) NOT NULL,
    [LastViewed]    DATETIME NULL,
    CONSTRAINT [PK_ContractTemplate] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ContractTemplate_User] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_UserContract_User] FOREIGN KEY ([Userid]) REFERENCES [dbo].[User] ([id])
);

