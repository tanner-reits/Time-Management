USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getCurrentTimeCardEntries]    Script Date: 8/24/2017 3:32:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/17/2017
-- Description:	Gets all entries for current user
--				in current pay period
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getCurrentTimeCardEntries] 
	-- Add the parameters for the stored procedure here
	@EmployeeID varchar(20)
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

	SELECT CONVERT(varchar, [EntryDate], 101) AS 'Entry Date'
      ,[ProjectID] AS 'Project ID'
      ,[Station]
      ,[LaborCode] AS 'Labor Code'
	  ,[Hours]
      ,[Road]
	FROM [QuesticaSandbox].[dbo].[ctbl0334TimeCardEntryDetails]
	WHERE EmployeeID = @EmployeeID AND EntryDate BETWEEN @StartDate AND @EndDate
	ORDER BY [Entry Date] ASC
END

GO

