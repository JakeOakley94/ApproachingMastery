GO

CREATE PROCEDURE [dbo].[uspRemoveAssignment]
	 @guidGoalID AS UNIQUEIDENTIFIER 
	,@lngAssignmentID AS BIGINT

AS
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

DELETE FROM 
	TGoalAssignments 
WHERE
	guidGoalID = @guidGoalID 
AND lngAssignmentID = @lngAssignmentID


COMMIT TRANSACTION 
GO
