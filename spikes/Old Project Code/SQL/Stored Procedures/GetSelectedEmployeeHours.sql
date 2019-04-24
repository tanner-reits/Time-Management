USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getSelectedEmpTotalHours]    Script Date: 8/24/2017 3:35:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 8/16/17
-- Description:	Gets total hours for current pay period
--				for employee selected on management screen
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getSelectedEmpTotalHours] 
	-- Add the parameters for the stored procedure here
	@FirstName varchar(4000),
	@LastName varchar(4000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @StartDate datetime
	DECLARE @EndDate datetime

	SET @StartDate =  (SELECT MIN(PayPeriodStartDate) FROm tblTimeCardHeader  
		WHERE Validated = 0 )
	SET @EndDate =  (SELECT MAX(PayPeriodEndDate) FROm tblTimeCardHeader  
		WHERE Validated = 0 )

	SELECT ISNULL(SUM(Hours), 0) AS Total FROM ctbl0334TimeCardEntryDetails
	WHERE EmployeeID = (SELECT EmployeeID FROM tblEmployee WHERE EmpFirstName = @FirstName AND EmpLastName = @LastName)
	AND EntryDate BETWEEN @StartDate AND @EndDate
END

GO

