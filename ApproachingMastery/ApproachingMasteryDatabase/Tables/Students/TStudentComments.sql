CREATE TABLE [dbo].[TStudentComments]
(
	 guidStudentID			UNIQUEIDENTIFIER	NOT NULL
	,guidMessageID			UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT TStudentComments_PK						PRIMARY KEY (guidStudentID, guidMessageID)
	,CONSTRAINT TStudentComments_TMessages_FK			FOREIGN KEY (guidMessageID) REFERENCES TMessages (guidMessageID)
	,CONSTRAINT TStudentComments_TStudents_FK			FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
)
