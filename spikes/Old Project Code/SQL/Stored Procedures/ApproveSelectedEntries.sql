USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334approveSelectedEntry]    Script Date: 8/24/2017 3:32:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 8/17/19
-- Description:	Approves a selected entry 
-- =============================================
CREATE PROCEDURE [dbo].[csp0334approveSelectedEntry] 
	-- Add the parameters for the stored procedure here
	@EntryID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ctbl0334TimeCardEntryDetails
	SET Approved = 1
	WHERE EntryID = @EntryID
	AND TimePeriodID = (SELECT MAX(TimePeriodID) FROM tblTimecardHeader)
END

GO

