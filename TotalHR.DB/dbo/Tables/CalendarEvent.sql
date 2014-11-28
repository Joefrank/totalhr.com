CREATE TABLE [dbo].[CalendarEvent] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (100) NOT NULL,
    [Description]    NVARCHAR (500) NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [Created]        DATETIME       CONSTRAINT [DF_CalendarEvent_Created] DEFAULT (getdate()) NOT NULL,
    [LastModifed]    DATETIME       NULL,
    [LastModifiedBy] INT            NULL,
    [StartOfEvent]   DATETIME       NOT NULL,
    [EndOfEvent]     DATETIME       NOT NULL,
    [Location]       NVARCHAR (50)  NULL,
    [CalendarId]     INT            NOT NULL,
    CONSTRAINT [PK_CalendarEvent] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_CalendarEvent_Calendar] FOREIGN KEY ([CalendarId]) REFERENCES [dbo].[Calendar] ([id])
);

