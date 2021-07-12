CREATE TABLE [dbo].[TGoalTypes]
(
	 shtGoalTypeID			SMALLINT		NOT NULL	IDENTITY(1,1)
	,strGoalType			NVARCHAR(50)		NOT NULL
	,strUpperGoalType AS UPPER(strGoalType)
	 CONSTRAINT TGoalTypes_PK PRIMARY KEY (shtGoalTypeID)
)
