CREATE VIEW [dbo].[VUserLogins]
	AS 
SELECT
	 TUL.guidLoginID
	,TUL.strEmailAddress
	,TUL.blnEmailValidated
	,TUL.blnActive
	,TUR.shtUserRoleID
	,TUR.strUserRole
FROM
	TUserRoles AS TUR
	INNER JOIN TUserLogins AS TUL
	ON TUL.shtUserRoleID = TUR.shtUserRoleID
	
