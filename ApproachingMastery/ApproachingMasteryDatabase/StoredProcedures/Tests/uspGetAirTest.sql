CREATE PROCEDURE [dbo].[uspGetAirTest]
	@guidAirTestID UNIQUEIDENTIFIER
AS
	SELECT
		*
	FROM
		TAirTests
	WHERE 
		guidAirTestID = @guidAirTestID
RETURN 0
