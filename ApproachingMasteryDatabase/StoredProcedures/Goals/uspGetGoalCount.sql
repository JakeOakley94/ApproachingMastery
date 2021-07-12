go


CREATE PROCEDURE [dbo].[uspGetGoalCount]
    @Num_Goals int output,
	@blnIsCompleted bit

AS

    SET NOCOUNT ON;

	IF @blnIsCompleted = 0
	
	BEGIN

	SELECT @Num_Goals =COUNT(*) 
         FROM TGoals
         WHERE dteDateCompleted is null

     END

		ELSE 

			BEGIN
				 SELECT @Num_Goals =COUNT(*) 
			     FROM TGoals
				 WHERE dteDateCompleted is not null
			END

     SELECT @Num_Goals

return
