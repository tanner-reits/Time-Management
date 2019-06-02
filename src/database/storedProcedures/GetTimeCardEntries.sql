USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334GetTimeCardEntries]    Script Date: 6/2/2019 2:43:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 6/1/2019
-- Description:	Responsible for getting time card 
--				entries for employee with specific TimePeriodID
-- =============================================
CREATE PROCEDURE [dbo].[csp0334GetTimeCardEntries]
	-- Add the parameters for the stored procedure here
	@TimePeriodID INT,
	@EmployeeID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CONVERT(varchar, t.TimeDate, 101) AS 'Date',
		CONVERT(VARCHAR, t.ProjectID) + ' - ' + p.PDescription AS Project,
		CONVERT(VARCHAR, t.SpecID) + ' - ' + s.SDescription AS Station,
		CONVERT(VARCHAR, t.HourType) + ' - ' + h.HourDescription AS LaborCode,
		t.HourTime AS 'Hours'
	FROM tblTimecards AS t
	LEFT JOIN tblSpec s ON t.SpecID = s.SpecID AND t.ProjectID = s.ProjectID
	LEFT JOIN tlkpHourTypes h ON t.HourType = h.HourType	
	LEFT JOIN tblProjects p ON t.ProjectID = p.ProjectID 
	WHERE t.TimePeriodID = @TimePeriodID AND t.EmployeeID = @EmployeeID
END
GO


