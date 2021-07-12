CREATE PROCEDURE [dbo].[uspAddStudentAirTest]
	 @guidStudentID		UNIQUEIDENTIFIER
	,@shtAirTestYear		SMALLINT
	,@dblMathGrade		FLOAT
	,@dblReadingGrade	FLOAT	
	
AS
	
	SET XACT_ABORT ON

	BEGIN TRANSACTION

	DECLARE  @result INTEGER
			,@AirTestID UNIQUEIDENTIFIER = NULL

	BEGIN TRY

		EXEC @result = uspAddUpdateAirTest @AirTestID OUTPUT, @shtAirTestYear, @dblMathGrade, @dblReadingGrade
		-- SELECT * FROM TAirTests
		IF @result > 0
		BEGIN
			ROLLBACK
			RETURN 1
		END
		ELSE
		BEGIN
			INSERT INTO TStudentAirTests (guidStudentID, guidAirTestID)
			VALUES (@guidStudentID, @AirTestID)
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
