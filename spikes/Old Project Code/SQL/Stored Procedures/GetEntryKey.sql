USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334getTimeCardEntryKey]    Script Date: 8/24/2017 3:36:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/18/17
-- Description:	Gets the entry key ID for the selected
--				row in the time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334getTimeCardEntryKey] 
	-- Add the parameters for the stored procedure here
	@EntryDate date,
	@EmployeeID varchar(20),
	@ProjectID varchar(10),
	@LaborCode int,
	@Station int,
	@Hours float,
	@Road bit

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EntryID FROM ctbl0334TimeCardEntryDetails
	WHERE EntryDate = @EntryDate
	AND EmployeeID = @EmployeeID
	AND ProjectID = @ProjectID
	AND LaborCode = @LaborCode
	AND Station = @Station
	AND Hours = @Hours
	AND Road = @Road
END

GO

