CREATE PROCEDURE [dbo].[uspAddSchool]
	 @guidSchoolDistrictID	UNIQUEIDENTIFIER
	,@strSchoolName			NVARCHAR(255)
	,@guidSchoolID			UNIQUEIDENTIFIER	OUTPUT
AS

	DECLARE  @result					INTEGER = 0
			,@SUCCESS					INTEGER = 0
			,@SCHOOL_ALREADY_EXISTS		INTEGER = 1
			,@UNKNOWN_ERROR				INTEGER = 2
	SET XACT_ABORT ON 
	SET NOCOUNT ON

	SELECT @guidSchoolID = NULL
	
	BEGIN TRY
		BEGIN TRANSACTION
		DECLARE @upperInputName NVARCHAR(255) = UPPER(@strSchoolName)
		SELECT
			 @guidSchoolID = guidSchoolID
		FROM
			VSchoolDistrictSchools (TABLOCKX)
		WHERE
			strUpperSchoolName = @upperInputName
		AND guidSchoolDistrictID = @guidSchoolDistrictID

		IF @guidSchoolID IS NOT NULL
			SELECT @result = @SCHOOL_ALREADY_EXISTS
		ELSE
		BEGIN
			DECLARE @generatedKeys AS TABLE (guidSchoolID UNIQUEIDENTIFIER)
			INSERT INTO TSchools (strSchoolName) OUTPUT inserted.guidSchoolID INTO @generatedKeys
			VALUES (@strSchoolName)

			SELECT
				@guidSchoolID = guidSchoolID
			FROM
				@generatedKeys

			INSERT INTO TSchoolDistrictSchools (guidSchoolDistrictID, guidSchoolID)
			VALUES (@guidSchoolDistrictID, @guidSchoolID)
		END
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		SELECT @result = @UNKNOWN_ERROR
	END CATCH

RETURN @result
