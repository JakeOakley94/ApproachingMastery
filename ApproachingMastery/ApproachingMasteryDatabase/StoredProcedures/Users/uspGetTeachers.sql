CREATE PROCEDURE [dbo].[uspGetTeachers]
	 @guidLoginID UNIQUEIDENTIFIER

AS
	SELECT
		*
	FROM
		VAllUserInformation
	WHERE
		guidLoginID <> @guidLoginID
	
RETURN 0
