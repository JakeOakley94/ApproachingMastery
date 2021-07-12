
GO
CREATE VIEW [dbo].VStudentAccomodations
AS
	SELECT
		 TSA.guidStudentID
		,TA.*
	FROM
		TAccomodations AS TA
		INNER JOIN TStudentAccomodations TSA
			ON TA.shtAccomodationID = TSA.shtAccomodationID
		
GO

CREATE PROCEDURE [dbo].[uspGetStudentAccomodations]

	@guidStudentID	UNIQUEIDENTIFIER

	as 

	SELECT
		 VSA.*
	FROM
			VStudentAccomodations AS VSA
	WHERE	
		VSA.guidStudentID = @guidStudentID


RETURN 0