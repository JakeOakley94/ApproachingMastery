CREATE PROCEDURE [dbo].[uspGetStudentGoals]
	 @guidStudentID UNIQUEIDENTIFIER
	,@shtGoalArea	SMALLINT
AS
	SELECT
		*
	FROM
		VStudentGoals
	WHERE
		shtGoalArea = @shtGoalArea
	AND guidStudentID = @guidStudentID

RETURN 0
