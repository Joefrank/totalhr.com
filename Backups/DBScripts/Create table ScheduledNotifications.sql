USE [totalhr]
GO

if not exists(select 1 from sys.tables where name = 'ScheduledNotifications')
begin

CREATE TABLE [dbo].[ScheduledNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GenericNotificationlTypeId] [int] NOT NULL,
	[NotificationTypeId] [int] NOT NULL,
	[ObjectId] [int] NOT NULL,
	[MessageBody] [nvarchar](3000) NOT NULL,
	[MessageTitle] [nvarchar](1000) NOT NULL,
	[RecipientListid] [int] NOT NULL,
	[ScheduledSendDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[SenderName] [nvarchar](100) NOT NULL,
	[SenderEmail] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_ScheduledMessages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[ScheduledNotifications] ADD  CONSTRAINT [DF_ScheduledMessages_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]


end

