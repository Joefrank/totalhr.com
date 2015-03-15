CREATE TABLE [dbo].[ChatRoom] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Description]  NVARCHAR (500) NULL,
    [Created]      DATETIME       CONSTRAINT [DF_ChatRoom_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    INT            NOT NULL,
    [Private]      BIT            CONSTRAINT [DF_ChatRoom_Private] DEFAULT ((0)) NOT NULL,
    [InvitedUsers] NVARCHAR (100) NULL,
    CONSTRAINT [PK_ChatRoom] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ChatRoom_ChatRoom] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id])
);

