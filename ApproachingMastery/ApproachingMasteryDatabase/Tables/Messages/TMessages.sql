CREATE TABLE [dbo].[TMessages]
(
	 guidMessageID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,guidSenderID			UNIQUEIDENTIFIER	NOT NULL
	,guidRecipientID		UNIQUEIDENTIFIER	NULL
	,guidParentMessageID	UNIQUEIDENTIFIER	NULL
	,strMessage				NVARCHAR(1000)		NOT NULL
	,dtmMessageDateTime		DATETIME			NOT NULL DEFAULT GETUTCDATE()
	 CONSTRAINT TMessages_PK PRIMARY KEY (guidMessageID)
	,CONSTRAINT TMessages_TUserLogins_Sender_FK FOREIGN KEY (guidSenderID) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TMessages_TUserLogins_Recipient_FK FOREIGN KEY (guidRecipientID) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TMessages_CK CHECK(guidParentMessageID <> guidMessageID)
	,CONSTRAINT TMesssage_TMessages_FK FOREIGN KEY (guidParentMessageID) REFERENCES TMessages (guidMessageID)
)
