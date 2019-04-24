USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getAllTimeCardEntries]    Script Date: 8/24/2017 3:32:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/19/2017
-- Description:	Gets all time entries for a specified
--				time period for time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getAllTimeCardEntries] 
	-- Add the parameters for the stored procedure here
	@EmployeeID int,
	@TimePeriodID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CONVERT(varchar, TimeDate, 101) AS 'Entry Date', 
		ProjectID AS 'Project ID', 
		SpecID AS Station, 
		HourType AS 'Labor Code', 
		CAST(HourTime AS float) AS Hours
	FROM tblTimecards
	WHERE EmployeeID = @EmployeeID
	AND TimePeriodID = @TimePeriodID
END

GO

