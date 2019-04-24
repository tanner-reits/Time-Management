USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getLaborCodes]    Script Date: 8/24/2017 3:34:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/17/2017
-- Description:	Gets available labor codes for
--				time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getLaborCodes] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT HourType, CAST(HourType AS varchar(50)) + ' - ' + CAST(HourDescription AS varchar(50)) AS 'LaborCode' 
	FROM tlkpHourTypes
END

GO

