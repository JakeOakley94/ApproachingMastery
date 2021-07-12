CREATE TABLE [dbo].[TStudentObjectives]
(
	 guidStudentID		UNIQUEIDENTIFIER	NOT NULL
	,guidAssignedBy		UNIQUEIDENTIFIER	NOT NULL
	,guidGoalID			UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT TStudentObjectives_PK PRIMARY KEY (guidStudentID, guidAssignedBy, guidGoalID)
	,CONSTRAINT TStudentObjectives_TStudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
	,CONSTRAINT TStudentObjectives_TUserLogins_FK FOREIGN KEY (guidAssignedBy) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TStudentObjectives_TGoals_FK FOREIGN KEY (guidGoalID) REFERENCES TGoals (guidGoalID)
)
