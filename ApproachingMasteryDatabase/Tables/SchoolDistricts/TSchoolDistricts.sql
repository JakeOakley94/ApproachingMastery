CREATE TABLE [dbo].[TSchoolDistricts]
(
	 guidSchoolDistrictID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,strSchoolDistrictName			NVARCHAR(255)			NOT NULL
	,strSchoolDistrictDomainName	NVARCHAR(255)			NOT NULL
	,strUpperDomainName AS UPPER(strSchoolDistrictDomainName)
	 CONSTRAINT TSchoolDistricts_PK PRIMARY KEY (guidSchoolDistrictID)
	,CONSTRAINT TSchoolDistricts_UN UNIQUE (strUpperDomainName)
)
