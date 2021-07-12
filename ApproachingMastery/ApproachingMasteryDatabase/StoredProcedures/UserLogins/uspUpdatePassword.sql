-- ----------------------------------------------------------------------------
-- Name: uspUpdatePassword
-- Abstract: Updates the password for the specified user
-- ----------------------------------------------------------------------------
GO 
CREATE PROCEDURE uspUpdatePassword
	 @guidLoginID	UNIQUEIDENTIFIER
	,@strPassword	NVARCHAR(50)
	,@strIPAddress	NVARCHAR(50)
AS

SET XACT_ABORT ON
SET NOCOUNT ON

DECLARE  @SUCCESS				INTEGER = 0
		,@PREVIOUS_PASSWORD		INTEGER = 1
		,@ERROR					INTEGER = 2


BEGIN TRANSACTION

	DECLARE @intResult INTEGER

	SELECT @intResult = @ERROR
	-- First check if the password was already used
	DECLARE @intAlreadyUsed BIT
	DECLARE @tempPW VARBINARY(MAX)
	SELECT
		 @intAlreadyUsed = COUNT(guidLoginID)
	FROM
		ufnGetLastNPasswords(@guidLoginID,5)
	WHERE
		dbo.ufnHashPassword(guidSalt, @strPassword) = biPasswordHash

	-- was the password used?
	IF @intAlreadyUsed = 0
	BEGIN -- no, update the password
		DECLARE @guidSalt UNIQUEIDENTIFIER = NEWID()
		INSERT INTO TUserLoginPasswords (guidLoginID, guidSalt, biPasswordHash)
		VALUES (@guidLoginID, @guidSalt, dbo.ufnHashPassword(@guidSalt,@strPassword))
		SELECT @intResult = @SUCCESS
		-- add login event
		EXEC uspAddLoginEvent @guidLoginID, 3, @strIPAddress
		COMMIT
	END
	ELSE
		BEGIN
		SELECT @intResult = @PREVIOUS_PASSWORD
		ROLLBACK
		END
	return @intResult
		
