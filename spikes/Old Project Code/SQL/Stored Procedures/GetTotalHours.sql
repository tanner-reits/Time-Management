USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getTotalHours]    Script Date: 8/24/2017 3:36:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 8/16/17
-- Description:	Gets total hours for current pay period
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getTotalHours] 
	-- Add the parameters for the stored procedure here
	@EmployeeID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @StartDate datetime
	DECLARE @EndDate datetime

	SET @StartDate =  (SELECT MIN(PayPeriodStartDate) FROm tblTimeCardHeader  
	WHERE Validated = 0 )

	SET @EndDate =  (SELECT MAX(PayPeriodEndDate) FROm tblTimeCardHeader  
	WHERE Validated = 0 )

	SELECT ISNULL(SUM(Hours), 0) AS Total FROM ctbl0334TimeCardEntryDetails
	WHERE EntryDate BETWEEN @StartDate AND @EndDate
	AND EmployeeID = @EmployeeID

END

GO

