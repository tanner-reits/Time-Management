USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334InsertTimeCardDetails]    Script Date: 8/24/2017 3:37:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/17/2017
-- Description:	Inserts project information into temp-table
--				for time-entry application
-- =============================================
CREATE PROCEDURE [dbo].[csp0334InsertTimeCardDetails] 
	-- Add the parameters for the stored procedure here
@EntryDate datetime,
@Username varchar(20),
@ProjectID varchar(10),
@Station int,
@LaborCode int,
@Road bit,
@Hours float,
@Manager bit

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DECLARE @TimePeriodID int = (SELECT TimePeriodID FROM tblTimecardHeader WHERE @EntryDate BETWEEN PayPeriodStartDate AND PayPeriodEndDate)
DECLARE @EmployeeID int = (SELECT EmployeeID FROM tblEmployee WHERE UserID = @Username)

IF (@Manager = 1)
	INSERT INTO [QuesticaSandbox].[dbo].[ctbl0334TimeCardEntryDetails] (EntryDate, TimePeriodID, Username, EmployeeID, ProjectID, Station, LaborCode, Road, Hours, Approved) 
	VALUES (@EntryDate, @TimePeriodID, @Username, @EmployeeID, @ProjectID, @Station, @LaborCode, @Road, @Hours, 1)
ELSE
	INSERT INTO [QuesticaSandbox].[dbo].[ctbl0334TimeCardEntryDetails] (EntryDate, TimePeriodID, Username, EmployeeID, ProjectID, Station, LaborCode, Road, Hours) 
	VALUES (@EntryDate, @TimePeriodID, @Username, @EmployeeID, @ProjectID, @Station, @LaborCode, @Road, @Hours)
END

GO

