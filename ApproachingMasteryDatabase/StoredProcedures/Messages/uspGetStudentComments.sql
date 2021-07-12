CREATE PROC [dbo].[uspGetStudentComments]
	@guidStudentID UNIQUEIDENTIFIER
	AS
	SELECT
		*
	FROM
		VStudentMessages (TABLOCKX)
	WHERE
		guidStudentID = @guidStudentID
