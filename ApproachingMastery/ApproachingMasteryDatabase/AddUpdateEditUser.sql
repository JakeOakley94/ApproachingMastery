
USE dbApproachingMastery

IF OBJECT_ID('VUserInformation')					IS NOT NULL DROP VIEW VUserInformation
IF OBJECT_ID('VInvalidUserLoginAttempts')			IS NOT NULL DROP VIEW VInvalidUserLoginAttempts
IF OBJECT_ID('VUserLoginPasswordResets')			IS NOT NULL DROP VIEW VUserLoginPasswordResets
IF OBJECT_ID('VUserLoginSuccess')					IS NOT NULL DROP VIEW VUserLoginSuccess

IF OBJECT_ID('uspCreateNewLogin')					IS NOT NULL DROP PROCEDURE uspCreateNewLogin
IF OBJECT_ID('uspAddLoginEvent')					IS NOT NULL DROP PROCEDURE uspAddLoginEvent
IF OBJECT_ID('uspUpdatePassword')					IS NOT NULL DROP PROCEDURE uspUpdatePassword
IF OBJECT_ID('uspLoginUser')						IS NOT NULL DROP PROCEDURE uspLoginUser
IF OBJECT_ID('ufnCheckAccountLocked')				IS NOT NULL DROP FUNCTION ufnCheckAccountLocked

IF OBJECT_ID('ufnGetLastNPasswords')				IS NOT NULL DROP FUNCTION ufnGetLastNPasswords
IF OBJECT_ID('ufnHashPassword')						IS NOT NULL DROP FUNCTION ufnHashPassword

-- ----------------------------------------------------------------------------
-- Views for TUsers
-- ----------------------------------------------------------------------------

-- ----------------------------------------------------------------------------
-- Name: VUserInformation
-- Abstract: Gets all of the user information
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VUserInformation
AS
	SELECT
		 TUR.intUserRoleID
		,TUR.strUserRole
		,TUL.guidLoginID
		,TUL.strEmailAddress
		,TUL.blnEmailValidated
		,TUL.guidEmailValidationID
		,TUL.guidPasswordResetID
		,TUL.blnActive
		,TU.strFirstName
		,TU.strMiddleName
		,TU.strLastName
		,TU.strPhoneNumber
		,TU.strClass
		,TI.guidImageID
		,TI.blbImage
	FROM
		TUserRoles AS TUR
		INNER JOIN TUserLogins AS TUL
			INNER JOIN TUsers AS TU
				INNER JOIN TImages AS TI
				ON TI.guidImageID = TU.guidImageID
			ON TU.guidLoginID = TUL.guidLoginID
		ON TUL.intUserRoleID = TUR.intUserRoleID
GO

-- ----------------------------------------------------------------------------
-- Name: VInvalidUserLoginAttempts
-- Abstract: Gets all of the invalid login attempts per user
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VInvalidUserLoginAttempts
AS
	SELECT
		 TULE.guidLoginID
		,TULE.dtmEventTime
		,TULE.strIPAddress
		,TULET.*
	FROM
		TUserLoginEvents AS TULE
		INNER JOIN TUserLoginEventTypes AS TULET
		ON TULET.intEventTypeID = TULE.intEventTypeID
	WHERE
		TULET.intEventTypeID = 2
GO

-- ----------------------------------------------------------------------------
-- Name: VUserLoginPasswordResets
-- Abstract: Gets all of the password resets per user
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VUserLoginPasswordResets
AS
	SELECT
		 TULE.guidLoginID
		,TULE.dtmEventTime
		,TULE.strIPAddress
		,TULET.*
	FROM
		TUserLoginEvents AS TULE
		INNER JOIN TUserLoginEventTypes AS TULET
		ON TULET.intEventTypeID = TULE.intEventTypeID
	WHERE
		TULET.intEventTypeID = 3
GO

-- ----------------------------------------------------------------------------
-- Name: VUserLoginSuccess
-- Abstract: Gets all of the successfull logins per user
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VUserLoginSuccess
AS
	SELECT
		 TULE.guidLoginID
		,TULE.dtmEventTime
		,TULE.strIPAddress
		,TULET.*
	FROM
		TUserLoginEvents AS TULE
		INNER JOIN TUserLoginEventTypes AS TULET
		ON TULET.intEventTypeID = TULE.intEventTypeID
	WHERE
		TULET.intEventTypeID = 1
GO

-- ----------------------------------------------------------------------------
-- Procedures and Functions for TUsers
-- ----------------------------------------------------------------------------

-- ----------------------------------------------------------------------------
-- Name: uspCreateLogin
-- Abstract: Creates a new login
-- ----------------------------------------------------------------------------
GO 
CREATE PROCEDURE uspCreateNewLogin
	 @strEmailAddress	VARCHAR(50)
	,@strPassword		VARCHAR(50)
