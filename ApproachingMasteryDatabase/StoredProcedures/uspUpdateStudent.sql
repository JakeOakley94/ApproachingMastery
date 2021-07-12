CREATE PROCEDURE [dbo].[uspUpdateStudent]
     @guidStudentID AS UNIQUEIDENTIFIER 
    ,@strFirstName  AS VARCHAR(50)
	,@strMiddleName AS VARCHAR(50)
	,@strLastName   AS VARCHAR(50)
	,@dteBirthday   AS VARCHAR(50)
	,@blnActive     AS BIT
	,@guidImageID   AS UNIQUEIDENTIFIER 
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

UPDATE TStudents

SET strFirstName = @strFirstName
    ,strLastName = @strLastName 
	,strMiddleName = @strMiddleName
	,dteBirthday = @dteBirthday
	,blnActive = @blnActive
	,guidImageID = @guidImageID

WHERE guidStudentID = @guidStudentID

COMMIT TRANSACTION 
