
-- Script for testing school district functions

-- Declare Variables
DECLARE  @schoolDistrictID  UNIQUEIDENTIFIER
		,@guidSchoolID		UNIQUEIDENTIFIER
	    ,@intResult			INTEGER


		DECLARE @printStatement NVARCHAR(MAX)

-- Test adding a new district
EXECUTE @intResult = uspAddSchoolDistrict 'Test District 1', 'approaching-mastery.com', @schoolDistrictID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL')
PRINT @printStatement

-- test adding a duplicate district
EXECUTE @intResult = uspAddSchoolDistrict 'Test District 1', 'approachinG-mastery.com', @schoolDistrictID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL')
PRINT @printStatement

-- test adding a duplicate district with different capitalization in the domain name
EXECUTE @intResult = uspAddSchoolDistrict 'Test District 1', 'approachinG-masterY.com', @schoolDistrictID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL')
PRINT @printStatement

-- test adding a district with a different name but the same domain
EXECUTE @intResult = uspAddSchoolDistrict 'Test District 3', 'approachinG-mastery.com', @schoolDistrictID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL')
PRINT @printStatement

-- add 2 new schools
EXECUTE @intResult = uspAddSchool @schoolDistrictID, 'Test School 1', @guidSchoolID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL') + 'SchoolID:' + COALESCE(CONVERT(NVARCHAR(50),@guidSchoolID),'NULL')
PRINT @printStatement


EXECUTE @intResult = uspAddSchool @schoolDistrictID, 'Test School 2', @guidSchoolID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL') + 'SchoolID:' + COALESCE(CONVERT(NVARCHAR(50),@guidSchoolID),'NULL')
PRINT @printStatement

-- add duplicate name
EXECUTE @intResult = uspAddSchool @schoolDistrictID, 'Test School 2', @guidSchoolID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL') + 'SchoolID:' + COALESCE(CONVERT(NVARCHAR(50),@guidSchoolID),'NULL')
PRINT @printStatement

-- add duplicate with different casing
EXECUTE @intResult = uspAddSchool @schoolDistrictID, 'Test sCHOOL 2', @guidSchoolID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL') + 'SchoolID:' + COALESCE(CONVERT(NVARCHAR(50),@guidSchoolID),'NULL')
PRINT @printStatement



-- test adding a second district
EXECUTE @intResult = uspAddSchoolDistrict 'Test DisIrict 2', 'cincinnatistate.edu', @schoolDistrictID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL')
PRINT @printStatement

EXECUTE @intResult = uspAddSchool @schoolDistrictID, 'Test School 3', @guidSchoolID OUTPUT
SELECT @printStatement = 'Result: ' + CONVERT(NVARCHAR(10),@intResult) + ', SchoolDistrictID: ' + COALESCE(CONVERT(NVARCHAR(50),@schoolDistrictID),'NULL') + 'SchoolID:' + COALESCE(CONVERT(NVARCHAR(50),@guidSchoolID),'NULL')
PRINT @printStatement

SELECT * FROM TSchoolDistricts
SELECT * FROM TSchools
-- test view
SELECT * FROM VSchoolDistrictSchools



