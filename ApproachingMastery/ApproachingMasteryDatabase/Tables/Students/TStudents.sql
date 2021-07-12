CREATE TABLE [dbo].[TStudents]
(
	 guidStudentID		UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,strFirstName		NVARCHAR(50)				NOT NULL
	,strMiddleName		NVARCHAR(50)				NOT NULL
	,strLastName		NVARCHAR(50)				NOT NULL
	,dteBirthday		DATE					NOT NULL
	,blnActive			BIT						NOT NULL DEFAULT 1
	,dteIEPDueDate		DATE					NULL
	,dteETRDueDate		DATE					NULL
	,strGradeLevel      NVARCHAR(50)            NULL
	,guidImageID		UNIQUEIDENTIFIER		NULL
	CONSTRAINT TStudents_PK PRIMARY KEY (guidStudentID)
	CONSTRAINT TStudents_TImages_FK FOREIGN KEY (guidImageID) REFERENCES TImages (guidImageID)
)
