-- TODO: Run openHistorian EnableHistorianSqlClr.sql first
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'GetTrendingData')
   DROP FUNCTION [GetTrendingData];
GO

IF EXISTS (SELECT name FROM sys.assemblies WHERE name = 'openHistorianXDALink')
   DROP ASSEMBLY [openHistorianXDALink];
GO

-- TODO: Set proper paths for installed assemblies
CREATE ASSEMBLY [openHistorianXDALink] AUTHORIZATION dbo FROM 'C:\Program Files\openXDA\openHistorianXDALink.dll'
WITH PERMISSION_SET = UNSAFE
GO

-- Queries trending data from the openHistorian
CREATE FUNCTION GetTrendingData(@historianServer nvarchar(256), @instanceName nvarchar(256), @startTime datetime2, @stopTime datetime2, @channelIDs nvarchar(4000) = null, @seriesCount int = 3)
RETURNS TABLE
(
   [ChannelID] int,
   [SeriesID] int,
   [Time] datetime2,
   [Value] real
)
AS EXTERNAL NAME [openHistorianXDALink].SqlFunctions.GetTrendingData;
GO
