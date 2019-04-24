USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getManagerEmployees]    Script Date: 8/24/2017 3:34:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 8/1/17
-- Description:	Gets user list from employee table
--				based on employee sub department
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getManagerEmployees] 
	-- Add the parameters for the stored procedure here
	@FirstName varchar(4000), 
	@LastName varchar(4000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EmpFirstName + ' ' + EmpLastName AS Name
	FROM tblEmployee
	WHERE EmpSubDept IN (SELECT SubDept FROM ctbl0334TimeCardManagers
						WHERE MngFirstName = @FirstName AND MngLastName = @LastName)
	AND EmpActive = 1
	ORDER BY EmpSubDept ASC, Name ASC
END

GO

