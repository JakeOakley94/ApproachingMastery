CREATE VIEW [dbo].[VSchoolDistrictSchools]
AS
	SELECT
		 TSD.guidSchoolDistrictID
		,TSD.strSchoolDistrictName
		,TSD.strSchoolDistrictDomainName
		,TS.guidSchoolID
		,TS.strSchoolName
		,UPPER(TS.strSchoolName) AS strUpperSchoolName
	FROM
		TSchoolDistricts AS TSD
		INNER JOIN TSchoolDistrictSchools AS TSDS
			INNER JOIN TSchools AS TS
			ON TS.guidSchoolID = TSDS.guidSchoolID
		ON TSDS.guidSchoolDistrictID = TSD.guidSchoolDistrictID
