CREATE PROCEDURE [dbo].[uspAddStudentABCEntry]
	  @guidStudentID		UNIQUEIDENTIFIER
	 ,@guidABCID			UNIQUEIDENTIFIER  NULL
	 ,@dtmIncidentDate		DATETIME
	 ,@guidAddedBy			UNIQUEIDENTIFIER
	 ,@strAntecedent		NVARCHAR(1000)
	 ,@strBehavior			NVARCHAR(1000)
	 ,@strConsequence		NVARCHAR(1000)
	
AS
	
	SET XACT_ABORT ON

	BEGIN TRANSACTION

	DECLARE  @result INTEGER
	DECLARE @guidNewID UNIQUEIDENTIFIER 
	SELECT @guidNewID = @guidABCID

	BEGIN TRY

		EXEC @result = uspAddUpdateABCEntry @guidNewID OUTPUT, @dtmIncidentDate, @guidAddedBy, @strAntecedent, @strBehavior, @strConsequence
		-- SELECT * FROM TAirTests
		IF @result > 0
		BEGIN
			ROLLBACK
			RETURN 1
		END
		ELSE
		BEGIN

		    IF @guidNewID = @guidABCID
			BEGIN
				COMMIT
				RETURN 0
			END

			INSERT INTO TStudentsABCCharts(guidStudentID, guidABCID)
			VALUES (@guidStudentID,@guidNewID)
			IF @@ROWCOUNT > 0
			BEGIN
				COMMIT
				RETURN 0
			END
			ELSE
			BEGIN
				ROLLBACK
				RETURN 2
			END
		END
	END TRY
	BEGIN CATCH
		print ERROR_MESSAGE()
		ROLLBACK
		RETURN 3
	END CATCH

RETURN 0
