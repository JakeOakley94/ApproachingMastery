CREATE PROCEDURE [dbo].[uspAddUpdateStudentAccomodationTest]
	 @guidStudentID		UNIQUEIDENTIFIER
	,@guidAccomodationTestID UNIQUEIDENTIFIER = NULL output
	,@shtAccomodationID smallint
	,@dteDate			Date
	,@strTestGiven		VARCHAR(50)
	,@dblPercentile		FLOAT
	,@shtTestTypeID     smallint	
	
AS	

	SET XACT_ABORT ON

	BEGIN TRANSACTION

	DECLARE  @result INTEGER

	BEGIN TRY
	IF @guidAccomodationTestID IS NOT NULL
	BEGIN
		PRINT 'AccomodationTestID not null'

		UPDATE
			TAccomodationTests
		SET
			 dteDate = @dteDate
			,strTestGiven = @strTestGiven
			,dblPercentile = @dblPercentile
			,shtTestTypeID = @shtTestTypeID

		WHERE
			guidTestID = @guidAccomodationTestID

			if @@ROWCOUNT=0
				RETURN -1
			ELSE
			BEGIN
				COMMIT
				RETURN 0
			END
	END

	ELSE


		BEGIN

			DECLARE @inserted TABLE (guidInsertedID UNIQUEIDENTIFIER)

			INSERT INTO TAccomodationTests(dteDate, strTestGiven, dblPercentile, shtTestTypeID)
			OUTPUT inserted.guidTestID INTO @inserted(guidInsertedID)
			VALUES (@dteDate, @strTestGiven, @dblPercentile, @shtTestTypeID)
		
			SELECT TOP 1
				@guidAccomodationTestID = guidInsertedID
			FROM
				@inserted;


		IF @@ROWCOUNT = 0
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

		BEGIN TRY

		BEGIN
			INSERT INTO TStudentAccomodationTests(guidStudentID, shtAccomodationID, guidTestID)
			VALUES (@guidStudentID, @shtAccomodationID, @guidAccomodationTestID)
		
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