-- ----------------------------------------------------------------------------
-- Name: VUserLoginPasswordResets
-- Abstract: Gets all of the password resets per user
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VUserLoginPasswordResets
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
		TULET.shtEventTypeID = 3
GO

