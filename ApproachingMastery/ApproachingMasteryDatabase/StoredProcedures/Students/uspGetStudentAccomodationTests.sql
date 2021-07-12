CREATE PROCEDURE [dbo].[uspGetStudentAccomodationTests]
	@guidStudentID	UNIQUEIDENTIFIER

	as 

	SELECT
		 TSAT.guidStudentID
		,TAT.*
	FROM
		TStudentAccomodationTests AS TSAT
		INNER JOIN TAccomodationTests TAT
			ON TSAT.guidTestID = TAT.guidTestID
		
	WHERE	
		TSAT.guidStudentID = @guidStudentID


RETURN 0




     