CREATE TABLE [dbo].[ChatMessage] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [ChatUserid]     INT             NOT NULL,
    [ChatRoomId]     INT             NOT NULL,
    [MessageDate]    DATETIME        CONSTRAINT [DF_ChatMessage_MessageDate] DEFAULT (getdate()) NOT NULL,
    [MessageContent] NVARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ChatMessage_ChatMessage] FOREIGN KEY ([Id]) REFERENCES [dbo].[ChatMessage] ([Id])
);

