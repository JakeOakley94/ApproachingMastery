CREATE PROCEDURE [dbo].[uspAddTestAccounts]
AS
	-- Creates an account that will be locked

DECLARE  @userID UNIQUEIDENTIFIER
	    ,@email NVARCHAR(255) = 'locked_account@approaching-mastery.com'
		,@strPassword NVARCHAR(255) = '7B86xLAL66'
		,@intResult INTEGER
		
EXEC uspCreateNewLogin @email, @strPassword, '0.0.0.0', 'Locky', 'm', 'McLockerson', '666-666-6666'
SELECT @userID = guidLoginID
FROM
	TUserLogins
WHERE
	strEmailAddress = @EMAIL

exec uspAddLoginEvent @userID, 2, '0.0.0.0'
WAITFOR DELAY '00:00:02';
exec uspAddLoginEvent @userID, 2, '0.0.0.0'
WAITFOR DELAY '00:00:02';
exec uspAddLoginEvent @userID, 2, '0.0.0.0'
WAITFOR DELAY '00:00:02';
exec uspAddLoginEvent @userID, 2, '0.0.0.0'
WAITFOR DELAY '00:00:02';
exec uspAddLoginEvent @userID, 2, '0.0.0.0'
WAITFOR DELAY '00:00:02';
exec uspAddLoginEvent @userID, 2, '0.0.0.0'
WAITFOR DELAY '00:00:02';
exec uspAddLoginEvent @userID, 2, '0.0.0.0'

 
-- create test account 
select @email = 'test_teacher@approaching-mastery.com'
select @strPassword = 'TPvP8ELz34'

EXEC uspCreateNewLogin @email, @strPassword, '0.0.0.0', 'Testy', '', 'McTesterson', '555-555-5555'
UPDATE TUserLogins SET blnEmailValidated = 1
					   ,blnActive = 1


select @email = 'test_teacher@approaching-mastery.com'
select @strPassword = 'TPvP8ELz34'

EXEC uspCreateNewLogin @email, @strPassword, '0.0.0.0', 'Testy', '', 'McTesterson', '555-555-5555'
UPDATE TUserLogins SET blnEmailValidated = 1
					   ,blnActive = 1

select @email = 'imma_test@approaching-mastery.com'
select @strPassword = 'TPvP8ELz34'

EXEC uspCreateNewLogin @email, @strPassword, '0.0.0.0', 'imma', '', 'McTesterson', '555-555-5555'
UPDATE TUserLogins SET blnEmailValidated = 1
					   ,blnActive = 1


select @email = 'ura_test@approaching-mastery.com'
select @strPassword = 'TPvP8ELz34'

EXEC uspCreateNewLogin @email, @strPassword, '0.0.0.0', 'ura', '', 'McTesterson', '555-555-5555'
UPDATE TUserLogins SET blnEmailValidated = 1
					   ,blnActive = 1

select @email = 'hesa_test@approaching-mastery.com'
select @strPassword = 'TPvP8ELz34'

EXEC uspCreateNewLogin @email, @strPassword, '0.0.0.0', 'hesa', '', 'McTesterson', '555-555-5555'
UPDATE TUserLogins SET blnEmailValidated = 1
					   ,blnActive = 1

RETURN 0
