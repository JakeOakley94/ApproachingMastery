DECLARE @sql AS NVARCHAR(MAX) = ''

	SELECT
		@sql = @sql + '
		GRANT EXECUTE ON OBJECT::[' + ROUTINE_SCHEMA + '].[' + ROUTINE_NAME + '] TO WebApp;' 
	FROM
		dbApproachingMastery.INFORMATION_SCHEMA.ROUTINES
	WHERE 
		ROUTINE_TYPE = 'PROCEDURE'
	AND SPECIFIC_NAME LIKE 'usp%'

	print @sql
	exec (@sql)