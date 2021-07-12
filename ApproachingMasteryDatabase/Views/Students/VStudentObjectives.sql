CREATE VIEW [dbo].[VStudentObjectives]
	AS
SELECT
	 TS.*
	,TSO.guidAssignedBy
	,TG.*
FROM
	TStudents AS TS (TABLOCKX)
	INNER JOIN TStudentObjectives AS TSO (TABLOCKX)
		INNER JOIN TGoals AS TG (TABLOCKX)
		ON TG.guidGoalID = TSO.guidGoalID
	ON TSO.guidStudentID = TS.guidStudentID
