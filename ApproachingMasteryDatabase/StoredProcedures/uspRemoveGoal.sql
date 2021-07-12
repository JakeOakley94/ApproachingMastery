GO
CREATE PROCEDURE [dbo].[uspRemoveGoal]
	@guidGoalID AS UNIQUEIDENTIFIER 


AS
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

DELETE FROM TGoalAssignments 
WHERE guidGoalID = @guidGoalID 

DELETE FROM TGoals
WHERE guidGoalID = @guidGoalID 

COMMIT TRANSACTION 
GO
