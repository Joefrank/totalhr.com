CREATE TABLE [dbo].[CalendarAssociation] (
    [EventAssociationId] INT             IDENTITY (1, 1) NOT NULL,
    [EventId]            INT             NOT NULL,
    [AssociationTypeid]  INT             NOT NULL,
    [AssociationValue]   NVARCHAR (1000) NOT NULL,
    [Created]            DATETIME        CONSTRAINT [DF_CalendarAssociation_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          INT             NOT NULL,
    CONSTRAINT [PK_CalendarAssociation] PRIMARY KEY CLUSTERED ([EventAssociationId] ASC),
    CONSTRAINT [FK_CalendarAssociation_CalendarEvent] FOREIGN KEY ([EventId]) REFERENCES [dbo].[CalendarEvent] ([id])
);

