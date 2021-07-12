CREATE PROCEDURE [dbo].[uspRemoveAirTest]
	 @guidAirTestID UNIQUEIDENTIFIER
AS

	DELETE FROM TStudentAirTests

	WHERE 

		guidAirTestID = @guidAirTestID

   DELETE FROM TAirTests

   WHERE

   guidAirTestID = @guidAirTestID

	DECLARE @rowsDeleted INTEGER
	SELECT @rowsDeleted = @@ROWCOUNT

	IF @rowsDeleted > 0 
		return 0	


RETURN 1

