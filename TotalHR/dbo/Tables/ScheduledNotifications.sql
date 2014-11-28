CREATE TABLE [dbo].[ScheduledNotifications] (
    [Id]                         INT             IDENTITY (1, 1) NOT NULL,
    [GenericNotificationlTypeId] INT             NOT NULL,
    [NotificationTypeId]         INT             NOT NULL,
    [ObjectId]                   INT             NOT NULL,
    [MessageBody]                NVARCHAR (3000) NOT NULL,
    [MessageTitle]               NVARCHAR (1000) NOT NULL,
    [RecipientListid]            INT             NOT NULL,
    [ScheduledSendDate]          DATETIME        NOT NULL,
    [StatusId]                   INT             NOT NULL,
    [DateCreated]                DATETIME        CONSTRAINT [DF_ScheduledMessages_DateCreated] DEFAULT (getdate()) NOT NULL,
    [SenderName]                 NVARCHAR (100)  NOT NULL,
    [SenderEmail]                NVARCHAR (150)  NOT NULL,
    CONSTRAINT [PK_ScheduledMessages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

