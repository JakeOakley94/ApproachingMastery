CREATE PROCEDURE [dbo].[uspGetStudent]
	@guidStudentID UNIQUEIDENTIFIER
AS
SET NOCOUNT ON

	SELECT
		*
	FROM
		TStudents (TABLOCKX)
	WHERE
		guidStudentID = @guidStudentID

RETURN 0