AS
	DECLARE  @SUCCESS			INTEGER = 1
			,@USER_ID_EXISTS	INTEGER = 2
			,@UNKNOWN_ERROR		INTEGER = 3

	DECLARE @userIDExists INTEGER
	DECLARE @intResult INTEGER
	DECLARE @newID AS UNIQUEIDENTIFIER
	SET NOCOUNT ON -- Report only errors 
	SET XACT_ABORT ON -- ROLEBACK ON ERROR
	SELECT @intResult = @UNKNOWN_ERROR
	BEGIN TRANSACTION

	-- CHECK IF THE EMAIL ADDRESS ALREADY EXISTS
	SELECT
		@userIDExists = COUNT(strEmailAddress)
	FROM
		TUserLogins (TABLOCKX)
	WHERE
		UPPER(strEmailAddress)=UPPER(@strEmailAddress)

	-- DOES THE EMAIL ADDRESS EXIST?
	IF(@userIDExists > 0)
	BEGIN
		-- YES, SET THE RESULT
		SELECT @intResult = @USER_ID_EXISTS
	END
	ELSE
		-- NO, CREATE NEW LOGIN
	BEGIN
		DECLARE @generatedIDs TABLE(id UNIQUEIDENTIFIER)
		INSERT INTO TUserLogins (strEmailAddress, guidEmailValidationID) OUTPUT Inserted.guidLoginID INTO @generatedIDs
		VALUES	(@strEmailAddress, NEWID()) 
		
		SELECT @intResult = @SUCCESS
	END
	  -- was a new login created?
	IF @intResult = @SUCCESS
	BEGIN -- yes, set the password
		SELECT @intResult = @UNKNOWN_ERROR
		SELECT TOP 1
			@newID = id
		FROM
			@generatedIDs
			EXEC uspUpdatePassword @newID, @strPassword, @intResult OUTPUT
	END
	

	COMMIT TRANSACTION

	-- SELECT TO GET THE RESULT TO THE CALLING METHOD
	SELECT
		 @intResult AS intResult
		,guidLoginID
	FROM
		TUserLogins
	WHERE
		UPPER(strEmailAddress)=UPPER(@strEmailAddress)	
GO

-- ----------------------------------------------------------------------------
-- Name: ufnGetLastNPasswords
-- Abstract: Gets the last N passwords for the specified user
-- ----------------------------------------------------------------------------
GO
CREATE FUNCTION ufnGetLastNPasswords
(
	 @guidLoginID UNIQUEIDENTIFIER
	,@intNumberOfRows INTEGER = 5
)
RETURNS TABLE
AS
RETURN SELECT TOP (@intNumberOfRows)
		*
	FROM
		TUserLoginPasswords (TABLOCKX)
	WHERE
		guidLoginID = @guidLoginID
	ORDER BY
		intPasswordID DESC
GO

-- ----------------------------------------------------------------------------
-- Name: ufnHashPassword
-- Abstract: Hashes the specified password with the specified salt
-- ----------------------------------------------------------------------------
CREATE FUNCTION ufnHashPassword
(
	 @guidSalt		UNIQUEIDENTIFIER
	,@strPassword	VARCHAR(50)
)
	RETURNS VARBINARY(MAX)
AS
	BEGIN
		DECLARE @result VARBINARY(MAX)
		SELECT @result = HASHBYTES('SHA2_512', @strPassword + CAST(@guidSalt AS VARCHAR(36)))
		return @result
	END
GO

-- ----------------------------------------------------------------------------
-- Name: uspUpdatePassword
-- Abstract: Updates the password for the specified user
-- ----------------------------------------------------------------------------
GO 
CREATE PROCEDURE uspUpdatePassword
	 @guidLoginID	UNIQUEIDENTIFIER
	,@strPassword	VARCHAR(50)
	,@intResult		INTEGER OUTPUT
AS

SET XACT_ABORT ON
SET NOCOUNT ON

DECLARE  @SUCCESS				INTEGER = 1
		,@PREVIOUS_PASSWORD		INTEGER = 2
		,@ERROR					INTEGER = 3


BEGIN TRANSACTION


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
		EXEC uspAddLoginEvent @guidLoginID, 3
	END
	ELSE
		SELECT @intResult = @PREVIOUS_PASSWORD
		
COMMIT TRANSACTION

GO

GO
CREATE PROCEDURE uspAddLoginEvent
	 @guidLoginID				UNIQUEIDENTIFIER
	,@intEventTypeID			INTEGER
AS

SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRANSACTION

	INSERT INTO TUserLoginEvents (guidLoginID,intEventTypeID, strIPAddress)
	VALUES (@guidLoginID, @intEventTypeID, dbo.GetCurrentIP())

COMMIT TRANSACTION
GO

GO
CREATE FUNCTION ufnCheckAccountLocked
(
	@guidLoginID	UNIQUEIDENTIFIER
)
	RETURNS BIT
AS
BEGIN
	
	DECLARE  @dtmLastChange		DATETIME = NULL
			,@dtmLastSuccess	DATETIME = NULL
			,@dtmLastCheck		DATETIME = NULL
			,@errorCount		INTEGER	 = 0
			,@MAX_NUMBER_ATTEMPTS INTEGER = 5

	SELECT
		@dtmLastChange = MAX(dtmEventTime)
	FROM
		VUserLoginPasswordResets
	

	SELECT
		@dtmLastSuccess = MAX(dtmEventTime)
	FROM
		VUserLoginSuccess
	
	-- IF THE LAST 2 CHECKS ARE NOT NULL, GET THE MOST RECENT
	IF @dtmLastChange IS NOT NULL AND @dtmLastSuccess IS NOT NULL
		SELECT @dtmLastCheck = MAX(checkDate) FROM (VALUES(@dtmLastChange), (@dtmLastSuccess)) AS AllChecks(checkDate)
	ELSE -- ONE OF THEM IS NULL
	BEGIN
		IF @dtmLastChange IS NOT NULL -- THE LAST CHANGE IS NOT
			SELECT @dtmLastCheck = @dtmLastChange
		ELSE -- THE LAST SUCCESS IS NOT
		BEGIN
			IF @dtmLastSuccess IS NOT NULL
				SELECT @dtmLastCheck = @dtmLastSuccess
			ELSE -- THE BOTH ARE, USE EPOC TIME
				SELECT @dtmLastCheck = '1/1/1970'
		END
	END

	SELECT 
		@errorCount = COUNT(guidLoginID)
	FROM
		VInvalidUserLoginAttempts
	WHERE
		guidLoginID = @guidLoginID
	AND dtmEventTime > @dtmLastCheck
	
	DECLARE @intResult INTEGER

	IF @errorCount >= @MAX_NUMBER_ATTEMPTS
		SELECT @intResult = 1
	ELSE
		SELECT @intResult = 0

	RETURN @intResult
END


GO
CREATE PROCEDURE uspLoginUser
	 @strEmailAddress	VARCHAR(50)
	,@strPassword		VARCHAR(50)
	,@intResult			INTEGER			OUTPUT
AS
	DECLARE  @SUCCESS					INTEGER = 1
			,@INVALID_USERNAME_PASSWORD INTEGER = 2
			,@ACCOUNT_LOCKED			INTEGER = 3
			,@ACCOUNT_DISABLED			INTEGER = 4
			,@EMAIL_NOT_VERIFIED		INTEGER	= 5
			,@isAccountLocked			INTEGER = 0
			,@guidLoginID				UNIQUEIDENTIFIER
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
		UPPER(strEmailAddress)=UPPER(@strEmailAddress)

	IF @guidLoginID IS NULL	RETURN @INVALID_USERNAME_PASSWORD
	IF @blnActive = 0
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 4
		RETURN @ACCOUNT_DISABLED
	END
	IF @blnEmailVerified = 0
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 4
		RETURN @EMAIL_NOT_VERIFIED
	END
	
	IF dbo.ufnCheckAccountLocked(@guidLoginID) = 1
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 4
		RETURN @ACCOUNT_LOCKED
	END

	SELECT TOP 1
		 @guidSalt = guidSalt
		,@biPasswordHash = biPasswordHash
	FROM
		TUserLoginPasswords
	WHERE
		guidLoginID = @guidLoginID
	ORDER BY intPasswordID

	IF DBO.ufnHashPassword(@guidSalt,@strPassword) = @biPasswordHash
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 1
		RETURN @SUCCESS
	END
	ELSE
	BEGIN
		EXEC uspAddLoginEvent @guidLoginID, 2
		IF DBO.ufnCheckAccountLocked(@guidLoginID)=1
			RETURN @ACCOUNT_LOCKED
		ELSE
			RETURN @INVALID_USERNAME_PASSWORD
	END
GO

		



-- testing

-- DELETE FROM TUserLoginPasswords
-- DELETE FROM TUserLogins


EXEC uspCreateNewLogin 'bcapuana@cincinnatistate.edu', 'test12345'

DECLARE @intResult INTEGER
EXEC uspLoginUser 'bcapuana@cincinnatistate.edu','test1234', @intResult

SELECT * FROM TUserLoginEvents ORDER BY dtmEventTime DESC
UPDATE TUserLogins
SET blnEmailValidated = 1
