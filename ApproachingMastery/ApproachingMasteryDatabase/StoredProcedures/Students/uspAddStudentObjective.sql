CREATE PROCEDURE [dbo].[uspAddStudentObjective]
	@guidStudentID	    AS UNIQUEIDENTIFIER
   ,@guidGoalID		    AS UNIQUEIDENTIFIER = NULL OUTPUT
   ,@guidAssignedBy		AS UNIQUEIDENTIFIER
   ,@dteDateAssigned	AS DATE
   ,@dteDateCompleted	AS DATE
   ,@dteDueDate			AS DATE
   ,@shtGoalType		AS SMALLINT
   ,@shtGoalArea		AS SMALLINT
   ,@strDescription		AS VARCHAR(500)
AS
SET XACT_ABORT ON
SET NOCOUNT ON 
BEGIN TRANSACTION 
	
	DECLARE @intResult INTEGER = 0

	EXEC @intResult = dbo.uspAddUpdateGoal @guidGoalID OUTPUT, @dteDateAssigned, @dteDateCompleted, @dteDueDate, @shtGoalType, @shtGoalArea, @strDescription

	IF @intResult <> 0
	BEGIN
		ROLLBACK 
		RETURN 1
	END
	
	INSERT INTO TStudentObjectives(guidStudentID, guidGoalID, guidAssignedBy)
	VALUES (@guidStudentID, @guidGoalID, @guidAssignedBy)
	COMMIT TRANSACTION
	
RETURN 0
