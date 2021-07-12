-- Table for user roles
CREATE TABLE [dbo].[TUserRoles]
(
	 shtUserRoleID				SMALLINT			NOT NULL
	,strUserRole				NVARCHAR(50)			NOT NULL
	,strUpperRole AS UPPER(strUserRole)
	 CONSTRAINT TUserRoles_PK PRIMARY KEY (shtUserRoleID)
	,CONSTRAINT TUserRoles_UN UNIQUE (strUpperRole) -- ensures that the role is unique regardless of case
)
