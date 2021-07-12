CREATE PROCEDURE [dbo].[uspRemoveStudentAccommodation]
	@guidStudentID UNIQUEIDENTIFIER
   ,@shtAccomodationID SMALLINT
AS
    DELETE FROM TStudentAccomodationDetails WHERE guidStudentID = @guidStudentID AND shtAccomodationID = @shtAccomodationID
	DELETE FROM TStudentAccomodationTests WHERE guidStudentID = @guidStudentID AND shtAccomodationID = @shtAccomodationID
	DELETE FROM TStudentAccomodations WHERE guidStudentID = @guidStudentID AND shtAccomodationID = @shtAccomodationID
RETURN 0
