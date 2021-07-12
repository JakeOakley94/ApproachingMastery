CREATE PROCEDURE [dbo].[uspAddSampleStudents]
AS
INSERT INTO TStudents(guidStudentID, strFirstName, strMiddleName, strLastName, dteBirthday, blnActive)
VALUES   ('80600BDB-59E0-4C69-B1D2-082C355A80FD','Christin','Dove','Causer','11/24/2014',0)
		,('0959F931-2237-4C70-A574-7B6C31F4A6E0','Diana','Parminder','Blanchet','03/07/2011',1)
		,('2E69BFC0-2991-4EC3-A479-8E4AED394F1A','Simona','Cainneach','Hawking','12/04/2012',1)
		,('3D90EE41-C35E-421A-9314-D65D20E2D769','Gweneth','Aurel','Arthursson','11/04/2011',0)
		,('80C35FA5-11D3-4532-9496-F29B1A6AE96E','Epiktetos','Theotman','Bardsley','01/05/2013',0)
RETURN 0
