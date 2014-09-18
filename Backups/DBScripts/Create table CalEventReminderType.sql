USE [totalhr]
GO

/****** Object:  Table [dbo].[CalEventReminderType]    Script Date: 08/11/2014 23:00:36 ******/

if not exists(select 1 from sys.tables where name = 'CalEventReminderType')
begin

CREATE TABLE [dbo].[CalEventReminderType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[frequency] [int] NOT NULL,
	[frequencytype] [int] NOT NULL,
	[created] [datetime] NOT NULL,
	[createdby] [int] NOT NULL,
	[lastmodified] [datetime] NULL,
	[lastmodifiedby] [int] NULL,
	[obsolete] [bit] NOT NULL,
 CONSTRAINT [PK_CalEventReminderType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[CalEventReminderType] ADD  CONSTRAINT [DF_CalEventReminderType_created]  DEFAULT (getdate()) FOR [created]


ALTER TABLE [dbo].[CalEventReminderType] ADD  CONSTRAINT [DF_CalEventReminderType_obsolete]  DEFAULT ((0)) FOR [obsolete]


End
