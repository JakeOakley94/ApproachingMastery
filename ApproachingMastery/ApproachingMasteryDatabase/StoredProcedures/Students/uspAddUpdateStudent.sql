CREATE PROCEDURE [dbo].[uspAddUpdateStudent]
	 @guidStudentID UNIQUEIDENTIFIER = NULL OUTPUT
	,@strFirstName NVARCHAR(50)
	,@strMiddleName NVARCHAR(50)
	,@strLastName NVARCHAR(50)
	,@dteBirthday DATE
	,@blnActive BIT
	,@dteIEPDueDate DATE
	,@dteETRDueDate DATE
	,@strGradeLevel NVARCHAR(50)
AS
	SET NOCOUNT ON
	SET XACT_ABORT ON

	BEGIN TRANSACTION
	BEGIN TRY
	IF @guidStudentID IS NOT NULL
	BEGIN
		PRINT 'StudentID is not null'

		UPDATE
			TStudents
		SET
			 strFirstName = @strFirstName
			,strMiddleName = @strMiddleName
			,strLastName = @strLastName
			,dteBirthday = @dteBirthday
			,blnActive = @blnActive
			,dteIEPDueDate = @dteIEPDueDate
			,dteETRDueDate = @dteETRDueDate
			,strGradeLevel = @strGradeLevel
		WHERE
			guidStudentID = @guidStudentID
	END
	ELSE
	BEGIN
		DECLARE @inserted AS TABLE (guidStudentID UNIQUEIDENTIFIER)
		INSERT INTO TStudents (strFirstName, strMiddleName, strLastName, dteBirthday,dteIEPDueDate, dteETRDueDate, strGradeLevel)
		OUTPUT inserted.guidStudentID INTO @inserted
		VALUES (@strFirstName, @strMiddleName, @strLastName, @dteBirthday, @dteIEPDueDate, @dteETRDueDate, @strGradeLevel)
		SELECT TOP 1
			@guidStudentID = guidStudentID
		FROM
			@inserted
	END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		RETURN 1
	END CATCH
	COMMIT TRANSACTION
RETURN 0
