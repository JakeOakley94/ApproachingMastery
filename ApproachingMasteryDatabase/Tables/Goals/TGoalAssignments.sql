CREATE TABLE [dbo].[TGoalAssignments]
(
	 guidGoalID				UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,lngAssignmentID		BIGINT					NOT NULL
	,strDetails				NVARCHAR(500)			NOT NULL
	,strScore				NVARCHAR(50)				
	,guidImageID			UNIQUEIDENTIFIER
	,guidAddedBy			UNIQUEIDENTIFIER		NOT NULL
	,guidCompletedBy		UNIQUEIDENTIFIER
	,guidCompletionDate		DATE					NULL
	 CONSTRAINT TGoalAssignments_PK PRIMARY KEY	(guidGoalID,lngAssignmentID)
	,CONSTRAINT TGoalAssignemnts_CK CHECK (RTRIM(LTRIM(strDetails)) !='')
	,CONSTRAINT TGoalAssignments_TGoals_FK FOREIGN KEY (guidGoalID) REFERENCES TGoals (guidGoalID)
	,CONSTRAINT TGoalAssignments_AddedBy_TUserLogins_FK FOREIGN KEY (guidAddedBy) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TGoalAssignments_CompeltedBy_TUserLogins_FK FOREIGN KEY (guidCompletedBy) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TGoalAssignments_TImages_FK FOREIGN KEY (guidImageID) REFERENCES TImages (guidImageID)
)
