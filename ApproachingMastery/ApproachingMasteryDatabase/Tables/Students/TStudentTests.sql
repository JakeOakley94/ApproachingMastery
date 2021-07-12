CREATE TABLE [dbo].[TStudentTests]
(
	 guidStudentID		UNIQUEIDENTIFIER		NOT NULL
	,shtTestNameID		SMALLINT				NOT NULL
	,shtTestYear		SMALLINT				NOT NULL
	,shtSemesterID		SMALLINT				NOT NULL
	,guidTestID			UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,dblGrade			FLOAT					NOT NULL
	 CONSTRAINT TStudentTests_PK PRIMARY KEY (guidStudentID, shtTestNameID, shtTestYear, shtSemesterID)
	,CONSTRAINT TSTudentTests_UN UNIQUE(guidTestID)
	,CONSTRAINT TStudentTests_TStudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
	,CONSTRAINT TStudentTests_TTestNames_FK FOREIGN KEY (shtTestNameID) REFERENCES TTestNames (shtTestNameID)
	,CONSTRAINT TStudentTests_TSemesters_FK FOREIGN KEY (shtSemesterID) REFERENCES TSemesters (shtSemsterID)
)
