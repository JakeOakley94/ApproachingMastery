CREATE TABLE [dbo].[TGoals]
(
	 guidGoalID				UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,dteDateAssigned		DATE					NOT NULL
	,dteDateCompleted		DATE					NULL
	,dteDateDue				DATE					NOT NULL
	,shtGoalType			SMALLINT				NOT NULL
	,shtGoalArea			SMALLINT				NOT NULL
	,strDescription			NVARCHAR(500)			NOT NULL
	 CONSTRAINT TGoals_PK PRIMARY KEY (guidGoalID)
	,CONSTRAINT TGoals_CK CHECK (RTRIM(LTRIM(strDescription)) != '')
	,CONSTRAINT TGoals_TGoalTypes_FK FOREIGN KEY (shtGoalType) REFERENCES TGoalTypes (shtGoalTypeID)
	,CONSTRAINT TGoals_TGoalAreas_FK FOREIGN KEY (shtGoalArea) REFERENCES TGoalAreas (shtGoalAreaID)
)
