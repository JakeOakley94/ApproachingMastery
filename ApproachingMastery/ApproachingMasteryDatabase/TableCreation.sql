-- ---------------------------------------------------------------
-- Name: ApproachingMastery.com Database Setup
-- Abstract: Creates the tables for the site database
-- Original Author: Benjamin Capuana
-- ----------------------------------------------------------------
-- When making changes please add comment where the change was made
-- ----------------------------------------------------------------

USE dbApproachingMastery

SET NOCOUNT ON

-- Drop any existing tables -- BE CAREFUL ABOUT THIS ALWAYS LEAVE COMMENTED IF YOU DON'T WANT TO DELETE THE WHOLE DATABASE

IF OBJECT_ID('TSchoolDistrictSchools')			IS NOT NULL DROP TABLE TSchoolDistrictSchools
IF OBJECT_ID('TSchoolDistrictUsers')			IS NOT NULL DROP TABLE TSchoolDistrictUsers
IF OBJECT_ID('TSchoolUsers')					IS NOT NULL DROP TABLE TSchoolUsers
IF OBJECT_ID('TSchoolStudents')					IS NOT NULL DROP TABLE TSchoolStudents
IF OBJECT_ID('TSchools')						IS NOT NULL DROP TABLE TSchoolDistricts
IF OBJECT_ID('TStudentAccomodations')			IS NOT NULL DROP TABLE TStudentAccomodations
IF OBJECT_ID('TUserMessages')					IS NOT NULL DROP TABLE TUserMessages
IF OBJECT_ID('TStudentTeacherComments')			IS NOT NULL DROP TABLE TStudentTeacherComments
IF OBJECT_ID('TStudentTeachers')				IS NOT NULL DROP TABLE TStudentTeachers
IF OBJECT_ID('TMessages')						IS NOT NULL DROP TABLE TMessages
IF OBJECT_ID('TAccomodations')					IS NOT NULL DROP TABLE TAccomodations
IF OBJECT_ID('TStudents')						IS NOT NULL DROP TABLE TStudents
IF OBJECT_ID('TUserLoginEvents')				IS NOT NULL DROP TABLE TUserLoginEvents
IF OBJECT_ID('TUserLoginEventTypes')			IS NOT NULL DROP TABLE TUserLoginEventTypes
IF OBJECT_ID('TUserLoginPasswords')				IS NOT NULL DROP TABLE TUserLoginPasswords
IF OBJECT_ID('TUsers')							IS NOT NULL DROP TABLE TUsers
IF OBJECT_ID('TUserLogins')						IS NOT NULL DROP TABLE TUserLogins
IF OBJECT_ID('TUserRoles')						IS NOT NULL DROP TABLE TUserRoles
IF OBJECT_ID('TImages')							IS NOT NULL DROP TABLE TImages
IF OBJECT_ID('GetCurrentIP')					IS NOT NULL DROP FUNCTION GetCurrentIP


CREATE TABLE TSchoolDistricts
(
	 guidSchoolDistrictID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,strSchoolDistrictName			VARCHAR(50)			NOT NULL
	,strSchoolDistrictDomainName	VARCHAR(50)			NOT NULL
	,strUpperDomainName AS UPPER(strSchoolDistrictDomainName)
	 CONSTRAINT TSchoolDistricts_PK PRIMARY KEY (guidSchoolDistrictID)
	,CONSTRAINT TSchoolDistricts_UN UNIQUE (strUpperDomainName)
)

CREATE TABLE TSchools
(
	 guidSchooDistrictID			UNIQUEIDENTIFIER	NOT NULL 
	,guidSchoolID					UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,strSchoolName					VARCHAR(50)			NOT NULL
	CONSTRAINT TSchools_PK PRIMARY KEY (guidSchoolDistrictID)
)

CREATE TABLE TSchoolDistrictUsers
(
	 guidSchoolDistrictID			UNIQUEIDENTIFIER	NOT NULL
	,guidUserID						UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT	TSchoolDistrictUsers_PK PRIMARY KEY (guidSchoolDistrictID, guidUserID)
) 

CREATE TABLE TSchoolUsers
(
	 guidSchoolID					UNIQUEIDENTIFIER	NOT NULL
	,guidUserID						UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT	TSchoolUsers_PK PRIMARY KEY (guidSchoolID, guidUserID)
)

