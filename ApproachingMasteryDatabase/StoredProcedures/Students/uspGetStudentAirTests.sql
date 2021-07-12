CREATE PROCEDURE [dbo].[uspGetStudentAirTests]
	@guidStudentID	UNIQUEIDENTIFIER
AS
	SELECT
		TAT.*
	FROM
		TStudents AS TS
		INNER JOIN TStudentAirTests AS TSAT
			INNER JOIN TAirTests AS TAT
			ON TAT.guidAirTestID = TSAT.guidAirTestID
		ON TSAT.guidStudentID = TS.guidStudentID
	where
		TS.guidStudentID = @guidStudentID

RETURN 0
