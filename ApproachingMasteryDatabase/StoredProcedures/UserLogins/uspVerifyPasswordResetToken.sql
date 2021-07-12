CREATE PROCEDURE [dbo].[ufnVerifyPasswordResetToken]
	 @guidToken		UNIQUEIDENTIFIER
	,@guidLoginID	UNIQUEIDENTIFIER OUTPUT
AS
	
	DECLARE  @lngTokenID BIGINT
			,@dtmExpiration DATETIME
	SELECT
		 @guidLoginID = guidLoginID
		,@lngTokenID = lngTokenID
		,@dtmExpiration = dtmExpiration
	FROM
		VUserLoginPasswordResetTokens
	WHERE
		biTokenHash = dbo.ufnHashPassword(@guidToken,convert(varchar,dtmExpiration,1)+CAST(guidLoginID AS NVARCHAR(36)))
	
	DECLARE @timeSpan INTEGER = DATEDIFF(MINUTE, SYSDATETIME(), @dtmExpiration)
	IF @timeSpan < 0 return 1

RETURN 0
