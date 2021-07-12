CREATE PROCEDURE [dbo].[uspRemoveStudentABCEntry]
	 @guidStudentID		UNIQUEIDENTIFIER
	,@guidABCID			UNIQUEIDENTIFIER
AS
	


	DELETE FROM TStudentsABCCharts WHERE guidABCID = @guidABCID AND guidStudentID = @guidStudentID

	IF @@ROWCOUNT = 0 
		RETURN 1
	
RETURN 0
