CREATE PROCEDURE [dbo].[uspUpdateIEPDueDate]
     @guidStudentID AS UNIQUEIDENTIFIER 
	,@IEPdueDate  AS   DATE
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

UPDATE TStudents

SET dteIEPDueDate = @IEPdueDate

WHERE guidStudentID = @guidStudentID

COMMIT TRANSACTION 