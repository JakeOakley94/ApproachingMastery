CREATE TABLE [dbo].[TSchoolDistrictSchools]
(
	 guidSchoolDistrictID	UNIQUEIDENTIFIER
	,guidSchoolID			UNIQUEIDENTIFIER
	 CONSTRAINT TSchoolDistrictSchools_PK PRIMARY KEY (guidSchoolDistrictID, guidSchoolID)
	,CONSTRAINT TSchoolDistrictSchools_TSchoolDistricts_FK
		FOREIGN KEY (guidSchoolDistrictID) REFERENCES TSchoolDistricts(guidSchoolDistrictID)
	,CONSTRAINT TSchoolDistrictSchools_TSchools_FK
		FOREIGN KEY (guidSchoolID) REFERENCES TSchools(guidSchoolID)
)
