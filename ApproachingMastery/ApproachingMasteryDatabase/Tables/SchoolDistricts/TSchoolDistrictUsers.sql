CREATE TABLE [dbo].[TSchoolDistrictUsers]
(
	 guidSchoolDistrictID			UNIQUEIDENTIFIER	NOT NULL
	,guidLoginID					UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT	TSchoolDistrictUsers_PK PRIMARY KEY (guidSchoolDistrictID, guidLoginID)
	 CONSTRAINT TSchoolDistrictUsers_TSchools_FK FOREIGN KEY (guidSchoolDistrictID) REFERENCES TSchoolDistricts (guidSchoolDistrictID)
	 CONSTRAINT TSchoolDistrictUsers_TUserLogins_FK FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
)
