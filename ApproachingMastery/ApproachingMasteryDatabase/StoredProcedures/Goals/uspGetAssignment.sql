GO
CREATE PROCEDURE [dbo].[uspGetAssignment]
	@guidGoalID AS UNIQUEIDENTIFIER
	
AS

SELECT * FROM TGoalAssignments(TABLOCKX) WHERE guidGoalID = @guidGoalID

GO