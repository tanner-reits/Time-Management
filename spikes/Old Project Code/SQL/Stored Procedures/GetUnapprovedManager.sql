USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getUnapprovedManager]    Script Date: 8/24/2017 3:36:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits	
-- Create date: 8/22/17
-- Description:	Gets managers that have not approved
--				time for employees
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getUnapprovedManager] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT (mng.MngFirstName + ' ' + mng.MngLastName) AS Manager, (emp.EmpFirstName + ' ' + emp.EmpLastName) AS Employee 
	FROM ctbl0334TimeCardEntryDetails AS tce
	LEFT JOIN tblEmployee AS emp
		ON tce.EmployeeID = emp.EmployeeID
	LEFT JOIN ctbl0334TimeCardManagers AS mng
		ON emp.EmpSubDept = mng.SubDept
	WHERE tce.Approved = 0 AND tce.TimePeriodID IN (SELECT TimePeriodID FROM tblTimecardHeader WHERE Validated = 0) AND emp.EmpSubDept != 2
END

GO

