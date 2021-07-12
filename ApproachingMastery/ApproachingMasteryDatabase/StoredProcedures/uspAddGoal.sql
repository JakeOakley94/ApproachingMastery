GO
CREATE PROCEDURE [dbo].[uspAddGoal]
	@dteDateAssigned   AS DATETIME
   ,@dteDateCompleted  AS DATETIME
   ,@shtGoalType       AS SMALLINT
   ,@shtGoalArea       AS SMALLINT
   ,@strDescription    AS VARCHAR
AS
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TGoals WITH (TABLOCKX) (dteDateAssigned,dteDateCompleted, shtGoalType, shtGoalArea, strDescription)
VALUES (@dteDateAssigned,@dteDateCompleted,@shtGoalType,@shtGoalArea,@strDescription)

COMMIT TRANSACTION 
GO
