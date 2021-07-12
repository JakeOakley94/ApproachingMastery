CREATE TABLE [dbo].[TStudentAccomodations]
(
	 guidStudentID			UNIQUEIDENTIFIER	NOT NULL
	,shtAccomodationID		SMALLINT			NOT NULL
	,guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT TStudentAccomodations_PK					PRIMARY KEY (guidStudentID, shtAccomodationID)
	,CONSTRAINT TStudentAccomodations_TTeacherStudents_FK	
		FOREIGN KEY (guidStudentID, guidLoginID) REFERENCES TTeacherStudents (guidStudentID, guidLoginID)
	,CONSTRAINT TStudentAccomodations_TAccomodations_FK
		FOREIGN KEY (shtAccomodationID) REFERENCES TAccomodations (shtAccomodationID)
)
