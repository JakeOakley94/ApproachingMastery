CREATE TABLE [dbo].[TSchools]
(
	 guidSchoolID					UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,strSchoolName					NVARCHAR(255)			NOT NULL
	CONSTRAINT TSchools_PK PRIMARY KEY (guidSchoolID)
)
