USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334deleteTimeCardEntry]    Script Date: 8/24/2017 3:32:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits	
-- Create date: 7/18/17
-- Description:	Deletes an entry from the temp-table
--				with matching paramters for time-
--				entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334deleteTimeCardEntry] 
	-- Add the parameters for the stored procedure here
	@EntryID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM ctbl0334TimeCardEntryDetails
	WHERE EntryID = @EntryID
	
END

GO

