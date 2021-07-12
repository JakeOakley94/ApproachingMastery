-- Table for user login information
CREATE TABLE [dbo].[TUserLogins]
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL	DEFAULT NEWID()
	,strEmailAddress		NVARCHAR(50)			NOT NULL
	,blnEmailValidated		BIT					NOT NULL	DEFAULT 0
	,shtUserRoleID			SMALLINT			NOT NULL	DEFAULT 1
	,blnActive				BIT					NOT NULL	DEFAULT 1
	,guidEmailValidationID	UNIQUEIDENTIFIER				DEFAULT NEWID()
	,strUpperEmailAddress AS UPPER(strEmailAddress)
	 CONSTRAINT TUserLogins_PK PRIMARY KEY (guidLoginID)
	,CONSTRAINT TUserLogins_TUserRoles_FK FOREIGN KEY (shtUserRoleID) REFERENCES TUserRoles (shtUserRoleID)
	,CONSTRAINT	TUserLogins_UN UNIQUE (strUpperEmailAddress)
	,CONSTRAINT TUserLogins_emalValidation_UN UNIQUE(guidEmailValidationID)

)
