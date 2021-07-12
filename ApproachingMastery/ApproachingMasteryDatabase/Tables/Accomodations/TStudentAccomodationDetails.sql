CREATE TABLE [dbo].[TStudentAccomodationDetails]
(
	 guidStudentID			UNIQUEIDENTIFIER		NOT NULL
	,shtAccomodationID		SMALLINT				NOT NULL
	,shtDetailTypeID		SMALLINT				NOT NULL
	,strDetailValue			NVARCHAR(500)			NOT NULL
	 CONSTRAINT TAccomodationDetails_PK PRIMARY KEY (guidStudentID, shtAccomodationID, shtDetailTypeID)
	,CONSTRAINT TStudentAccomodationdetails_TStudentAccomdations_FK 
		FOREIGN KEY (guidStudentID, shtAccomodationID) REFERENCES TStudentAccomodations (guidStudentID, shtAccomodationID)
	,CONSTRAINT TStudentAccomodationdetails_TAccomodationDetailTypes_FK FOREIGN KEY (shtDetailTypeID)
		REFERENCES TAccomodationDetailTypes (shtDetailTypeID)
	
)
