CREATE TABLE [dbo].[TTestNames]
(
	 shtTestNameID		SMALLINT				NOT NULL
	,strTestName		NVARCHAR(50)			NOT NULL
	,strUpperTestName AS UPPER(strTestName)
	 CONSTRAINT TTestNames_PK PRIMARY KEY (shtTestNameID)
	,CONSTRAINT TTestNames_UN UNIQUE (strTestName)
	,CONSTRAINT TTestNames_CK CHECK (RTRIM(LTRIM(strTestName))!='')
)
