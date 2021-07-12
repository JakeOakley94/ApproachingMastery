CREATE PROCEDURE [dbo].[uspAddStudentTest]
	 @guidStudentID		UNIQUEIDENTIFIER
	,@shtTestNameID		SMALLINT
	,@shtTestYear		SMALLINT
	,@shtSemesterID		SMALLINT
	,@dblGrade			FLOAT
	
AS
	
	SET XACT_ABORT ON

	BEGIN TRANSACTION

	DECLARE  @result INTEGER
			,@testID UNIQUEIDENTIFIER = NULL

	BEGIN TRY

		EXEC @result = uspAddUpdateTest @testID OUTPUT, @shtTestNameID, @shtTestYear, @shtSemesterID, @dblGrade
		SELECT * FROM TTests
		IF @result > 0
		BEGIN
			ROLLBACK
			RETURN 1
		END
		ELSE
		BEGIN
			INSERT INTO TStudentTests (guidStudentID, guidTestID)
			VALUES (@guidStudentID, @testID)
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
