CREATE TABLE [dbo].[UserProfile] (
    [UserId]    INT      NOT NULL,
    [ProfileId] INT      NOT NULL,
    [Created]   DATETIME NOT NULL,
    [CreatedBy] INT      NOT NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC, [ProfileId] ASC)
);

