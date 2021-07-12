-- ----------------------------------------------------------------------------
-- Name: ufnHashPassword
-- Abstract: Hashes the specified password with the specified salt
-- ----------------------------------------------------------------------------
CREATE FUNCTION ufnHashPassword
(
	 @guidSalt		UNIQUEIDENTIFIER
	,@strPassword	NVARCHAR(50)
)
	RETURNS VARBINARY(MAX)
AS
	BEGIN
		DECLARE @result VARBINARY(MAX)
		SELECT @result = HASHBYTES('SHA2_512', @strPassword + CAST(@guidSalt AS NVARCHAR(36)))
		return @result
	END
GO