CREATE TABLE TSchoolStudents
(
	 guidSchoolID					UNIQUEIDENTIFIER	NOT NULL
	,guidStudentID					UNIQUEIDENTIFIER	NOT NULL
	 CONSTRAINT	TSchoolStudents_PK PRIMARY KEY (guidSchoolID, guidStudentID)
)


CREATE TABLE TImages
(
	 guidImageID		UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,blbImage			VARBINARY(MAX)			NOT NULL
	CONSTRAINT TImages_PK PRIMARY KEY (guidImageID)
)

CREATE TABLE TStudents
(
	 guidStudentID		UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,strFirstName		VARCHAR(50)				NOT NULL
	,strMiddleName		VARCHAR(50)				NOT NULL
	,strLastName		VARCHAR(50)				NOT NULL
	,dteBirthday		DATE					NOT NULL
	,blnActive			BIT						NOT NULL DEFAULT 1
	,guidImageID		UNIQUEIDENTIFIER
	CONSTRAINT TStudents_PK PRIMARY KEY (guidStudentID)
)


CREATE TABLE TUsers
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,strFirstName			VARCHAR(50)			NOT NULL
	,strMiddleName			VARCHAR(50)			NOT NULL
	,strLastName			VARCHAR(50)			NOT NULL
	,strPhoneNumber			VARCHAR(50)			NOT NULL
	,strClass				VARCHAR(50)			NOT NULL
	,guidImageID			UNIQUEIDENTIFIER
	
	CONSTRAINT TUsers_PK PRIMARY KEY (guidLoginID)
)

CREATE TABLE TUserLogins
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL	DEFAULT NEWID()
	,strEmailAddress		VARCHAR(50)			NOT NULL
	,blnEmailValidated		BIT					NOT NULL	DEFAULT 0
	,intUserRoleID			INTEGER				NOT NULL	DEFAULT 2
	,blnActive				BIT					NOT NULL	DEFAULT 1
	,guidEmailValidationID	UNIQUEIDENTIFIER
	,guidPasswordResetID	UNIQUEIDENTIFIER
	 CONSTRAINT TUserLogins_PK PRIMARY KEY (guidLoginID)
)

CREATE TABLE TUserRoles
(
	 intUserRoleID				INTEGER				NOT NULL
	,strUserRole				VARCHAR(50)			NOT NULL
	,strUpperRole AS UPPER(strUserRole)
	 CONSTRAINT TUserRoles_PK PRIMARY KEY (intUserRoleID)
	,CONSTRAINT TUserRoles_UN UNIQUE (strUpperRole) -- ensures that the role is unique regardless of case
)

CREATE TABLE TUserLoginEventTypes
(
	 intEventTypeID			INTEGER				NOT NULL
	,strEventType			VARCHAR(50)			NOT NULL
	,strUpperEventType	AS UPPER(strEventType)
	 CONSTRAINT TUserLoginEventTypes_PK PRIMARY KEY (intEventTypeID)
	,CONSTRAINT TUserLoginEventTypes_UN UNIQUE (strUpperEventType)
)

CREATE TABLE TUserLoginEvents
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,intEventTypeID			INTEGER				NOT NULL
	,dtmEventTime			DATETIME			NOT NULL DEFAULT GETUTCDATE()
	,strIPAddress			VARCHAR(50)			NOT NULL
	 CONSTRAINT TUserLoginEvents_PK PRIMARY KEY (guidLoginID, intEventTypeID, dtmEventTime)
)

CREATE TABLE TUserLoginPasswords
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,intPasswordID			INTEGER				NOT NULL IDENTITY(1,1) 
	,guidSalt				UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,biPasswordHash			VARBINARY(MAX)		NOT NULL
	CONSTRAINT TTeacherPasswords_PK PRIMARY KEY (guidLoginID,intPasswordID)
)

CREATE TABLE TStudentTeachers
(
	 guidStudentID			UNIQUEIDENTIFIER	NOT NULL
	,guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,blnIsGenEdTeacher		BIT					NOT NULL
	CONSTRAINT TStudenTUsers_PK PRIMARY KEY (guidStudentID, guidLoginID)
)

CREATE TABLE TMessages
(
	 guidMessageID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,strMessage				VARCHAR(MAX)		NOT NULL
	,dtmMessageDateTime		DATETIME			NOT NULL DEFAULT GETUTCDATE()
	CONSTRAINT TMessages_PK PRIMARY KEY (guidMessageID)
)

