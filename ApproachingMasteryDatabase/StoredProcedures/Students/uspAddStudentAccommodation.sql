CREATE PROCEDURE [dbo].[uspAddStudentAccommodation]
	 @guidStudentID			UNIQUEIDENTIFIER
	,@guidAddedBy			UNIQUEIDENTIFIER
	,@shtAccommodationID	SMALLINT
AS
	SET XACT_ABORT ON
	BEGIN TRANSACTION
	BEGIN TRY
		INSERT INTO TStudentAccomodations(guidStudentID, guidLoginID, shtAccomodationID)
		VALUES (@guidStudentID, @guidAddedBy, @shtAccommodationID)
	END TRY
	BEGIN CATCH
	    PRINT ERROR_MESSAGE()
		ROLLBACK
		RETURN 2
	END CATCH
	COMMIT
RETURN 0
