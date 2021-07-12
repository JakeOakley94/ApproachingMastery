CREATE PROCEDURE [dbo].[uspEditStudentAccommodationDetail]
	@guidStudentID UNIQUEIDENTIFIER
   ,@shtAccomodationID SMALLINT
   ,@shtDetailTypeID SMALLINT
   ,@strDetailValue VARCHAR(50)

AS

	BEGIN TRANSACTION
	IF @guidStudentID IS NOT NULL
	BEGIN

		PRINT 'Student ID is NOT null'
	
		UPDATE TStudentAccomodationDetails SET  strDetailValue = @strDetailValue

		
	WHERE guidStudentID = @guidStudentID
	AND shtAccomodationID = @shtAccomodationID
	AND shtDetailTypeID = @shtDetailTypeID

		IF @@ROWCOUNT > 0
		BEGIN
			COMMIT
			RETURN 0
		END
		ELSE
		BEGIN
			ROLLBACK
			RETURN 1
		END

	END
	



RETURN 0
