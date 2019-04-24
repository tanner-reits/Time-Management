USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334approveAllEntries]    Script Date: 8/24/2017 3:31:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 8/7/2017
-- Description:	Approvies all time entries for a selected
--				employee for time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334approveAllEntries] 
	-- Add the parameters for the stored procedure here
	@EmployeeID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ctbl0334TimeCardEntryDetails
	SET Approved = 1
	WHERE EmployeeID = @EmployeeID AND TimePeriodID = (SELECT MAX(TimePeriodID) FROM tblTimecardHeader)
END

GO

