-- ----------------------------------------------------------------------------
-- Name: VUserLoginSuccess
-- Abstract: Gets all of the successfull logins per user
-- ----------------------------------------------------------------------------
GO
CREATE VIEW VUserLoginSuccess
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
		TULET.shtEventTypeID = 1
GO