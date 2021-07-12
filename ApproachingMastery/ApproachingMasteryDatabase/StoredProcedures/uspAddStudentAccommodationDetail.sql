CREATE PROCEDURE [dbo].[uspAddStudentAccommodationDetail]
	@guidStudentID UNIQUEIDENTIFIER 
	,@shtAccomodationID SMALLINT
	,@shtDetailTypeID SMALLINT
	,@strDetailValue VARCHAR(50)
AS
	SET XACT_ABORT ON

	BEGIN TRY

	INSERT INTO TStudentAccomodationDetails(guidStudentID, shtAccomodationID, shtDetailTypeID, strDetailValue)
	VALUES(@guidStudentID, @shtAccomodationID, @shtDetailTypeID, @strDetailValue)
	END TRY
	BEGIN CATCH
	ROLLBACK
	PRINT ERROR_MESSAGE()
	RETURN 1
	END CATCH

RETURN 0
