CREATE TABLE [dbo].[TUserLoginPasswordResetTokens]
(
	 guidLoginID			UNIQUEIDENTIFIER	NOT NULL
	,lngTokenID				BIGINT				NOT NULL
	 CONSTRAINT TUserLoginPasswordResetTokens_PK PRIMARY KEY (guidLoginID, lngTokenID)
	,CONSTRAINT TUserLoginPasswordResetTokens_TUserLogins_FK FOREIGN KEY (guidLoginID) REFERENCES TUserLogins (guidLoginID)
	,CONSTRAINT TUserLoginPasswordResetTokens_TPasswordResetTokes_FK FOREIGN KEY (lngTokenID) REFERENCES TPasswordResetTokens (lngTokenID)
)