GO
CREATE PROCEDURE uspAddLoginEvent
	 @guidLoginID				UNIQUEIDENTIFIER
	,@intEventTypeID			INTEGER
	,@strIPAddress				VARCHAR(50)
AS

SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRANSACTION

	INSERT INTO TUserLoginEvents (guidLoginID,shtEventTypeID, strIPAddress)
	VALUES (@guidLoginID, @intEventTypeID, @strIPAddress)

COMMIT TRANSACTION
GO