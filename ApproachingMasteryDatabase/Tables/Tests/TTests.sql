CREATE TABLE [dbo].[TTests]
(
	 guidTestID			UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,shtTestNameID		SMALLINT				NOT NULL
	,shtTestYear		SMALLINT				NOT NULL
	,shtSemesterID		SMALLINT				NOT NULL
	,dblGrade			FLOAT					NOT NULL
	 CONSTRAINT TTests_PK PRIMARY KEY (guidTestID)
	,CONSTRAINT TTests_TTestNames_FK FOREIGN KEY (shtTestNameID) REFERENCES TTestNames (shtTestNameID)
	,CONSTRAINT TTests_TSemesters_FK FOREIGN KEY (shtSemesterID) REFERENCES TSemesters (shtSemsterID)
)
