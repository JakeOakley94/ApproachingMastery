﻿CREATE TABLE [dbo].[TStudentsABCCharts]
(
	 guidStudentID		UNIQUEIDENTIFIER	NOT NULL
	,guidABCID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	 CONSTRAINT TStudentsABCCharts_PK PRIMARY KEY (guidStudentID, guidABCID)
	,CONSTRAINT TStudentsABCCharts_TSTudents_FK FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)
	,CONSTRAINT TStudentsABCChrts_TABCEntries_FK FOREIGN KEY (guidABCID) REFERENCES TABCEntries (guidABCID)

)
