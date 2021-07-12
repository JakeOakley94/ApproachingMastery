CREATE PROCEDURE [dbo].[uspAddUpdateMessage]
	 @guidMessageID			UNIQUEIDENTIFIER NULL OUTPUT
	,@guidSenderID			UNIQUEIDENTIFIER
	,@guidRecipientID		UNIQUEIDENTIFIER NULL
	,@guidParentMessageID	UNIQUEIDENTIFIER NULL
	,@strMessage			NVARCHAR(1000)
AS
	SET XACT_ABORT ON
	SET NOCOUNT ON
	BEGIN TRANSACTION
	BEGIN TRY
		-- IS THE VALUE NULL?
		IF @guidMessageID IS NULL
		BEGIN
			-- YES, INSERT NEW ENTRY
			DECLARE @inserted TABLE(guidMessageID UNIQUEIDENTIFIER)

			INSERT INTO TMessages (guidSenderID, guidRecipientID, guidParentMessageID, strMessage, dtmMessageDateTime)
			OUTPUT inserted.guidMessageID INTO @inserted
			VALUES (@guidSenderID, @guidRecipientID, @guidParentMessageID, @strMessage, SYSDATETIME())

			SELECT TOP 1
				@guidMessageID = guidMessageID
			FROM
				@inserted
		END
		ELSE -- no, update the existing message
		BEGIN
			UPDATE TMessages
			SET
				 guidSenderID = @guidSenderID
				,guidRecipientID = @guidRecipientID
				,guidParentMessageID = @guidParentMessageID
				,strMessage = @strMessage
				,dtmMessageDateTime = SYSDATETIME()
			WHERE
				guidMessageID = @guidMessageID
		END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		RETURN 1
		ROLLBACK
	END CATCH


RETURN 0
