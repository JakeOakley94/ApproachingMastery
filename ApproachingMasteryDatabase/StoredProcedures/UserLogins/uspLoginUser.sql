CREATE PROCEDURE uspLoginUser
	 @strEmailAddress	NVARCHAR(50)
	,@strPassword		NVARCHAR(50)
	,@strIPAddress		VARCHAR(50)
	,@guidLoginID		UNIQUEIDENTIFIER	OUTPUT
AS
	DECLARE  @SUCCESS					INTEGER = 0
			,@INVALID_USERNAME_PASSWORD INTEGER = 1
			,@ACCOUNT_LOCKED			INTEGER = 2
			,@ACCOUNT_DISABLED			INTEGER = 3
			,@EMAIL_NOT_VERIFIED		INTEGER	= 4
			,@isAccountLocked			INTEGER = 0
			,@blnActive					BIT     = 0
			,@blnEmailVerified			BIT		= 0
			,@guidSalt					UNIQUEIDENTIFIER
			,@biPasswordHash			VARBINARY(MAX)

	SELECT
		 @guidLoginID = guidLoginID
		,@blnActive = blnActive
		,@blnEmailVerified = blnEmailValidated
	FROM
		TUserLogins
	WHERE
		strUpperEmailAddress=UPPER(@strEmailAddress)

	IF @guidLoginID IS NULL	RETURN @INVALID_USERNAME_PASSWORD
	IF @blnActive = 0
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 4, @strIPAddress
		RETURN @ACCOUNT_DISABLED
	END
	IF @blnEmailVerified = 0
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 4,@strIPAddress
		RETURN @EMAIL_NOT_VERIFIED
	END
	
	IF dbo.ufnCheckAccountLocked(@guidLoginID) = 1
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 4, @strIPAddress
		RETURN @ACCOUNT_LOCKED
	END

	SELECT TOP 1
		 @guidSalt = guidSalt
		,@biPasswordHash = biPasswordHash
	FROM
		TUserLoginPasswords
	WHERE
		guidLoginID = @guidLoginID
	ORDER BY lngPasswordID DESC

	IF dbo.ufnHashPassword(@guidSalt,@strPassword) = @biPasswordHash
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 1, @strIPAddress
		RETURN @SUCCESS
	END
	ELSE
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 2, @strIPAddress
		IF DBO.ufnCheckAccountLocked(@guidLoginID)=1
			RETURN @ACCOUNT_LOCKED
		ELSE
			RETURN @INVALID_USERNAME_PASSWORD
	END