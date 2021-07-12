CREATE TABLE [dbo].[TSchoolStudents]
(
	 guidSchoolID					UNIQUEIDENTIFIER	NOT NULL
	,guidStudentID					UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT	TSchoolStudents_PK PRIMARY KEY (guidSchoolID, guidStudentID)
	 CONSTRAINT TSchoolStudents_TSchools_FK FOREIGN KEY (guidSchoolID) REFERENCES TSchools (guidSchoolID)
	 CONSTRAINT TSchoolStudents_TUserStudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
)
