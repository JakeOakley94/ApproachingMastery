-- ----------------------------------------------------------------------------
-- Name: VUserInformation
-- Abstract: Gets all of the user information
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VAllUserInformation
AS
	SELECT
		 TUL.guidLoginID
		,TUR.shtUserRoleID
		,TUR.strUserRole
		,TUL.strEmailAddress
		,TUL.blnEmailValidated
		,TUL.guidEmailValidationID
		,TUL.blnActive
		,TU.strFirstName
		,TU.strMiddleName
		,TU.strLastName
		,TU.strPhoneNumber
		,TU.strClass
		,TI.guidImageID
		,TI.blbImage
	FROM
		TUserRoles AS TUR
		INNER JOIN TUserLogins AS TUL
			INNER JOIN TUsers AS TU
				LEFT OUTER JOIN TImages AS TI
				ON TI.guidImageID = TU.guidImageID
			ON TU.guidLoginID = TUL.guidLoginID
		ON TUL.shtUserRoleID = TUR.shtUserRoleID
GO