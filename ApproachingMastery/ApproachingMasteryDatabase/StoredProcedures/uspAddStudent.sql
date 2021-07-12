CREATE PROCEDURE [dbo].[uspAddStudent]

     @strFirstName  AS VARCHAR(50)
	,@strMiddleName AS VARCHAR(50)
	,@strLastName   AS VARCHAR(50)
	,@dteBirthday   AS VARCHAR(50)
	,@blnActive     AS BIT
	,@IEPDueDate    AS DATE
	,@ETRDueDate    AS DATE
	,@guidImageID   AS UNIQUEIDENTIFIER 
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TStudents WITH (TABLOCKX) (strFirstName,strMiddleName,strLastName,dteBirthday,blnActive,dteIEPDueDate, dteETRDueDate, guidImageID)
VALUES (@strFirstName,@strMiddleName,@strLastName, @dteBirthday, @blnActive, @IEPDueDate, @ETRDueDate, @guidImageID )

COMMIT TRANSACTION 

