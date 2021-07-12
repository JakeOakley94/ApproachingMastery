CREATE PROCEDURE [dbo].[uspValidateEmail]
	 @guidEmailValidationID UNIQUEIDENTIFIER
	,@guidLoginID UNIQUEIDENTIFIER OUTPUT
AS
	SET XACT_ABORT ON
	SELECT @guidLoginID = NULL
	BEGIN TRY
	BEGIN TRANSACTION
		UPDATE
			TUserLogins
		SET
			 blnEmailValidated = 1
		WHERE
			guidEmailValidationID IS NOT NULL
		AND guidEmailValidationID = @guidEmailValidationID

		SELECT
			@guidLoginID = guidLoginID
		FROM
			TUserLogins
		WHERE
			guidEmailValidationID IS NOT NULL
		AND guidEmailValidationID = @guidEmailValidationID
	COMMIT
	END TRY
	BEGIN CATCH
	ROLLBACK
		RETURN 1
	END CATCH

RETURN 0
