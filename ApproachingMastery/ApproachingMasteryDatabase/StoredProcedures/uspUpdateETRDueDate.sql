CREATE PROCEDURE [dbo].[uspUpdateETRDueDate]

     @guidStudentID AS UNIQUEIDENTIFIER 
	,@ETRdueDate  AS   DATE
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

UPDATE TStudents

SET dteETRDueDate = @ETRdueDate

WHERE guidStudentID = @guidStudentID

COMMIT TRANSACTION 