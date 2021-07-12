CREATE PROCEDURE [dbo].[uspAddStudentGoal]
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
	DECLARE @guidNewID UNIQUEIDENTIFIER

	SELECT @guidNewID = @guidGoalID


	EXEC @intResult = dbo.uspAddUpdateGoal @guidNewID OUTPUT, @dteDateAssigned, @dteDateCompleted, @dteDueDate, @shtGoalType, @shtGoalArea, @strDescription

	IF @intResult <> 0
	BEGIN
		ROLLBACK 
		RETURN 1
	END
	IF @guidNewID = @guidGoalID
	BEGIN
		COMMIT
		RETURN 0
	END

	
	INSERT INTO TStudentGoals(guidStudentID, guidGoalID, guidAssignedBy)
	VALUES (@guidStudentID, @guidNewID, @guidAssignedBy)
	COMMIT TRANSACTION
	
RETURN 0
