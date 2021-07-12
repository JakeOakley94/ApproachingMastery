CREATE PROCEDURE [dbo].[uspGetStudentAccommodationDetails]
	@guidStudentID UNIQUEIDENTIFIER
   ,@shtAccomodationID smallint
   
   AS 

   SELECT * FROM TStudentAccomodationDetails WHERE guidStudentID = @guidStudentID and shtAccomodationID = @shtAccomodationID

   return 0