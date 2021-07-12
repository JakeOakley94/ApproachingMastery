CREATE TABLE [dbo].[TStudentAccomodationTests]
(
	 guidStudentID				UNIQUEIDENTIFIER		NOT NULL
	,shtAccomodationID			SMALLINT				NOT NULL
	,guidTestID					UNIQUEIDENTIFIER		NOT NULL
	 CONSTRAINT TStudentAccomodationTests_PK PRIMARY KEY (guidStudentID, shtAccomodationID, guidTestID)
	,CONSTRAINT TStudentAccomodationTests_TStudents_FK
		FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
	,CONSTRAINT TStudentAccomodationTests_TAccomodations_FK
		FOREIGN KEY (shtAccomodationID) REFERENCES TAccomodations (shtAccomodationID)
	,CONSTRAINT TStudentAccomodationTests_TAccomodationTests_FK
		FOREIGN KEY (guidTestID) REFERENCES TAccomodationTests (guidTestID)
)
