USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getDatesTimecard]    Script Date: 8/24/2017 3:33:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mike Everts/Tanner Reits
-- Create date: 7/17/2017
-- Description:	Used to retrieve the open payperiod dates
--				for the timecard entry program.
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getDatesTimecard]
 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DECLARE @Startdate datetime 
DECLARE @enddate datetime

SET @Startdate =  (SELECT MIN(PayPeriodStartDate) FROM tblTimeCardHeader  
WHERE Validated = 0 )

SET @enddate =  (SELECT MAX(PayPeriodEndDate) FROM tblTimeCardHeader  
WHERE Validated = 0 )

--PRINT @startdate
--PRINT @enddate

SELECT CONVERT(varchar, TimecardDate, 101) AS Date
FROM [EAGLE].[dbo].[tblTimecardDates] 
WHERE TimecardDate BETWEEN @Startdate AND @enddate

END

GO

