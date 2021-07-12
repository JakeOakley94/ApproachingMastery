CREATE PROCEDURE [dbo].[uspGetUserLogin]
	@guidLoginID UNIQUEIDENTIFIER
AS
	SELECT * FROM TUserLogins WHERE guidLoginID = @guidLoginID;
RETURN 0
