
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
	WHERE guidLoginID = @guidLoginID
	

	SELECT
		@dtmLastSuccess = MAX(dtmEventTime)
	FROM
		VUserLoginSuccess
	WHERE guidLoginID = @guidLoginID
	
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
