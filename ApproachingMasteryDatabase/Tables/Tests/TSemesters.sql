CREATE TABLE [dbo].[TSemesters]
(
	 shtSemsterID		SMALLINT		NOT NULL IDENTITY(1,1)
	,strSemester		NVARCHAR(50)		NOT NULL
	,strUpperSemester AS UPPER(strSemester)
	 CONSTRAINT TSemesters_PK PRIMARY KEY (shtSemsterID)
	,CONSTRAINT TSemesters_UN UNIQUE (strUpperSemester)
)
