CREATE PROCEDURE [dbo].[uspAddUpdateComment]
	 @guidMessageID			UNIQUEIDENTIFIER NULL OUTPUT
	,@guidStudentID			UNIQUEIDENTIFIER
	,@guidSenderID			UNIQUEIDENTIFIER
	,@strMessage			NVARCHAR(1000)
AS
	SET XACT_ABORT ON
	SET NOCOUNT ON
	BEGIN TRANSACTION
	BEGIN TRY
		-- Is the message id null?
		IF @guidMessageID IS NULL
		BEGIN
			-- yes, add a new message and insert into student comments
			DECLARE @result INTEGER
			EXEC @result = uspAddUpdateMessage @guidMessageID OUTPUT, @guidSenderID, NULL, NULL, @strMessage
			IF @result = 0
			BEGIN
				INSERT INTO TStudentComments (guidMessageID, guidStudentID)
				VALUES (@guidMessageID, @guidStudentID)
				COMMIT TRANSACTION
				RETURN 0
			END
			ELSE
			BEGIN
				ROLLBACK TRANSACTION
				RETURN @result
			END
		END
		ELSE -- NO, UPDATE THE MESSAGE
		BEGIN
			EXEC @result = uspAddUpdateMessage @guidMessageID OUTPUT, @strMessage
			IF @result <> 0
			BEGIN
				ROLLBACK TRANSACTION
				RETURN @result
			END
			ELSE
			BEGIN
				COMMIT TRANSACTION
				RETURN 0
			END
		END
	END TRY
	BEGIN CATCH
		ROLLBACK
		RETURN 2
	END CATCH

RETURN 0
