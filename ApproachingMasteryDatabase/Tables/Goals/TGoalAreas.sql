CREATE TABLE [dbo].[TGoalAreas]
(
	 shtGoalAreaID			SMALLINT		NOT NULL	IDENTITY(1,1)
	,strGoalArea			NVARCHAR(50)		NOT NULL
	,strUpperGoalArea AS UPPER(strGoalArea)
	 CONSTRAINT TGoalAreas_PK PRIMARY KEY (shtGoalAreaID)
)
