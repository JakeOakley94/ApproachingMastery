CREATE PROCEDURE [dbo].[uspRemoveStudentAccommodationDetail]
	@guidStudentID UNIQUEIDENTIFIER
	,@shtAccomodationID smallint
	,@shtDetailTypeID  smallint

AS
	
	DELETE FROM TStudentAccomodationDetails WHERE guidStudentID = @guidStudentID AND shtAccomodationID = @shtAccomodationID and shtDetailTypeID = @shtDetailTypeID


RETURN 0
