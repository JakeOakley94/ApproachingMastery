CREATE PROCEDURE [dbo].[uspRemoveStudent]
	@guidStudentID AS UNIQUEIDENTIFIER 
AS

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

DELETE FROM TStudents 
WHERE guidStudentID = @guidStudentID


COMMIT TRANSACTION 
	

