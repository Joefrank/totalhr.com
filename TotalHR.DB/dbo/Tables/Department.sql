CREATE TABLE [dbo].[Department] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (500) NULL,
    [CreatedBy]      INT            NOT NULL,
    [Created]        DATETIME       CONSTRAINT [DF_Department_Created] DEFAULT (getdate()) NOT NULL,
    [LastModifiedBy] INT            NULL,
    [LastModified]   DATETIME       NULL,
    [CompanyId]      INT            NOT NULL,
    [LineManagerId]  INT            NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Department_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([ID]),
    CONSTRAINT [FK_Department_User] FOREIGN KEY ([LineManagerId]) REFERENCES [dbo].[User] ([id])
);



