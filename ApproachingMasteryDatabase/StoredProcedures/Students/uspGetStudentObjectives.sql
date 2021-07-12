CREATE PROCEDURE [dbo].[uspGetStudentObjectives]
	 @guidStudentID UNIQUEIDENTIFIER
	,@shtGoalArea	SMALLINT
AS
	SELECT
		*
	FROM
		VStudentObjectives
	WHERE
		shtGoalArea = @shtGoalArea
	AND guidStudentID = @guidStudentID

RETURN 0
