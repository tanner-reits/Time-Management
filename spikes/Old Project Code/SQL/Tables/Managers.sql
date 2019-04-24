USE [QuesticaSandbox]
GO

/****** Object:  Table [dbo].[ctbl0334TimeCardManagers]    Script Date: 8/24/2017 3:30:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ctbl0334TimeCardManagers](
	[ManagerUsername] [varchar](50) NOT NULL,
	[MngFirstName] [varchar](50) NOT NULL,
	[MngLastName] [varchar](50) NOT NULL,
	[SubDept] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

