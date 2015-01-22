CREATE TABLE [dbo].[UserProfile] (
    [UserId]    INT      NOT NULL,
    [ProfileId] INT      NOT NULL,
    [Created]   DATETIME NOT NULL,
    [CreatedBy] INT      NOT NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC, [ProfileId] ASC),
    CONSTRAINT [FK_UserProfile_Profile] FOREIGN KEY ([ProfileId]) REFERENCES [dbo].[Profile] ([id]),
    CONSTRAINT [FK_UserProfile_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([id])
);



