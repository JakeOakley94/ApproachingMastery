CREATE TABLE [dbo].[TAccomodations]
(
	 shtAccomodationID		SMALLINT			NOT NULL IDENTITY(1,1)
	,strAccomodation		NVARCHAR(50)			NOT NULL
	,strUpperAccomodation AS UPPER(strAccomodation)
	 CONSTRAINT TAccomodations_PK PRIMARY KEY (shtAccomodationID)
	,CONSTRAINT TAccomodations_UN UNIQUE (strUpperAccomodation)
)
