CREATE PROCEDURE [dbo].[uspAddSchoolDistrict]
	 @strSchoolDistrictName AS NVARCHAR(255)
	,@strDomainName AS NVARCHAR(255)
	,@guidSchoolDistrictID AS UNIQUEIDENTIFIER OUTPUT
AS
	DECLARE  @SUCCESS			AS INTEGER = 0
			,@ALREADY_EXISTS	AS INTEGER = 1
			,@UNKNOWN_ERROR		AS INTEGER = 2
			,@result			AS INTEGER = 0

	SET NOCOUNT ON 
	SET XACT_ABORT ON
	SELECT @guidSchoolDistrictID = NULL

	BEGIN TRY
		BEGIN TRANSACTION
		DECLARE @upperInputName NVARCHAR(255) = UPPER(@strDomainName)
		SELECT
			@guidSchoolDistrictID = guidSchoolDistrictID
		FROM
			TSchoolDistricts (TABLOCKX)
		WHERE
			strUpperDomainName = @upperInputName
		

		IF @guidSchoolDistrictID IS NOT NULL 
			SELECT @result = @ALREADY_EXISTS
		ELSE
			BEGIN
				DECLARE @generatedKeys TABLE (guidSchoolDistrictID UNIQUEIDENTIFIER)

				INSERT INTO TSchoolDistricts (strSchoolDistrictName, strSchoolDistrictDomainName)
				OUTPUT inserted.guidSchoolDistrictID INTO @generatedKeys
				VALUES (@strSchoolDistrictName, @strDomainName) 

				SELECT TOP 1
					@guidSchoolDistrictID = guidSchoolDistrictID
				FROM
					@generatedKeys
			END
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		SELECT ERROR_MESSAGE()
		SELECT @result = @UNKNOWN_ERROR
	END CATCH
	
RETURN @result
