USE dbApproachingMastery

-- --------------------------------------------------------------------------------
-- Drop Procedures 
-- --------------------------------------------------------------------------------
IF OBJECT_ID('uspAddStudent')			    IS NOT NULL DROP PROCEDURE uspAddStudent 
IF OBJECT_ID('uspAddUser')			        IS NOT NULL DROP PROCEDURE uspAddUser 
IF OBJECT_ID('uspAddUserLogin')			    IS NOT NULL DROP PROCEDURE uspAddUserLogin 
IF OBJECT_ID('uspAddUserLoginPasswords')    IS NOT NULL DROP PROCEDURE uspAddUserLoginPasswords 
IF OBJECT_ID('uspAddMessage')               IS NOT NULL DROP PROCEDURE uspAddMessage 
IF OBJECT_ID('uspAddTeacherMessage')        IS NOT NULL DROP PROCEDURE uspAddTeacherMessage 
IF OBJECT_ID('uspAddUserLoginEvent')        IS NOT NULL DROP PROCEDURE uspAddUserLoginEvent 
IF OBJECT_ID('uspAddImage')                 IS NOT NULL DROP PROCEDURE uspAddImage 
IF OBJECT_ID('uspAddStudentTeacher')        IS NOT NULL DROP PROCEDURE uspAddStudentTeacher 
IF OBJECT_ID('uspAddStudentAccomodation')   IS NOT NULL DROP PROCEDURE uspAddStudentAccomodation 
IF OBJECT_ID('uspAddStudentTeacherComment') IS NOT NULL DROP PROCEDURE uspAddStudentTeacherComment 
IF OBJECT_ID('uspAddComment')               IS NOT NULL DROP PROCEDURE uspAddComment 


 --------------------------------------------------------------------------------


GO 
CREATE PROCEDURE uspAddStudent
	 @strFirstName  AS VARCHAR(50)
	,@strMiddleName AS VARCHAR(50)
	,@strLastName   AS VARCHAR(50)
	,@dteBirthday   AS VARCHAR(50)
	,@blnActive     AS BIT
	,@guidImageID   AS UNIQUEIDENTIFIER 
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TStudents WITH (TABLOCKX) (strFirstName,strMiddleName,strLastName,dteBirthday,blnActive,guidImageID)
VALUES (@strFirstName,@strMiddleName,@strLastName, @dteBirthday, @blnActive, @guidImageID )

COMMIT TRANSACTION 

GO

GO
CREATE PROCEDURE uspAddUserLogin
	@strEmailAddress     AS VARCHAR(50) 
   ,@blnEmailValidated   AS BIT
   ,@intUserRole         AS INTEGER 
   ,@blnActive           AS BIT 
   ,@guidEmailValidation AS UNIQUEIDENTIFIER 
   ,@guidPasswordResetID AS UNIQUEIDENTIFIER
AS 
SET NOCOUNT ON		-- Report only errors
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TUserLogins WITH (TABLOCKX) (strEmailAddress,blnEmailValidated,intUserRoleID,blnActive, guidEmailValidationID, guidPasswordResetID)
VALUES (@strEmailAddress, @blnEmailValidated, @intUserRole, @blnActive, @guidEmailValidation, @guidPasswordResetID)
 
COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddUser 
     @guidLoginID    AS UNIQUEIDENTIFIER
	,@strFirstName   AS VARCHAR(50)
	,@strMiddleName  AS VARCHAR(50) 
	,@strLastName    AS VARCHAR(50)
	,@strPhoneNumber AS VARCHAR(50)
	,@strClass       AS VARCHAR 
	,@guidImageID    AS UNIQUEIDENTIFIER 
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TUsers WITH (TABLOCKX) (guidLoginID,strFirstName,strMiddleName,strLastName,strPhoneNumber, strClass, guidImageID)
VALUES (@guidLoginID,@strFirstName,@strMiddleName,@strLastName, @strPhoneNumber, @strClass, @guidImageID )

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddUserLoginPasswords
     @guidLoginID    AS UNIQUEIDENTIFIER
    ,@intPasswordID  AS INTEGER	
	,@guidSalt       AS UNIQUEIDENTIFIER
	,@biPasswordHash AS VARBINARY(MAX)
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TUserLoginPasswords WITH (TABLOCKX) (guidLoginID, intPasswordID, guidSalt,biPasswordHash)
VALUES (@guidLoginID,@intPasswordID,@guidSalt,@biPasswordHash)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddMessage
     @guidMessageID      AS UNIQUEIDENTIFIER
    ,@strMessage         AS VARCHAR(MAX)	
	
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

