
CREATE PROCEDURE [dbo].[uspGetAvailableStudents]
	@guidLoginID  UNIQUEIDENTIFIER
AS
	SELECT
		Distinct TS.*
FROM
	TSchoolDistrictUsers AS TSDU
	INNER JOIN TSchoolDistrictSchools AS TSDS
		INNER JOIN TSchoolStudents AS TSS
			INNER JOIN TStudents AS TS
			ON TS.guidStudentID = TSS.guidStudentID
		ON TSS.guidSchoolID = TSDS.guidSchoolID
	ON TSDS.guidSchoolDistrictID = TSDU.guidSchoolDistrictID
where
	ts.guidStudentID NOT IN
	(
		SELECT
			guidStudentID
		FROM
			VTeacherStudents
		WHERE
			guidLoginID = @guidLoginID
	)
AND tsdu.guidLoginID = @guidLoginID
AND TS.blnActive = 1
RETURN 0
