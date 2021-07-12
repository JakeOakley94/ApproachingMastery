CREATE VIEW [dbo].[VUserLoginPasswordResetTokens]
	AS 
SELECT
	 TUL.guidLoginID
	,TPRT.*
FROM
	TUserLogins as TUL
	INNER JOIN TUserLoginPasswordResetTokens AS TULPRT
		INNER JOIN TPasswordResetTokens as TPRT
		ON TPRT.lngTokenID = TULPRT.lngTokenID
	ON TULPRT.guidLoginID = TUL.guidLoginID

