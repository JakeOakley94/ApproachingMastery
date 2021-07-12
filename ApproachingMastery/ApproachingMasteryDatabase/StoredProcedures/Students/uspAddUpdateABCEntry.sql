CREATE PROCEDURE [dbo].[uspAddUpdateABCEntry]
	  @guidABCEntryID		UNIQUEIDENTIFIER NULL OUTPUT
	 ,@dtmIncidentDate		DATETIME
	 ,@guidAddedBy			UNIQUEIDENTIFIER
	 ,@strAntecedent		NVARCHAR(1000)
	 ,@strBehavior			NVARCHAR(1000)
	 ,@strConsequence		NVARCHAR(1000)
AS
	SET XACT_ABORT ON
	BEGIN TRANSACTION
	BEGIN TRY
	-- UPDATE EXISTING IF NOT NULL
	IF @guidABCEntryID IS NOT NULL
	BEGIN
		PRINT 'ABCEntry ID is NOT null'
		UPDATE
			TABCEntries
		SET
			 dtmIncidentDate = @dtmIncidentDate
			,guidAddedBy = @guidAddedBy
			,strAntecedent  =  @strAntecedent
			,strBehavior  =@strBehavior
			,strConsequence = @strConsequence
		WHERE
			guidABCID = @guidABCEntryID

		IF @@ROWCOUNT > 0
		BEGIN
			COMMIT
			RETURN 0
		END
		ELSE
		BEGIN
			ROLLBACK
			RETURN 1
		END

	END
	ELSE -- ADD NEW ONE
	BEGIN
		PRINT 'ABCEntry ID IS NULL'
		DECLARE @inserted TABLE (guidABCID UNIQUEIDENTIFIER)
		INSERT INTO TABCEntries(guidAddedBy, dtmIncidentDate, strAntecedent, strBehavior, strConsequence)
		OUTPUT inserted.guidABCID INTO @inserted
		VALUES( @guidAddedBY, @dtmIncidentDate, @strAntecedent, @strBehavior, @strConsequence)

		SELECT TOP 1
			@guidABCEntryID = guidABCID
		FROM
			@inserted

		print @guidABCEntryID
		IF @guidABCEntryID IS NOT NULL
		BEGIN
			COMMIT
			RETURN 0
		END
		ELSE
		BEGIN
			ROLLBACK
			RETURN 1
		END
	END
	END TRY
	BEGIN CATCH
		print ERROR_MESSAGE()
		ROLLBACK
		RETURN 2
	END CATCH

RETURN 0
