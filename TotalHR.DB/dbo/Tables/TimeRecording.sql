CREATE TABLE [dbo].[TimeRecording] (
    [Id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [UserId]      INT        NOT NULL,
    [StartTime]   DATETIME   NOT NULL,
    [EndTime]     DATETIME   NULL,
    [TypeId]      SMALLINT   NOT NULL,
    [AddedDate]   DATETIME   NOT NULL,
    [UpdatedDate] DATETIME   NULL,
    [TimeStamp]   ROWVERSION NOT NULL,
    [NoteId]      BIGINT     NOT NULL,
    [AddedById]   INT        NOT NULL,
    [UpdatedById] INT        NULL,
    [TaskRefId]   INT        NULL,
    CONSTRAINT [PK_TimeRecording] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TimeRecording_Notes] FOREIGN KEY ([NoteId]) REFERENCES [dbo].[Notes] ([Id]),
    CONSTRAINT [FK_TimeRecording_TaskScheduler] FOREIGN KEY ([TaskRefId]) REFERENCES [dbo].[TaskScheduler] ([Id]),
    CONSTRAINT [FK_TimeRecording_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_TimeRecording_User1] FOREIGN KEY ([AddedById]) REFERENCES [dbo].[User] ([id]),
    CONSTRAINT [FK_TimeRecording_User2] FOREIGN KEY ([UpdatedById]) REFERENCES [dbo].[User] ([id])
);


