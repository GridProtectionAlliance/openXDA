-- TODO: Run openHistorian EnableHistorianSqlClr.sql first
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'GetTrendingData')
   DROP FUNCTION [GetTrendingData];
GO

IF EXISTS (SELECT name FROM sys.assemblies WHERE name = 'openHistorian.XDALink.SqlClr')
   DROP ASSEMBLY [openHistorian.XDALink.SqlClr];
GO

-- TODO: Set proper paths for installed assemblies
CREATE ASSEMBLY [openHistorian.XDALink.SqlClr] AUTHORIZATION dbo FROM 'C:\Program Files\openXDA\openHistorian.XDALink.SqlClr.dll'
WITH PERMISSION_SET = UNSAFE
GO

-- Queries trending data from the openHistorian, example: "SELECT * FROM GetTrendingData('127.0.0.1', 'XDA', '2015-05-04 00:00:00', '2015-05-04 00:10:00', '11,21,31', default)"
CREATE FUNCTION GetTrendingData(@historianServer nvarchar(256), @instanceName nvarchar(256), @startTime datetime2, @stopTime datetime2, @channelIDs nvarchar(MAX), @seriesCount int = 3)
RETURNS TABLE
(
   [ChannelID] int,
   [SeriesID] int,
   [Time] datetime2,
   [Value] real
)
AS EXTERNAL NAME [openHistorian.XDALink.SqlClr].XDAFunctions.GetTrendingData;
GO