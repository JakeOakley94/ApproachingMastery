CREATE PROCEDURE [dbo].[uspRemoveAccomodationTest]

	 @guidTestID		UNIQUEIDENTIFIER
	
AS

	DELETE FROM TStudentAccomodationTests WHERE guidTestID = @guidTestID 

	IF @@ROWCOUNT = 0 
		RETURN 1

		
	DELETE FROM TAccomodationTests WHERE guidTestID = @guidTestID 

	IF @@ROWCOUNT = 0 
		RETURN 1


	
RETURN 0
