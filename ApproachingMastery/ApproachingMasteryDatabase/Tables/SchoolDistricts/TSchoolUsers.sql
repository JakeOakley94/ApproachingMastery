CREATE TABLE [dbo].[TSchoolUsers]
(
	 guidSchoolID					UNIQUEIDENTIFIER	NOT NULL
	,guidLoginID					UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT	TSchoolUsers_PK PRIMARY KEY (guidSchoolID, guidLoginID)
	 CONSTRAINT TSchoolUsers_TSchools_FK FOREIGN KEY (guidSchoolID) REFERENCES TSchools (guidSchoolID)
	 CONSTRAINT TSchoolUsers_TUserLogins_FK FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
)
