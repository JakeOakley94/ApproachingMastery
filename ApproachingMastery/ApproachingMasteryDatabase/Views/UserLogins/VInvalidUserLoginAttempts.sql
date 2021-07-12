-- ----------------------------------------------------------------------------
-- Name: VInvalidUserLoginAttempts
-- Abstract: Gets all of the invalid login attempts per user
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VInvalidUserLoginAttempts
AS
	SELECT
		 TULE.guidLoginID
		,TULE.dtmEventTime
		,TULE.strIPAddress
		,TULET.*
	FROM
		TUserLoginEvents AS TULE
		INNER JOIN TUserLoginEventTypes AS TULET
		ON TULET.shtEventTypeID = TULE.shtEventTypeID
	WHERE
		TULET.shtEventTypeID = 2
GO

