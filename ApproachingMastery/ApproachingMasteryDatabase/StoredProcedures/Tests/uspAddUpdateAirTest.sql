CREATE PROCEDURE [dbo].[uspAddUpdateAirTest]
	  @guidAirTestID		UNIQUEIDENTIFIER NULL OUTPUT
	 ,@shtAirTestYear		SMALLINT
	 ,@dblMathGrade			FLOAT
	 ,@dblReadingGrade			FLOAT
AS
	SET XACT_ABORT ON
	BEGIN TRANSACTION
	BEGIN TRY
	-- UPDATE EXISTING IF NOT NULL
	IF @guidAirTestID IS NOT NULL
	BEGIN
		PRINT 'AirTest ID is NOT null'
		UPDATE
			TAirTests
		SET
			 dblMathGrade = @dblMathGrade
			,dblReadingGrade = @dblReadingGrade
			,shtYear = @shtAirTestYear
		WHERE
			guidAirTestID = @guidAirTestID

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
		PRINT 'AirTest ID IS NULL'
		DECLARE @inserted TABLE (guidAirTestID UNIQUEIDENTIFIER)
		INSERT INTO TAirTests (shtYear, dblMathGrade, dblReadingGrade)
		OUTPUT inserted.guidAirTestID INTO @inserted
		VALUES( @shtAirTestYear, @dblMathGrade, @dblReadingGrade)

		SELECT TOP 1
			@guidAirTestID = guidAirTestID
		FROM
			@inserted

		print @guidAirTestID
		IF @guidAirTestID IS NOT NULL
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
