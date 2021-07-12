-- Table for users
CREATE TABLE [dbo].[TUsers]
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,strFirstName			NVARCHAR(50)			NOT NULL
	,strMiddleName			NVARCHAR(50)			NOT NULL
	,strLastName			NVARCHAR(50)			NOT NULL
	,strPhoneNumber			NVARCHAR(50)			NOT NULL
	,guidImageID			UNIQUEIDENTIFIER
	,strClass				NVARCHAR(50)			NULL
	CONSTRAINT TUsers_PK PRIMARY KEY (guidLoginID)
	CONSTRAINT TUsers_TUserLogins_FK FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
	CONSTRAINT TUsers_TImages_FK FOREIGN KEY (guidImageID) REFERENCES TImages (guidImageID), 
    
)
