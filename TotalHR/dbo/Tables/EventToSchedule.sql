CREATE TABLE [dbo].[EventToSchedule] (
    [id]                INT            IDENTITY (1, 1) NOT NULL,
    [EventId]           INT            NOT NULL,
    [CompanyId]         INT            NOT NULL,
    [RecipientListName] NVARCHAR (250) NULL,
    [Description]       NVARCHAR (500) NULL,
    [CreatedBy]         INT            NOT NULL,
    [Created]           DATETIME       NOT NULL,
    CONSTRAINT [PK_EventToSchedule] PRIMARY KEY CLUSTERED ([id] ASC)
);

