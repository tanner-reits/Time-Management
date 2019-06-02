USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334GetTimecardPeriods]    Script Date: 6/2/2019 2:48:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 6/1/2019
-- Description:	Responsible for getting time card periods for time entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334GetTimecardPeriods] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TimePeriodID, 
		CONVERT(varchar, PayPeriodStartDate, 101) + ' - ' 
		+ CONVERT(varchar, PayPeriodEndDate, 101)
		AS DateRange
	FROM tblTimecardHeader
END
GO


