CREATE TABLE [dbo].[CalEventReminderType] (
    [id]             INT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50) NOT NULL,
    [frequency]      INT          NOT NULL,
    [frequencytype]  INT          NOT NULL,
    [created]        DATETIME     CONSTRAINT [DF_CalEventReminderType_created] DEFAULT (getdate()) NOT NULL,
    [createdby]      INT          NOT NULL,
    [lastmodified]   DATETIME     NULL,
    [lastmodifiedby] INT          NULL,
    [obsolete]       BIT          CONSTRAINT [DF_CalEventReminderType_obsolete] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CalEventReminderType] PRIMARY KEY CLUSTERED ([id] ASC)
);

