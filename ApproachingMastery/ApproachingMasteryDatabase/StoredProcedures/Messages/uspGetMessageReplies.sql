CREATE PROCEDURE [dbo].[uspGetMessageReplies]
	@guidParentID UNIQUEIDENTIFIER
AS
	SELECT * FROM TMessages WHERE guidParentMessageID = @guidParentID
RETURN 0
