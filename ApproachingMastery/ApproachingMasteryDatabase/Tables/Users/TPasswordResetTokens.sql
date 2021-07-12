CREATE TABLE [dbo].[TPasswordResetTokens]
(
	 lngTokenID				BIGINT				NOT NULL IDENTITY(1,1)
	,biTokenHash			VARBINARY(MAX)		NOT NULL
	,dtmExpiration			DATETIME			NOT NULL
	 CONSTRAINT TPasswordResetTokens_PK PRIMARY KEY (lngTokenID)
)
