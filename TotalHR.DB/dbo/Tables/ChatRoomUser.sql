CREATE TABLE [dbo].[ChatRoomUser] (
    [UserId]        INT           NOT NULL,
    [ChatRoomId]    INT           NOT NULL,
    [NickName]      NVARCHAR (50) NOT NULL,
    [LoggedOnTime]  DATETIME      NOT NULL,
    [LastPing]      DATETIME      NULL,
    [Created]       DATETIME      CONSTRAINT [DF_ChatRoomUser_Created] DEFAULT (getdate()) NOT NULL,
    [LastUpdated]   DATETIME      NULL,
    [LastUpdatedBy] INT           NULL,
    CONSTRAINT [PK_ChatRoomUser] PRIMARY KEY CLUSTERED ([UserId] ASC, [ChatRoomId] ASC),
    CONSTRAINT [FK_ChatRoomUser_ChatRoom] FOREIGN KEY ([ChatRoomId]) REFERENCES [dbo].[ChatRoom] ([Id]),
    CONSTRAINT [FK_ChatRoomUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_ChatRoomUser_User1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[User] ([id])
);

