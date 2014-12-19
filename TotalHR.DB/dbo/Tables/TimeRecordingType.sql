CREATE TABLE [dbo].[TimeRecordingType] (
    [Id]          SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Type]        NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (500) NULL,
    CONSTRAINT [PK_TimeRecordingType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

