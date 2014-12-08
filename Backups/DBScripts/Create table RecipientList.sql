USE [totalhr]
GO

if not exists(select 1 from sys.tables where name = 'RecipientList')
begin

CREATE TABLE [dbo].[RecipientList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Lastupdated] [datetime] NULL,
	[LastUpdatedBy] [int] NULL,
 CONSTRAINT [PK_RecipientList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[RecipientList] ADD  CONSTRAINT [DF_RecipientList_Created]  DEFAULT (getdate()) FOR [Created]


end

