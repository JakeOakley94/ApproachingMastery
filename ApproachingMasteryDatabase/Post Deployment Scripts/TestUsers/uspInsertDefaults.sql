CREATE PROCEDURE [dbo].[uspInsertDefaults]
AS
		DELETE FROM TAccomodations
		DELETE FROM TUserLoginEventTypes
		DELETE FROM	TUserRoles

	INSERT INTO TAccomodations (strAccomodation)
	VALUES	 ('Extended Time')
			,('Frequent Breaks')
			,('Small Groups')
			,('Math Tools')
			,('Read Aloud')
			,('Scribe')


	INSERT INTO TUserLoginEventTypes (shtEventTypeID, strEventType)
	VALUES	 (1, 'SUCCESS')
			,(2, 'FAILURE')
			,(3, 'RESET')
			,(4, 'LOCKED OR DISABLED')

	INSERT INTO TUserRoles (shtUserRoleID, strUserRole)
	VALUES   (1, 'TEACHER')
			,(2, 'ADMIN')

	INSERT INTO TGoalTypes (strGoalType)
	VALUES   ('None/Generic')
			,('Counter')
			,('Assignments Completed')

	INSERT INTO TGoalAreas (strGoalArea)
	VALUES   ('Behavioral')
			,('Accademic')

RETURN 0
