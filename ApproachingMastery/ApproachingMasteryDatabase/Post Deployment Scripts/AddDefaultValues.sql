

DECLARE @rebuildDefaults AS INTEGER = 0
DECLARE @rebuildTestAccounts AS INTEGER = 1
DECLARE @rebuildStudentTestData AS INTEGER = 0
IF @rebuildDefaults = 1 EXEC uspInsertDefaults
IF @rebuildTestAccounts = 1 EXEC uspAddTestAccounts
IF @rebuildStudentTestData = 1
BEGIN
	EXEC uspAddSampleStudents
END
