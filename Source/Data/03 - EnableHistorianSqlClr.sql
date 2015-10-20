EXEC sp_configure 'clr enabled', 1
GO

RECONFIGURE
GO

-- Queries measurement data from the openHistorian
CREATE FUNCTION GetHistorianData(@historianServer nvarchar(256), @instanceName nvarchar(256), @startTime datetime2, @stopTime datetime2, @measurementIDs nvarchar(4000) = null)
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

-- Queries trending data from the openHistorian, example: "SELECT * FROM GetTrendingData('127.0.0.1', 'XDA', '2015-05-04 00:00:00', '2015-05-04 00:10:00', '11,21,31', default)"
CREATE FUNCTION GetTrendingData(@historianServer nvarchar(256), @instanceName nvarchar(256), @startTime datetime2, @stopTime datetime2, @channelIDs nvarchar(4000) = null, @seriesCount int = 3)
RETURNS TABLE
(
   [ChannelID] int,
   [SeriesID] int,
   [Time] datetime2,
   [Value] real
)
AS EXTERNAL NAME [openHistorian.XDALink.SqlClr].XDAFunctions.GetTrendingData;
GO
