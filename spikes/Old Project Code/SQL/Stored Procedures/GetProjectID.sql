USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getProjectID]    Script Date: 8/24/2017 3:35:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/17/2017
-- Description:	Gets available project ID's for
--				the time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getProjectID] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT p.projectID AS 'ID' , CAST(p.ProjectID AS varchar(50)) + ' - ' + CAST(c.Cname AS varchar(50)) AS 'Project' FROM tblProjects AS p
LEFT JOIN tblCompany c on p.CompanyID = c.CompanyID
LEFT JOIN tblMasterQueue mc on p.ProjectID = mc.ProjectID
WHERE PStatus = 'Sold' AND PDescription != 'Parts Order' AND AccRelease = 0 AND SpecID = 1
END

GO

