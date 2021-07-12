-- ----------------------------------------------------------------------------
-- Name: ufnGetLastNPasswords
-- Abstract: Gets the last N passwords for the specified user
-- ----------------------------------------------------------------------------
GO
CREATE FUNCTION ufnGetLastNPasswords
(
	 @guidLoginID UNIQUEIDENTIFIER
	,@intNumberOfRows INTEGER = 5
)
RETURNS TABLE
AS
RETURN SELECT TOP (@intNumberOfRows)
		 guidLoginID
		,lngPasswordID
		,guidSalt
		,biPasswordHash
	FROM
		TUserLoginPasswords (TABLOCKX)
	WHERE
		guidLoginID = @guidLoginID
	ORDER BY
		lngPasswordID DESC
GO