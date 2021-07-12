CREATE PROCEDURE [dbo].[uspAddStudentImage]

     @guidStudentID AS UNIQUEIDENTIFIER 
	,@guidImageID   AS UNIQUEIDENTIFIER 
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

UPDATE TStudents

SET guidImageID = @guidImageID

WHERE guidStudentID = @guidStudentID

COMMIT TRANSACTION 