CREATE TABLE TStudentTeacherComments
(
	 guidStudentID			UNIQUEIDENTIFIER	NOT NULL
	,guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,guidMessageID			UNIQUEIDENTIFIER	NOT NULL
	,guidParentMessageID	UNIQUEIDENTIFIER				
	 CONSTRAINT TStudentTeacherComments_PK PRIMARY KEY (guidStudentID,guidLoginID,guidCommentID)
	,CONSTRAINT TSTudentTeacherComments_Check CHECK(guidParentMessageID != guidMessageID)
)

CREATE TABLE TUserMessages
(
	 guidSenderID			UNIQUEIDENTIFIER	NOT NULL
	,guidRecipientID		UNIQUEIDENTIFIER	NOT NULL
	,guidMessageID			UNIQUEIDENTIFIER	NOT NULL
	,guidParentMessageID	UNIQUEIDENTIFIER
	 CONSTRAINT TTeacherMessages_PK PRIMARY KEY (guidSenderID, guidRecipientID, guidMessageID)
	,CONSTRAINT TTeacherMessages_CHECK CHECK(guidParentMessageID != guidMessageID)
)

CREATE TABLE TAccomodations
(
	 guidAccomodationID		UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,strAccomodation		VARCHAR(50)			NOT NULL
	CONSTRAINT TAccomodations_PK PRIMARY KEY (guidAccomodationID)
)

CREATE TABLE TStudentAccomodations
(
	 guidStudentID			UNIQUEIDENTIFIER	NOT NULL
	,guidAccomodationID		UNIQUEIDENTIFIER	NOT NULL
	,guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,strValue1				VARCHAR(50)
	,strValue2				VARCHAR(50)
	CONSTRAINT TStudentAccomodations_PK PRIMARY KEY (guidStudentID, guidAccomodationID)
)


-- -------------------------------------------------
-- Foreign Keys
-- Don't forget to add them to the list
-- -------------------------------------------------
-- #     Parent							Child						COLUMNS
-- -	 ------							----------					-------
-- 1     TStudents						TImages						guidImageID
-- 2	 TUsers							TImages						guidImageID
-- 3     TStudentTeachers     			TUsers						guidLoginID
-- 4     TStudentTeachers     			TStudents					guidStudentID
-- 5     TStudentTeacherComments		TStudenTUsers				guidStudentID,guidLoginID
-- 6	 TStudentTeacherComments		TComments					guidCommentID
-- 7     TStudentTeacherComments		TComments					guidParentID/guidCommentID
-- 8     TTeacherMessages				TUsers						guidSenderID/guidLoginID
-- 9     TTeacherMessages				TUsers						guidRecipientID/guidLoginID
-- 10    TTeacherMessages				TMessages					guidMessageID
-- 11    TTeacherMessages				TMessages					guidParentID/guidMessageID
-- 12	 TStudentAccomodations			TStudenTUsers				guidStudentID,guidLoginID
-- 13	 TStudentAccomodations			TAccomodations				guidAccomodationID
-- 14    TUsers							TUserLogins					guidUserLoginID
-- 15    TUserLoginPasswords			TUserLogins					guidUserLoginID
-- 16	 TUserLogins					TUserRoles					intRoleID
-- 17	 TUserLoginEvents				TUsers						guidLoginID
-- 18	 TUserLoginEvents				TUserLoginEventTypes		intEventTypeID


-- 1
ALTER TABLE TStudents ADD CONSTRAINT TStudents_TImages_FK 
FOREIGN KEY (guidImageID) REFERENCES TImages (guidImageID)

-- 2
ALTER TABLE TUsers ADD CONSTRAINT TUsers_TImages_FK 
FOREIGN KEY (guidImageID) REFERENCES TImages (guidImageID)

-- 3
ALTER TABLE TStudentTeachers ADD CONSTRAINT TStudenTUsers_TStudents_FK
FOREIGN KEY (guidStudentID) REFERENCES TStudents (guidStudentID)

-- 4
ALTER TABLE TStudentTeachers ADD CONSTRAINT TStudenTUsers_TUsers_FK
FOREIGN KEY (guidLoginID) REFERENCES TUsers (guidLoginID)

-- 5
ALTER TABLE TStudentTeacherComments ADD CONSTRAINT TStudentTeacherComments_TStudentTeachers_FK
FOREIGN KEY (guidStudentID,guidLoginID) REFERENCES TStudentTeachers (guidStudentID,guidLoginID)

