---------------- PQDigest TableSpace -------------
CREATE TABLE [PQDigest.Setting](
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.Url', 'http://localhost:8989', '')
GO
INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.APIKey', '', '')
GO
INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.APIToken', '', '')
GO

CREATE TABLE [PQDigest.EventViewWidget] (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL DEFAULT(1),
    Setting varchar(max) NOT NULL Default('{}'),
    Type varchar(max) NOT NULL Default('OpenSEE')
)
GO

INSERT [PQDigest.EventViewWidget] (Name, Type) VALUES ('OpenSEE','OpenSEE')
GO

INSERT [PQDigest.EventViewWidget] (Name, Type) VALUES ('TrendGraph','TrendGraph')
GO

INSERT [PQDigest.EventViewWidget] (Name, Type) VALUES ('PQICurves','PQICurves')
GO

INSERT [PQDigest.EventViewWidget] (Name, Type) VALUES ('pqi','pqi')
GO


CREATE TABLE [PQDigest.HomeScreenWidget] (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL DEFAULT(1),
    Setting varchar(max) NOT NULL Default('{}'),
    Type varchar(max) NOT NULL Default('OpenSEE'),
    TimeFrame INT NOT NULL Default(30)
)

INSERT [PQDigest.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('EPRI PQ Health Index - Last 30 Days','PQHealthIndex', 30)
GO

INSERT [PQDigest.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('Historical Event Counts - Last 30 Days','EventCountChart', 30)
GO

INSERT [PQDigest.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('Magnitude Duration Chart - Last 30 Days','MagDurChart', 30)
GO

INSERT [PQDigest.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('Meter Activity - Last 30 Days','EventCountTable', 30)
GO

CREATE VIEW [PQDigest.ChannelView] AS
    SELECT DISTINCT 
        Channel.ID,
	    Channel.Name,
	    Channel.Description,
        Channel.Trend,
        Asset.ID as AssetID,
	    Asset.AssetKey,
	    Asset.AssetName,
        Meter.ID as MeterID,
	    Meter.AssetKey AS MeterKey,
	    Meter.Name AS MeterName,
        Meter.ShortName AS MeterShortName,
	    Phase.Name AS Phase,
        MeasurementType.Name AS MeasurementType,
        MeasurementCharacteristic.Name AS MeasurementCharacteristic,
	    ChannelGroup.Name AS ChannelGroup,
	    ChannelGroupType.DisplayName AS ChannelGroupType,
	    ChannelGroupType.Unit
    FROM 
	    Channel LEFT JOIN
	    Phase ON Channel.PhaseID = Phase.ID LEFT JOIN
	    Asset ON Asset.ID = Channel.AssetID LEFT JOIN
	    Meter ON Meter.ID = Channel.MeterID LEFT JOIN
        MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID LEFT JOIN
        MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID LEFT JOIN
	    ChannelGroupType ON Channel.MeasurementCharacteristicID = ChannelGroupType.MeasurementCharacteristicID AND Channel.MeasurementTypeID = ChannelGroupType.MeasurementTypeID LEFT JOIN
	    ChannelGroup ON ChannelGroup.ID = ChannelGroupType.ChannelGroupID
GO
