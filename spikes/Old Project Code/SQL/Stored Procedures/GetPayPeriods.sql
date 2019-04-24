USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getPayPeriods]    Script Date: 8/24/2017 3:35:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/19/2017
-- Description:	Gets pay-period date ranges for
--				time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getPayPeriods] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TimePeriodID AS ID, CONVERT(varchar, CAST(PayPeriodStartDate AS date), 101) + ' - ' + CONVERT(varchar, CAST(PayPeriodEndDate AS date), 101) AS Period
	FROM tblTimecardHeader
	ORDER BY ID DESC
END

GO