-- 6
ALTER TABLE TStudentTeacherComments ADD CONSTRAINT TStudentTeacherComments_Comments_FK
FOREIGN KEY (guidCommentID) REFERENCES TComments (guidCommentID)

-- 7
ALTER TABLE TStudentTeacherComments ADD CONSTRAINT TStudentTeacherComments_TComments_Parent_FK
FOREIGN KEY (guidCommentID) REFERENCES TComments (guidCommentID)

-- 8
ALTER TABLE TTeacherMessages ADD CONSTRAINT TTeacherMessages_TUsers_Sender_FK
FOREIGN KEY (guidSenderID) REFERENCES TUsers (guidLoginID)

-- 9
ALTER TABLE TTeacherMessages ADD CONSTRAINT TTeacherMessages_TUsers_Recipient_FK
FOREIGN KEY (guidRecipientID) REFERENCES TUsers (guidLoginID)

-- 10
ALTER TABLE TTeacherMessages ADD CONSTRAINT TTeacherMessages_TMessages_FK
FOREIGN KEY (guidMessageID) REFERENCES TMessages (guidMessageID)

-- 11
ALTER TABLE TTeacherMessages ADD CONSTRAINT TTeacherMessages_TMessages_Parent_FK
FOREIGN KEY (guidParentID) REFERENCES TMessages (guidMessageID)

-- 12
ALTER TABLE TStudentAccomodations ADD CONSTRAINT TStudentAccomodations_TStudentTeachers_FK
FOREIGN KEY (guidStudentID, guidLoginID) REFERENCES TStudentTeachers (guidStudentID, guidLoginID)

-- 13
ALTER TABLE TStudentAccomodations ADD CONSTRAINT TStudentAccomodations_TAccomodations_FK
FOREIGN KEY (guidAccomodationID) REFERENCES TAccomodations (guidAccomodationID)

-- 14
ALTER TABLE TUsers ADD CONSTRAINT TUsers_TUserLogins_FK
FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)

-- 15
ALTER TABLE TUserLoginPasswords ADD CONSTRAINT TUserLoginsPasswords_TUserLogins_FK
FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)

-- 16
ALTER TABLE TUserLogins ADD CONSTRAINT TUserLogins_TUserRoles_FK
FOREIGN KEY (intUserRoleID) REFERENCES TUserRoles (intUserRoleID)

-- 17
ALTER TABLE TUserLoginEvents ADD CONSTRAINT TUserLoginEvents_TUserLogins_FK
FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)

-- 18
ALTER TABLE TUserLoginEvents ADD CONSTRAINT TUserLoginEvents_TUserLoginEventTypes_FK
FOREIGN KEY (intEventTypeID) REFERENCES TUserLoginEventTypes (intEventTypeID)

-- 19
ALTER TABLE TSchoolDistrictSchools ADD CONSTRAINT TSchoolDistrictSchools_TSchoolDistricts_PK
FOREIGN KEY (guidSchoolDistrictID) REFERENCES TSchoolDistricts (guidSchoolDistrictID)

-- --------------------------------------------------------
-- Add some default data
-- --------------------------------------------------------
INSERT INTO TAccomodations (strAccomodation)
VALUES	 ('Extended Time')
		,('Frequent Breaks')
		,('Small Groups')
		,('Math Tools')
		,('Read Aloud and Scribe')

INSERT INTO TUserLoginEventTypes (intEventTypeID, strEventType)
VALUES	 (1, 'SUCCESS')
		,(2, 'FAILURE')
		,(3, 'RESET')
		,(4, 'LOCKED OR DISABLED')

INSERT INTO TUserRoles (intUserRoleID, strUserRole)
VALUES   (1, 'TEACHER')
		,(2, 'ADMIN')




-- -------------------------------------------------------------------
-- FUNCTIONS
-- -------------------------------------------------------------------

GO
-- This function is from : https://stackoverflow.com/questions/9941074/how-to-get-the-client-ip-address-from-sql-server-2008-itself
CREATE FUNCTION [dbo].[GetCurrentIP] ()
RETURNS varchar(255)
AS
BEGIN
    DECLARE @IP_Address varchar(255);

    SELECT @IP_Address = client_net_address
    FROM sys.dm_exec_connections
    WHERE Session_id = @@SPID;

    Return @IP_Address;
END

GO