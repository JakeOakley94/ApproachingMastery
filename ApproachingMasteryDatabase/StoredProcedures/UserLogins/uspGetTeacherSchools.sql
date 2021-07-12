CREATE PROCEDURE [dbo].[uspGetTeacherSchools]
	@guidLoginID	UNIQUEIDENTIFIER
AS
	SELECT
		TS.*
	FROM
		TSchoolDistrictUsers AS TSDU
		INNER JOIN TSchoolDistrictSchools AS TSDS
			INNER JOIN TSchools AS TS
			ON TS.guidSchoolID = TSDS.guidSchoolID
		ON TSDU.guidSchoolDistrictID = TSDS.guidSchoolDistrictID
	WHERE
		TSDU.guidLoginID = @guidLoginID
RETURN 0
