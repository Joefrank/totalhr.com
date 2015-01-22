CREATE TABLE [dbo].[UserRole] (
    [UserId]    INT      NOT NULL,
    [RoleId]    INT      NOT NULL,
    [Created]   DATETIME NOT NULL,
    [CreatedBy] INT      NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserRole_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([id]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([id])
);



