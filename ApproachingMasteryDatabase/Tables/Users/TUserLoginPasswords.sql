CREATE TABLE [dbo].[TUserLoginPasswords]
(
     guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,lngPasswordID			BIGINT				NOT NULL IDENTITY(1,1) 
	,guidSalt				UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,biPasswordHash			VARBINARY(MAX)		NOT NULL
	CONSTRAINT TUserLoginPasswords_PK PRIMARY KEY (guidLoginID,lngPasswordID)
	CONSTRAINT TUserLoginPasswords_TUserLogins FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
)
