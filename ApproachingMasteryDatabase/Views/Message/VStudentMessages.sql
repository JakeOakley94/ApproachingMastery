CREATE VIEW [dbo].[VStudentMessages]
	AS

SELECT
	 TS.*
	,TM.*
FROM
	TStudents as TS
	INNER JOIN TStudentComments AS TSC
		INNER JOIN TMessages AS TM
		ON TM.guidMessageID = TSC.guidMessageID
	ON TSC.guidStudentID = TS.guidStudentID
