-- Table for the user login event types
CREATE TABLE [dbo].[TUserLoginEventTypes]
(
	 shtEventTypeID			SMALLINT			NOT NULL
	,strEventType			NVARCHAR(50)			NOT NULL
	,strUpperEventType	AS UPPER(strEventType)
	 CONSTRAINT TUserLoginEventTypes_PK PRIMARY KEY (shtEventTypeID)
	,CONSTRAINT TUserLoginEventTypes_UN UNIQUE (strUpperEventType)
)

