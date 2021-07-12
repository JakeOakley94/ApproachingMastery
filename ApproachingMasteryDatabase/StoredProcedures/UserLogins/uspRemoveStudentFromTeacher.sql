CREATE PROCEDURE [dbo].[uspRemoveStudentFromTeacher]
	 @guidLoginID UNIQUEIDENTIFIER
	,@guidStudentID UNIQUEIDENTIFIER
AS
	DELETE FROM TTeacherStudents
	WHERE 
		guidLoginID = @guidLoginID
	AND guidStudentID = @guidStudentID

	DECLARE @rowsDeleted INTEGER
	SELECT @rowsDeleted = @@ROWCOUNT

	IF @rowsDeleted > 0 return 0

RETURN 1
