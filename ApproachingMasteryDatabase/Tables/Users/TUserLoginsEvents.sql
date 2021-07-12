-- Table for Login Events
CREATE TABLE [dbo].[TUserLoginEvents]
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,shtEventTypeID			SMALLINT			NOT NULL
	,dtmEventTime			DATETIME			NOT NULL DEFAULT GETUTCDATE()
	,strIPAddress			NVARCHAR(50)			NOT NULL
	 CONSTRAINT TUserLoginEvents_PK PRIMARY KEY (guidLoginID, shtEventTypeID, dtmEventTime)
	 CONSTRAINT TUserLoginEvents_TUserLogins_FK FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
	 CONSTRAINT TUserLoginEvents_TUserLoginEventTypes_FK FOREIGN KEY (shtEventTypeID) REFERENCES TUserLoginEventTypes (shtEventTypeID)
)
