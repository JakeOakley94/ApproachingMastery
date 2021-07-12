CREATE TABLE [dbo].[TAccomodationDetailTypes]
(
	 shtDetailTypeID			SMALLINT			NOT NULL	IDENTITY (1,1)
	,strDetailType				NVARCHAR(50)			NOT NULL
	,strUpperDetailType	AS UPPER(strDetailType)
	 CONSTRAINT TAccomodationDetailTypes_PK PRIMARY KEY (shtDetailTypeID)
	,CONSTRAINT TAccomodationDetailTypes_UN UNIQUE (strUpperDetailType)
)
