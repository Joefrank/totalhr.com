USE [totalhr]
GO

if not exists(select 1 from sys.tables where name ='Recipient')
begin

CREATE TABLE [dbo].[Recipient](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RecipientUserId] [int] NOT NULL,
	[RecipientEmail] [nvarchar](150) NULL,
	[RecipientPhone] [nvarchar](30) NULL,
	[NoNotifications] [int] NOT NULL,
	[LastNotificationDate] [datetime] NULL,
 CONSTRAINT [PK_Recipient] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



ALTER TABLE [dbo].[Recipient] ADD  CONSTRAINT [DF_Recipient_NoNotifications]  DEFAULT ((0)) FOR [NoNotifications]

end


