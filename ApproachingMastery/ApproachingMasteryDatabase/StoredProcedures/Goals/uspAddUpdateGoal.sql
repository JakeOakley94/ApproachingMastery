GO
CREATE PROCEDURE [dbo].[uspAddUpdateGoal]
    @guidGoalID		   AS UNIQUEIDENTIFIER = NULL OUTPUT
   ,@dteDateAssigned   AS DATE
   ,@dteDateCompleted  AS DATE
   ,@dteDueDate		   AS DATE
   ,@shtGoalType       AS SMALLINT
   ,@shtGoalArea       AS SMALLINT
   ,@strDescription    AS VARCHAR(500)
AS
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

IF @guidGoalID IS NULL
BEGIN
	SET NOCOUNT ON 
	DECLARE @inserted AS TABLE (guidGoalID UNIQUEIDENTIFIER)

	INSERT INTO TGoals WITH (TABLOCKX) (dteDateAssigned,dteDateCompleted, dteDateDue, shtGoalType, shtGoalArea, strDescription)
	OUTPUT inserted.guidGoalID INTO @inserted
	VALUES (@dteDateAssigned,@dteDateCompleted,@dteDueDate, @shtGoalType,@shtGoalArea,@strDescription)
	SELECT TOP 1
		@guidGoalID = guidGoalID
	FROM
		@inserted

	IF @guidGoalID IS NOT NULL
	BEGIN
		COMMIT TRANSACTION
		RETURN 0
	END
	ELSE
	BEGIN
		ROLLBACK
		RETURN 1
	END

END

ELSE
BEGIN
	SET NOCOUNT OFF
	UPDATE
		TGoals
	SET
		dteDateAssigned = @dteDateAssigned
		,dteDateCompleted = @dteDateCompleted
		,dteDateDue = @dteDueDate
		,shtGoalType = @shtGoalType
		,shtGoalArea = @shtGoalArea
		,strDescription = @strDescription
	WHERE
		guidGoalID = @guidGoalID
	
	DECLARE @intAffectedRows INTEGER = @@ROWCOUNT

	IF @intAffectedRows > 0
	BEGIN
		COMMIT TRANSACTION
		RETURN 0
	END
	ELSE
	BEGIN
		ROLLBACK
		RETURN 1
	END

END	
GO
