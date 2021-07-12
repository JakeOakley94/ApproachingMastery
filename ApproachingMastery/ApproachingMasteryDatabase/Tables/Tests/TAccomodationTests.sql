CREATE TABLE [dbo].[TAccomodationTests]
(
	 guidTestID				UNIQUEIDENTIFIER		NOT NULL DEFAULT NEWID()
	,strTestGiven			NVARCHAR(50)			NOT NULL 
	,dblPercentile			FLOAT					NOT NULL 
	,dteDate				DATE					NOT NULL
	,shtTestTypeID			SMALLINT				NOT NULL, 
	 CONSTRAINT TAccomodationTests_PK PRIMARY KEY (guidTestID)
    ,CONSTRAINT TAccomodationTests_CK CHECK(RTRIM(LTRIM(strTestGiven))!='')
	,CONSTRAINT TAccomodationTests_TAccomodationTestTypes FOREIGN KEY (shtTestTypeID) REFERENCES TAccomodationTestTypes (shtTestTypeID)
)
