CREATE TABLE [dbo].[TUserMessages]
(
	 guidSenderID			UNIQUEIDENTIFIER	NOT NULL
	,guidRecipientID		UNIQUEIDENTIFIER	NOT NULL
	,guidMessageID			UNIQUEIDENTIFIER	NOT NULL
	,guidParentMessageID	UNIQUEIDENTIFIER
	 CONSTRAINT TUserMessages_PK					PRIMARY KEY (guidSenderID, guidRecipientID, guidMessageID)
	,CONSTRAINT TUserMessages_CHECK					CHECK(guidParentMessageID != guidMessageID)
	,CONSTRAINT TUserMessages_TMessages_FK			FOREIGN KEY (guidMessageID) REFERENCES TMessages (guidMessageID)
	,CONSTRAINT TUserMessages_TMessages_Parent_FK	FOREIGN KEY	(guidParentMessageID) REFERENCES TMessages (guidMessageID)
)
