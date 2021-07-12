CREATE TABLE [dbo].[TTeacherStudents]
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,guidStudentID			UNIQUEIDENTIFIER	NOT NULL
	CONSTRAINT TStudenTUsers_PK PRIMARY KEY (guidStudentID, guidLoginID)
	CONSTRAINT TTeacherStudents_TUserLogins_FK FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
	CONSTRAINT TTeacherStudents_TStudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
)
