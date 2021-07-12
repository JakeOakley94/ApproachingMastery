CREATE TABLE [dbo].[TAirTests]
(
	 guidAirTestID			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID()
	,dblMathGrade			FLOAT				NOT NULL
	,dblReadingGrade		FLOAT				NOT NULL
	,shtYear				SMALLINT			NOT NULL
	 CONSTRAINT TAirTests_PK PRIMARY KEY (guidAirTestID)
)
