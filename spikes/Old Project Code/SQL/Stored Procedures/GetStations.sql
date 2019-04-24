USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getStations]    Script Date: 8/24/2017 3:36:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/17/2017
-- Description:	Gets available stations for selected
--				project ID's for time-entry apps
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getStations]
	-- Add the parameters for the stored procedure here
	@Project varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Project = '99999'
		SELECT SpecID AS 'ID', CAST(SpecID AS varchar(50)) + ' - ' + CAST(SDescription AS varchar(50)) AS 'Station' 
		FROM tblSpec
		WHERE SpecID != 1 AND ProjectID = @Project
	ELSE  
		SELECT SpecID AS 'ID', CAST(SpecID AS varchar(50)) + ' - ' + CAST(SDescription AS varchar(50)) AS 'Station' 
		FROM tblSpec 
		WHERE ProjectID = @Project
END

GO

