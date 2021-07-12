CREATE PROCEDURE [dbo].[uspAddAssignment]
	@guidGoalID      AS UNIQUEIDENTIFIER 
   ,@strDetails      AS VARCHAR 
   ,@strScore        AS VARCHAR 
   ,@guidImageID     AS UNIQUEIDENTIFIER
   ,@guidAddedBy     AS UNIQUEIDENTIFIER
   ,@guidCompletedBy AS UNIQUEIDENTIFIER

AS
SET XACT_ABORT ON

DECLARE @lngAssignmentID AS BIGINT 

BEGIN TRANSACTION 

SELECT @lngAssignmentID = MAX(lngAssignmentID) + 1 
FROM TGoalAssignments (TABLOCKX)

-- If table is empty default to 1
SELECT @lngAssignmentID = COALESCE(@lngAssignmentID, 1)

INSERT INTO TGoalAssignments WITH (TABLOCKX) (guidGoalID, lngAssignmentID, strDetails, strScore, guidImageID, guidAddedBy, guidCompletedBy)
VALUES (@guidGoalID,@lngAssignmentID, @strDetails, @strScore, @guidImageID, @guidAddedBy, @guidCompletedBy)

COMMIT TRANSACTION 

GO
	

