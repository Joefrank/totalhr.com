CREATE TABLE [dbo].[UserProfilePicture] (
    [UserId]        INT      NOT NULL,
    [FileId]        INT      NOT NULL,
    [Created]       DATETIME NOT NULL,
    [CreatedBy]     INT      NOT NULL,
    [LastUpdatedBy] INT      NULL,
    [LastUpdated]   DATETIME NULL,
    [Width]         INT      NOT NULL,
    [Height]        INT      NOT NULL,
    CONSTRAINT [PK_UserProfilePicture] PRIMARY KEY CLUSTERED ([UserId] ASC, [FileId] ASC),
    CONSTRAINT [FK_UserProfilePicture_File] FOREIGN KEY ([FileId]) REFERENCES [dbo].[File] ([id]),
    CONSTRAINT [FK_UserProfilePicture_User] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_UserProfilePicture_User1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_UserProfilePicture_UserProfilePicture] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([id])
);

