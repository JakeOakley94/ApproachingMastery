CREATE PROCEDURE [dbo].[uspAddAssignment]
	@guidGoalID      AS UNIQUEIDENTIFIER 
   ,@strDetails      AS VARCHAR(500)
   ,@strScore        AS VARCHAR(50)
   ,@guidImageID     AS UNIQUEIDENTIFIER
   ,@guidAddedBy     AS UNIQUEIDENTIFIER
   ,@guidCompletedBy AS UNIQUEIDENTIFIER
   ,@guidCompletionDate AS DATETIME NULL
   ,@lngAssignmentID AS BIGINT OUTPUT
AS
SET XACT_ABORT ON

BEGIN TRANSACTION 

	IF @lngAssignmentID=0
	BEGIN
		PRINT 'ASSIGNMENT ID IS NULL'
		SELECT @lngAssignmentID = MAX(lngAssignmentID) + 1 
		FROM TGoalAssignments (TABLOCKX)
		WHERE guidGoalID = @guidGoalID

		-- If table is empty default to 1
		SELECT @lngAssignmentID = COALESCE(@lngAssignmentID, 1)

		INSERT INTO TGoalAssignments WITH (TABLOCKX) (guidGoalID, lngAssignmentID, strDetails, strScore, guidImageID, guidAddedBy, guidCompletedBy, guidCompletionDate)
		VALUES (@guidGoalID,@lngAssignmentID, @strDetails, @strScore, @guidImageID, @guidAddedBy, @guidCompletedBy, @guidCompletionDate)
	END
	ELSE
	BEGIN
		UPDATE
			TGoalAssignments
		SET
			 strDetails = @strDetails
			,strScore = @strScore
			,guidImageID = @guidImageID
			,guidAddedby = @guidAddedBy
			,guidCompletedBy = @guidCompletedBy
			,guidCompletionDate = @guidCompletionDate
		WHERE
			guidGoalID = @guidGoalID
		AND lngAssignmentID = @lngAssignmentID
	END

COMMIT TRANSACTION 

GO
	

