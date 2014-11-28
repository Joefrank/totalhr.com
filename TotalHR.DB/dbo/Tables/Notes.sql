CREATE TABLE [dbo].[Notes] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Note]        NVARCHAR (MAX) NOT NULL,
    [AddedById]   INT            NOT NULL,
    [AddedDate]   DATETIME       NOT NULL,
    [UpdatedById] INT            NULL,
    [UpdatedDate] DATETIME       NULL,
    [UpdateLog]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notes_User] FOREIGN KEY ([AddedById]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_Notes_User1] FOREIGN KEY ([UpdatedById]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_Notes_User2] FOREIGN KEY ([AddedById]) REFERENCES [dbo].[User] ([id])
);

