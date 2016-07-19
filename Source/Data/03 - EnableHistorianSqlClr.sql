EXEC sp_configure 'clr enabled', 1
GO

RECONFIGURE
GO

-- Queries measurement data from the openHistorian
CREATE FUNCTION GetHistorianData(@historianServer nvarchar(256), @instanceName nvarchar(256), @startTime datetime2, @stopTime datetime2, @measurementIDs nvarchar(MAX))
RETURNS TABLE
(
   [ID] bigint,
   [Time] datetime2,
   [Value] real
)
AS EXTERNAL NAME [openHistorian.SqlClr].HistorianFunctions.GetHistorianData;
GO

-- Returns the unsigned high-double-word (int) from a quad-word (bigint)
CREATE FUNCTION HighDoubleWord(@quadWord bigint)
RETURNS int
AS EXTERNAL NAME [openHistorian.SqlClr].HistorianFunctions.HighDoubleWord;
GO

-- Returns the low-double-word (int) from a quad-word (bigint)
CREATE FUNCTION LowDoubleWord(@quadWord bigint)
RETURNS int
AS EXTERNAL NAME [openHistorian.SqlClr].HistorianFunctions.LowDoubleWord;
GO

-- Makes a quad-word (bigint) from two double-words (int)
CREATE FUNCTION MakeQuadWord(@highWord int, @lowWord int)
RETURNS bigint
AS EXTERNAL NAME [openHistorian.SqlClr].HistorianFunctions.MakeQuadWord;
GO

-- Queries trending data from the openHistorian, example: "SELECT * FROM ExternalGetTrendingData('127.0.0.1', 'XDA', '2015-05-04 00:00:00', '2015-05-04 00:10:00', '11,21,31', default)"
CREATE FUNCTION ExternalGetTrendingData(@historianServer nvarchar(256), @instanceName nvarchar(256), @startTime datetime2, @stopTime datetime2, @channelIDs nvarchar(MAX), @seriesCount int = 3)
RETURNS TABLE
(
   [ChannelID] int,
   [SeriesID] int,
   [Time] datetime2,
   [Value] real
)
AS EXTERNAL NAME [openHistorian.XDALink.SqlClr].XDAFunctions.GetTrendingData;
GO

-- Queries trending data from the openHistorian, example: "SELECT * FROM GetTrendingData('2015-05-04 00:00:00', '2015-05-04 00:10:00', '11,21,31', default)"
CREATE FUNCTION GetTrendingData(@startTime datetime2, @stopTime datetime2, @channelIDs nvarchar(MAX) = null, @seriesCount int = 3)
RETURNS @trendingData TABLE
(
   [ChannelID] int,
   [SeriesID] int,
   [Time] datetime2,
   [Value] real
)
AS
BEGIN
	DECLARE @server VARCHAR(MAX)
	DECLARE @instanceName VARCHAR(MAX)

	SELECT
		@server = COALESCE(Server.Value, '127.0.0.1'),
		@instanceName = COALESCE(InstanceName.Value, 'XDA')
	FROM
		(SELECT NULL) AS Dummy(dummy) LEFT OUTER JOIN
		Setting Server ON Server.Name = 'Historian.Server' LEFT OUTER JOIN
		Setting InstanceName ON InstanceName.Name = 'Historian.InstanceName'

	INSERT INTO @trendingData
	SELECT *
	FROM ExternalGetTrendingData(@server, @instanceName, @startTime, @stopTime, @channelIDs, @seriesCount)

	RETURN
END
GO

-- Queries trending data from the openHistorian, example: "SELECT * FROM GetTypedTrendingData('2015-05-04 00:00:00', '2015-05-04 00:10:00', '11,21,31', default)"
CREATE FUNCTION GetTypedTrendingData(@startTime datetime2, @stopTime datetime2, @channelIDs nvarchar(MAX) = null, @seriesCount int = 3)
RETURNS @trendingData TABLE
(
   [ChannelID] int,
   [SeriesType] varchar(50),
   [Time] datetime2,
   [Value] real
)
AS
BEGIN
	DECLARE @server VARCHAR(MAX)
	DECLARE @instanceName VARCHAR(MAX)

	SELECT
		@server = COALESCE(Server.Value, '127.0.0.1'),
		@instanceName = COALESCE(InstanceName.Value, 'XDA')
	FROM
		(SELECT NULL) AS Dummy(dummy) LEFT OUTER JOIN
		Setting Server ON Server.Name = 'Historian.Server' LEFT OUTER JOIN
		Setting InstanceName ON InstanceName.Name = 'Historian.InstanceName'

	INSERT INTO @trendingData
	SELECT
        ChannelID,
        CASE SeriesID
            WHEN 0 THEN 'Minimum'
            WHEN 1 THEN 'Maximum'
            WHEN 2 THEN 'Average'
            ELSE 'Unknown'
        END AS SeriesType,
        Time,
        Value
	FROM ExternalGetTrendingData(@server, @instanceName, @startTime, @stopTime, @channelIDs, @seriesCount)

	RETURN
END
GO
