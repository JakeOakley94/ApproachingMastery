CREATE PROCEDURE [dbo].[uspAddTeacherStudent]
	 @guidUserID		UNIQUEIDENTIFIER
	,@guidStudentID		UNIQUEIDENTIFIER	NULL
	,@guidSchoolID		UNIQUEIDENTIFIER	
	,@strFirstName		NVARCHAR(50)		NULL
	,@strMiddleName		NVARCHAR(50)		NULL
	,@strLastName		NVARCHAR(50)		NULL
	,@dteBirthday		DATE				NULL
	,@strGrade			NVARCHAR(50)		NULL
	,@dteIEPDueDate		DATE				NULL
	,@dteETRDueDate		DATE				NULL
AS
	
	SET NOCOUNT ON
	SET XACT_ABORT ON

	IF @guidUserID IS NULL AND @guidStudentID IS NULL
		RETURN 1

	
	IF @guidUserID IS NOT NULL AND @guidStudentID IS NOT NULL
	BEGIN
		BEGIN TRANSACTION
		BEGIN TRY
			INSERT INTO TTeacherStudents(guidLoginID, guidStudentID)
			VALUES (@guidUserID, @guidStudentID)
		COMMIT
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK
			RETURN 2
		END CATCH
	END

	IF @guidUserID IS NOT NULL AND @guidStudentID IS NULL
	BEGIN
		BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @result INTEGER = -1
			EXEC @result = uspAddUpdateStudent @guidStudentID OUTPUT, @strFirstName, @strMiddleName, @strLastName, @dteBirthday, 1, @dteIEPDueDate, @dteETRDueDate, @strGrade
			IF @result = 0
			BEGIN
				INSERT INTO TTeacherStudents(guidLoginID, guidStudentID)
				VALUES (@guidUserID, @guidStudentID)	
				INSERT INTO TSchoolStudents(guidSchoolID, guidStudentID)
				VALUES (@guidSchoolID, @guidStudentID)
				COMMIT
				RETURN 0
			END
			ELSE
				BEGIN
					ROLLBACK
					RETURN 4
				END
		END TRY
		BEGIN CATCH
			ROLLBACK
			RETURN 3
		END CATCH
	END
RETURN -1

