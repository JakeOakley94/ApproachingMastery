CREATE TABLE [dbo].[TStudentAirTests]
(
	 guidStudentID		UNIQUEIDENTIFIER NOT NULL
	,guidAirTestID		UNIQUEIDENTIFIER NOT NULL
	 CONSTRAINT TStudentAirTests_PK PRIMARY KEY (guidStudentID, guidAirTestID)
	,CONSTRAINT TStudentAirTests_TStudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
	,CONSTRAINT TStudentAirTests_TAirTests_FK FOREIGN KEY (guidAirTestID) REFERENCES TAirTests (guidAirTestID)

)
