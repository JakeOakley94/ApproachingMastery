CREATE PROCEDURE [dbo].[uspGetStudentTests]
	 @guidStudentID		UNIQUEIDENTIFIER
	,@shtTestNameID		SMALLINT
AS
	SELECT
		TST.*
	FROM
		TStudents AS TS
		INNER JOIN TStudentTests AS TST
		ON TST.guidStudentID = TS.guidStudentID
	WHERE	
		TS.guidStudentID = @guidStudentID
	AND tst.shtTestNameID = @shtTestNameID
RETURN 0
