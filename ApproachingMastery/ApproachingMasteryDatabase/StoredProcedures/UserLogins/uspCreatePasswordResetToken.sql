CREATE PROCEDURE [dbo].[uspCreatePasswordResetToken]
	 @strEmailAddress	UNIQUEIDENTIFIER
	,@guidToken			UNIQUEIDENTIFIER	
AS
	
	DECLARE  @dtmExpirationDateTime		DATETIME = DATEADD(DAY,1,SYSDATETIME())
			,@biTokenHash				VARBINARY(MAX)
			,@guidLoginID				UNIQUEIDENTIFIER
	DECLARE @inserted					TABLE(lngTokenID BIGINT)

	BEGIN TRANSACTION

	BEGIN TRY
		
		INSERT INTO TPasswordResetTokens(biTokenHash, dtmExpiration)
		OUTPUT inserted.lngTokenID INTO @inserted
		VALUES (@biTokenHash, @dtmExpirationDateTime)

		SELECT
			@guidLoginID = guidLoginID
		FROM
			TUserLogins
		WHERE
			strUpperEmailAddress = UPPER(@strEmailAddress)

		IF @guidLoginID = NULL
		BEGIN
			ROLLBACK
			RETURN 1
		END

		SELECT @biTokenHash = dbo.ufnHashPassword(@guidToken,CONVERT(VARCHAR,@dtmExpirationDateTime,1)+CAST(@guidLoginID AS NVARCHAR(36)))

		INSERT INTO TUserLoginPasswordResetTokens(guidLoginID, lngTokenID)
		VALUES(@guidLoginID, (SELECT lngTokenID FROM @inserted))

		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		RETURN 1
	END CATCH

RETURN 0
