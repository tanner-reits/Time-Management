USE [QuesticaSandbox]
GO

/****** Object:  StoredProcedure [dbo].[csp0334updateTimeCardEntry]    Script Date: 8/24/2017 3:37:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tanner Reits
-- Create date: 7/19/2017
-- Description:	Updates entry of timeCardDetails
--				table with matching entry ID
-- =============================================
CREATE PROCEDURE [dbo].[csp0334updateTimeCardEntry] 
	-- Add the parameters for the stored procedure here
	@EntryID int,
	@EntryDate datetime,
	@ProjectID varchar(10),
	@Station int,
	@LaborCode int,
	@Hours float,
	@Road bit,
	@Manager bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE ctbl0334TimeCardEntryDetails
	SET EntryDate = @EntryDate, ProjectID = @ProjectID, Station = @Station, LaborCode = @LaborCode, "Hours" = @Hours, Road = @Road, EditTimeStamp = GETDATE()
	WHERE EntryID = @EntryID
END

GO