DECLARE @dtmMessageDateTime AS DATETIME

SET @dtmMessageDateTime = GETUTCDATE()

BEGIN TRANSACTION 

INSERT INTO TMessages WITH (TABLOCKX) (guidMessageID, strMessage, dtmMessageDateTime)
VALUES (@guidMessageID,@strMessage, @dtmMessageDateTime)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddTeacherMessage
      @guidSenderID    AS UNIQUEIDENTIFIER
	 ,@guidRecipientID AS UNIQUEIDENTIFIER
	 ,@guidMessageID   AS UNIQUEIDENTIFIER
	 ,@guidParentID    AS UNIQUEIDENTIFIER
AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TTeacherMessages WITH (TABLOCKX) (guidSenderID, guidRecipientID, guidMessageID, guidParentID)
VALUES (@guidSenderID, @guidRecipientID, @guidMessageID, @guidParentID)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddUserLoginEvent 
	@guidLogin      AS UNIQUEIDENTIFIER
   ,@intEventTypeID AS INTEGER 


AS 
    DECLARE @dtmEventTime   AS DATETIME 
    DECLARE @IP_Address AS VARCHAR(255)

	SET @dtmEventTime = GETUTCDATE()

    SELECT @IP_Address = client_net_address
    FROM sys.dm_exec_connections
    WHERE Session_id = @@SPID;



SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TUserLoginEvents WITH (TABLOCKX) (guidLoginID, intEventTypeID, dtmEventTime, strIPAddress)
VALUES (@guidLogin, @intEventTypeID, @dtmEventTime, @IP_Address)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddImage
	@blbImage AS VARBINARY(MAX)

AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TImages WITH (TABLOCKX) (blbImage)
VALUES (@blbImage)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddStudentTeacher
	@guidStudentID     AS UNIQUEIDENTIFIER 
   ,@guidLoginID       AS UNIQUEIDENTIFIER 
   ,@blnIsGenEdTeacher AS BIT  

AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TStudentTeachers WITH (TABLOCKX) (guidStudentID, guidLoginID, blnIsGenEdTeacher)
VALUES (@guidStudentID, @guidLoginID, @blnIsGenEdTeacher)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddStudentAccomodation
	@guidStudentID       AS UNIQUEIDENTIFIER 
   ,@guidAccomodationID  AS UNIQUEIDENTIFIER 
   ,@guidLoginID         AS UNIQUEIDENTIFIER 
   ,@strValue1           AS VARCHAR(50) 
   ,@strValue2           AS VARCHAR(50) 

AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TStudentAccomodations WITH (TABLOCKX) (guidStudentID, guidAccomodationID, guidLoginID, strValue1, strValue2)
VALUES (@guidStudentID, @guidAccomodationID, @guidLoginID, @strValue1, @strValue2)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddStudentTeacherComment
	@guidStudentID       AS UNIQUEIDENTIFIER 
   ,@guidLoginID         AS UNIQUEIDENTIFIER 
   ,@guidCommentID       AS UNIQUEIDENTIFIER   
   ,@guidParentID        AS UNIQUEIDENTIFIER 

AS 

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TStudentTeacherComments WITH (TABLOCKX) (guidStudentID, guidLoginID, guidCommentID, guidParentID)
VALUES (@guidStudentID, @guidLoginID, @guidCommentID, @guidParentID)

COMMIT TRANSACTION 

GO

GO 
CREATE PROCEDURE uspAddComment
   @guidCommentID       AS UNIQUEIDENTIFIER   
  ,@strComment        AS VARCHAR(MAX)
   

AS 

DECLARE @dtmCommentDateTime AS DATETIME 

SET @dtmCommentDateTime = GETUTCDATE()

SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION 

INSERT INTO TComments WITH (TABLOCKX) (guidCommentID, strComment, dtmCommentDateTiime)
VALUES (@guidCommentID, @strComment, @dtmCommentDateTime)

COMMIT TRANSACTION 

GO











