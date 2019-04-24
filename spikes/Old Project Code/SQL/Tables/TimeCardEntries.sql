USE [QuesticaSandbox]
GO

/****** Object:  Table [dbo].[ctbl0334TimeCardEntryDetails]    Script Date: 8/24/2017 3:28:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ctbl0334TimeCardEntryDetails](
	[EntryID] [int] IDENTITY(1,1) NOT NULL,
	[EntryDate] [date] NOT NULL,
	[TimePeriodID] [int] NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[ProjectID] [varchar](10) NOT NULL,
	[Station] [int] NOT NULL,
	[LaborCode] [int] NOT NULL,
	[Hours] [float] NOT NULL,
	[Road] [bit] NOT NULL,
	[EntryTimeStamp] [datetime] NOT NULL CONSTRAINT [DF_ctbl0334TimeCardEntryDetails_TimeStamp]  DEFAULT (getdate()),
	[EditTimeStamp] [datetime] NULL,
	[Approved] [bit] NOT NULL CONSTRAINT [DF_ctbl0334TimeCardEntryDetails_Approved]  DEFAULT ((0)),
 CONSTRAINT [PK_ctbl0334TimeCardEntryDetails] PRIMARY KEY CLUSTERED 
(
	[EntryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

