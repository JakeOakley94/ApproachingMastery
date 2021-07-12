CREATE PROCEDURE [dbo].[uspAddUpdateUserInfo]
	 @guidLoginID		UNIQUEIDENTIFIER
	,@strFirstName		NVARCHAR(50)
	,@strMiddleName		NVARCHAR(50)
	,@strLastName		NVARCHAR(50)
	,@strPhoneNumber	VARCHAR(50)
	,@guidImageID		UNIQUEIDENTIFIER
	,@strClass			NVARCHAR(50)
AS
	SET XACT_ABORT ON

	BEGIN TRANSACTION
	BEGIN TRY
	DECLARE @numLogins INTEGER = 0
	SELECT
		@numLogins = COUNT(guidLoginID)
	FROM
		TUsers (TABLOCKX)
	WHERE
		guidLoginID = @guidLoginID

	IF @numLogins = 0
	BEGIN
		INSERT INTO TUsers (guidLoginID, strFirstName, strMiddleName, strLastName, strPhoneNumber, guidImageID, strClass)
		VALUES (@guidLoginID, @strFirstName, @strMiddleName, @strLastName, @strPhoneNumber, @guidImageID, @strClass)
	END
	ELSE
	BEGIN
		UPDATE 
			TUsers
		SET
			 strFirstName = @strFirstName
			,strMiddleName = @strMiddleName
			,strLastName = @strLastName
			,strPhoneNumber = @strPhoneNumber
			,guidImageID = @guidImageID
			,strClass = @strClass
		WHERE
			guidLoginID = @guidLoginID
	END
	COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		RETURN 2
	END CATCH

RETURN 0
