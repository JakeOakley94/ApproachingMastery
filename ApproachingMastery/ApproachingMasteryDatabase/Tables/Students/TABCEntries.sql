CREATE TABLE [dbo].[TABCEntries]
(
	 guidABCID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,guidAddedBy		UNIQUEIDENTIFIER	NOT NULL
	,strAntecedent		NVARCHAR(1000)		NOT NULL
	,strBehavior		NVARCHAR(1000)		NOT NULL
	,strConsequence		NVARCHAR(1000)		NOT NULL
	,dtmIncidentDate	DATETIME			NOT NULL
	 CONSTRAINT TABCEntries_PK PRIMARY KEY (guidABCID)
	,CONSTRAINT ABCEntry_Antecedent_CK CHECK (RTRIM(LTRIM(strAntecedent))!='')
	,CONSTRAINT ABCEntry_Behavior_CK CHECK (RTRIM(LTRIM(strBehavior))!='')
	,CONSTRAINT ABCEntry_Consequence_CK CHECK (RTRIM(LTRIM(strConsequence))!='')
	,CONSTRAINT ABCEntry_TUserLogins_FK FOREIGN KEY (guidAddedBy) REFERENCES TUserLogins (guidLoginID)
)
