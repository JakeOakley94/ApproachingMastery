CREATE VIEW [dbo].[VStudentGoals]
	AS
SELECT
	 TS.*
	,TSG.guidAssignedBy
	,TG.*
FROM
	TStudents AS TS (TABLOCKX)
	INNER JOIN TStudentGoals AS TSG (TABLOCKX)
		INNER JOIN TGoals AS TG (TABLOCKX)
		ON TG.guidGoalID = TSG.guidGoalID
	ON TSG.guidStudentID = TS.guidStudentID
