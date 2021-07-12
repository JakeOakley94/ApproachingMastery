CREATE VIEW [dbo].[VTeacherStudents]
	AS
SELECT
	 Tu.guidLoginID
	,TS.*
FROM
	TUserLogins AS TU
	INNER JOIN TTeacherStudents AS TTS
		 INNER JOIN TStudents AS TS
		ON TTS.guidStudentID = TS.guidStudentID
	ON TTS.guidLoginID = TU.guidLoginID

