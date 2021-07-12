CREATE PROCEDURE [dbo].[uspAddUpdateTest]
	  @guidStudentID	UNIQUEIDENTIFIER
	 ,@guidTestID		UNIQUEIDENTIFIER NULL
	 ,@shtTestNameID	SMALLINT
	 ,@shtTestYear		SMALLINT
	 ,@shtSemesterID	SMALLINT
	 ,@dblGrade			FLOAT
AS
	SET XACT_ABORT ON
	BEGIN TRANSACTION
	BEGIN TRY

	-- UPDATE EXISTING IF NOT NULL
	IF @guidTestID IS NOT NULL
	BEGIN
		PRINT 'Test ID is NOT null'
		UPDATE
			TStudentTests
		SET
			 shtTestNameID = @shtTestNameID
			,shtTestYear = @shtTestYear
			,shtSemesterID = @shtSemesterID
			,dblGrade = @dblGrade
		WHERE
			guidTestID = @guidTestID

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
		PRINT 'TEST ID IS NULL'
		DECLARE @inserted TABLE (guidTestID UNIQUEIDENTIFIER)
		INSERT INTO TStudentTests (guidStudentID, shtTestNameID, shtTestYear, shtSemesterID, dblGrade)
		OUTPUT inserted.guidTestID INTO @inserted
		VALUES(@guidStudentID, @shtTestNameID, @shtTestYear, @shtSemesterID, @dblGrade)

		SELECT TOP 1
			@guidTestID = guidTestID
		FROM
			@inserted

		print @guidTestID
		IF @guidTestID IS NOT NULL
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
