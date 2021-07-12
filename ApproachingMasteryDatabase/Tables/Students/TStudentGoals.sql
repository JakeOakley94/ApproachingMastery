CREATE TABLE [dbo].[TStudentGoals]
(
	 guidStudentID		UNIQUEIDENTIFIER	NOT NULL
	,guidAssignedBy		UNIQUEIDENTIFIER	NOT NULL
	,guidGoalID			UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT TStudentGoals_PK PRIMARY KEY (guidStudentID, guidAssignedBy, guidGoalID)
	,CONSTRAINT TStudentGoals_TStudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
	,CONSTRAINT TStudentGoals_TUserLogins_FK FOREIGN KEY (guidAssignedBy) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TStudentGoals_TGoals_FK FOREIGN KEY (guidGoalID) REFERENCES TGoals (guidGoalID)

)
