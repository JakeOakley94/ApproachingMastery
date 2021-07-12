-- ----------------------------------------------------------------------------
-- Name: uspCreateLogin
-- Abstract: Creates a new login
-- ----------------------------------------------------------------------------
GO 
CREATE PROCEDURE uspCreateNewLogin
	 @strEmailAddress	NVARCHAR(50)
	,@strPassword		NVARCHAR(50)
	,@strIPAddress		VARCHAR(50)
	,@strFirstName		NVARCHAR(50)
	,@strMiddleName		NVARCHAR(50)
	,@strLastName		NVARCHAR(50)
	,@strPhoneNumber	VARCHAR(50)
AS
DECLARE  @SUCCESS			INTEGER = 0
			,@USER_ID_EXISTS	INTEGER = 2
			,@UNKNOWN_ERROR		INTEGER = 3

	DECLARE @userIDExists INTEGER
	DECLARE @intResult INTEGER
	DECLARE @newID AS UNIQUEIDENTIFIER
	SET NOCOUNT ON -- Report only errors 
	SET XACT_ABORT ON -- ROLEBACK ON ERROR
	SELECT @intResult = @UNKNOWN_ERROR
	

	-- CHECK IF THE EMAIL ADDRESS ALREADY EXISTS
	SELECT
		@userIDExists = COUNT(strEmailAddress)
	FROM
		TUserLogins (TABLOCKX)
	WHERE
		UPPER(strEmailAddress)=UPPER(@strEmailAddress)

	BEGIN TRANSACTION
	-- DOES THE EMAIL ADDRESS EXIST?
	IF(@userIDExists > 0)
	BEGIN
		-- YES, SET THE RESULT
		SELECT @intResult = @USER_ID_EXISTS
	END
	ELSE
		-- NO, CREATE NEW LOGIN
	BEGIN
		PRINT 'CREATING NEW LOGIN'
		DECLARE @generatedIDs TABLE(id UNIQUEIDENTIFIER)
		INSERT INTO TUserLogins (strEmailAddress, guidEmailValidationID) OUTPUT Inserted.guidLoginID INTO @generatedIDs
		VALUES	(@strEmailAddress, NEWID()) 
		SELECT @intResult = @SUCCESS
	END
	  -- was a new login created?
	IF @intResult = @SUCCESS
	BEGIN -- yes, set the password
		PRINT 'ADDING USER INFO'
		SELECT TOP 1
			@newID = id
		FROM
			@generatedIDs
		DECLARE @pwResult INTEGER
		DECLARE @userResult INTEGER
		EXEC @pwResult = uspUpdatePassword @newID, @strPassword, @strIPAddress
		EXEC @userResult = uspAddUpdateUserInfo @newID, @strFirstName, @strMiddleName, @strLastName, @strPhoneNumber, NULL, NULL
		
		IF @pwResult!=0 OR @userResult!=0
		BEGIN
			ROLLBACK
			SELECT ERROR_MESSAGE()
			RETURN @UNKNOWN_ERROR
		END
		

	END
	
	COMMIT TRANSACTION	
	

	-- SELECT TO GET THE RESULT TO THE CALLING METHOD
	SELECT
		 @intResult AS intResult
		,guidLoginID
		,guidEmailValidationID
	FROM
		TUserLogins
	WHERE
		UPPER(strEmailAddress)=UPPER(@strEmailAddress)
RETURN 0