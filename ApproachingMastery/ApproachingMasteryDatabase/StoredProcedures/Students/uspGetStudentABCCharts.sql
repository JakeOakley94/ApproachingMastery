CREATE PROCEDURE [dbo].[uspGetStudentABCCharts]
	@guidStudentID UNIQUEIDENTIFIER
AS
	
	SELECT
		TAE.*
	FROM
		TStudents AS TS
		INNER JOIN TStudentsABCCharts AS TAC
			INNER JOIN TABCEntries AS TAE
			ON TAE.guidABCID = TAC.guidABCID
		ON TAC.guidStudentID = TS.guidStudentID
	WHERE 
		TS.guidStudentID = @guidStudentID

RETURN 0
