USE [BlogIt]
GO

/****** Object:  Table [dbo].[bl_Category]    Script Date: 10/24/2015 8:45:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [MailTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Template] [nvarchar](100) NOT NULL,
	[TemplateId] [int] NOT NULL,
	[Subject] [nvarchar](950) NULL, 
	[HtmlBody] [nvarchar] (max) NOT NULL,
	[TextBody] [nvarchar] (max) NOT NULL,
	[IsActive] [bit] NOT NULL
)
