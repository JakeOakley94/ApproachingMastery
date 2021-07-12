CREATE PROCEDURE [dbo].[uspGetLoginInfo]
	@guidLoginID AS UNIQUEIDENTIFIER
AS
	DECLARE @numFound AS INTEGER = 0
	
	SELECT
		@numFound = COUNT(guidLoginID)
	FROM
		VUserLogins
	WHERE
		guidLoginID = @guidLoginID

	if @numFound = 0
		return 1

	SELECT
		 *
	FROM
		VUserLogins
	WHERE
		guidLoginID = @guidLoginID

RETURN 0
