USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getSelectedEmployeeTimeCardEntries]    Script Date: 8/24/2017 3:35:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 8/1/17
-- Description:	Gets timecard entries for selected
--				employee for management screen of 
--				time-entry app
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getSelectedEmployeeTimeCardEntries] 
	-- Add the parameters for the stored procedure here
	@FirstName varchar(4000),
	@LastName varchar(4000)

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
WHERE EmployeeID = (SELECT EmployeeID FROM tblEmployee
	WHERE EmpFirstName = @FirstName AND EmpLastName = @LastName) 
AND EntryDate BETWEEN @StartDate AND @EndDate
ORDER BY [Entry Date] ASC
END

GO

