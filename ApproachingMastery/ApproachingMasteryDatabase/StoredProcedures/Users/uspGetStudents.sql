CREATE PROCEDURE [dbo].[uspGetStudents]
	 @guidUserID UNIQUEIDENTIFIER
	,@blnIsActive BIT
AS
	SELECT
		*
	FROM
		VTeacherStudents
	WHERE
		guidLoginID = @guidUserID
	AND blnActive = @blnIsActive
RETURN 0
