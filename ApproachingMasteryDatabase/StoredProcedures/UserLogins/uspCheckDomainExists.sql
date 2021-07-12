CREATE PROCEDURE [dbo].[uspCheckDomainExists]
	@strEmailDomain AS NVARCHAR(255)
AS
	DECLARE @domainCount INTEGER = 0
	SELECT
		@domainCount = COUNT(strSchoolDistrictDomainName)
	FROM
		TSchoolDistricts
	WHERE
		strUpperDomainName = UPPER(@strEmailDomain)

	IF @domainCount > 0 RETURN 0

RETURN 1
