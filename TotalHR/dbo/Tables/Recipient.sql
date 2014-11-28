CREATE TABLE [dbo].[Recipient] (
    [id]                   INT            IDENTITY (1, 1) NOT NULL,
    [RecipientUserId]      INT            NOT NULL,
    [RecipientEmail]       NVARCHAR (150) NULL,
    [RecipientName]        NVARCHAR (100) NULL,
    [RecipientPhone]       NVARCHAR (30)  NULL,
    [NoNotifications]      INT            CONSTRAINT [DF_Recipient_NoNotifications] DEFAULT ((0)) NOT NULL,
    [LastNotificationDate] DATETIME       NULL,
    [RecipientListId]      INT            NOT NULL,
    CONSTRAINT [PK_Recipient] PRIMARY KEY CLUSTERED ([id] ASC)
);

