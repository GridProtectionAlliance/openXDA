
-- The following commented statements are used to create a database
-- from scratch and create a new user with access to the database.
--
--  * To change the database name, replace all [openXDA] with the desired database name.

--USE [master]
--GO
--CREATE DATABASE [openXDA]
--GO
--USE [openXDA]
--GO

----- TABLES -----

CREATE TABLE AccessLog(
    ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserName varchar(200) NOT NULL,
    AccessGranted bit NOT NULL,
    CreatedOn datetime NOT NULL CONSTRAINT [DF_AccessLog_Timestamp]  DEFAULT (getutcdate())
)
GO


CREATE TABLE Setting
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NULL,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

CREATE TABLE ConfigurationLoader
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE DataReader
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FilePattern VARCHAR(500) NOT NULL,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE DataOperation
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE WebControllerExtension
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO


CREATE TABLE DataWriter
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE FileGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    DataStartTime DATETIME2 NOT NULL,
    DataEndTime DATETIME2 NOT NULL,
    ProcessingStartTime DATETIME2 NOT NULL,
    ProcessingEndTime DATETIME2 NOT NULL,
    ProcessingVersion INT NOT NULL DEFAULT 0,
    Error INT NOT NULL DEFAULT 0
)
GO

CREATE NONCLUSTERED INDEX IX_FileGroup_DataStartTime
ON FileGroup(DataStartTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_FileGroup_DataEndTime
ON FileGroup(DataEndTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_FileGroup_ProcessingStartTime
ON FileGroup(ProcessingStartTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_FileGroup_ProcessingEndTime
ON FileGroup(ProcessingEndTime ASC)
GO

CREATE TABLE DataFile
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID),
    FilePath VARCHAR(MAX) NOT NULL,
    FilePathHash INT NOT NULL,
    FileSize BIGINT NOT NULL,
    CreationTime DATETIME NOT NULL,
    LastWriteTime DATETIME NOT NULL,
    LastAccessTime DATETIME NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_DataFile_FileGroupID
ON DataFile(FileGroupID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DataFile_FilePathHash
ON DataFile(FilePathHash ASC)
GO

CREATE TABLE FileGroupField
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE FileGroupFieldValue
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID),
    FileGroupFieldID INT NOT NULL REFERENCES FileGroupField(ID),
    Value VARCHAR(MAX) NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FileGroupFieldValue_FileGroupID
ON FileGroupFieldValue(FileGroupID ASC)
GO

CREATE NONCLUSTERED INDEX IX_FileGroupFieldValue_FileGroupFieldID
ON FileGroupFieldValue(FileGroupFieldID ASC)
GO

CREATE TABLE Location
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LocationKey VARCHAR(50) NOT NULL UNIQUE,
    Name VARCHAR(200) NOT NULL,
    Alias VARCHAR(200) NULL,
    ShortName VARCHAR(50) NULL,
    Latitude FLOAT NOT NULL DEFAULT 0.0,
    Longitude FLOAT NOT NULL DEFAULT 0.0,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE Meter
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetKey VARCHAR(50) NOT NULL UNIQUE,
    LocationID INT NOT NULL REFERENCES Location(ID),
    Name VARCHAR(200) NOT NULL,
    Alias VARCHAR(200) NULL,
    ShortName VARCHAR(50) NULL,
    Make VARCHAR(200) NOT NULL,
    Model VARCHAR(200) NOT NULL,
    TimeZone VARCHAR(200) NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE HostRegistration
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    RegistrationKey VARCHAR(50) NOT NULL UNIQUE,
    APIToken VARCHAR(50) NOT NULL,
    URL VARCHAR(200) NOT NULL,
    CheckedIn DATETIME NULL
)
GO

CREATE TABLE HostSetting
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    HostRegistrationID INT NOT NULL REFERENCES HostRegistration(ID),
    Name VARCHAR(50) NOT NULL,
    Value VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE NodeType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL
)
GO

INSERT INTO NodeType VALUES('FileProcessor', 'openXDA.Nodes.dll', 'openXDA.Nodes.Types.FileProcessing.FileProcessorNode')
GO

INSERT INTO NodeType VALUES('Analysis', 'openXDA.Nodes.dll', 'openXDA.Nodes.Types.Analysis.AnalysisNode')
GO

INSERT INTO NodeType VALUES('EventEmail', 'openXDA.Nodes.dll', 'openXDA.Nodes.Types.Email.EventEmailNode')
GO

INSERT INTO NodeType VALUES('FilePruner', 'openXDA.Nodes.dll', 'openXDA.Nodes.Types.FilePruning.FilePrunerNode')
GO

INSERT INTO NodeType VALUES('EPRICapBankAnalysis', 'openXDA.Nodes.dll', 'openXDA.Nodes.Types.EPRICapBankAnalysis.EPRICapBankAnalysisNode')
GO

CREATE TABLE Node
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    NodeTypeID INT NOT NULL REFERENCES NodeType(ID),
    HostRegistrationID INT NULL REFERENCES HostRegistration(ID),
    Name VARCHAR(50) NOT NULL,
    MinimumHostCount INT NOT NULL DEFAULT 1
)
GO

INSERT INTO Node VALUES((SELECT ID FROM NodeType WHERE TypeName = 'openXDA.Nodes.Types.FileProcessing.FileProcessorNode'), NULL, 'File Processor', 1)
GO

INSERT INTO Node VALUES((SELECT ID FROM NodeType WHERE TypeName = 'openXDA.Nodes.Types.FilePruning.FilePrunerNode'), NULL, 'File Pruner', 1)
GO

INSERT INTO Node VALUES((SELECT ID FROM NodeType WHERE TypeName = 'openXDA.Nodes.Types.Email.EventEmailNode'), NULL, 'Emailer', 1)
GO

INSERT INTO Node VALUES((SELECT ID FROM NodeType WHERE TypeName = 'openXDA.Nodes.Types.Analysis.AnalysisNode'), NULL, 'Analyzer', 1)
GO

INSERT INTO Node VALUES((SELECT ID FROM NodeType WHERE TypeName = 'openXDA.Nodes.Types.Analysis.AnalysisNode'), NULL, 'Analyzer', 2)
GO

INSERT INTO Node VALUES((SELECT ID FROM NodeType WHERE TypeName = 'openXDA.Nodes.Types.Analysis.AnalysisNode'), NULL, 'Analyzer', 4)
GO

CREATE TABLE NodeSetting
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    NodeID INT NOT NULL REFERENCES Node(ID),
    Name VARCHAR(50) NOT NULL,
    Value VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE AnalysisTask
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID),
    MeterID INT NOT NULL REFERENCES Meter(ID),
    NodeID INT NULL REFERENCES Node(ID),
    Priority INT NOT NULL
)
GO

CREATE TABLE MeterFacility
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    FacilityID INT NOT NULL
)
GO

CREATE TABLE AssetType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE Asset
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetTypeID INT NOT NULL REFERENCES AssetType(ID),
    AssetKey VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL,
	AssetName VARCHAR(200) NOT NULL,
	VoltageKV FLOAT NOT NULL,
	Spare Bit NULL Default 0
)
GO

CREATE TABLE AssetSpare
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT UNIQUE NOT NULL REFERENCES Asset(ID),
    SpareAssetID INT NOT NULL REFERENCES Asset(ID)
)
GO

CREATE VIEW AssetSpareView AS
	SELECT 
	AssetSpare.ID AS ID,
	AssetSpare.AssetID AS AssetID,
	AssetSpare.SpareAssetID AS SpareAssetID,
	Parent.AssetName AS Assetname,
	Parent.AssetKey AS AssetKey,
	Child.AssetName AS SpareName,
	Child.AssetKey AS SpareKey,
	AssetType.Name AS AssetType
FROM
	AssetSpare LEFT JOIN Asset Child ON Child.ID = AssetSpare.SpareAssetID LEFT JOIN
	Asset Parent ON Parent.ID = AssetSpare.AssetID LEFT JOIN
	AssetType ON Parent.AssetTypeID = AssetType.ID
GO

CREATE TABLE Customer
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    CustomerKey VARCHAR(50) NOT NULL UNIQUE,
    Name VARCHAR(200) NULL,
    Phone VARCHAR(20) NULL,
	Description VARCHAR(200) NULL
)
GO

CREATE TABLE CustomerAsset
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    CustomerID INT NOT NULL REFERENCES Customer(ID),
    AssetID INT NOT NULL REFERENCES Asset(ID)
)
GO

Create View CustomerAssetDetail AS
SELECT 
	CustomerAsset.ID AS ID,
	Customer.CustomerKey AS CustomerKey,
	Customer.Name AS CustomerName,
	Asset.AssetKey AS AssetKey,
	Asset.AssetName AS AssetName,
	AssetType.Name AS AssetType,
	Customer.ID AS CustomerID,
	Asset.ID AS AssetID
FROM
	CustomerAsset LEFT JOIN Asset ON Asset.ID = CustomerAsset.AssetID LEFT JOIN
	Customer ON Customer.ID = CustomerAsset.CustomerID LEFT JOIN
	AssetType ON Asset.AssetTypeID = AssetType.ID
GO


CREATE TABLE AssetRelationshipType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(MAX) NULL,
	BiDirectional BIT NOT NULL DEFAULT 0,
	JumpConnection VARCHAR(MAX) NOT NULL DEFAULT 'SELECT 0',
	PassThrough VARCHAR(MAX) NOT NULL DEFAULT 'SELECT 0',
)
GO

CREATE TABLE AssetRelationship
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetRelationshipTypeID INT NOT NULL REFERENCES AssetRelationshipType(ID),
    ParentID INT NOT NULL REFERENCES Asset(ID),
    ChildID INT NOT NULL REFERENCES Asset(ID)
)
GO

-- View with Procedures Due To Spare Logic --

CREATE VIEW AssetConnection AS 
SELECT
	AssetRelationship.ID AS ID,
	AssetRelationship.AssetRelationshipTypeID AS AssetRelationshipTypeID,
	(SELECT (CASE WHEN Child.Spare = 1 THEN SpareChild.SpareAssetID ELSE AssetRelationship.ChildID END)) AS ChildID,
	(SELECT (CASE WHEN Parent.Spare = 1 THEN SpareParent.SpareAssetID ELSE AssetRelationship.ParentID END)) AS ParentID
FROM
	AssetRelationship JOIN ASSET CHILD ON AssetRelationship.ChildID = Child.ID JOIN
	Asset Parent ON AssetRelationship.ParentID = Parent.ID LEFT JOIN
	AssetSpare SpareChild ON Child.ID = SpareChild.AssetID LEFT JOIN
	AssetSpare SpareParent ON Parent.ID = SpareParent.AssetID
GO

CREATE TRIGGER TR_INSERT_AssetConnection ON AssetConnection
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO AssetRelationship (ChildID, ParentID, AssetRelationshipTypeID)
		SELECT 
			ChildID AS ChildID,
			ParentID AS ParentID,
			AssetRelationshipTypeID AS AssetRelationshipTypeID
	FROM INSERTED
END
GO

CREATE TRIGGER TR_UPDATE_AssetConnection ON AssetConnection
INSTEAD OF UPDATE AS
BEGIN

		UPDATE AssetRelationship
		SET
			AssetRelationship.ChildID = AssetRelationship.ChildID,
			AssetRelationship.ParentID = AssetRelationship.ParentID,
			AssetRelationship.AssetRelationshipTypeID = AssetRelationship.AssetRelationshipTypeID
		FROM
			AssetRelationship INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = AssetRelationship.ID;
END
GO

CREATE TRIGGER TR_DELETE_AssetConnection ON AssetConnection
INSTEAD OF DELETE AS 
BEGIN
	DELETE FROM AssetRelationship WHERE ID IN (SELECT DELETED.ID FROM DELETED)
END
GO

-- End Spare Logic 

CREATE TABLE BusAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
)
GO

CREATE TABLE BreakerAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
	ThermalRating FLOAT NOT NULL DEFAULT(0),
	Speed FLOAT NOT NULL DEFAULT(0),
	TripTime INT NULL DEFAULT(0),
	PickupTime INT NULL DEFAULT(0),
	TripCoilCondition FLOAT NULL DEFAULT(0)
)
GO

CREATE TABLE CapacitorBankAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    NumberOfBanks INT NOT NULL,
    CapacitancePerBank INT NOT NULL,
    CktSwitcher VARCHAR(20) NOT NULL,
    MaxKV FLOAT NOT NULL,
    UnitKV FLOAT NOT NULL,
    UnitKVAr FLOAT NOT NULL,
    NegReactanceTol FLOAT NOT NULL,
    PosReactanceTol FLOAT NOT NULL,
    Nparalell INT NOT NULL,
    Nseries INT NOT NULL,
    NSeriesGroup INT NOT NULL DEFAULT(0),
    NParalellGroup INT NOT NULL DEFAULT(0),
    Fused BIT NOT NULL,
    VTratioBus FLOAT NOT NULL DEFAULT(0),
    NumberLVCaps INT NOT NULL DEFAULT(0),
    NumberLVUnits INT NOT NULL DEFAULT(0),
    LVKVAr FLOAT NOT NULL DEFAULT(0),
    LVKV FLOAT NOT NULL DEFAULT(0),
    LVNegReactanceTol FLOAT NOT NULL DEFAULT(0),
    LVPosReactanceTol FLOAT NOT NULL DEFAULT(0),
    LowerXFRRatio FLOAT NOT NULL DEFAULT(0),
    Nshorted FLOAT NOT NULL,
    BlownFuses INT NOT NULL DEFAULT(0),
    BlownGroups INT NOT NULL DEFAULT(0),
    RelayPTRatioPrimary INT NULL DEFAULT(0),
    RelayPTRatioSecondary INT NULL DEFAULT(0),
    Rv FLOAT NOT NULL DEFAULT(0),
    Rh FLOAT NOT NULL DEFAULT(0),
    Compensated BIT NOT NULL,
    NLowerGroups INT NOT NULL,
    ShortedGroups FLOAT NOT NULL DEFAULT(0),
    Sh FLOAT NOT NULL DEFAULT(0),
)
GO

CREATE TABLE CapacitorBankRelayAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    OnVoltageThreshhold FLOAT NOT NULL
)
GO


CREATE TABLE LineAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    MaxFaultDistance FLOAT NULL,
    MinFaultDistance FLOAT NULL,
)
GO

CREATE TABLE LineSegmentAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    Length FLOAT NOT NULL,
    R0 FLOAT NOT NULL,
    X0 FLOAT NOT NULL,
    R1 FLOAT NOT NULL,
    X1 FLOAT NOT NULL,
	ThermalRating FLOAT NOT NULL,
    IsEnd BIT NOT NULL DEFAULT(0)
)
GO

CREATE TABLE LineSegmentConnections
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ParentSegment INT REFERENCES Asset(ID),
    ChildSegment INT REFERENCES Asset(ID),
)
GO

CREATE TABLE TransformerAttributes
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    R0 FLOAT NOT NULL DEFAULT(0),
    X0 FLOAT NOT NULL DEFAULT(0),
    R1 FLOAT NOT NULL DEFAULT(0),
    X1 FLOAT NOT NULL DEFAULT(0),
	ThermalRating FLOAT NOT NULL,
	SecondaryVoltageKV FLOAT NULL,
	PrimaryVoltageKV FLOAT NULL,
	Tap FLOAT NULL
)
GO


-- Correspoding Views and Trigger 
CREATE VIEW Line AS
	SELECT 
		AssetID AS ID,
		MaxFaultDistance,
		MinFaultDistance,
		AssetKey,
		VoltageKV,
		Description,
		AssetName,
		AssetTypeID,
		Spare
	FROM Asset JOIN LineAttributes ON Asset.ID = LineAttributes.AssetID
GO

CREATE VIEW AssetView AS
	SELECT 
		Asset.ID AS ID,
		AssetKey,
		VoltageKV,
		Asset.Description,
		AssetName,
		AssetType.Name AS AssetType,
		AssetTypeID,
		Spare
	FROM Asset JOIN	AssetType ON AssetType.ID = Asset.AssetTypeID WHERE AssetType.ID != 5  


GO

CREATE TRIGGER TR_INSERT_Line ON Line
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, AssetName,VoltageKV, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'Line') AS AssetTypeID,
			Description AS Description,
			AssetName AS AssetName,
			VoltageKV AS VoltageKV,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO LineAttributes (AssetID, MinFaultDistance, MaxFaultDistance)
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID,
			MinFaultDistance AS MinFaultDistance,
			MaxFaultDistance AS MaxFaultDistance
	FROM INSERTED

END
GO

CREATE TRIGGER TR_UPDATE_Line ON Line
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE (VoltageKV) OR UPDATE(Spare))
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
	UPDATE LineAttributes
		SET
			LineAttributes.MaxFaultDistance = INSERTED.MaxFaultDistance,
			LineAttributes.MinFaultDistance = INSERTED.MinFaultDistance
		FROM
			LineAttributes 
	INNER JOIN
		INSERTED
	ON 
		INSERTED.ID = LineAttributes.AssetID;
END
GO

-- END Line Model Triggers 
-- Bus Model 
CREATE VIEW Bus AS
	SELECT 
		AssetID AS ID,
		AssetKey,
		VoltageKV,
		Description,
		AssetName,
		AssetTypeID,
		Spare
	FROM Asset JOIN BusAttributes ON Asset.ID = BusAttributes.AssetID
GO

CREATE TRIGGER TR_INSERT_Bus ON BUS
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, AssetName, VoltageKV, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'Bus') AS AssetTypeID,
			Description AS Description,
			AssetName AS AssetName,
			VoltageKV AS VoltageKV,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO BusAttributes (AssetID)
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID
	FROM INSERTED

END
GO

CREATE TRIGGER TR_UPDATE_Bus ON BUS
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE (VoltageKV) OR UPDATE(Spare))
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
END
GO


-- Breaker Model 
CREATE VIEW Breaker AS
	SELECT 
		AssetID AS ID,
		AssetKey,
		VoltageKV,
		ThermalRating,
		Speed,
		Description,
		AssetName,
		AssetTypeID,
		TripTime,
		PickupTime,
		TripCoilCondition,
		Spare
	FROM Asset JOIN BreakerAttributes ON Asset.ID = BreakerAttributes.AssetID
GO

CREATE TRIGGER TR_INSERT_Breaker ON Breaker
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, VoltageKV, AssetName, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'Breaker') AS AssetTypeID,
			Description AS Description,
			VoltageKV AS VoltageKV,
			AssetName AS AssetName,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO BreakerAttributes (AssetID, ThermalRating, Speed, TripTime, PickupTime, TripCoilCondition)
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID,
			ThermalRating AS ThermalRating,
			Speed AS Speed,
			TripTime AS TripTime,
			PickupTime AS PickupTime,
			TripCoilCondition AS TripCoilCondition
	FROM INSERTED

END
GO

CREATE TRIGGER TR_UPDATE_Breaker ON Breaker
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE (VoltageKV) OR UPDATE(Spare))
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
	UPDATE BreakerAttributes
		SET
			BreakerAttributes.ThermalRating = INSERTED.ThermalRating,
			BreakerAttributes.Speed = INSERTED.Speed,
			BreakerAttributes.TripTime = INSERTED.TripTime,
			BreakerAttributes.PickupTime = INSERTED.PickupTime,
			BreakerAttributes.TripCoilCondition = INSERTED.TripCoilCondition
		FROM
			BreakerAttributes 
	INNER JOIN
		INSERTED
	ON 
		INSERTED.ID = BreakerAttributes.AssetID;
END
GO

-- Capacitor Bank Relay Model 
CREATE VIEW CapBankRelay AS
	SELECT 
		AssetID AS ID,
		AssetKey,
		VoltageKV,
		Description,
		AssetName,
		AssetTypeID,
		Spare,
        OnVoltageThreshhold
	FROM Asset JOIN CapacitorBankRelayAttributes ON Asset.ID = CapacitorBankRelayAttributes.AssetID
GO

CREATE TRIGGER TR_INSERT_CapBankRelay ON CapBankRelay
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, AssetName, VoltageKV, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'CapacitorBankRelay') AS AssetTypeID,
			Description AS Description,
			AssetName AS AssetName,
			VoltageKV AS VoltageKV,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO CapacitorBankRelayAttributes (AssetID, OnVoltageThreshhold )
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID,
            OnVoltageThreshhold AS OnVoltageThreshhold
	    FROM INSERTED

END
GO

CREATE TRIGGER TR_UPDATE_CapBankRelay ON CapBankRelay
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE(VoltageKV) OR Update(Spare) )
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
    UPDATE CapBankRelay
		SET
			CapBankRelay.OnVoltageThreshhold = INSERTED.OnVoltageThreshhold
		FROM
			CapBankRelay 
	INNER JOIN
		INSERTED
	ON 
		INSERTED.ID = CapBankRelay.ID;
END
GO


-- Capacitor Bank Model 
CREATE VIEW CapBank AS
	SELECT 
		AssetID AS ID,
		AssetKey,
		VoltageKV,
		Description,
		AssetName,
		AssetTypeID,
		Spare,
        NumberOfBanks,
        CapacitancePerBank,
        CktSwitcher,
        MaxKV,
        UnitKV,
        UnitKVAr,
        NegReactanceTol,
        PosReactanceTol,
        Nparalell,
        Nseries,
        NSeriesGroup,
        NParalellGroup,
        Fused,
        VTratioBus,
        NumberLVCaps,
        NumberLVUnits,
        LVKVAr,
        LVKV,
        LVNegReactanceTol,
        LVPosReactanceTol,
        LowerXFRRatio,
        Nshorted,
        BlownFuses,
        BlownGroups,
        RelayPTRatioPrimary,
        RelayPTRatioSecondary,
        Sh,
        Rv,
        Rh,
        Compensated,
        NLowerGroups,
        ShortedGroups
	FROM Asset JOIN CapacitorBankAttributes ON Asset.ID = CapacitorBankAttributes.AssetID
GO

CREATE TRIGGER TR_INSERT_CapBank ON CapBank
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, AssetName, VoltageKV, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank') AS AssetTypeID,
			Description AS Description,
			AssetName AS AssetName,
			VoltageKV AS VoltageKV,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO CapacitorBankAttributes (AssetID, CapacitancePerBank, NumberOfBanks, CktSwitcher, MaxKV, UnitKV, UnitKVAr, NegReactanceTol,
        PosReactanceTol, Nparalell, Nseries, NSeriesGroup, NParalellGroup, Fused, VTratioBus, NumberLVCaps, NumberLVUnits, LVKVAr,
        LVKV, LVNegReactanceTol, LVPosReactanceTol, LowerXFRRatio, Nshorted, BlownFuses, BlownGroups,
        RelayPTRatioPrimary, RelayPTRatioSecondary,Rv, Rh, Compensated, NLowerGroups, ShortedGroups,Sh)
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID,
			CapacitancePerBank AS CapacitancePerBank,
            NumberOfBanks AS NumberOfBanks,
            CktSwitcher AS CktSwitcher,
            MaxKV AS MaxKV,
            UnitKV AS UnitKV,
            UnitKVAr AS UnitKVAr,
            NegReactanceTol AS NegReactanceTol,
            PosReactanceTol AS PosReactanceTol,
            Nparalell AS Nparalell,
            Nseries AS Nseries,
            NSeriesGroup AS NSeriesGroup,
            NParalellGroup AS NParalellGroup,
            Fused AS Fused,
            VTratioBus AS VTratioBus,
            NumberLVCaps AS NumberLVCaps,
            NumberLVUnits AS NumberLVUnits,
            LVKVAr AS LVKVAr,
            LVKV AS LVKV,
            LVNegReactanceTol AS LVNegReactanceTol,
            LVPosReactanceTol AS LVPosReactanceTol,
            LowerXFRRatio AS LowerXFRRatio,
            Nshorted AS Nshorted,
            BlownFuses AS BlownFuses,
            BlownGroups AS BlownGroups,
            RelayPTRatioPrimary AS RelayPTRatioPrimary,
            RelayPTRatioSecondary AS RelayPTRatioPrimary,
            Rv AS Rv,
            Rh AS Rh,
            Compensated AS Compensated,
            NLowerGroups AS NLowerGroups,
            ShortedGroups AS ShortedGroups,
            Sh AS Sh
	    FROM INSERTED
END
GO

CREATE TRIGGER TR_UPDATE_CapBank ON CapBank
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE(VoltageKV) OR Update(Spare) )
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
	UPDATE CapacitorBankAttributes
		SET
			CapacitorBankAttributes.CapacitancePerBank = INSERTED.CapacitancePerBank,
            CapacitorBankAttributes.NumberOfBanks = INSERTED.NumberOfBanks,
            CapacitorBankAttributes.CktSwitcher = INSERTED.CktSwitcher,
            CapacitorBankAttributes.MaxKV = INSERTED.MaxKV,
            CapacitorBankAttributes.UnitKV = INSERTED.UnitKV,
            CapacitorBankAttributes.UnitKVAr = INSERTED.UnitKVAr,
            CapacitorBankAttributes.NegReactanceTol = INSERTED.NegReactanceTol,
            CapacitorBankAttributes.PosReactanceTol = INSERTED.PosReactanceTol,
            CapacitorBankAttributes.Nparalell = INSERTED.Nparalell,
            CapacitorBankAttributes.Nseries = INSERTED.Nseries,
            CapacitorBankAttributes.NSeriesGroup = INSERTED.NSeriesGroup,
            CapacitorBankAttributes.NParalellGroup = INSERTED.NParalellGroup,
            CapacitorBankAttributes.Fused = INSERTED.Fused,
            CapacitorBankAttributes.VTratioBus = INSERTED.VTratioBus,
            CapacitorBankAttributes.NumberLVCaps = INSERTED.NumberLVCaps,
            CapacitorBankAttributes.NumberLVUnits = INSERTED.NumberLVUnits,
            CapacitorBankAttributes.LVKVAr = INSERTED.LVKVAr,
            CapacitorBankAttributes.LVKV = INSERTED.LVKV,
            CapacitorBankAttributes.LVNegReactanceTol = INSERTED.LVNegReactanceTol,
            CapacitorBankAttributes.LVPosReactanceTol = INSERTED.LVPosReactanceTol,
            CapacitorBankAttributes.LowerXFRRatio = INSERTED.LowerXFRRatio,
            CapacitorBankAttributes.Nshorted = INSERTED.Nshorted,
            CapacitorBankAttributes.BlownFuses = INSERTED.BlownFuses,
            CapacitorBankAttributes.BlownGroups = INSERTED.BlownGroups,
            CapacitorBankAttributes.RelayPTRatioPrimary = INSERTED.RelayPTRatioPrimary,
            CapacitorBankAttributes.RelayPTRatioSecondary = INSERTED.RelayPTRatioSecondary,
            CapacitorBankAttributes.Rv = INSERTED.Rv,
            CapacitorBankAttributes.Rh = INSERTED.Rh,
            CapacitorBankAttributes.Compensated = INSERTED.Compensated,
            CapacitorBankAttributes.NLowerGroups = INSERTED.NLowerGroups,
            CapacitorBankAttributes.ShortedGroups = INSERTED.ShortedGroups,
            CapacitorBankAttributes.Sh = INSERTED.Sh
		FROM
			CapacitorBankAttributes 
	INNER JOIN
		INSERTED
	ON 
		INSERTED.ID = CapacitorBankAttributes.AssetID;
END
GO


-- Line Segment Model 
CREATE VIEW LineSegment AS
	SELECT 
		AssetID AS ID,
		AssetKey,
		Length,
		R0,
		X0,
		R1,
		X1,
		ThermalRating,
		Description,
		AssetName,
		VoltageKV,
		AssetTypeID,
		Spare,
        IsEnd
	FROM Asset JOIN LineSegmentAttributes ON Asset.ID = LineSegmentAttributes.AssetID
GO

CREATE TRIGGER TR_INSERT_LineSegment ON LineSegment
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, AssetName, VoltageKV, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'LineSegment') AS AssetTypeID,
			Description AS Description,
			AssetName AS AssetName,
			VoltageKV AS VoltageKV,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO LineSegmentAttributes (AssetID, Length, R0, X0, R1, X1, ThermalRating, IsEnd)
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID,
			Length AS Length,
			R0 AS R0,
			X0 AS X0,
			R1 AS R1,
			X1 AS X1,
			ThermalRating AS ThermalRating,
            IsEnd AS IsEnd
	FROM INSERTED

END
GO

CREATE TRIGGER TR_UPDATE_LineSegment ON LineSegment
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE(VoltageKV) OR Update(Spare))
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV  = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
	UPDATE LineSegmentAttributes
		SET
			LineSegmentAttributes.R0 = INSERTED.R0,
			LineSegmentAttributes.X0 = INSERTED.X0,
			LineSegmentAttributes.R1 = INSERTED.R1,
			LineSegmentAttributes.X1 = INSERTED.X1,
			LineSegmentAttributes.ThermalRating = INSERTED.ThermalRating,
            LineSegmentAttributes.IsEnd = INSERTED.IsEnd
		FROM
			LineSegmentAttributes 
	INNER JOIN
		INSERTED
	ON 
		INSERTED.ID = LineSegmentAttributes.AssetID;
END
GO


-- Transformers 
CREATE VIEW Transformer AS
	SELECT 
		AssetID AS ID,
		AssetKey,
		R0,
		X0,
		R1,
		X1,
		ThermalRating,
		SecondaryVoltageKV,
		PrimaryVoltageKV,
		TAP,
		Description,
		AssetName,
		VoltageKV,
		AssetTypeID,
		Spare
	FROM Asset JOIN TransformerAttributes ON Asset.ID = TransformerAttributes.AssetID
GO

CREATE TRIGGER TR_INSERT_Tranformer ON Transformer
INSTEAD OF INSERT AS 
BEGIN
	INSERT INTO Asset (AssetKey, AssetTypeID, Description, AssetName, VoltageKV, Spare)
		SELECT 
			AssetKey AS AssetKey,
			(SELECT ID FROM AssetType WHERE Name = 'Transformer') AS AssetTypeID,
			Description AS Description,
			AssetName AS AssetName,
			VoltageKV AS VoltageKV,
			Spare AS Spare
	FROM INSERTED

	INSERT INTO TransformerAttributes (AssetID, R0, X0, R1, X1, ThermalRating, SecondaryVoltageKV, PrimaryVoltageKV, Tap )
		SELECT 
			(SELECT ID FROM Asset WHERE AssetKey = INSERTED.AssetKey) AS AssetID,
			R0 AS R0,
			X0 AS X0,
			R1 AS R1,
			X1 AS X1,
			ThermalRating AS ThermalRating,
			SecondaryVoltageKV AS SecondaryVoltageKV,
			PrimaryVoltageKV AS PrimaryVoltageKV,
			Tap AS Tap

	FROM INSERTED

END
GO

CREATE TRIGGER TR_UPDATE_Tranformer ON Transformer
INSTEAD OF UPDATE AS
BEGIN
IF (UPDATE(AssetKey) OR UPDATE(Description) OR UPDATE (AssetName) OR UPDATE(VoltageKV) OR UPDATE(Spare))
	BEGIN
		UPDATE Asset
		SET
			Asset.AssetKey = INSERTED.AssetKey,
			Asset.Description = INSERTED.Description,
			Asset.AssetName = INSERTED.AssetName,
			Asset.VoltageKV = INSERTED.VoltageKV,
			Asset.Spare = INSERTED.Spare
		FROM
			ASSET 
		INNER JOIN
			INSERTED
		ON 
			INSERTED.ID = ASSET.ID;
	END
	UPDATE TransformerAttributes
		SET
			TransformerAttributes.R0 = INSERTED.R0,
			TransformerAttributes.X0 = INSERTED.X0,
			TransformerAttributes.R1 = INSERTED.R1,
			TransformerAttributes.X1 = INSERTED.X1,
			TransformerAttributes.ThermalRating = INSERTED.ThermalRating,
			TransformerAttributes.SecondaryVoltageKV = INSERTED.SecondaryVoltageKV,
			TransformerAttributes.PrimaryVoltageKV = INSERTED.PrimaryVoltageKV,
			TransformerAttributes.Tap = INSERTED.Tap
		FROM
			TransformerAttributes 
	INNER JOIN
		INSERTED
	ON 
		INSERTED.ID = TransformerAttributes.AssetID;
END
GO

--*************** End Model Section *********

CREATE TABLE Structure
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetKey VARCHAR(50) NOT NULL UNIQUE,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    Latitude FLOAT NOT NULL DEFAULT 0.0,
    Longitude FLOAT NOT NULL DEFAULT 0.0
)
GO

CREATE TABLE MeterAsset
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    AssetID INT NOT NULL REFERENCES Asset(ID),
)
GO

CREATE TABLE AssetLocation
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LocationID INT NOT NULL REFERENCES Location(ID),
    AssetID INT NOT NULL REFERENCES Asset(ID)
)
GO

CREATE TABLE MeasurementType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE MeasurementCharacteristic
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL,
    Display BIT NOT NULL DEFAULT 0
)
GO

CREATE TABLE Phase
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE Channel
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
	AssetID INT NOT NULL REFERENCES Asset(ID),
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    Name VARCHAR(200) NOT NULL,
    Adder FLOAT NOT NULL DEFAULT 0,
    Multiplier FLOAT NOT NULL DEFAULT 1,
    SamplesPerHour FLOAT NOT NULL,
    PerUnitValue FLOAT NULL,
    HarmonicGroup INT NOT NULL,
    Description VARCHAR(MAX) NULL,
    Enabled INT NOT NULL DEFAULT 1
)
GO

CREATE NONCLUSTERED INDEX IX_Channel_MeterID
ON Channel(MeterID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Channel_MeasurementTypeID
ON Channel(MeasurementTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Channel_MeasurementCharacteristicID
ON Channel(MeasurementCharacteristicID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Channel_PhaseID
ON Channel(PhaseID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Channel_MeterID_MeasurementTypeID_MeasurementCharacteristicID_PhaseID_HarmonicGroup
ON Channel(MeterID ASC, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup)
GO

-- Channel Group and Type
CREATE TABLE ChannelGroup
(
	ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	Name VARCHAR(200) NOT NULL,
	Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE ChannelGroupType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelGroupID INT NOT NULL REFERENCES ChannelGroup(ID),
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
	DisplayName VARCHAR(20) NOT NULL
)
GO


CREATE TABLE AssetChannel
(
    ID INT IDENTITY(1, 1) NOT NULL,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    ChannelID INT NOT NULL REFERENCES Channel(ID)
)
GO

CREATE TABLE SeriesType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

INSERT INTO SeriesType(Name, Description) VALUES('Values', 'Instantaneous data values')
GO

INSERT INTO SeriesType(Name, Description) VALUES('Minimum', 'Minimum data values')
GO

INSERT INTO SeriesType(Name, Description) VALUES('Maximum', 'Maximum data values')
GO

INSERT INTO SeriesType(Name, Description) VALUES('Average', 'Average data values')
GO

INSERT INTO SeriesType(Name, Description) VALUES('Duration', 'Duration data values')
GO

CREATE TABLE Series
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    SeriesTypeID INT NOT NULL REFERENCES SeriesType(ID),
    SourceIndexes VARCHAR(200) NOT NULL
)
GO

CREATE TABLE MeterConfiguration
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    DiffID INT NULL REFERENCES MeterConfiguration(ID),
    ConfigKey VARCHAR(50) NOT NULL,
    ConfigText VARCHAR(MAX) NOT NULL,
    RevisionMajor INT NULL DEFAULT(0),
    RevisionMinor INT NULL DEFAULT(0),
    CONSTRAINT UC_MeterConfiguration UNIQUE(MeterID, RevisionMajor, RevisionMinor)
)
GO

CREATE NONCLUSTERED INDEX IX_MeterConfiguration_MeterID
ON MeterConfiguration(MeterID)
GO

CREATE NONCLUSTERED INDEX IX_MeterConfiguration_DiffID
ON MeterConfiguration(DiffID)
GO

CREATE TABLE FileGroupMeterConfiguration
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID),
    MeterConfigurationID INT NOT NULL REFERENCES MeterConfiguration(ID)
)
GO

CREATE NONCLUSTERED INDEX IX_FileGroupMeterConfiguration_FileGroupID
ON FileGroupMeterConfiguration(FileGroupID)
GO

CREATE NONCLUSTERED INDEX IX_FileGroupMeterConfiguration_MeterConfigurationID
ON FileGroupMeterConfiguration(MeterConfigurationID)
GO

CREATE TABLE AssetGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    DisplayDashboard Bit NOT NULL Default(1)
)
GO

CREATE TABLE MeterAssetGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    AssetGroupID INT NOT NULL REFERENCES AssetGroup(ID),
)
GO

CREATE NONCLUSTERED INDEX IX_MeterAssetGroup_MeterID
ON MeterAssetGroup(MeterID ASC)
GO

CREATE NONCLUSTERED INDEX IX_MeterAssetGroup_AssetGroupID
ON MeterAssetGroup(AssetGroupID ASC)
GO

CREATE TRIGGER Meter_AugmentAllAssetsGroup
ON Meter
AFTER INSERT
AS BEGIN
    SET NOCOUNT ON;

    INSERT INTO MeterAssetGroup(MeterID, AssetGroupID)
    SELECT Meter.ID, AssetGroup.ID
    FROM inserted Meter CROSS JOIN AssetGroup
    WHERE AssetGroup.Name = 'AllAssets'
END
GO

CREATE TABLE AssetAssetGroup
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    AssetID INT NOT NULL REFERENCES Asset(ID),
    AssetGroupID INT NOT NULL REFERENCES AssetGroup(ID),
)
GO

CREATE NONCLUSTERED INDEX IX_AssetAssetGroup_AssetID
ON AssetAssetGroup(AssetID ASC)
GO

CREATE NONCLUSTERED INDEX IX_AssetAssetGroup_AssetGroupID
ON AssetAssetGroup(AssetGroupID ASC)
GO

CREATE TRIGGER Asset_AugmentAllAssetsGroup
ON Asset
AFTER INSERT
AS BEGIN
    SET NOCOUNT ON;

    INSERT INTO AssetAssetGroup(AssetID, AssetGroupID)
    SELECT Asset.ID, AssetGroup.ID
    FROM inserted Asset CROSS JOIN AssetGroup
    WHERE AssetGroup.Name = 'AllAssets'
END
GO

CREATE TABLE AssetGroupAssetGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ParentAssetGroupID INT NOT NULL REFERENCES AssetGroup(ID),
    ChildAssetGroupID INT NOT NULL REFERENCES AssetGroup(ID),
)
GO

CREATE NONCLUSTERED INDEX IX_AssetGroupAssetGroup_ParentAssetGroupID
ON AssetGroupAssetGroup(ParentAssetGroupID ASC)
GO

CREATE NONCLUSTERED INDEX IX_AssetGroupAssetGroup_ChildAssetGroupID
ON AssetGroupAssetGroup(ChildAssetGroupID ASC)
GO

CREATE TABLE MaintenanceWindow
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    StartTime DATETIME,
    EndTime DATETIME
)
GO

CREATE TABLE BreakerChannel
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    BreakerNumber VARCHAR(120) NOT NULL
)
GO

CREATE TABLE AuditLog
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TableName VARCHAR(200) NOT NULL,
    PrimaryKeyColumn VARCHAR(200) NOT NULL,
    PrimaryKeyValue VARCHAR(MAX) NOT NULL,
    ColumnName VARCHAR(200) NOT NULL,
    OriginalValue VARCHAR(MAX) NULL,
    NewValue VARCHAR(MAX) NULL,
    Deleted BIT NOT NULL DEFAULT 0,
    UpdatedBy VARCHAR(200) NULL DEFAULT suser_name(),
    UpdatedOn DATETIME NULL DEFAULT getutcdate(),
)
GO

CREATE TABLE PQViewSite(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	SiteID INT NOT NULL,
	StationKey VARCHAR(20) NULL,
	LineKey VARCHAR(20) NULL,
	PQIFacility INT NULL,
	Enabled BIT NOT NULL DEFAULT 1
)
GO

CREATE TABLE EDNAPoint(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	BreakerID INT NOT NULL FOREIGN KEY REFERENCES Asset(ID),
	Point VARCHAR(20) NOT NULL,
)
GO


INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.dat', 'FaultData.dll', 'FaultData.DataReaders.COMTRADEReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.d00', 'FaultData.dll', 'FaultData.DataReaders.COMTRADEReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.rcd', 'FaultData.dll', 'FaultData.DataReaders.EMAXReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.rcl', 'FaultData.dll', 'FaultData.DataReaders.EMAXReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.pqd', 'FaultData.dll', 'FaultData.DataReaders.PQDIFReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.sel', 'FaultData.dll', 'FaultData.DataReaders.SELEVEReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.eve', 'FaultData.dll', 'FaultData.DataReaders.SELEVEReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.cev', 'FaultData.dll', 'FaultData.DataReaders.SELEVEReader', 1)
GO

INSERT INTO DataReader(FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\*.txt', 'FaultData.dll', 'FaultData.DataReaders.SELLDPReader', 1)
GO


INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.ConfigurationOperation', 1)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.EventOperation', 2)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.DisturbanceSeverityOperation', 3)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.FaultLocationOperation', 4)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.DoubleEndedFaultOperation', 5)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('openXDA.HIDS.dll', 'openXDA.HIDS.TrendingDataSummaryOperation', 7)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('openXDA.HIDS.dll', 'openXDA.HIDS.DailySummaryOperation', 8)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('openXDA.HIDS.dll', 'openXDA.HIDS.DataQualityOperation', 9)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('openXDA.HIDS.dll', 'openXDA.HIDS.HIDSAlarmOperation', 10)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.StatisticOperation', 11)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.DataPusherOperation', 12)
GO

INSERT INTO AssetGroup(Name, DisplayDashboard) VALUES('AllAssets', 1)
GO

-- -------- --
-- Security --
-- -------- --
CREATE TABLE ApplicationRole
(
    ID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Description VARCHAR(MAX) NULL,
    NodeID UNIQUEIDENTIFIER NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    CreatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy VARCHAR(200) NOT NULL DEFAULT SUSER_NAME(),
    UpdatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy VARCHAR(200) NOT NULL DEFAULT SUSER_NAME()
)
GO

CREATE TABLE SecurityGroup
(
    ID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Description VARCHAR(MAX) NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy VARCHAR(200) NOT NULL DEFAULT SUSER_NAME(),
    UpdatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy VARCHAR(200) NOT NULL DEFAULT SUSER_NAME()
)
GO

CREATE TABLE UserAccount
(
    ID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Password VARCHAR(200) NULL,
    FirstName VARCHAR(200) NULL,
    LastName VARCHAR(200) NULL,
    DefaultNodeID UNIQUEIDENTIFIER NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    Phone VARCHAR(200) NULL,
    PhoneConfirmed BIT NOT NULL DEFAULT 0,
    Email VARCHAR(200) NULL,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    LockedOut BIT NOT NULL DEFAULT 0,
    Approved BIT NOT NULL DEFAULT 0,
    UseADAuthentication BIT NOT NULL DEFAULT 1,
    TSCID INT NULL,
    RoleID INT NULL,
    Title varchar(200) NULL,
    Department varchar(200) NULL,
    DepartmentNumber varchar(200) NULL,
    MobilePhone VARCHAR(200) NULL,
    ReceiveNotifications BIT NOT NULL DEFAULT 1,
    ChangePasswordOn DATETIME NULL DEFAULT DATEADD(DAY, 90, GETUTCDATE()),
    CreatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    UpdatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy VARCHAR(50) NOT NULL DEFAULT SUSER_NAME()
)
GO

CREATE TABLE ApplicationRoleSecurityGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ApplicationRoleID UNIQUEIDENTIFIER NOT NULL REFERENCES ApplicationRole(ID),
    SecurityGroupID UNIQUEIDENTIFIER NOT NULL REFERENCES SecurityGroup(ID)
)
GO

CREATE TABLE ApplicationRoleUserAccount
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ApplicationRoleID UNIQUEIDENTIFIER NOT NULL REFERENCES ApplicationRole(ID),
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID)
)
GO

CREATE TABLE SecurityGroupUserAccount
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    SecurityGroupID UNIQUEIDENTIFIER NOT NULL REFERENCES SecurityGroup(ID),
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID)
)
GO

CREATE TABLE UserAccountAssetGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID),
    AssetGroupID INT NOT NULL REFERENCES AssetGroup(ID),
    Dashboard BIT NOT NULL DEFAULT 1,
    Email BIT NOT NULL DEFAULT 0
)
GO

CREATE TRIGGER UserAccount_AugmentAllAssetsGroup
ON UserAccount
AFTER INSERT
AS BEGIN
    SET NOCOUNT ON;

    INSERT INTO UserAccountAssetGroup(UserAccountID, AssetGroupID)
    SELECT UserAccount.ID, AssetGroup.ID
    FROM inserted UserAccount CROSS JOIN AssetGroup
    WHERE AssetGroup.Name = 'AllAssets'
END
GO

INSERT INTO UserAccount(Name, UseADAuthentication, Approved) VALUES('External', 0, 1)
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('Administrator', 'Admin Role')
GO

INSERT INTO SecurityGroup(Name, Description) VALUES('S-1-5-32-545', 'All Windows authenticated users')
GO

INSERT INTO ApplicationRoleSecurityGroup(ApplicationRoleID, SecurityGroupID) VALUES((SELECT ID FROM ApplicationRole), (SELECT ID FROM SecurityGroup))
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('Engineer', 'Engineer Role')
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('Viewer', 'Viewer Role')
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('Developer', 'Developer Role')
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('DataPusher', 'Data Pusher Role')
GO



-- ----- --
-- Email --
-- ----- --

CREATE TABLE XSLTemplate
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) UNIQUE NOT NULL,
    Template VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE EmailCategory
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
)
GO

CREATE TABLE EmailType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailCategoryID INT NOT NULL REFERENCES EmailCategory(ID),
    XSLTemplateID INT NOT NULL REFERENCES XSLTemplate(ID),
    SMS BIT NOT NULL DEFAULT 0
)
GO

CREATE TABLE EventEmailParameters
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailTypeID INT NOT NULL UNIQUE REFERENCES EmailType(ID),
    TriggersEmailSQL VARCHAR(MAX) NOT NULL DEFAULT 'SELECT 0',
    EventDetailSQL VARCHAR(MAX) NOT NULL DEFAULT 'SELECT '''' FOR XML PATH(''EventDetail''), TYPE',
    TriggerSource VARCHAR(50) NOT NULL DEFAULT '',
    MinDelay FLOAT NOT NULL DEFAULT 10,
    MaxDelay FLOAT NOT NULL DEFAULT 60
)
GO

CREATE TABLE UserAccountEmailType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID),
    EmailTypeID INT NOT NULL REFERENCES EmailType(ID)
)
GO

CREATE TABLE DisturbanceEmailCriterion
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailTypeID INT NOT NULL REFERENCES EmailType(ID),
    SeverityCode INT NOT NULL
)
GO

CREATE TABLE FaultEmailCriterion
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailTypeID INT NOT NULL REFERENCES EmailType(ID),
    EmailOnReclose INT NOT NULL DEFAULT 0
)
GO

CREATE TABLE AlarmEmailCriterion
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailTypeID INT NOT NULL REFERENCES EmailType(ID),
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID)
)
GO

CREATE TABLE SentEmail
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    TimeSent DATETIME NOT NULL,
    ToLine VARCHAR(MAX) NOT NULL,
    Subject VARCHAR(500) NOT NULL,
    Message VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE FileBlob
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    DataFileID INT NOT NULL REFERENCES DataFile(ID),
    Blob VARBINARY(MAX) NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FileBlob_DataFileID
ON FileBlob(DataFileID ASC)
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Daily', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Disturbance', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Fault', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Alarm', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Breaker', '')
GO

INSERT INTO EmailCategory(Name) VALUES('Daily')
GO

INSERT INTO EmailCategory(Name) VALUES('Event')
GO

INSERT INTO EmailCategory(Name) VALUES('Alarm')
GO

INSERT INTO EmailType(EmailCategoryID, XSLTemplateID) VALUES(1, 1)
GO

INSERT INTO EmailType(EmailCategoryID, XSLTemplateID) VALUES(2, 2)
GO

INSERT INTO EmailType(EmailCategoryID, XSLTemplateID) VALUES(2, 3)
GO

INSERT INTO EmailType(EmailCategoryID, XSLTemplateID) VALUES(3, 4)
GO

INSERT INTO EmailType(EmailCategoryID, XSLTemplateID) VALUES(2, 5)
GO

INSERT INTO EventEmailParameters(EmailTypeID) VALUES(2)
GO

INSERT INTO EventEmailParameters(EmailTypeID) VALUES(3)
GO

INSERT INTO EventEmailParameters(EmailTypeID) VALUES(5)
GO

-- ------ --
-- Events --
-- ------ --

-- EventData references the IDs in other tables,
-- but no foreign key constraints are defined.
-- If they were defined, the records in this
-- table would need to be deleted before we
-- could delete records in the referenced table.
CREATE TABLE EventData
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL,
    RuntimeID INT NOT NULL,
    TimeDomainData VARBINARY(MAX) NOT NULL,
    MarkedForDeletion INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_EventData_FileGroupID
ON EventData(FileGroupID ASC)
GO

CREATE NONCLUSTERED INDEX IX_EventData_RuntimeID
ON EventData(RuntimeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_EventData_MarkedForDeletion
ON EventData(MarkedForDeletion ASC)
GO


-- ChannelData references the IDs in other tables,
-- but no foreign key constraints are defined.
-- If they were defined, the records in this
-- table would need to be deleted before we
-- could delete records in the referenced table.
CREATE TABLE ChannelData
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL,
    RuntimeID INT NOT NULL,
    TimeDomainData VARBINARY(MAX) NULL,
    MarkedForDeletion INT NOT NULL,
	SeriesID INT NOT NULL,
	EventID INT NOT NULL,
	EventDataID INT NULL
)
GO


--Indices for Channel Data potentially Neccesarry
--CREATE NONCLUSTERED INDEX IX_EventData_FileGroupID
--ON EventData(FileGroupID ASC)
--GO

--CREATE NONCLUSTERED INDEX IX_EventData_RuntimeID
--ON EventData(RuntimeID ASC)
--GO

--CREATE NONCLUSTERED INDEX IX_EventData_MarkedForDeletion
--ON EventData(MarkedForDeletion ASC)
--GO


CREATE TABLE EventType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE Event
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID),
    MeterID INT NOT NULL REFERENCES Meter(ID),
	AssetID INT NOT NULL REFERENCES Asset(ID),
    EventTypeID INT NOT NULL REFERENCES EventType(ID),
    EventDataID INT NULL REFERENCES EventData(ID),
    Name VARCHAR(200) NOT NULL,
    Alias VARCHAR(200) NULL,
    ShortName VARCHAR(50) NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    Samples INT NOT NULL,
    TimeZoneOffset INT NOT NULL,
    SamplesPerSecond INT NOT NULL,
    SamplesPerCycle INT NOT NULL,
    Description VARCHAR(MAX) NULL,
    FileVersion INT NOT NULL DEFAULT 0,
    UpdatedBy VARCHAR(200) NULL
)
GO

CREATE NONCLUSTERED INDEX IX_Event_FileGroupID
ON Event(FileGroupID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_MeterID
ON Event(MeterID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_EventTypeID
ON Event(EventTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_EventDataID
ON Event(EventDataID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_StartTime
ON Event(StartTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_EndTime
ON Event(EndTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_MeterID_StartTime_ID_EventID_PhaseID
ON Event ( MeterID ASC, StartTime ASC ) INCLUDE ( EventTypeID)
GO


CREATE TABLE Disturbance
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    EventTypeID INT NOT NULL REFERENCES EventType(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    Magnitude FLOAT NOT NULL,
    PerUnitMagnitude FLOAT NOT NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    DurationSeconds FLOAT NOT NULL,
    DurationCycles FLOAT NOT NULL,
    StartIndex INT NOT NULL,
    EndIndex INT NOT NULL,
    UpdatedBy VARCHAR(200) NULL
)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_EventID
ON Disturbance(EventID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_EventTypeID
ON Disturbance(EventTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_PhaseID
ON Disturbance(PhaseID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_StartTime
ON Disturbance(StartTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_EndTime
ON Disturbance(EndTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_StartTime_ID_EventID_PhaseID
ON Disturbance ( StartTime ASC ) INCLUDE ( ID, EventID, PhaseID)
GO

CREATE TABLE BreakerRestrike (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
	InitialExtinguishSample int NOT NULL,
	InitialExtinguishTime datetime2(7) NOT NULL,
	InitialExtinguishVoltage float NOT NULL,
	RestrikeSample int NOT NULL,
	RestrikeTime datetime2(7) NOT NULL,
	RestrikeVoltage float NOT NULL,
	RestrikeCurrentPeak float NOT NULL,
	RestrikeVoltageDip float NOT NULL,
	TransientPeakSample int NOT NULL,
	TransientPeakTime datetime2(7) NOT NULL,
	TransientPeakVoltage float NOT NULL,
	PerUnitTransientPeakVoltage float NOT NULL,
	FinalExtinguishSample int NOT NULL,
	FinalExtinguishTime datetime2(7) NOT NULL,
	FinalExtinguishVoltage float NOT NULL,
	I2t float NOT NULL,
)
GO

CREATE NONCLUSTERED INDEX IX_BreakerRestrike_EventID
ON BreakerRestrike(EventID ASC)
GO

CREATE TABLE VoltageEnvelope
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE VoltageCurve
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE VoltageCurvePoint
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    VoltageCurveID INT NOT NULL REFERENCES VoltageCurve(ID),
    PerUnitMagnitude FLOAT NOT NULL,
    DurationSeconds FLOAT NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE WorkbenchVoltageCurve
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Visible BIT NOT NULL
)
GO

CREATE TABLE WorkbenchVoltageCurvePoint
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    VoltageCurveID INT NOT NULL REFERENCES WorkbenchVoltageCurve(ID),
    PerUnitMagnitude FLOAT NOT NULL,
    DurationSeconds FLOAT NOT NULL,
    LoadOrder INT NOT NULL
)
GO


CREATE TABLE VoltageEnvelopeCurve
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    VoltageEnvelopeID INT NOT NULL REFERENCES VoltageEnvelope(ID),
    VoltageCurveID INT NOT NULL REFERENCES VoltageCurve(ID)
)
GO

CREATE TABLE WorkbenchFilter
(
    ID INT IDENTITY(1,1) NOT NULL,
    Name VARCHAR(50) NOT NULL,
    UserID UNIQUEIDENTIFIER NOT NULL,
    TimeRange VARCHAR(512) NOT NULL,
    Meters VARCHAR(MAX) NULL,
    Lines VARCHAR(MAX) NULL,
    EventTypes VARCHAR(50) NOT NULL,
    IsDefault BIT NOT NULL
)
GO

CREATE TABLE DisturbanceSeverity
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    VoltageEnvelopeID INT NOT NULL REFERENCES VoltageEnvelope(ID),
    DisturbanceID INT NOT NULL REFERENCES Disturbance(ID),
    SeverityCode INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_DisturbanceSeverity_VoltageEnvelopeID
ON DisturbanceSeverity(VoltageEnvelopeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DisturbanceSeverity_DisturbanceID
ON DisturbanceSeverity(DisturbanceID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DisturbanceSeverity_SeverityCode
ON DisturbanceSeverity(SeverityCode ASC)
GO

CREATE TABLE BreakerOperationType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE BreakerOperation
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    BreakerOperationTypeID INT NOT NULL REFERENCES BreakerOperationType(ID),
    BreakerNumber VARCHAR(120) NOT NULL,
    TripCoilEnergized DATETIME2 NOT NULL,
    StatusBitSet DATETIME2 NOT NULL,
    StatusBitChatter INT NOT NULL,
    APhaseCleared DATETIME2 NOT NULL,
    BPhaseCleared DATETIME2 NOT NULL,
    CPhaseCleared DATETIME2 NOT NULL,
    BreakerTiming FLOAT NOT NULL,
    StatusTiming FLOAT NOT NULL,
    APhaseBreakerTiming FLOAT NOT NULL,
    BPhaseBreakerTiming FLOAT NOT NULL,
    CPhaseBreakerTiming FLOAT NOT NULL,
    DcOffsetDetected INT NOT NULL,
    BreakerSpeed FLOAT NOT NULL,
    UpdatedBy VARCHAR(50) NULL
)
GO

CREATE NONCLUSTERED INDEX IX_BreakerOperation_EventID
ON BreakerOperation(EventID ASC)
GO

CREATE TABLE EventSentEmail
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    SentEmailID INT NOT NULL REFERENCES SentEmail(ID)
)
GO

CREATE NONCLUSTERED INDEX IX_EventSentEmail_EventID
ON EventSentEmail(ID ASC)
GO

CREATE TABLE SnapshotHarmonics
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    SpectralData varchar(max) NULL
)
GO

CREATE NONCLUSTERED INDEX IX_SnapshotHarmonics_EventID
ON SnapshotHarmonics(EventID ASC)
GO

INSERT INTO EventType(Name, Description) VALUES ('Fault', 'Fault')
GO

INSERT INTO EventType(Name, Description) VALUES ('RecloseIntoFault', 'Reclose Into Fault')
GO

INSERT INTO EventType(Name, Description) VALUES ('BreakerOpen', 'Breaker Opening - Nonfault')
GO

INSERT INTO EventType(Name, Description) VALUES ('Interruption', 'Interruption')
GO

INSERT INTO EventType(Name, Description) VALUES ('Sag', 'Sag')
GO

INSERT INTO EventType(Name, Description) VALUES ('Swell', 'Swell')
GO

INSERT INTO EventType(Name, Description) VALUES ('Transient', 'Transient')
GO

INSERT INTO EventType(Name, Description) VALUES ('Other', 'Other')
GO

INSERT INTO EventType(Name, Description) VALUES ('Test', 'Test')
GO

INSERT INTO EventType(Name, Description) VALUES ('Snapshot', 'Snapshot')
GO

INSERT INTO VoltageEnvelope(Name, Description) VALUES ('ITIC', 'ITI (CBEMA) Power Acceptability Curves - Tolerance curves for 120 V computer equipment')
GO

INSERT INTO VoltageCurve(Name) VALUES ('ITIC Upper')
GO

INSERT INTO VoltageCurve(Name) VALUES ('ITIC Lower')
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (1, 2.0, 0.001, 1)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (1, 1.4, 0.003, 2)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (1, 1.2, 0.003, 3)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (1, 1.2, 0.5, 4)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (1, 1.1, 0.5, 5)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (2, 0.0, 0.02, 1)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (2, 0.7, 0.02, 2)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (2, 0.7, 0.5, 3)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (2, 0.8, 0.5, 4)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (2, 0.8, 10.0, 5)
GO

INSERT INTO VoltageCurvePoint(VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES (2, 0.9, 10.0, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('ITIC Upper', 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 2, 0.001, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 1.4, 0.003, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 1.2, 0.003, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 1.2, 0.5, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 1.1, 0.5, 5)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 5, 0.0001667, 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(1, 1.1, 1000000, 6)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('ITIC Lower', 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0, 0.02, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0.7, 0.02, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0.7, 0.5, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0.8, 0.5, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0.8, 10, 5)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0.9, 10, 6)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(2, 0.9, 1000000, 7)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('SEMI', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0, 0.02, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.5, 0.02, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.5, 0.2, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.7, 0.2, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.7, 0.5, 5)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.8, 0.5, 6)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.8, 10, 7)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.9, 10, 8)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(3, 0.9, 1000000, 9)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1668 Recommended Type I & II', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.5, 0.01, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.5, 0.2, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.7, 0.2, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.7, 0.5, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.8, 0.5, 5)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.8, 2, 6)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 1, 2, 7)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 1, 0.01, 8)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(4, 0.5, 0.01, 9)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1668 Recommended Type III', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.5, 0.01, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.5, 0.05, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.7, 0.05, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.7, 0.1, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.8, 0.1, 5)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.8, 2, 6)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 1, 2, 7)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 1, 0.01, 8)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(5, 0.5, 0.01, 9)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 1.0 Transients', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(6, 0, 1E-06, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(6, 0, 0.01, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(6, 5, 0.01, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(6, 5, 1E-06, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(6, 0, 1E-06, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.1.1 Instantaneous Sag', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(7, 0.1, 0.01, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(7, 0.1, 0.5, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(7, 0.9, 0.5, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(7, 0.9, 0.01, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(7, 0.1, 0.01, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.1.2 Instantaneous Swell', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(8, 1.1, 0.01, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(8, 1.1, 0.5, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(8, 1.8, 0.5, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(8, 1.8, 0.01, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(8, 1.1, 0.01, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.2.1 Mom. Interruption', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(9, 0, 0.01, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(9, 0, 3, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(9, 0.1, 3, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(9, 0.1, 0.01, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(9, 0, 0.01, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.2.2 Momentary Sag', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(10, 0.1, 0.5, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(10, 0.1, 3, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(10, 0.9, 3, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(10, 0.9, 0.5, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(10, 0.1, 0.5, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.2.3 Momentary Swell',0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(11, 1.1, 0.5, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(11, 1.1, 3, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(11, 1.4, 3, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(11, 1.4, 0.5, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(11, 1.1, 0.5, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.3.1 Temp. Interruption', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(12, 0, 3, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(12, 0, 60, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(12, 0.1, 60, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(12, 0.1, 3, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(12, 0, 3, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.3.2 Temporary Sag', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(13, 0.1, 3, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(13, 0.1, 60, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(13, 0.9, 60, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(13, 0.9, 3, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(13, 0.1, 3, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 2.3.3 Temporary Swell', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(14, 1.1, 3, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(14, 1.1, 60, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(14, 1.2, 60, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(14, 1.2, 3, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(14, 1.1, 3, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 3.1 Sustained Int.', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(15, 0, 60, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(15, 0, 1000000, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(15, 0.1, 1000000, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(15, 0.1, 60, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(15, 0, 60, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 3.2 Undervoltage', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(16, 0.8, 60, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(16, 0.8, 1000000, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(16, 0.9, 1000000, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(16, 0.9, 60, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(16, 0.8, 60, 5)
GO


INSERT WorkbenchVoltageCurve(Name, Visible) VALUES('IEEE 1159 3.3 Overvoltage', 0)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(17, 1.1, 60, 1)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(17, 1.1, 1000000, 2)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(17, 1.2, 1000000, 3)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(17, 1.2, 60, 4)
GO

INSERT WorkbenchVoltageCurvePoint (VoltageCurveID, PerUnitMagnitude, DurationSeconds, LoadOrder) VALUES(17, 1.1, 60, 5)
GO


INSERT INTO VoltageEnvelopeCurve(VoltageEnvelopeID, VoltageCurveID) VALUES(1, 1)
GO

INSERT INTO VoltageEnvelopeCurve(VoltageEnvelopeID, VoltageCurveID) VALUES(1, 2)
GO

INSERT INTO BreakerOperationType(Name, Description) VALUES('Normal', 'Breaker operated normally')
GO

INSERT INTO BreakerOperationType(Name, Description) VALUES('Late', 'Breaker operated slowly')
GO

INSERT INTO BreakerOperationType(Name, Description) VALUES('Indeterminate', 'Breaker operation type could not be determined')
GO

-- -------------- --
-- Fault Location --
-- -------------- --

CREATE TABLE OutputChannel
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    SeriesID INT NOT NULL REFERENCES Series(ID),
    ChannelKey VARCHAR(20) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE SourceImpedance
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetLocationID INT NOT NULL REFERENCES AssetLocation(ID),
    RSrc FLOAT NOT NULL,
    XSrc FLOAT NOT NULL
)
GO

CREATE TABLE LineImpedance
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LineID INT NOT NULL UNIQUE REFERENCES Asset(ID),
    R0 FLOAT NOT NULL,
    X0 FLOAT NOT NULL,
    R1 FLOAT NOT NULL,
    X1 FLOAT NOT NULL
)
GO


CREATE TABLE SegmentType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE FaultSegment
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    SegmentTypeID INT NOT NULL REFERENCES SegmentType(ID),
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    StartSample INT NOT NULL,
    EndSample INT NOT NULL
)
GO

CREATE TABLE FaultDetectionLogic
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterAssetID INT NOT NULL REFERENCES MeterAsset(ID),
    Expression VARCHAR(500) NOT NULL
)
GO

CREATE TABLE FaultLocationAlgorithm
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(1024) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    MethodName VARCHAR(80) NOT NULL,
    ExecutionOrder INT NOT NULL
)
GO

-- FaultCurve references the IDs in the Event table,
-- but no foreign key constraint is defined.
-- If it was defined, the records in this
-- table would need to be deleted before we
-- could delete records in the referenced table.
CREATE TABLE FaultCurve
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL,
    Algorithm VARCHAR(80) NOT NULL,
    Data VARBINARY(MAX) NOT NULL,
	AngleData VARBINARY(MAX) NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FaultCurve_EventID
ON FaultCurve(EventID ASC)
GO

CREATE TABLE FaultCurveStatistic
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FaultCurveID INT NOT NULL REFERENCES FaultCurve(ID),
    FaultNumber INT NOT NULL,
    Maximum FLOAT NOT NULL,
    Minimum FLOAT NOT NULL,
    Average FLOAT NOT NULL,
    StandardDeviation FLOAT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FaultCurveStatistic_FaultCurveID
ON FaultCurveStatistic(FaultCurveID ASC)
GO

CREATE TABLE FaultGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    FaultDetectionLogicResult INT NULL,
    DefaultFaultDetectionLogicResult INT NOT NULL,
    FaultValidationLogicResult INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FaultGroup_EventID
ON FaultGroup(EventID ASC)
GO

CREATE TABLE FaultSummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    Algorithm VARCHAR(80) NOT NULL,
    FaultNumber INT NOT NULL,
    CalculationCycle INT NOT NULL,
    Distance FLOAT NOT NULL,
    CurrentMagnitude FLOAT NOT NULL,
    CurrentLag FLOAT NOT NULL,
    PrefaultCurrent FLOAT NOT NULL,
    PostfaultCurrent FLOAT NOT NULL,
    ReactanceRatio FLOAT NOT NULL,
    Inception DATETIME2 NOT NULL,
    DurationSeconds FLOAT NOT NULL,
    DurationCycles FLOAT NOT NULL,
    FaultType VARCHAR(200) NOT NULL,
    IsSelectedAlgorithm INT NOT NULL,
    IsValid INT NOT NULL,
    IsSuppressed INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FaultSummary_EventID
ON FaultSummary(EventID ASC)
GO

CREATE NONCLUSTERED INDEX IX_FaultSummary_Inception
ON FaultSummary(Inception ASC)
GO

CREATE TABLE FaultCauseMetrics
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    FaultNumber INT NOT NULL,
    TreeFaultResistance FLOAT NULL,
    LightningMilliseconds FLOAT NULL,
    InceptionDistanceFromPeak FLOAT NULL,
    PrefaultThirdHarmonic FLOAT NULL,
    GroundCurrentRatio FLOAT NULL,
    LowPrefaultCurrentRatio FLOAT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_FaultCauseMetrics_EventID
ON FaultCauseMetrics(EventID ASC)
GO

CREATE TABLE NearestStructure
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FaultSummaryID INT NOT NULL REFERENCES FaultSummary(ID),
    StructureID INT NOT NULL REFERENCES Structure(ID),
    Deviation FLOAT NULL
)
GO

CREATE TABLE DoubleEndedFaultDistance
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LocalFaultSummaryID INT NOT NULL UNIQUE REFERENCES FaultSummary(ID),
    RemoteFaultSummaryID INT NOT NULL UNIQUE REFERENCES FaultSummary(ID),
    Distance FLOAT NOT NULL,
    Angle FLOAT NOT NULL,
    IsValid INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_DoubleEndedFaultDistance_LocalFaultSummaryID
ON DoubleEndedFaultDistance(LocalFaultSummaryID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DoubleEndedFaultDistance_RemoteFaultSummaryID
ON DoubleEndedFaultDistance(RemoteFaultSummaryID ASC)
GO

INSERT INTO FaultLocationAlgorithm(AssemblyName, TypeName, MethodName, ExecutionOrder) VALUES('FaultAlgorithms.dll', 'FaultAlgorithms.FaultLocationAlgorithms', 'Simple', 1)
GO

INSERT INTO FaultLocationAlgorithm(AssemblyName, TypeName, MethodName, ExecutionOrder) VALUES('FaultAlgorithms.dll', 'FaultAlgorithms.FaultLocationAlgorithms', 'Reactance', 2)
GO

INSERT INTO FaultLocationAlgorithm(AssemblyName, TypeName, MethodName, ExecutionOrder) VALUES('FaultAlgorithms.dll', 'FaultAlgorithms.FaultLocationAlgorithms', 'Takagi', 3)
GO

INSERT INTO FaultLocationAlgorithm(AssemblyName, TypeName, MethodName, ExecutionOrder) VALUES('FaultAlgorithms.dll', 'FaultAlgorithms.FaultLocationAlgorithms', 'ModifiedTakagi', 4)
GO

INSERT INTO FaultLocationAlgorithm(AssemblyName, TypeName, MethodName, ExecutionOrder) VALUES('FaultAlgorithms.dll', 'FaultAlgorithms.FaultLocationAlgorithms', 'Novosel', 5)
GO

INSERT INTO SegmentType(Name, Description) VALUES('Prefault', 'Before fault inception')
GO

INSERT INTO SegmentType(Name, Description) VALUES('AN Fault', 'Line A to neutral fault')
GO

INSERT INTO SegmentType(Name, Description) VALUES('BN Fault', 'Line B to neutral fault')
GO

INSERT INTO SegmentType(Name, Description) VALUES('CN Fault', 'Line C to neutral fault')
GO

INSERT INTO SegmentType(Name, Description) VALUES('AB Fault', 'Line A to line B fault')
GO

INSERT INTO SegmentType(Name, Description) VALUES('BC Fault', 'Line B to line C fault')
GO

INSERT INTO SegmentType(Name, Description) VALUES('CA Fault', 'Line C to line A fault')
GO

INSERT INTO SegmentType(Name, Description) VALUES('ABG Fault', 'Line A to line B fault with ground')
GO

INSERT INTO SegmentType(Name, Description) VALUES('BCG Fault', 'Line B to line C fault with ground')
GO

INSERT INTO SegmentType(Name, Description) VALUES('CAG Fault', 'Line C to line A fault with ground')
GO

INSERT INTO SegmentType(Name, Description) VALUES('3-Phase Fault', 'Fault on all three lines')
GO

INSERT INTO SegmentType(Name, Description) VALUES('Postfault', 'After the fault ends')
GO

CREATE TABLE LightningStrike(
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	EventID int NOT NULL FOREIGN KEY REFERENCES Event(ID),
	Service varchar(50) NOT NULL,
	UTCTime datetime2(7) NOT NULL,
	DisplayTime varchar(50) NOT NULL,
	Amplitude float NOT NULL,
	Latitude float NOT NULL,
	Longitude float NOT NULL
)

GO


-- -------- --
-- Trending --
-- -------- --

CREATE TABLE HourlyTrendingSummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    Time DATETIME NOT NULL,
    Minimum FLOAT NOT NULL,
    Maximum FLOAT NOT NULL,
    Average FLOAT NOT NULL,
    ValidCount INT NOT NULL,
    InvalidCount INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_HourlyTrendingSummary_ChannelID
ON HourlyTrendingSummary(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_HourlyTrendingSummary_Time
ON HourlyTrendingSummary(Time ASC)
GO

CREATE TABLE DailyTrendingSummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    Date Date NOT NULL,
    Minimum FLOAT NOT NULL,
    Maximum FLOAT NOT NULL,
    Average FLOAT NOT NULL,
    ValidCount INT NOT NULL,
    InvalidCount INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_DailyTrendingSummary_ChannelID
ON DailyTrendingSummary(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DailyTrendingSummary_Date
ON DailyTrendingSummary(Date ASC)
GO

CREATE TABLE ChannelNormal
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    Average FLOAT NOT NULL,
    MeanSquare FLOAT NOT NULL,
    StandardDeviation FLOAT NOT NULL,
    Count INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_ChannelNormal_ChannelID
ON ChannelNormal(ChannelID ASC)
GO

-- ------------ --
-- Data Quality --
-- ------------ --

CREATE TABLE DefaultDataQualityRangeLimit
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
    High FLOAT NULL,
    Low FLOAT NULL,
    RangeInclusive INT NOT NULL DEFAULT 0,
    PerUnit INT NOT NULL DEFAULT 0
)
GO

CREATE NONCLUSTERED INDEX IX_DefaultDataQualityRangeLimit_MeasurementTypeID
ON DefaultDataQualityRangeLimit(MeasurementTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DefaultDataQualityRangeLimit_MeasurementCharacteristicID
ON DefaultDataQualityRangeLimit(MeasurementCharacteristicID ASC)
GO

CREATE TABLE DataQualityRangeLimit
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    High FLOAT NULL,
    Low FLOAT NULL,
    RangeInclusive INT NOT NULL DEFAULT 0,
    PerUnit INT NOT NULL DEFAULT 0,
    Enabled INT NOT NULL DEFAULT 1
)
GO

CREATE NONCLUSTERED INDEX IX_DataQualityRangeLimit_ChannelID
ON DataQualityRangeLimit(ChannelID ASC)
GO

CREATE TABLE MeterDataQualitySummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    Date DATE NOT NULL,
    ExpectedPoints INT NOT NULL,
    GoodPoints INT NOT NULL,
    LatchedPoints INT NOT NULL,
    UnreasonablePoints INT NOT NULL,
    NoncongruentPoints INT NOT NULL,
    DuplicatePoints INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_MeterDataQualitySummary_MeterID
ON MeterDataQualitySummary(MeterID ASC)
GO

CREATE NONCLUSTERED INDEX IX_MeterDataQualitySummary_Date
ON MeterDataQualitySummary(Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_MeterDataQualitySummary_MeterID_Date
ON MeterDataQualitySummary(MeterID ASC, Date ASC)
GO

CREATE TABLE ChannelDataQualitySummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    Date DATE NOT NULL,
    ExpectedPoints INT NOT NULL,
    GoodPoints INT NOT NULL,
    LatchedPoints INT NOT NULL,
    UnreasonablePoints INT NOT NULL,
    NoncongruentPoints INT NOT NULL,
    DuplicatePoints INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_ChannelDataQualitySummary_ChannelID
ON ChannelDataQualitySummary(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_ChannelDataQualitySummary_Date
ON ChannelDataQualitySummary(Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_ChannelDataQualitySummary_ChannelID_Date
ON ChannelDataQualitySummary(ChannelID ASC, Date ASC)
GO

CREATE TABLE Unit
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE PQMeasurement
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(MAX) NOT NULL,
    Description VARCHAR(MAX) NULL,
    UnitID INT NOT NULL REFERENCES Unit(ID),
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    HarmonicGroup INT NOT NULL DEFAULT 0,
    Enabled bit NOT NULL DEFAULT 1
)
GO

CREATE TABLE PQTrendStat
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    PQMeasurementTypeID INT NOT NULL REFERENCES PQMeasurement(ID),
    Date DATE NOT NULL,
    Max FLOAT NULL,
    CP99 FLOAT NULL,
    CP95 FLOAT NULL,
    Avg FLOAT NULL,
    CP05 FLOAT NULL,
    CP01 FLOAT NULL,
    Min FLOAT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_PQTrendStat_Date
ON PQTrendStat(Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_PQTrendStat_MeterID_Date
ON PQTrendStat(MeterID ASC, Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_PQTrendStat_MeterID_PQMeasurementTypeID_Date
ON PQTrendStat(Date DESC, MeterID ASC, PQMeasurementTypeID ASC)
GO


INSERT INTO Unit (Name) VALUES ('Volts')
GO

INSERT INTO Unit (Name) VALUES ('Amps')
GO

INSERT INTO Unit (Name) VALUES ('KW')
GO

INSERT INTO Unit (Name) VALUES ('KVAR')
GO

INSERT INTO Unit (Name) VALUES ('KVA')
GO

INSERT INTO Unit (Name) VALUES ('Per Unit')
GO

INSERT INTO Unit (Name) VALUES ('Percent')
GO

CREATE TABLE StepChangeMeasurement(
    ID INT PRIMARY KEY IDENTITY(1,1),
    PQMeasurementID INT FOREIGN KEY REFERENCES PQMeasurement(ID) NOT NULL,
    Setting FLOAT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_StepChangeMeasurement_PQMeasurement
ON StepChangeMeasurement(PQMeasurementID ASC)
GO

CREATE TABLE StepChangeStat(
    ID INT PRIMARY KEY IDENTITY(1,1),
    MeterID INT FOREIGN KEY REFERENCES Meter(ID) NOT NULL,
    Date Date NOT NULL,
    StepChangeMeasurementID INT FOREIGN KEY REFERENCES StepChangeMeasurement(ID) NOT NULL,
    Value FLOAT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_StepChangeStat_Date
ON StepChangeStat(Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_StepChangeStat_MeterID_Date
ON StepChangeStat(MeterID ASC, Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_StepChangeStat_MeterID_StepChangeMeasurementID_Date
ON StepChangeStat(MeterID ASC,StepChangeMeasurementID ASC, Date ASC)
GO


-- ------ --
-- Alarms --
-- ------ --

CREATE TABLE AlarmType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE AlarmSeverity
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Color VARCHAR(10) NOT NULL
)
GO

CREATE TABLE AlarmGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    SeverityID INT NOT NULL REFERENCES AlarmSeverity(ID)
)
GO

CREATE TABLE Alarm
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AlarmGroupID  INT NOT NULL REFERENCES AlarmGroup(ID),
    SeriesID INT NOT NULL REFERENCES Series(ID),
    [Manual] Bit NOT NULL DEFAULT(0)
    
)
GO

CREATE TABLE AlarmFactor
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AlarmGroupID  INT NOT NULL REFERENCES AlarmGroup(ID),
    Factor FLOAT NOT NULL DEFAULT(1.0),
    SeverityID INT NOT NULL REFERENCES AlarmSeverity(ID)
)
GO

CREATE TABLE AlarmDay
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name  VARCHAR(25)
)
GO

CREATE TABLE AlarmValue
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AlarmID  INT NOT NULL REFERENCES Alarm(ID),
    AlarmDayID  INT NULL REFERENCES AlarmDay(ID),
    StartHour INT NOT NULL,
    EndHour INT NULL,
    Value FLOAT NOT NULL,
    Formula VARCHAR(MAX) NOT NULL
)
GO


CREATE TABLE AlarmLog
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AlarmID  INT NOT NULL REFERENCES Alarm(ID),
    AlarmFactorID  INT NULL REFERENCES AlarmFactor(ID),
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NULL,
)
GO

CREATE TABLE ALarmDayGroup (
	ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	Description  VARCHAR(50)
)
GO

CREATE TABLE ALarmDayGroupAlarmDay (
	ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	AlarmDayID INT NULL References AlarmDay(ID),
	AlarmDayGroupID INT NOT NULL References AlarmDayGroup(ID)
)
GO



-- Views for UI --
CREATE VIEW AlarmGroupView AS 
SELECT 
	AlarmGroup.ID,
	AlarmGroup.Name,
	AlarmSeverity.Name as AlarmSeverity,
	COUNT(DISTINCT Channel.ID) as Channels,
	COUNT(DISTINCT Channel.MeterID) as Meters,
	MAX(AlarmLog.StartTime) AS LastAlarmStart,
	(SELECT AL2.EndTime 
		FROM AlarmLog AL2 
		WHERE AL2.StartTime = MAX(AlarmLog.StartTime) AND AL2.AlarmID IN (SELECT ID FROM ALARM AL WHERE AL.AlarmGroupID = Alarmgroup.ID) AND AL2.AlarmFactorID IS NULL
		) AS LastAlarmEnd,
	(SELECT CH.Name 
		FROM AlarmLog AL2 LEFT JOIN Channel CH ON AL2.AlarmID IN (SELECT A.ID FROM Alarm A LEFT JOIN Series S ON A.SeriesID = S.ID WHERE S.ChannelID = CH.ID)
		WHERE AL2.StartTime = MAX(AlarmLog.StartTime) AND AL2.AlarmID IN (SELECT ID FROM ALARM AL WHERE AL.AlarmGroupID = Alarmgroup.ID) AND AL2.AlarmFactorID IS NULL
		) AS LastChannel,
	(SELECT M.Name 
		FROM AlarmLog AL2 LEFT JOIN Channel CH ON AL2.AlarmID IN (SELECT A.ID FROM Alarm A LEFT JOIN Series S ON A.SeriesID = S.ID WHERE S.ChannelID = CH.ID)
			LEFT JOIN Meter M ON M.ID = Ch.MeterID
		WHERE AL2.StartTime = MAX(AlarmLog.StartTime) AND AL2.AlarmID IN (SELECT ID FROM ALARM AL WHERE AL.AlarmGroupID = Alarmgroup.ID) AND AL2.AlarmFactorID IS NULL
		) AS LastMeter,
	AlarmType.Description AS AlarmType
	 
FROM 
	AlarmGroup LEFT JOIN
	Alarm ON Alarm.AlarmGroupID = AlarmGroup.ID LEFT JOIN
	AlarmType ON AlarmGroup.AlarmTypeID = AlarmType.ID LEFT JOIN
	Series ON Alarm.SeriesID = Series.ID LEFT JOIN
	Channel ON Series.ChannelID = Channel.ID LEFT JOIN 
	AlarmSeverity ON AlarmGroup.SeverityID = AlarmSeverity.ID LEFT JOIN
	AlarmLog ON AlarmLog.AlarmID = Alarm.ID
WHERE AlarmLog.AlarmFactorID IS NULL
GROUP BY
	AlarmGroup.ID,
	AlarmGroup.Name,
	AlarmSeverity.Name,
	AlarmType.Description
GO

CREATE VIEW ChannelOverviewView AS
SELECT
	Channel.ID,
	Meter.Name as Meter,
	Channel.Name as Channel,
	MeasurementType.Name + ' ' + MeasurementCharacteristic.Name as Type,
	Phase.Name as Phase,
	Asset.AssetName as Asset
FROM
	Channel JOIN
	Meter ON Meter.ID = Channel.MeterID JOIN
	Asset ON Asset.ID = Channel.AssetID JOIN
	Phase ON Phase.ID = Channel.PhaseID JOIN
	MeasurementCharacteristic ON MeasurementCharacteristic.ID = Channel.MeasurementCharacteristicID JOIN
	MeasurementType ON MeasurementType.ID = Channel.MeasurementTypeID
GO

CREATE VIEW ChannelAlarmGroupView AS
SELECT 
	AlarmGroup.ID,
	AlarmGroup.Name,
	AlarmSeverity.ID as AlarmSeverityID,
	AlarmSeverity.Name as AlarmSeverity,
	Channel.ID as ChannelID,
	Meter.ID as MeterID,
	'N/A' as TimeInAlarm
	 
FROM 
	Alarm LEFT JOIN
	AlarmGroup ON Alarm.AlarmGroupID = AlarmGroup.ID LEFT JOIN
	Series ON Alarm.SeriesID = Series.ID LEFT JOIN
	Channel ON Series.ChannelID = Channel.ID LEFT JOIN 
	Meter ON Channel.MeterID = Meter.ID LEFT JOIN
	AlarmSeverity ON AlarmGroup.SeverityID = AlarmSeverity.ID
GO

CREATE VIEW MeterAlarmGroupView AS
SELECT 
	AlarmGroup.ID,
	AlarmGroup.Name,
	AlarmSeverity.Name as AlarmSeverity,
	Meter.ID as MeterID,
	COUNT(Channel.ID) AS Channel,
	'N/A' as TimeInAlarm
	 
FROM 
	Alarm LEFT JOIN
	AlarmGroup ON Alarm.AlarmGroupID = AlarmGroup.ID LEFT JOIN
	Series ON Alarm.SeriesID = Series.ID LEFT JOIN
	Channel ON Series.ChannelID = Channel.ID LEFT JOIN 
	Meter ON Channel.MeterID = Meter.ID LEFT JOIN
	AlarmSeverity ON AlarmGroup.SeverityID = AlarmSeverity.ID
GROUP BY
	AlarmGroup.ID,
	AlarmGroup.Name, AlarmSeverity.Name,
	Meter.ID
GO

CREATE VIEW AlarmDayGroupView AS SELECT
	AlarmDayGroup.ID,
	Description,
	AlarmDayID
	FROM
	ALarmDayGroupAlarmDay LEFT JOIN AlarmDayGroup ON ALarmDayGroupAlarmDay.AlarmDayGroupID = AlarmDayGroup.ID
GO

CREATE VIEW ActiveAlarmView AS SELECT 
	Alarm.ID AS AlarmID,
	Alarm.AlarmGroupID AS AlarmGroupID,
	AlarmGroup.AlarmTypeID AS AlarmTypeID,
	AlarmFactor.ID as AlarmFactorID,
	Alarm.SeriesID AS SeriesID,
	AlarmFactor.Factor AS Value
FROM
(SELECT ID, Factor, AlarmGroupID FROM Alarmfactor UNION SELECT 0 AS ID, 1.0 as Factor, AlarmGroup.ID AS AlarmGroupID FROM AlarmGroup) AlarmFactor LEFT JOIN
    Alarm ON AlarmFactor.AlarmGroupID = alarm.AlarmGroupID LEFT JOIN
    AlarmGroup ON Alarm.AlarmGroupID = AlarmGroup.ID
GO
-- Defaults --

INSERT INTO AlarmType(Name, Description) VALUES ('Upper Limit', 'Triggered when setpoint is exceeded')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Lower Limit', 'Triggered when value is below setpoint')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('In Range', 'Triggered when value is oustide specified Range')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Out of Range', 'Triggered when Value is within specified Range')
GO

INSERT INTO AlarmSeverity(Name, Color) VALUES ('Severe', '#ED1C16')
GO
INSERT INTO AlarmSeverity(Name, Color) VALUES ('Alert', '#F65314')
GO
INSERT INTO AlarmSeverity(Name, Color) VALUES ('Warning', '#FFBB00')
GO
INSERT INTO AlarmSeverity(Name, Color) VALUES ('Info', '#3B5998')
GO

INSERT INTO AlarmDay(Name) VALUES
('Weekday'),
('Weekend'),
('Monday'),
('Tuesday'),
('Wednesday'),
('Thursday'),
('Friday'),
('Saturday'),
('Sunday')
GO

INSERT INTO AlarmDayGroup(Description) VALUES
('Daily'),
('Weekly'),
('Workday/Weekend')
GO

INSERT INTO AlarmDayGroupAlarmDay (AlarmDayID, AlarmDayGroupID) VALUES
(NULL, (SELECT ID FROM AlarmDayGroup WHERE Description = 'Daily')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Weekend'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Workday/Weekend')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Weekday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Workday/Weekend')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Monday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Tuesday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Wednesday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Thursday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Friday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Saturday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly')), 
((SELECT ID FROM AlarmDay WHERE Name = 'Sunday'), (SELECT ID FROM AlarmDayGroup WHERE Description = 'Weekly'))
GO
-- Old Alarm Structure (pre SPC Tool) --
CREATE TABLE DefaultAlarmRangeLimit
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    Severity INT NOT NULL DEFAULT 0,
    High FLOAT NULL,
    Low FLOAT NULL,
    RangeInclusive INT NOT NULL DEFAULT 0,
    PerUnit INT NOT NULL DEFAULT 0
)
GO

CREATE NONCLUSTERED INDEX IX_DefaultAlarmRangeLimit_MeasurementTypeID
ON DefaultAlarmRangeLimit(MeasurementTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DefaultAlarmRangeLimit_MeasurementCharacteristicID
ON DefaultAlarmRangeLimit(MeasurementCharacteristicID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DefaultAlarmRangeLimit_AlarmTypeID
ON DefaultAlarmRangeLimit(AlarmTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_DefaultAlarmRangeLimit_Severity
ON DefaultAlarmRangeLimit(Severity ASC)
GO

CREATE TABLE AlarmRangeLimit
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    Severity INT NOT NULL DEFAULT 0,
    High FLOAT NULL,
    Low FLOAT NULL,
    RangeInclusive INT NOT NULL DEFAULT 0,
    PerUnit INT NOT NULL DEFAULT 0,
    IsDefault INT NOT NULL DEFAULT 1,
    Enabled INT NOT NULL DEFAULT 1
)
GO

CREATE NONCLUSTERED INDEX IX_AlarmRangeLimit_ChannelID
ON AlarmRangeLimit(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_AlarmRangeLimit_AlarmTypeID
ON AlarmRangeLimit(AlarmTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_AlarmRangeLimit_Severity
ON AlarmRangeLimit(Severity ASC)
GO

CREATE TABLE HourOfWeekLimit
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    HourOfWeek INT NOT NULL,
    Severity INT NOT NULL,
    High FLOAT NOT NULL,
    Low FLOAT NOT NULL,
    Enabled INT NOT NULL DEFAULT 1
)
GO

CREATE NONCLUSTERED INDEX IX_HourOfWeekLimit_ChannelID
ON HourOfWeekLimit(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_HourOfWeekLimit_AlarmTypeID
ON HourOfWeekLimit(AlarmTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_HourOfWeekLimit_HourOfWeek
ON HourOfWeekLimit(HourOfWeek ASC)
GO

CREATE NONCLUSTERED INDEX IX_HourOfWeekLimit_Severity
ON HourOfWeekLimit(Severity ASC)
GO

CREATE TABLE MeterAlarmSummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    Date DATE NOT NULL,
    AlarmPoints INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_MeterAlarmSummary_MeterID
ON MeterAlarmSummary(MeterID ASC)
GO

CREATE NONCLUSTERED INDEX IX_MeterAlarmSummary_AlarmTypeID
ON MeterAlarmSummary(AlarmTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_MeterAlarmSummary_Date
ON MeterAlarmSummary(Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_MeterAlarmSummary_MeterID_Date
ON MeterAlarmSummary(MeterID ASC, Date ASC)
GO

CREATE TABLE ChannelAlarmSummary
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    Date DATE NOT NULL,
    AlarmPoints INT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_ChannelAlarmSummary_ChannelID
ON ChannelAlarmSummary(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_ChannelAlarmSummary_AlarmTypeID
ON ChannelAlarmSummary(AlarmTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_ChannelAlarmSummary_Date
ON ChannelAlarmSummary(Date ASC)
GO

CREATE NONCLUSTERED INDEX IX_ChannelAlarmSummary_ChannelID_Date
ON ChannelAlarmSummary(ChannelID ASC, Date ASC)
GO



/* ----End Alarm Structure ---- */

CREATE TABLE FaultNote
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FaultSummaryID INT NOT NULL,
    Note VARCHAR(MAX) NOT NULL,
    UserAccountID UNIQUEIDENTIFIER NOT NULL,
    Timestamp DATETIME NOT NULL
)
GO

ALTER TABLE FaultNote WITH CHECK ADD FOREIGN KEY(FaultSummaryID)
REFERENCES FaultSummary(ID)
GO

ALTER TABLE FaultNote WITH CHECK ADD FOREIGN KEY(UserAccountID)
REFERENCES UserAccount(ID)
GO

CREATE NONCLUSTERED INDEX IX_FaultNote_FaultSummaryID
ON FaultNote(FaultSummaryID ASC)
GO

CREATE NONCLUSTERED INDEX IX_FaultNote_UserAccountID
ON FaultNote(UserAccountID ASC)
GO

CREATE TABLE NoteType(
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	Name varchar(max) not null,
	ReferenceTableName varchar(max) not null,
)
GO

INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('Meter', 'Meter')
GO
INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('Event', 'Event')
GO
INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('Asset', 'Asset')
GO
INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('Location', 'Location')
GO
INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('Customer', 'Customer')
GO
INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('User', 'UserAccount')
GO
INSERT INTO NoteType (Name, ReferenceTableName) VALUES ('Company', 'Company')
GO


CREATE TABLE Note (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	NoteTypeID int Not NULL REFERENCES NoteType(ID),
	ReferenceTableID INT NOT NULL,
    Note VARCHAR(MAX) NOT NULL,
    UserAccount VARCHAR(MAX) NOT NULL DEFAULT SUSER_NAME(),
    Timestamp DATETIME NOT NULL DEFAULT GETUTCDATE(),
)
GO

CREATE NONCLUSTERED INDEX IX_Note_NoteTypeID_ReferenceTableID  
ON Note(NoteTypeID, ReferenceTableID)
GO

CREATE TABLE EventNote
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL,
    Note VARCHAR(MAX) NOT NULL,
    UserAccount VARCHAR(MAX) NOT NULL,
    Timestamp DATETIME NOT NULL,
)
GO

ALTER TABLE EventNote WITH CHECK ADD FOREIGN KEY(EventID)
REFERENCES Event(ID)
GO

CREATE NONCLUSTERED INDEX IX_EventNote_EventID
ON EventNote(EventID ASC)
GO

CREATE TABLE MetersToDataPush
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LocalXDAMeterID INT NOT NULL,
    RemoteXDAMeterID INT NULL,
    LocalXDAAssetKey varchar(200) NOT NULL,
    RemoteXDAAssetKey varchar(200) NOT NULL,
    RemoteXDAName varchar(200) NOT NULL,
    Obsfucate bit NOT NULL,
    Synced bit NOT NULL
)
GO

CREATE TABLE AssetsToDataPush
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LocalXDAAssetID INT NOT NULL,
    RemoteXDAAssetID INT NULL,
    LocalXDAAssetKey VARCHAR(200) NOT NULL,
    RemoteXDAAssetKey VARCHAR(200) NOT NULL,
	RemoteAssetCreatedByDataPusher bit NOT NULL DEFAULT (1)
)
GO

CREATE TABLE RemoteXDAInstance
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Address VARCHAR(200) NULL,
    Frequency VARCHAR(20) NOT NULL,
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID)
)
GO

CREATE TABLE RemoteXDAInstanceMeter(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RemoteXDAInstanceID INT NOT NULL,
    MetersToDataPushID INT NOT NULL
)
GO

ALTER TABLE RemoteXDAInstanceMeter WITH CHECK ADD FOREIGN KEY(RemoteXDAInstanceID)
REFERENCES RemoteXDAInstance(ID)
GO

ALTER TABLE RemoteXDAInstanceMeter WITH CHECK ADD FOREIGN KEY(MetersToDataPushID)
REFERENCES MetersToDataPush(ID)
GO

CREATE TABLE FileGroupLocalToRemote
(
    ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	RemoteXDAInstanceID int NOT NULL REFERENCES RemoteXDAInstance(ID),
    LocalFileGroupID INT NOT NULL,
    RemoteFileGroupID INT NOT NULL
)
GO

ALTER TABLE FileGroupLocalToRemote WITH CHECK ADD FOREIGN KEY(LocalFileGroupID)
REFERENCES FileGroup(ID)
GO


-- ------------ --
-- PQ Dashboard --
-- ------------ --

CREATE TABLE DashSettings
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(500) NOT NULL,
    Value NVARCHAR(500) NOT NULL,
    Enabled BIT NOT NULL
)
GO

CREATE TABLE UserDashSettings
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserAccountID UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(500) NOT NULL,
    Value NVARCHAR(500) NOT NULL,
    Enabled BIT NOT NULL
)
GO

CREATE TABLE EASExtension
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ServiceName VARCHAR(50) NOT NULL,
    HasResultFunction VARCHAR(50) NOT NULL,
    WebPage VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE PQIResult
(
    ID INT NOT NULL PRIMARY KEY REFERENCES Event(ID)
)
GO

CREATE TABLE ContourColorScale
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    NominalValue FLOAT NOT NULL DEFAULT 0.0
)
GO

CREATE TABLE ContourChannelType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ContourColorScaleID INT NOT NULL REFERENCES ContourColorScale(ID),
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    HarmonicGroup INT NOT NULL DEFAULT 0
)
GO

CREATE TABLE ContourColorScalePoint
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ContourColorScaleID INT NOT NULL REFERENCES ContourColorScale(ID),
    Value FLOAT NOT NULL,
    Color INT NOT NULL,
    OrderID INT NOT NULL
)
GO

CREATE TABLE ContourAnimation
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ColorScaleName VARCHAR(200) NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    StepSize INT NOT NULL,
    AccessedOn DATETIME NOT NULL DEFAULT GETUTCDATE()
)
GO

CREATE NONCLUSTERED INDEX IX_ContourAnimation_AccessedOn
ON ContourAnimation(AccessedOn ASC)
GO

CREATE TABLE ContourAnimationFrame
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ContourAnimationID INT NOT NULL REFERENCES ContourAnimation(ID),
    FrameIndex INT NOT NULL,
    FrameImage VARBINARY(MAX) NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_ContourAnimationFrame_ContourAnimationID
ON ContourAnimationFrame(ContourAnimationID ASC)
GO


CREATE TABLE CompanyType(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name VARCHAR(200) NOT NULL UNIQUE,
	Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE Company
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyID VARCHAR(8) NOT NULL UNIQUE,
    CompanyTypeID int NOT NULL REFERENCES CompanyType(ID),
    Name VARCHAR(200) NOT NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE CompanyType(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name VARCHAR(200) NOT NULL UNIQUE,
	Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE CompanyMeter
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PQMarkCompanyID INT NOT NULL REFERENCES Company(ID),
    MeterID INT NOT NULL REFERENCES Meter(ID),
    DisplayName VARCHAR(200) NOT NULL,
    Enabled BIT NOT NULL
)
GO

CREATE TABLE [dbo].[PQMarkCompanyCustomer](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PQMarkCompanyID] [int] NOT NULL FOREIGN KEY REFERENCES Company(ID),
	[CustomerID] [int] NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_PQMarkCompanyMeter_MeterID 
ON CompanyMeter(MeterID) 
GO

CREATE TABLE PQMarkAggregate
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    Year INT NOT NULL,
    Month INT NOT NULL,
    ITIC INT NOT NULL,
    SEMI INT NOT NULL,
    SARFI90 INT NOT NULL,
    SARFI70 INT NOT NULL,
    SARFI50 INT NOT NULL,
    SARFI10 INT NOT NULL,
    THDJson VARCHAR(MAX) NULL
)
GO

CREATE INDEX IX_PQMarkAggregate_MeterID
ON PQMarkAggregate(MeterID)
GO

CREATE INDEX IX_PQMarkAggregate_Year
ON PQMarkAggregate(Year)
GO

CREATE INDEX IX_PQMarkAggregate_Month
ON PQMarkAggregate(Month)
GO

CREATE TABLE PQMarkDuration
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Label NVARCHAR(50) NOT NULL,
    Min FLOAT NOT NULL,
    Max FLOAT NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE PQMarkVoltageBin
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Label NVARCHAR(50) NOT NULL,
    Min FLOAT NOT NULL,
    Max FLOAT NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE PQMarkRestrictedTableUserAccount
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PrimaryID INT NOT NULL,
    TableName VARCHAR(MAX) NOT NULL,
    UserAccount VARCHAR(MAX) NOT NULL,
)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'<1c', 0, 0.01666666666, 0)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'1 to 2 c', 0.01666666667, 0.03333333333, 1)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'2 to 3 c', 0.03333333334, 0.05, 2)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'3 to 4 c', 0.050000000001, 0.06666666667, 3)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'4 to 5 c', 0.06666666668, 0.08333333334, 4)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'5 to 10 c', 0.08333333335, 0.16666666666, 5)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'10 c to 0.25s', 0.166666666667, 0.24999999999, 6)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'0.25 to 0.5s', 0.25, 0.49999999999, 7)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'0.5 to 1s', 0.5, 0.99999999999, 8)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'1 to 2s', 1, 1.99999999999, 9)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'2 to 5s', 2, 4.99999999999, 10)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'5 to 10s', 5, 9.99999999999, 11)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'10 to 20s', 10, 19.9999999999, 12)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'20 to 30s', 20, 29.9999999999, 13)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'30 to 60s', 30, 59.99999999999, 14)
GO

INSERT PQMarkDuration(Label, Min, Max, LoadOrder) VALUES(N'1 to 2 min', 60, 119.9999999999, 15)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'00-10', 0, 9.999999999, 0)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'10-20', 10, 19.99999999, 1)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'20-30', 20, 29.999999999, 2)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'30-40', 30, 39.999999999, 3)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'40-50', 40, 49.999999999, 4)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'50-60', 50, 59.999999999, 5)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'60-70', 60, 69.999999999, 6)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'70-80', 70, 79.999999999, 7)
GO

INSERT PQMarkVoltageBin (Label, Min, Max, LoadOrder) VALUES(N'80-90', 80, 89.999999999, 8)
GO

CREATE TABLE Report
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    Month INT NOT NULL,
    Year INT NOT NULL,
    Results VARCHAR(4) NOT NULL,
    PDF VARBINARY(MAX) NOT NULL,
)
GO

CREATE TABLE EventStat
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    VPeak FLOAT NULL,
    VAMax FLOAT NULL,
    VBMax FLOAT NULL,
    VCMax FLOAT NULL,
    VABMax FLOAT NULL,
    VBCMax FLOAT NULL,
    VCAMax FLOAT NULL,
    VAMin FLOAT NULL,
    VBMin FLOAT NULL,
    VCMin FLOAT NULL,
    VABMin FLOAT NULL,
    VBCMin FLOAT NULL,
    VCAMin FLOAT NULL,
    IPeak FLOAT NULL,
    IAMax FLOAT NULL,
    IBMax FLOAT NULL,
    ICMax FLOAT NULL,
    IA2t FLOAT NULL,
    IB2t FLOAT NULL,
    IC2t FLOAT NULL,
    InitialMW FLOAT NULL,
    FinalMW FLOAT NULL,
    PQViewID INT NULL,
    CONSTRAINT UC_EventStat_EventID UNIQUE(EventID)
)
GO

CREATE TABLE RelayPerformance
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
	ChannelID INT NOT NULL REFERENCES Channel(ID),
	Imax1 FLOAT NULL,
	Imax2 FLOAT NULL,
    TripInitiate DATETIME2 NULL,
	TripTime INT NULL,
	PickupTime INT NULL,
	TripCoilCondition FLOAT NULL,
)
GO

-- CapBank Analytics

CREATE TABLE CBDataError (
    ID INT NOT NULL PRIMARY KEY,
    Description VARCHAR(200) NOT NULL
)
GO

CREATE TABLE CBOperation (
    ID INT NOT NULL PRIMARY KEY,
    Description VARCHAR(500) NOT NULL
)
GO

CREATE TABLE CBStatus (
    ID INT NOT NULL PRIMARY KEY,
    Description VARCHAR(500) NOT NULL
)
GO


CREATE TABLE CBRestrikeType (
    ID INT NOT NULL PRIMARY KEY,
    Description VARCHAR(200) NOT NULL
)
GO

CREATE TABLE CBSwitchingCondition (
    ID INT NOT NULL PRIMARY KEY,
    Description VARCHAR(200) NOT NULL
)
GO


CREATE TABLE CBAnalyticResult (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    CBStatusID INT NULL REFERENCES CBStatus(ID),
    DataErrorID INT NULL REFERENCES CBDataError(ID),
    CBOperationID INT NULL REFERENCES CBOperation(ID),
    DeEnergizedBanks INT NULL,
    EnergizedBanks INT NULL,
    InServiceBank INT NULL,
    DeltaQ FLOAT NULL,
    Ipre FLOAT NULL,
    Ipost FLOAT NULL,
    Vpre FLOAT NULL,
    Vpost FLOAT NULL,
    MVAsc FLOAT NULL,
    IsRes BIT NULL,
    ResFreq FLOAT NULL,
    THDpre FLOAT NULL,
    THDpost FLOAT NULL,
    THDVpre FLOAT NULL,
    THDVpost FLOAT NULL,
    StepPre INT NULL,
    StepPost INT NULL,
    SwitchingFreq FLOAT NULL,
    Vpeak FLOAT NULL,
    Xpre FLOAT NULL,
    Xpost FLOAT NULL,
    Time DATETIME2 NULL,
    Toffset FLOAT NULL,
    XshortedThld FLOAT NULL,
    XblownThld FLOAT NULL,
    dVThld FLOAT NULL,
    dVThldLG FLOAT NULL
    )
GO

CREATE TABLE CBRestrikeResult (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CBResultID INT NOT NULL REFERENCES CBAnalyticResult(ID),
    CBRestrikeTypeID INT NOT NULL REFERENCES CBRestrikeType(ID),
    Text FLOAT NULL,
    Trest FLOAT NULL,
    Text2 FLOAT NULL,
    Drest FLOAT NULL,
    Vmax FLOAT NULL,
    Imax FLOAT NULL
    )
GO

CREATE TABLE CBSwitchHealthAnalytic (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CBResultID INT NOT NULL REFERENCES CBAnalyticResult(ID),
    CBSwitchingConditionID INT NOT NULL REFERENCES CBSwitchingCondition(ID),
    R FLOAT NULL,
    X FLOAT NULL,
    Duration FLOAT NULL,
    I FLOAT NULL,
    )
GO

CREATE TABLE CBBankHealth (
    ID INT NOT NULL PRIMARY KEY,
    Description VARCHAR(200) NOT NULL
)
GO

CREATE TABLE CBCapBankResult (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CBResultID INT NOT NULL REFERENCES CBAnalyticResult(ID),
    CBBankHealthID INT NULL REFERENCES CBBankHealth(ID),
    CBOperationID INT NULL REFERENCES CBOperation(ID),
    BankInService INT NOT NULL,
    Vrelay FLOAT NULL,
    Ineutral FLOAT NULL,
    V0 FLOAT NULL,
    Z0 FLOAT NULL,
    XLV FLOAT NULL,
    X FLOAT NULL,
    Kfactor FLOAT NULL,
    dV FLOAT NULL,
    XUG FLOAT NULL,
    XLG FLOAT NULL,
    XVmiss FLOAT NULL,
    VUIEC FLOAT NULL,
    VUIEEE FLOAT NULL
)
GO
----- FUNCTIONS -----

CREATE FUNCTION AdjustDateTime2
(
    @dateTime2 DATETIME2,
    @timeTolerance FLOAT
)
RETURNS DATETIME2
BEGIN
    DECLARE @adjustSecond INT = @timeTolerance
    DECLARE @adjustNanosecond INT = (@timeTolerance - ROUND(@timeTolerance, 0, 1)) * 1000000000

    DECLARE @adjustedDateTime DATETIME2
    DECLARE @dateTimeLimit DATETIME2
    DECLARE @adjustedDateTimeLimit DATETIME2

    SELECT @dateTimeLimit =
        CASE WHEN @timeTolerance < 0.0 THEN '0001-01-01'
             ELSE '9999-12-31 23:59:59.9999999'
        END

    SET @adjustedDateTimeLimit = DATEADD(SECOND, -@adjustSecond, @dateTimeLimit)
    SET @adjustedDateTimeLimit = DATEADD(NANOSECOND, -@adjustNanosecond, @dateTimeLimit)

    SELECT @adjustedDateTime =
        CASE WHEN @timeTolerance < 0.0 AND @dateTime2 <= @adjustedDateTimeLimit THEN @dateTimeLimit
             WHEN @timeTolerance > 0.0 AND @dateTime2 >= @adjustedDateTimeLimit THEN @dateTimeLimit
             ELSE DATEADD(NANOSECOND, @adjustNanosecond, DATEADD(SECOND, @adjustSecond, @dateTime2))
        END

    RETURN @adjustedDateTime
END
GO

CREATE FUNCTION DateDiffTicks
(
    @startDate DATETIME2,
    @endDate DATETIME2
)
RETURNS BIGINT
AS
BEGIN
    DECLARE @startDay DATETIME2 = DATEADD(DAY, DATEDIFF(DAY, 0, @startDate), 0)
    DECLARE @endDay DATETIME2 = DATEADD(DAY, DATEDIFF(DAY, 0, @endDate), 0)

    DECLARE @startSeconds DATETIME2 = DATEADD(SECOND, DATEDIFF(SECOND, @startDay, @startDate), @startDay)
    DECLARE @endSeconds DATETIME2 = DATEADD(SECOND, DATEDIFF(SECOND, @endDay, @endDate), @endDay)

    RETURN
        (CONVERT(BIGINT, DATEDIFF(DAY, @startDate, @endDate)) * 24 * 60 * 60 * 10000000) -
        (CONVERT(BIGINT, DATEDIFF(SECOND, @startDay, @startDate)) * 10000000) +
        (CONVERT(BIGINT, DATEDIFF(SECOND, @endDay, @endDate)) * 10000000) -
        (DATEDIFF(NANOSECOND, @startSeconds, @startDate) / 100) +
        (DATEDIFF(NANOSECOND, @endSeconds, @endDate) / 100)
END
GO

CREATE FUNCTION GetSystemEventIDs
(
    @startTime DATETIME2,
    @endTime DATETIME2,
    @timeTolerance FLOAT
)
RETURNS @systemEvent TABLE
(
    EventID INT
)
AS BEGIN
    DECLARE @adjustedStartTime DATETIME2 = dbo.AdjustDateTime2(@startTime, -@timeTolerance)
    DECLARE @adjustedEndTime DATETIME2 = dbo.AdjustDateTime2(@endTime, @timeTolerance)
    DECLARE @minStartTime DATETIME2
    DECLARE @maxEndTime DATETIME2

    SELECT
        @minStartTime = MIN(dbo.AdjustDateTime2(StartTime, -@timeTolerance)),
        @maxEndTime = MAX(dbo.AdjustDateTime2(EndTime, @timeTolerance))
    FROM
        Event JOIN
        FileGroup ON Event.FileGroupID = FileGroup.ID
    WHERE
        StartTime <= @adjustedEndTime AND
        @adjustedStartTime <= EndTime AND
        ProcessingEndTime > '0001-01-01'

    WHILE @startTime != @minStartTime OR @endTime != @maxEndTime
    BEGIN
        SET @startTime = @minStartTime
        SET @endTime = @maxEndTime
        SET @adjustedStartTime = dbo.AdjustDateTime2(@startTime, -@timeTolerance)
        SET @adjustedEndTime = dbo.AdjustDateTime2(@endTime, @timeTolerance)

        SELECT
            @minStartTime = MIN(dbo.AdjustDateTime2(StartTime, -@timeTolerance)),
            @maxEndTime = MAX(dbo.AdjustDateTime2(EndTime, @timeTolerance))
        FROM
            Event JOIN
            FileGroup ON Event.FileGroupID = FileGroup.ID
        WHERE
            StartTime <= @adjustedEndTime AND
            @adjustedStartTime <= EndTime AND
            ProcessingEndTime > '0001-01-01'
    END

    INSERT INTO @systemEvent
    SELECT ID
    FROM Event
    WHERE @adjustedStartTime <= StartTime AND EndTime <= @adjustedEndTime

    RETURN
END
GO

CREATE FUNCTION GetDisturbancesInSystemEvent
(
    @startTime DATETIME2,
    @endTime DATETIME2,
    @timeTolerance FLOAT
)
RETURNS @Sequence TABLE
(
    ID INT PRIMARY KEY,
    SequenceNumber INT
)
AS BEGIN
    DECLARE @seq INT
    DECLARE @seqStartTime DATETIME2
    DECLARE @seqEndTime DATETIME2

    DECLARE @disturbanceID INT
    DECLARE @disturbanceStartTime DATETIME2
    DECLARE @disturbanceEndTime DATETIME2

    DECLARE DisturbanceCursor CURSOR FOR
    SELECT ID, StartTime, EndTime
    FROM Disturbance
    WHERE
        EventID IN (SELECT * FROM dbo.GetSystemEventIDs(@startTime, @endTime, @timeTolerance)) AND
        PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst')
    ORDER BY StartTime

    OPEN DisturbanceCursor

    FETCH NEXT FROM DisturbanceCursor
    INTO @disturbanceID, @disturbanceStartTime, @disturbanceEndTime

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT
            @seq =
                CASE
                    WHEN @seq IS NULL THEN 1
                    WHEN @disturbanceStartTime <= @seqEndTime THEN @seq
                    ELSE @seq + 1
                END,
            @seqStartTime =
                CASE
                    WHEN @seqStartTime IS NULL THEN @disturbanceStartTime
                    WHEN @disturbanceStartTime <= @seqEndTime THEN @seqStartTime
                    ELSE @disturbanceStartTime
                END,
            @seqEndTime =
                CASE
                    WHEN @seqEndTime IS NULL THEN @disturbanceEndTime
                    WHEN @disturbanceEndTime < @seqEndTime THEN @seqEndTime
                    ELSE @disturbanceEndTime
                END

        INSERT INTO @Sequence
        VALUES(@disturbanceID, @seq)

        FETCH NEXT FROM DisturbanceCursor
        INTO @disturbanceID, @disturbanceStartTime, @disturbanceEndTime
    END

    CLOSE DisturbanceCursor
    DEALLOCATE DisturbanceCursor

    RETURN
END
GO

CREATE FUNCTION HasImpactedComponents
(
    @disturbanceID INT
)
RETURNS INT
AS BEGIN
    RETURN 0
END
GO

CREATE FUNCTION EventHasImpactedComponents
(
    @eventID INT
)
RETURNS INT
AS BEGIN
    DECLARE @hasImpactedComponents INT

    SELECT @hasImpactedComponents =
        CASE WHEN EXISTS
        (
            SELECT *
            FROM Disturbance
            WHERE
                EventID = @eventID AND
                dbo.HasImpactedComponents(ID) <> 0
        )
        THEN 1
        ELSE 0
        END

    RETURN @hasImpactedComponents
END
GO


----- VIEWS -----

CREATE VIEW ActiveHost AS
SELECT *
FROM HostRegistration
WHERE
    CheckedIn IS NOT NULL AND
    CheckedIn > DATEADD(MINUTE, -5, GETUTCDATE())
GO

CREATE VIEW CBReportEventTable AS
SELECT
    CBAnalyticResult.ID AS ID,
    Event.AssetID AS CapBankID,
    Event.ID AS EventID,
    Phase.Name AS Phase,
    CBStatus.Description AS Status,
    CBAnalyticResult.DataErrorID AS DataErrorID,
    CBOperation.Description AS Operation,
    CBAnalyticResult.DeltaQ AS DeltaQ,
    CBAnalyticResult.MVAsc AS MVAsc,
    CBAnalyticResult.IsRes AS IsRes,
    CBAnalyticResult.Time AS Time
FROM
    CBAnalyticResult LEFT JOIN 
    Event ON CBAnalyticResult.EventID = Event.ID LEFT JOIN
    Phase ON CBAnalyticResult.PhaseID = Phase.ID LEFT JOIN
    CBStatus ON CBAnalyticResult.CBStatusID = CBStatus.ID LEFT JOIN
    CBOperation ON CBAnalyticResult.CBOperationID = CBOperation.ID
GO

CREATE VIEW AssetGroupView AS
SELECT
	AssetGroup.ID,
	AssetGroup.Name,
    AssetGroup.DisplayDashboard,
	COUNT(DISTINCT AssetGroupAssetGroup.ChildAssetGroupID) as AssetGroups,
	COUNT(DISTINCT MeterAssetGroup.MeterID) as Meters,
	COUNT(DISTINCT AssetAssetGroup.AssetID) as Assets,
	COUNT(DISTINCT UserAccountAssetGroup.UserAccountID) as Users
FROM
	AssetGroup LEFT JOIN
	AssetGroupAssetGroup ON AssetGroup.ID = AssetGroupAssetGroup.ParentAssetGroupID LEFT JOIN
	MeterAssetGroup ON AssetGroup.ID = MeterAssetGroup.AssetGroupID LEFT JOIN
	AssetAssetGroup ON AssetGroup.ID = AssetAssetGroup.AssetGroupID LEFT JOIN
	UserAccountAssetGroup ON AssetGroup.ID = UserAccountAssetGroup.AssetGroupID
GROUP BY
	AssetGroup.ID,AssetGroup.Name,AssetGroup.DisplayDashboard
GO

CREATE VIEW BreakerHistory
AS
SELECT  Breaker.ID AS BreakerID,
		RelayPerformance.EventID AS EventID,
        RelayPerformance.Imax1,
		RelayPerformance.Imax2,
		RelayPerformance.TripInitiate,
		RelayPerformance.TripTime / 10 AS TripTime,
		RelayPerformance.PickupTime / 10 AS PickupTime,
		RelayPerformance.TripCoilCondition,
		Breaker.TripCoilCondition AS TripCoilConditionAlert,
		Breaker.TripTime AS TripTimeAlert,
		Breaker.PickupTime AS PickupTimeAlert,
		RelayPerformance.ChannelID AS TripCoilChannelID
FROM    RelayPerformance LEFT OUTER JOIN
        Channel ON RelayPerformance.ChannelID = Channel.ID LEFT OUTER JOIN
        AssetChannel ON AssetChannel.ChannelID = Channel.ID LEFT JOIN
        Breaker ON Breaker.ID = AssetChannel.AssetID
GO



CREATE VIEW MeterDetail
AS
SELECT  Meter.ID,
        Meter.AssetKey,
        Meter.LocationID,
        Location.LocationKey AS LocationKey,
        Location.Name AS Location,
        Location.Latitude,
        Location.Longitude,
        Meter.Name,
        Meter.Alias,
        Meter.ShortName,
        Meter.Make,
        Meter.Model,
        CASE COALESCE (Meter.TimeZone, '')
            WHEN '' THEN COALESCE (Setting.Value, 'UTC')
            ELSE Meter.TimeZone END AS TimeZone,
        Meter.Description
FROM    Meter INNER JOIN
        Location ON Meter.LocationID = Location.ID LEFT OUTER JOIN
        Setting ON Setting.Name = 'DefaultMeterTimeZone'

GO

CREATE VIEW LineView
AS
SELECT	Line.ID,
		MaxFaultDistance,
		MinFaultDistance,
		AssetKey,
		VoltageKV,
		Description,
		AssetName,
		ISNULL((SELECT Sum(LineSegment.Length) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ChildID = LineSegment.ID 
			WHERE AssetRelationship.ParentID = Line.ID AND AssetRelationship.AssetRelationshipTypeID = (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment') ),0)
		+ ISNULL((SELECT Sum(LineSegment.Length) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ParentID = LineSegment.ID 
			WHERE AssetRelationship.ChildID = Line.ID AND AssetRelationship.AssetRelationshipTypeID = (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment')),0)
		AS Length,
		ISNULL((SELECT Sum(LineSegment.R0) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ChildID = LineSegment.ID 
			WHERE AssetRelationship.ParentID = Line.ID AND AssetRelationship.AssetRelationshipTypeID = (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment') ),0)
		+ ISNULL((SELECT Sum(LineSegment.R0) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ParentID = LineSegment.ID 
			WHERE AssetRelationship.ChildID = Line.ID AND AssetRelationship.AssetRelationshipTypeID =  (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment') ),0)
		AS R0,
		ISNULL((SELECT Sum(LineSegment.R1) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ChildID = LineSegment.ID 
			WHERE AssetRelationship.ParentID = Line.ID AND AssetRelationship.AssetRelationshipTypeID =  (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment') ),0)
		+ ISNULL((SELECT Sum(LineSegment.R1) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ParentID = LineSegment.ID 
			WHERE AssetRelationship.ChildID = Line.ID AND AssetRelationship.AssetRelationshipTypeID =  (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment') ),0)
		AS R1,
			ISNULL((SELECT Sum(LineSegment.X0) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ChildID = LineSegment.ID 
			WHERE AssetRelationship.ParentID = Line.ID AND AssetRelationship.AssetRelationshipTypeID =  (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment') ),0)
		+ ISNULL((SELECT Sum(LineSegment.X0) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ParentID = LineSegment.ID 
			WHERE AssetRelationship.ChildID = Line.ID AND AssetRelationship.AssetRelationshipTypeID =  (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment')),0)
		AS X0,
		ISNULL((SELECT Sum(LineSegment.X1) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ChildID = LineSegment.ID 
			WHERE AssetRelationship.ParentID = Line.ID AND AssetRelationship.AssetRelationshipTypeID = (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment')),0)
		+ ISNULL((SELECT Sum(LineSegment.X1) FROM AssetRelationship LEFT JOIN 
			LineSegment ON AssetRelationship.ParentID = LineSegment.ID 
			WHERE AssetRelationship.ChildID = Line.ID AND AssetRelationship.AssetRelationshipTypeID = (SELECT ID FROM AssetRelationShipType WHERE Name = 'Line-LineSegment')),0)
		AS X1
	FROM LINE

GO

CREATE VIEW MeterAssetDetail AS
	SELECT 
		MeterAsset.ID,
		MeterAsset.AssetID,
		MeterAsset.MeterID,
		Meter.AssetKey AS MeterKey,
		Asset.AssetKey AS AssetKey,
		AssetType.Name AS AssetType,
		FaultDetectionLogic.Expression AS FaultDetectionLogic,
		Asset.AssetName AS AssetName
	FROM
		MeterAsset LEFT JOIN Meter ON MeterAsset.MeterID = Meter.ID LEFT JOIN
		ASSET ON MeterAsset.AssetID = Asset.ID LEFT JOIN 
		AssetType ON Asset.AssetTypeID = AssetType.ID LEFT JOIN
		FaultDetectionLogic ON FaultDetectionLogic.MeterAssetID = MeterAsset.ID
GO

CREATE VIEW AssetConnectionDetail AS
SELECT
	AssetRelationship.ID,
	AssetRelationship.ChildID,
	Asset1.AssetKey AS ChildKey,
	AssetRelationship.ParentID,
	Asset2.AssetKey AS ParentKey,
	AssetRelationship.AssetRelationshipTypeID,
	AssetRelationshipType.Name AS AssetRelationshipType
FROM
	AssetRelationship LEFT JOIN Asset Asset1 ON Asset1.ID = AssetRelationship.ChildID LEFT JOIN
	Asset Asset2 ON Asset2.ID = AssetRelationship.ParentID LEFT JOIN
	AssetRelationshipType ON AssetRelationship.AssetRelationshipTypeID = AssetRelationshipType.ID
GO


CREATE VIEW ChannelDetail
AS
SELECT
    Channel.*,
     Meter.AssetKey AS MeterKey,
     Meter.Name AS MeterName,
     Asset.AssetKey AS AssetKey,
     Asset.AssetName,
     MeasurementType.Name AS MeasurementType,
     MeasurementCharacteristic.Name AS MeasurementCharacteristic,
     Phase.Name AS Phase,
     Series.SourceIndexes AS Mapping,
     Series.SeriesTypeID,
     SeriesType.Name AS SeriesType
 FROM
     Channel JOIN
     Meter ON Channel.MeterID = Meter.ID JOIN
     Asset ON Channel.AssetID = Asset.ID JOIN
     MeterAsset ON
         MeterAsset.MeterID = Meter.ID AND
         MeterAsset.AssetID = Asset.ID JOIN
     MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN
     MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
     Phase ON Channel.PhaseID = Phase.ID LEFT OUTER JOIN
     Series ON
         Series.ChannelID = Channel.ID AND
         Series.SourceIndexes <> '' LEFT OUTER JOIN
     SeriesType ON dbo.Series.SeriesTypeID = dbo.SeriesType.ID
GO


CREATE VIEW DefaultAlarmRangeLimitView
AS
SELECT
    DefaultAlarmRangeLimit.ID,
    DefaultAlarmRangeLimit.MeasurementTypeID,
    DefaultAlarmRangeLimit.AlarmTypeID,
    DefaultAlarmRangeLimit.MeasurementCharacteristicID,
    DefaultAlarmRangeLimit.Severity,
    DefaultAlarmRangeLimit.High,
    DefaultAlarmRangeLimit.Low,
    DefaultAlarmRangeLimit.PerUnit,
    DefaultAlarmRangeLimit.RangeInclusive,
    AlarmType.Name AS AlarmType,
    MeasurementCharacteristic.Name AS MeasurementCharacteristic,
    MeasurementType.Name AS MeasurementType
FROM
    DefaultAlarmRangeLimit JOIN
    AlarmType ON DefaultAlarmRangeLimit.AlarmTypeID = AlarmType.ID JOIN
    MeasurementCharacteristic ON DefaultAlarmRangeLimit.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
    MeasurementType ON DefaultAlarmRangeLimit.MeasurementTypeID = MeasurementType.ID
GO

 CREATE VIEW AlarmRangeLimitView
 AS
 SELECT
     AlarmRangeLimit.ID,
     AlarmRangeLimit.ChannelID,
     Channel.MeterID,
     Channel.AssetID,
     Channel.Name,
     AlarmRangeLimit.AlarmTypeID,
     AlarmRangeLimit.Severity,
     AlarmRangeLimit.High,
     AlarmRangeLimit.Low,
     AlarmRangeLimit.RangeInclusive,
     AlarmRangeLimit.PerUnit,
     AlarmRangeLimit.Enabled,
     MeasurementType.Name AS MeasurementType,
     MeasurementCharacteristic.Name AS MeasurementCharacteristic,
     Phase.Name AS Phase,
     Channel.HarmonicGroup,
     Channel.MeasurementTypeID,
     Channel.MeasurementCharacteristicID,
     Channel.PhaseID,
     AlarmRangeLimit.IsDefault,
     Meter.Name AS MeterName
 FROM
     AlarmRangeLimit JOIN
     Channel ON AlarmRangeLimit.ChannelID = Channel.ID JOIN
     MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN
     MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
     Phase ON Channel.PhaseID = Phase.ID JOIN
     Meter ON Channel.MeterID = Meter.ID
 GO

CREATE VIEW MeterAssetGroupView
AS
SELECT
    MeterAssetGroup.ID,
    Meter.Name AS MeterName,
    Meter.ID AS MeterID,
    AssetGroupID,
    Location.Name AS Location
FROM
    MeterAssetGroup JOIN
    Meter ON MeterAssetGroup.MeterID = Meter.ID JOIN
    Location ON Meter.LocationID = Location.ID
GO

 CREATE VIEW AssetAssetGroupView
 AS
 SELECT
     AssetAssetGroup.ID,
     Asset.AssetKey AS Assetname,
     Asset.AssetName AS LongAssetName,
     Asset.ID AS AssetID,
	 AssetType.Name AS AssetType,
	 (SELECT Top 1 LocationKey FROM Location WHERE Location.ID IN (SELECT LocationID FROM AssetLocation WHERE AssetLocation.AssetID = Asset.ID)) AS AssetLocation,
     AssetGroupID
 FROM
     AssetAssetGroup JOIN
     Asset ON AssetAssetGroup.AssetID = Asset.ID LEFT JOIN
	 AssetType ON Asset.AssetTypeID = AssetType.ID
GO

 CREATE VIEW AssetGroupAssetGroupView
 AS
 SELECT
     AssetGroupAssetGroup.ID,
     AssetGroupAssetGroup.ParentAssetGroupID,
     AssetGroupAssetGroup.ChildAssetGroupID,
     Parent.Name as ParentAssetGroupName,
     Child.Name as ChildAssetGroupName
 FROM
     AssetGroupAssetGroup JOIN
     AssetGroup as Parent ON AssetGroupAssetGroup.ParentAssetGroupID = Parent.ID JOIN
     AssetGroup as Child ON AssetGroupAssetGroup.ChildAssetGroupID = Child.ID
 GO


CREATE VIEW UserAccountAssetGroupView
AS
SELECT
    UserAccountAssetGroup.ID,
    UserAccountAssetGroup.UserAccountID,
    UserAccountAssetGroup.AssetGroupID,
    UserAccountAssetGroup.Dashboard,
    UserAccountAssetGroup.Email,
    UserAccount.Name AS Username,
    AssetGroup.Name AS GroupName
FROM
    UserAccountAssetGroup JOIN
    UserAccount ON UserAccountAssetGroup.UserAccountID = UserAccount.ID JOIN
    AssetGroup ON UserAccountAssetGroup.AssetGroupID = AssetGroup.ID
GO

CREATE VIEW DoubleEndedFaultSummary AS
SELECT
    DoubleEndedFaultDistance.ID,
    FaultSummary.EventID,
    'DoubleEnded' AS Algorithm,
    FaultSummary.FaultNumber,
    FaultSummary.CalculationCycle,
    DoubleEndedFaultDistance.Distance,
    DoubleEndedFaultDistance.Angle,
    FaultSummary.CurrentMagnitude,
    FaultSummary.Inception,
    FaultSummary.DurationSeconds,
    FaultSummary.DurationCycles,
    FaultSummary.FaultType,
    1 AS IsSelectedAlgorithm,
    DoubleEndedFaultDistance.IsValid,
    FaultSummary.IsSuppressed
FROM
    DoubleEndedFaultDistance JOIN
    FaultSummary ON DoubleEndedFaultDistance.LocalFaultSummaryID = FaultSummary.ID AND FaultSummary.IsSelectedAlgorithm <> 0
GO

CREATE VIEW ContourChannel
AS
SELECT
    ContourColorScale.ID AS ContourColorScaleID,
    ContourColorScale.Name AS ContourColorScaleName,
    Channel.MeterID AS MeterID,
    Channel.ID AS ChannelID,
    Channel.PerUnitValue
FROM
    ContourColorScale JOIN
    ContourChannelType ON ContourChannelType.ContourColorScaleID = ContourColorScale.ID JOIN
    Channel ON
        Channel.MeasurementTypeID = ContourChannelType.MeasurementTypeID AND
        Channel.MeasurementCharacteristicID = ContourChannelType.MeasurementCharacteristicID AND
        Channel.PhaseID = ContourChannelType.PhaseID AND
        Channel.HarmonicGroup = ContourChannelType.HarmonicGroup
GO

 CREATE VIEW EventView
 AS
 SELECT
     Event.ID,
     Event.FileGroupID,
     Event.MeterID,
     Event.AssetID,
     Event.EventTypeID,
     Event.EventDataID,
     Event.Name,
     Event.Alias,
     Event.ShortName,
     Event.StartTime,
     Event.EndTime,
     Event.Samples,
     Event.TimeZoneOffset,
     Event.SamplesPerSecond,
     Event.SamplesPerCycle,
     Event.Description,
     Event.UpdatedBy,
     Asset.AssetName,
     Meter.Name AS MeterName,
     Location.Name AS StationName,
     EventType.Name AS EventTypeName
 FROM
     Event JOIN
     Meter ON Event.MeterID = Meter.ID JOIN
     Location ON Meter.LocationID = Location.ID JOIN
     Asset ON Event.AssetID = Asset.ID JOIN
     MeterAsset ON MeterAsset.MeterID = Meter.ID AND MeterAsset.AssetID = Asset.ID JOIN
     EventType ON Event.EventTypeID = EventType.ID
GO

-- CREATE VIEW DisturbanceView
-- AS
-- SELECT
--     Disturbance.ID,
--     Disturbance.EventID,
--     Disturbance.EventTypeID,
--     Disturbance.PhaseID,
--     Disturbance.Magnitude,
--     Disturbance.PerUnitMagnitude,
--     Disturbance.StartTime,
--     Disturbance.EndTime,
--     Disturbance.DurationSeconds,
--     Disturbance.DurationCycles,
--     Disturbance.StartIndex,
--     Disturbance.EndIndex,
--     Event.MeterID,
--     (
--         SELECT MAX(SeverityCode) AS Expr1
--         FROM DisturbanceSeverity
--         WHERE (DisturbanceID = Disturbance.ID)
--     ) AS SeverityCode,
--     Meter.Name AS MeterName,
--     Phase.Name AS PhaseName,
--     Event.LineID
-- FROM
--     Disturbance JOIN
--     Event ON Disturbance.EventID = Event.ID JOIN
--     Meter ON Event.MeterID = Meter.ID JOIN
--     Phase ON Disturbance.PhaseID = Phase.ID
-- GO

-- CREATE VIEW BreakerView
-- AS
-- SELECT
--     BreakerOperation.ID,
--     Meter.ID AS MeterID,
--     Event.ID AS EventID,
--     EventType.Name AS EventType,
--     BreakerOperation.TripCoilEnergized AS Energized,
--     BreakerOperation.BreakerNumber,
--     MeterLine.LineName,
--     Phase.Name AS PhaseName,
--     CAST(BreakerOperation.BreakerTiming AS DECIMAL(16, 5)) AS Timing,
--     BreakerOperation.BreakerSpeed AS Speed,
--     BreakerOperationType.Name AS OperationType,
--     BreakerOperation.UpdatedBy
-- FROM
--     BreakerOperation JOIN
--     Event ON BreakerOperation.EventID = Event.ID JOIN
--     EventType ON EventType.ID = Event.EventTypeID JOIN
--     Meter ON Meter.ID = Event.MeterID JOIN
--     Line ON Line.ID = Event.LineID JOIN
--     MeterLine ON MeterLine.LineID = Event.LineID AND MeterLine.MeterID = Meter.ID JOIN
--     BreakerOperationType ON BreakerOperation.BreakerOperationTypeID = BreakerOperationType.ID JOIN
--     Phase ON BreakerOperation.PhaseID = Phase.ID
-- GO

CREATE VIEW FaultView
AS
SELECT
     FaultSummary.ID AS ID,
     FaultSummary.EventID,
     FaultSummary.Algorithm,
     FaultSummary.FaultNumber,
     FaultSummary.CalculationCycle,
     FaultSummary.Distance,
     FaultSummary.CurrentMagnitude,
     FaultSummary.CurrentLag,
     FaultSummary.PrefaultCurrent,
     FaultSummary.PostfaultCurrent,
     FaultSummary.Inception,
     FaultSummary.DurationSeconds,
     FaultSummary.DurationCycles,
     FaultSummary.FaultType,
     FaultSummary.IsSelectedAlgorithm,
     FaultSummary.IsValid,
     FaultSummary.IsSuppressed,
     Meter.Name AS MeterName,
     Meter.ShortName AS ShortName,
     Location.ShortName AS LocationName,
     Meter.ID AS MeterID,
     Asset.ID AS AssetID,
     Asset.AssetName AS AssetName,
     Asset.VoltageKV AS Voltage,
     Event.StartTime AS InceptionTime,
     CASE WHEN FaultSummary.Distance = '-1E308'
         THEN 'NaN'
         ELSE CAST(CAST(FaultSummary.Distance AS DECIMAL(16,2)) AS NVARCHAR(19))
     END AS CurrentDistance,
     ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY FaultSummary.IsSuppressed, FaultSummary.IsSelectedAlgorithm DESC, FaultSummary.Inception) AS RK
 FROM
     FaultSummary JOIN
     Event ON FaultSummary.EventID = Event.ID JOIN
     EventType ON Event.EventTypeID = EventType.ID JOIN
     Meter ON Event.MeterID = Meter.ID JOIN
     Location ON Meter.LocationID = Location.ID JOIN
     Asset ON Event.AssetID = Asset.ID     
 WHERE
     EventType.Name = 'Fault'
 GO



CREATE VIEW WorkbenchVoltageCurveView
AS
SELECT
    WorkbenchVoltageCurve.ID,
    WorkbenchVoltageCurve.Name,
    WorkbenchVoltageCurvePoint.ID AS CurvePointID,
    WorkbenchVoltageCurvePoint.PerUnitMagnitude,
    WorkbenchVoltageCurvePoint.DurationSeconds,
    WorkbenchVoltageCurvePoint.LoadOrder,
    WorkbenchVoltageCurve.Visible
FROM
    WorkbenchVoltageCurve JOIN
    WorkbenchVoltageCurvePoint ON
        WorkbenchVoltageCurve.ID = WorkbenchVoltageCurvePoint.VoltageCurveID AND
        WorkbenchVoltageCurve.ID = WorkbenchVoltageCurvePoint.VoltageCurveID AND
        WorkbenchVoltageCurve.ID = WorkbenchVoltageCurvePoint.VoltageCurveID
GO

CREATE VIEW EmailTypeView
AS
SELECT
    EmailType.EmailCategoryID,
    EmailType.XSLTemplateID,
    EmailType.ID,
    EmailCategory.Name AS EmailCategory,
    XSLTemplate.Name AS XSLTemplate,
    EmailCategory.Name + ' - ' + XSLTemplate.Name AS Name
FROM
    EmailType JOIN
    EmailCategory ON EmailType.EmailCategoryID = EmailCategory.ID JOIN
    XSLTemplate ON EmailType.XSLTemplateID = XSLTemplate.ID
GO

CREATE VIEW AuditLogView
AS
SELECT TOP(2000)
    ID,
    TableName,
    PrimaryKeyColumn,
    PrimaryKeyValue,
    ColumnName,
    OriginalValue,
    NewValue,
    Deleted,
    UpdatedBy,
    UpdatedOn
FROM AuditLog
WHERE UpdatedBy IS NOT NULL
GO

CREATE VIEW MetersWithHourlyLimits
AS
SELECT
    Meter.Name,
    COUNT(DISTINCT Channel.ID) AS Limits,
    Meter.ID
FROM
    HourOfWeekLimit JOIN
    Channel ON HourOfWeekLimit.ChannelID = Channel.ID JOIN
    Meter ON Channel.MeterID = Meter.ID
GROUP BY
    Meter.Name,
    Meter.ID
GO

CREATE VIEW HourOfWeekLimitView
AS
SELECT
    HourOfWeekLimit.ID,
    HourOfWeekLimit.ChannelID,
    HourOfWeekLimit.AlarmTypeID,
    HourOfWeekLimit.HourOfWeek,
    HourOfWeekLimit.Severity,
    HourOfWeekLimit.High,
    HourOfWeekLimit.Low,
    HourOfWeekLimit.Enabled, AlarmType.Name AS AlarmTypeName
FROM
    HourOfWeekLimit JOIN
    AlarmType ON HourOfWeekLimit.AlarmTypeID = AlarmType.ID
GO

CREATE VIEW ChannelsWithHourlyLimits
AS
SELECT
    Channel.Name,
    COUNT(DISTINCT HourOfWeekLimit.HourOfWeek) AS Limits,
    Channel.ID,
    Channel.MeterID,
    AlarmType.Name AS AlarmTypeName,
    MeasurementCharacteristic.Name AS MeasurementCharacteristic,
    MeasurementType.Name AS MeasurementType,
    Channel.HarmonicGroup,
    Phase.Name AS Phase
FROM
    HourOfWeekLimit JOIN
    Channel ON HourOfWeekLimit.ChannelID = Channel.ID JOIN
    AlarmType ON HourOfWeekLimit.AlarmTypeID = AlarmType.ID JOIN
    Meter ON Channel.MeterID = Meter.ID JOIN
    MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
    MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN
    Phase ON Channel.PhaseID = Phase.ID
GROUP BY
    Channel.Name,
    Channel.MeterID,
    Channel.ID,
    AlarmType.Name,
    MeasurementCharacteristic.Name,
    MeasurementType.Name,
    Channel.HarmonicGroup,
    Phase.Name
GO

CREATE VIEW MetersWithNormalLimits
AS
SELECT
    Meter.Name,
    COUNT(DISTINCT Channel.ID) AS Limits,
    Meter.ID
FROM
    AlarmRangeLimit JOIN
    Channel ON AlarmRangeLimit.ChannelID = Channel.ID JOIN
    Meter ON Channel.MeterID = Meter.ID
GROUP BY
    Meter.Name,
    Meter.ID
GO

CREATE VIEW ChannelsWithNormalLimits
AS
SELECT
    Channel.Name,
    Channel.ID,
    Channel.MeterID,
    AlarmType.Name AS AlarmTypeName,
    MeasurementCharacteristic.Name AS MeasurementCharacteristic,
    MeasurementType.Name AS MeasurementType,
    Channel.HarmonicGroup,
    Phase.Name AS Phase,
    High,
    Low,
    RangeInclusive,
    PerUnit,
    AlarmRangeLimit.Enabled,
    IsDefault
FROM
    AlarmRangeLimit JOIN
    Channel ON AlarmRangeLimit.ChannelID = Channel.ID JOIN
    AlarmType ON AlarmRangeLimit.AlarmTypeID = AlarmType.ID JOIN
    Meter ON Channel.MeterID = Meter.ID JOIN
    MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
    MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN
    Phase ON Channel.PhaseID = Phase.ID
GO

-- Each user can update this to create their own scalar stat view in openSEE
CREATE VIEW OpenSEEScalarStatView AS
	SELECT
    Event.ID AS EventID,
    Location.Name AS Station,
    Meter.Name AS Meter,
    Asset.AssetKey AS AssetKey,
	Asset.AssetName,
    EventType.Name AS [Event Type],
    FORMAT(DATEDIFF(MILLISECOND, Event.StartTime, Event.EndTime) / 1000.0, '0.###') AS [File Duration (seconds)],
    FORMAT(DATEDIFF(MILLISECOND, Event.StartTime, Event.EndTime) * System.Frequency / 1000.0, '0.##') AS [File Duration (cycles)],
    FORMAT(FaultSummary.Distance, '0.##') AS [Fault Distance (mi)],
    FORMAT(FaultSummary.DurationSeconds * 1000.0, '0') AS [Fault Duration (ms)],
    FORMAT(FaultSummary.DurationCycles, '0.##') AS [Fault Duration (cycles)],
    FORMAT(Sag.MagnitudePercent, '0.0') AS [Sag Magnitude (%)],
    FORMAT(Sag.MagnitudeVolts, '0') AS [Sag Magnitude (RMS volts)],
    FaultSummary.Algorithm,
    FORMAT(EventStat.VPeak, '0') AS [Voltage Peak (volts)],
    FORMAT(EventStat.VAMax, '0') AS [VA Maximum (RMS volts)],
    FORMAT(EventStat.VBMax, '0') AS [VB Maximum (RMS volts)],
    FORMAT(EventStat.VCMax, '0') AS [VC Maximum (RMS volts)],
    FORMAT(EventStat.VABMax, '0') AS [VAB Maximum (RMS volts)],
    FORMAT(EventStat.VBCMax, '0') AS [VBC Maximum (RMS volts)],
    FORMAT(EventStat.VCAMax, '0') AS [VCA Maximum (RMS volts)],
    FORMAT(EventStat.VAMin, '0') AS [VA Minimum (RMS volts)],
    FORMAT(EventStat.VBMin, '0') AS [VB Minimum (RMS volts)],
    FORMAT(EventStat.VCMin, '0') AS [VC Minimum (RMS volts)],
    FORMAT(EventStat.VABMin, '0') AS [VAB Minimum (RMS volts)],
    FORMAT(EventStat.VBCMin, '0') AS [VBC Minimum (RMS volts)],
    FORMAT(EventStat.VCAMin, '0') AS [VCA Minimum (RMS volts)],
    FORMAT(EventStat.IPeak, '0') AS [Current Peak (Amps)],
    FORMAT(EventStat.IAMax, '0') AS [IA Maximum (RMS Amps)],
    FORMAT(EventStat.IBMax, '0') AS [IB Maximum (RMS Amps)],
    FORMAT(EventStat.ICMax, '0') AS [IC Maximum (RMS Amps)],
    FORMAT(EventStat.IA2t, '0') AS [IA I2t (A2s)],
    FORMAT(EventStat.IB2t, '0') AS [IB I2t (A2s)],
    FORMAT(EventStat.IC2t, '0') AS [IC I2t (A2s)],
    VAN.Mapping AS [VAN Channel],
    VBN.Mapping AS [VBN Channel],
    VCN.Mapping AS [VCN Channel],
    IAN.Mapping AS [IAN Channel],
    IBN.Mapping AS [IBN Channel],
    ICN.Mapping AS [ICN Channel],
    IR.Mapping AS [IR Channel],
 	FORMAT(RP.Imax1, '0.000') AS [Lmax 1],
	FORMAT(RP.Imax2, '0.000') AS [Lmax 2],
 	FORMAT(RP.TripInitiate,'HH:mm:ss.fff') AS [Trip Initiation],
 	(RP.TripTime / 10) AS [Trip Time (microsec)],
 	(RP.PickupTime / 10) AS [Pickup Time (microsec)],
 	FORMAT(RP.TripCoilCondition, '0.000') AS [Trip Coil Condition (Aps)]
FROM
     Event JOIN
     MeterAsset ON
         Event.MeterID = MeterAsset.MeterID AND
         Event.AssetID = MeterAsset.AssetID JOIN
     Meter ON Event.MeterID = Meter.ID JOIN
     Location ON Meter.LocationID = Location.ID JOIN
     Asset ON Event.AssetID = Asset.ID JOIN
     EventType ON Event.EventTypeID = EventType.ID LEFT OUTER JOIN
     FaultSummary ON
         Event.ID = FaultSummary.EventID AND
         FaultSummary.IsSelectedAlgorithm <> 0 AND
         FaultSummary.FaultNumber = 1 LEFT OUTER JOIN
     EventStat ON Event.ID = EventStat.EventID LEFT OUTER JOIN
     ChannelDetail VAN ON
         Event.MeterID = VAN.MeterID AND
         Event.AssetID = VAN.AssetID AND
         VAN.MeasurementType = 'Voltage' AND
         VAN.Phase = 'AN' AND
         VAN.MeasurementCharacteristic = 'Instantaneous' AND
         VAN.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
     ChannelDetail VBN ON
         Event.MeterID = VBN.MeterID AND
         Event.AssetID = VBN.AssetID AND
         VBN.MeasurementType = 'Voltage' AND
         VBN.Phase = 'BN' AND
         VBN.MeasurementCharacteristic = 'Instantaneous' AND
         VBN.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
     ChannelDetail VCN ON
         Event.MeterID = VCN.MeterID AND
         Event.AssetID = VCN.AssetID AND
         VCN.MeasurementType = 'Voltage' AND
         VCN.Phase = 'CN' AND
         VCN.MeasurementCharacteristic = 'Instantaneous' AND
         VCN.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
     ChannelDetail IAN ON
         Event.MeterID = IAN.MeterID AND
         Event.AssetID = IAN.AssetID AND
         IAN.MeasurementType = 'Current' AND
         IAN.Phase = 'AN' AND
         IAN.MeasurementCharacteristic = 'Instantaneous' AND
         IAN.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
     ChannelDetail IBN ON
         Event.MeterID = IBN.MeterID AND
         Event.AssetID = IBN.AssetID AND
         IBN.MeasurementType = 'Current' AND
         IBN.Phase = 'BN' AND
         IBN.MeasurementCharacteristic = 'Instantaneous' AND
         IBN.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
     ChannelDetail ICN ON
        Event.MeterID = ICN.MeterID AND
         Event.AssetID = ICN.AssetID AND
         ICN.MeasurementType = 'Current' AND
         ICN.Phase = 'CN' AND
         ICN.MeasurementCharacteristic = 'Instantaneous' AND
         ICN.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
     ChannelDetail IR ON
         Event.MeterID = IR.MeterID AND
         Event.AssetID = IR.AssetID AND
         IR.MeasurementType = 'Current' AND
         IR.Phase = 'RES' AND
         IR.MeasurementCharacteristic = 'Instantaneous' AND
         IR.SeriesType IN ('Values', 'Instantaneous') LEFT OUTER JOIN
 	RelayPerformance RP ON
 		Event.ID = RP.EventID AND
 		RP.ChannelID IN 
 		(
 			SELECT ID fROM ChannelDetail RPD WHERE 
 				Event.MeterID = RPD.MeterID AND
 				Event.AssetID = RPD.AssetID
 		) CROSS JOIN
     (
         SELECT COALESCE(CONVERT(FLOAT,
         (
             SELECT TOP 1 Value
             FROM Setting
             WHERE Name = 'SystemFrequency'
         )), 60.0) AS Frequency
     ) System OUTER APPLY
     (
         SELECT TOP 1
             Disturbance.PerUnitMagnitude * 100 AS MagnitudePercent,
             Disturbance.Magnitude AS MagnitudeVolts
         FROM
             Disturbance JOIN
             EventType ON
                 Disturbance.EventTypeID = EventType.ID AND
                 EventType.Name = 'Sag' JOIN
             Phase ON
                 Disturbance.PhaseID = Phase.ID AND
                 Phase.Name = 'Worst'
         WHERE
             Disturbance.EventID = Event.ID AND
             Disturbance.StartTime <= dbo.AdjustDateTime2(FaultSummary.Inception, FaultSummary.DurationSeconds) AND
             Disturbance.EndTime >= FaultSummary.Inception
     ) Sag
 GO


CREATE FUNCTION RecursiveMeterSearch(@assetGroupID int)
RETURNS TABLE
AS
RETURN
    WITH AssetGroupHeirarchy AS
    (
        SELECT ParentAssetGroupID, ChildAssetGroupID
        FROM AssetGroupAssetGroup
        WHERE ParentAssetGroupID = @assetGroupID  -- anchor member
        UNION ALL
        SELECT b.ParentAssetGroupID, a.ChildAssetGroupID -- recursive member
        FROM
            AssetGroupAssetGroup AS a JOIN
            AssetGroupHeirarchy AS b ON b.ChildAssetGroupID = a.ParentAssetGroupID
    )
    SELECT DISTINCT MeterID AS ID
    FROM
        MeterAssetGroup LEFT JOIN
        AssetGroupHeirarchy ON MeterAssetGroup.AssetGroupID = AssetGroupHeirarchy.ChildAssetGroupID
    WHERE
        MeterAssetGroup.AssetGroupID = @assetGroupID OR
        MeterAssetGroup.AssetGroupID IN (SELECT ChildAssetGroupID FROM AssetGroupHeirarchy)
GO


----- PROCEDURES -----

CREATE PROCEDURE GetSystemEvent
    @startTime DATETIME2,
    @endTime DATETIME2,
    @timeTolerance FLOAT
AS BEGIN
    SELECT *
    FROM Event
    WHERE ID IN (SELECT * FROM dbo.GetSystemEventIDs(@startTime, @endTime, @timeTolerance))
END
GO

CREATE PROCEDURE GetPQIFacility
    @facilityID INT
AS BEGIN
    SELECT
        NULL AS FacilityName,
        NULL AS FacilityVoltage,
        NULL AS UtilitySupplyVoltage,
        NULL AS Address1,
        NULL AS Address2,
        NULL AS City,
        NULL AS StateOrProvince,
        NULL AS PostalCode,
        NULL AS Country,
        NULL AS CompanyName,
        NULL AS Industry
    WHERE
        1 IS NULL
END
GO

CREATE PROCEDURE GetImpactedComponents
    @facilityID INT,
    @magnitude FLOAT,
    @duration FLOAT
AS BEGIN
    SELECT
        NULL AS Facility,
        NULL AS Area,
        NULL AS SectionTitle,
        NULL AS SectionRank,
        NULL AS ComponentModel,
        NULL AS ManufacturerName,
        NULL AS SeriesName,
        NULL AS ComponentTypeName
    WHERE
        1 IS NULL
END
GO

CREATE PROCEDURE GetAllImpactedComponents
    @eventID INT
AS BEGIN
    DECLARE @facilityID INT
    DECLARE @magnitude FLOAT
    DECLARE @duration FLOAT
    DECLARE @subeventid INT
    CREATE TABLE #temp
    (
        Facility VARCHAR(64),
        Area VARCHAR(256),
        SectionTitle VARCHAR(256),
        SectionRank INT,
        ComponentModel VARCHAR(64),
        ManufacturerName VARCHAR(64),
        SeriesName VARCHAR(64),
        ComponentTypeName VARCHAR(32),
		EventID int
    )

    DECLARE dbCursor CURSOR FOR
    SELECT
        FacilityID,
        PerUnitMagnitude,
        DurationSeconds,
        Event.ID as EventID
    FROM
        Disturbance JOIN
        Event ON Disturbance.EventID = Event.ID JOIN
        MeterFacility ON MeterFacility.MeterID = Event.MeterID
    WHERE
        EventID = @eventID

    OPEN dbCursor
    FETCH NEXT FROM dbCursor INTO @facilityID, @magnitude, @duration, @subeventid

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #temp
        EXEC GetImpactedComponents @facilityID, @magnitude, @duration, @subeventid

        FETCH NEXT FROM dbCursor INTO @facilityID, @magnitude, @duration, @subeventid
    END

    CLOSE dbCursor
    DEALLOCATE dbCursor

    SELECT DISTINCT * FROM #temp

    DROP TABLE #temp
END
GO

-- Author: Kevin Conner
-- Source: http://stackoverflow.com/questions/116968/in-sql-server-2005-can-i-do-a-cascade-delete-without-setting-the-property-on-my
CREATE procedure usp_delete_cascade (
    @base_table_name varchar(200), @base_criteria nvarchar(1000)
)
as begin
    -- Adapted from http://www.sqlteam.com/article/performing-a-cascade-delete-in-sql-server-7
    -- Expects the name of a table, and a conditional for selecting rows
    -- within that table that you want deleted.
    -- Produces SQL that, when run, deletes all table rows referencing the ones
    -- you initially selected, cascading into any number of tables,
    -- without the need for "ON DELETE CASCADE".
    -- Does not appear to work with self-referencing tables, but it will
    -- delete everything beneath them.
    -- To make it easy on the server, put a "GO" statement between each line.

    declare @to_delete table (
        id int identity(1, 1) primary key not null,
        criteria nvarchar(1000) not null,
        table_name varchar(200) not null,
        processed bit not null,
        delete_sql varchar(1000)
    )

    insert into @to_delete (criteria, table_name, processed) values (@base_criteria, @base_table_name, 0)

    declare @id int, @criteria nvarchar(1000), @table_name varchar(200)
    while exists(select 1 from @to_delete where processed = 0) begin
        select top 1 @id = id, @criteria = criteria, @table_name = table_name from @to_delete where processed = 0 order by id desc

        insert into @to_delete (criteria, table_name, processed)
            select referencing_column.name + ' in (select [' + referenced_column.name + '] from [' + @table_name +'] where ' + @criteria + ')',
                referencing_table.name,
                0
            from  sys.foreign_key_columns fk
                inner join sys.columns referencing_column on fk.parent_object_id = referencing_column.object_id
                    and fk.parent_column_id = referencing_column.column_id
                inner join  sys.columns referenced_column on fk.referenced_object_id = referenced_column.object_id
                    and fk.referenced_column_id = referenced_column.column_id
                inner join  sys.objects referencing_table on fk.parent_object_id = referencing_table.object_id
                inner join  sys.objects referenced_table on fk.referenced_object_id = referenced_table.object_id
                inner join  sys.objects constraint_object on fk.constraint_object_id = constraint_object.object_id
            where referenced_table.name = @table_name
                and referencing_table.name != referenced_table.name

        update @to_delete set
            processed = 1
        where id = @id
    end

    select 'print ''deleting from ' + table_name + '...''; delete from [' + table_name + '] where ' + criteria from @to_delete order by id desc
end
GO

-- =============================================
-- Author:      <Author, William Ernest/ Stephen Wills>
-- Create date: <Create Date,12/1/2016>
-- Description: <Description, Calls usp_delete_cascade to perform cascading deletes for a table>
-- =============================================
CREATE PROCEDURE UniversalCascadeDelete
    @tableName VARCHAR(200),
    @baseCriteria NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @deleteSQL NVARCHAR(900)

    CREATE TABLE #DeleteCascade
    (
        DeleteSQL NVARCHAR(900)
    )

    INSERT INTO #DeleteCascade
    EXEC usp_delete_cascade @tableName, @baseCriteria

    DECLARE DeleteCursor CURSOR FOR
    SELECT *
    FROM #DeleteCascade

    OPEN DeleteCursor

    FETCH NEXT FROM DeleteCursor
    INTO @deleteSQL

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC sp_executesql @deleteSQL

        FETCH NEXT FROM DeleteCursor
        INTO @deleteSQL
    END

    CLOSE DeleteCursor
    DEALLOCATE DeleteCursor

    DROP TABLE #DeleteCascade
END
GO

CREATE PROCEDURE InsertIntoAuditLog(@tableName VARCHAR(128), @primaryKeyColumn VARCHAR(128), @deleted BIT = '0', @inserted BIT = '0') AS
BEGIN
    DECLARE @columnName varchar(100)
    DECLARE @cursorColumnNames CURSOR

    SET @cursorColumnNames = CURSOR FOR
    SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND TABLE_CATALOG = db_name()

    OPEN @cursorColumnNames

    FETCH NEXT FROM @cursorColumnNames INTO @columnName
    WHILE @@FETCH_STATUS = 0
    BEGIN

        DECLARE @sql VARCHAR(MAX)

        IF @deleted = '0' AND @inserted = '0'
        BEGIN
            SET @sql = 'DECLARE @oldVal NVARCHAR(MAX) ' +
                    'DECLARE @newVal NVARCHAR(MAX) ' +
                    'SELECT @oldVal = CONVERT(NVARCHAR(MAX), #deleted.' + @columnName + '), @newVal = CONVERT(NVARCHAR(MAX), #inserted.' + @columnName + ') FROM #deleted, #inserted ' +
                    'IF @oldVal <> @newVal BEGIN ' +
                    'INSERT INTO AuditLog (TableName, PrimaryKeyColumn, PrimaryKeyValue, ColumnName, OriginalValue, NewValue, Deleted, UpdatedBy) ' +
                    'SELECT ''' + @tableName + ''', ''' + @primaryKeyColumn + ''', CONVERT(NVARCHAR(MAX), #inserted.' + @primaryKeyColumn + '), ''' + @columnName + ''', ' +
                    'CONVERT(NVARCHAR(MAX), #deleted.' + @columnName + '), CONVERT(NVARCHAR(MAX), #inserted.' + @columnName + '), ''0'', #inserted.UpdatedBy ' +
                    'FROM #inserted JOIN #deleted ON #inserted.' + @primaryKeyColumn + ' = #deleted.' + @primaryKeyColumn + ' ' +
                    'END'

            EXECUTE (@sql)
        END

        FETCH NEXT FROM @cursorColumnNames INTO @columnName
    END

    CLOSE @cursorColumnNames
    DEALLOCATE @cursorColumnNames
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER Event_AuditUpdate
   ON  Event
   AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * INTO #deleted  FROM deleted
    SELECT * INTO #inserted FROM inserted
    EXEC InsertIntoAuditLog 'Event', 'ID'
    DROP TABLE #inserted
    DROP TABLE #deleted

END
GO

CREATE TRIGGER Disturbance_AuditUpdate
   ON Disturbance
   AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * INTO #deleted  FROM deleted
    SELECT * INTO #inserted FROM inserted
    EXEC InsertIntoAuditLog 'Disturbance', 'ID'
    DROP TABLE #inserted
    DROP TABLE #deleted
END
GO

CREATE TRIGGER BreakerOperation_AuditUpdate
   ON  BreakerOperation
   AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * INTO #deleted  FROM deleted
    SELECT * INTO #inserted FROM inserted
    EXEC InsertIntoAuditLog 'BreakerOperation', 'ID'
    DROP TABLE #inserted
    DROP TABLE #deleted
END
GO


----- Email Templates -----

UPDATE EventEmailParameters
SET TriggersEmailSQL = 'SELECT
    CASE WHEN EventType.Name = ''Fault''
        THEN 1
        ELSE 0
    END
FROM
    Event JOIN
    EventType ON Event.EventTypeID = EventType.ID
WHERE Event.ID = {0}'
WHERE EventEmailParameters.ID = 2
GO

UPDATE EventEmailParameters
SET EventDetailSQL = 'DECLARE @timeTolerance FLOAT = (SELECT CAST(Value AS FLOAT) FROM Setting WHERE Name = ''TimeTolerance'')
DECLARE @lineID INT
DECLARE @startTime DATETIME2
DECLARE @endTime DATETIME2

SELECT
    @lineID = LineID,
    @startTime = dbo.AdjustDateTime2(StartTime, -@timeTolerance),
    @endTime = dbo.AdjustDateTime2(EndTime, @timeTolerance)
FROM Event
WHERE ID = {0}

SELECT *
INTO #lineEvent
FROM Event
WHERE
    Event.LineID = @lineID AND
    Event.EndTime >= @startTime AND
    Event.StartTime <= @endTime

SELECT
    ROW_NUMBER() OVER(PARTITION BY Event.MeterID ORDER BY FaultSummary.Inception) AS FaultNumber,
    FaultSummary.ID AS FaultSummaryID,
    Meter.AssetKey AS MeterKey,
    Meter.Name AS MeterName,
    MeterLocation.AssetKey as StationKey,
    MeterLocation.Name AS StationName,
    Line.AssetKey AS LineKey,
    MeterLine.LineName,
    FaultSummary.FaultType,
    FaultSummary.Inception,
    FaultSummary.DurationCycles,
    FaultSummary.DurationSeconds * 1000.0 AS DurationMilliseconds,
    CASE WHEN FaultSummary.PrefaultCurrent <> -1E308 THEN FORMAT(FaultSummary.PrefaultCurrent, ''0.##########'') ELSE ''NaN'' END AS PrefaultCurrent,
    CASE WHEN FaultSummary.PostfaultCurrent <> -1E308 THEN FORMAT(FaultSummary.PostfaultCurrent, ''0.##########'') ELSE ''NaN'' END AS PostfaultCurrent,
    FaultSummary.ReactanceRatio,
    FaultSummary.CurrentMagnitude AS FaultCurrent,
    FaultSummary.Algorithm,
    FaultSummary.Distance AS SingleEndedDistance,
    DoubleEndedFaultSummary.Distance AS DoubleEndedDistance,
    DoubleEndedFaultSummary.Angle AS DoubleEndedAngle,
    RIGHT(DataFile.FilePath, CHARINDEX(CHAR(92), REVERSE(DataFile.FilePath)) - 1) AS FileName,
    FaultSummary.EventID,
    Event.StartTime AS EventStartTime,
    SimpleSummary.Distance AS Simple,
    ReactanceSummary.Distance AS Reactance,
    Event.EndTime AS EventEndTime
INTO #summaryData
FROM
    #lineEvent Event JOIN
    EventType ON
        Event.EventTypeID = EventType.ID AND
        EventType.Name = ''Fault'' JOIN
    FaultSummary ON
        FaultSummary.EventID = Event.ID AND
        FaultSummary.IsSelectedAlgorithm <> 0 JOIN
    FaultSummary SimpleSummary ON
        FaultSummary.EventID = SimpleSummary.EventID AND
        FaultSummary.Inception = SimpleSummary.Inception AND
        SimpleSummary.Algorithm = ''Simple'' LEFT OUTER JOIN
    FaultSummary ReactanceSummary ON
        FaultSummary.EventID = ReactanceSummary.EventID AND
        FaultSummary.Inception = ReactanceSummary.Inception AND
        ReactanceSummary.Algorithm = ''Reactance'' JOIN
    DataFile ON DataFile.FileGroupID = Event.FileGroupID JOIN
    Meter ON Event.MeterID = Meter.ID JOIN
    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
    MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Event.LineID JOIN
    Line ON Line.ID=MeterLine.LineID LEFT OUTER JOIN
    DoubleEndedFaultDistance ON DoubleEndedFaultDistance.LocalFaultSummaryID = FaultSummary.ID LEFT OUTER JOIN
    DoubleEndedFaultSummary ON DoubleEndedFaultSummary.ID = DoubleEndedFaultDistance.ID
WHERE
    DataFile.FilePath LIKE ''%.DAT'' OR
    DataFile.FilePath LIKE ''%.D00'' OR
    DataFile.FilePath LIKE ''%.PQD'' OR
    DataFile.FilePath LIKE ''%.RCD'' OR
    DataFile.FilePath LIKE ''%.RCL'' OR
    DataFile.FilePath LIKE ''%.SEL'' OR
    DataFile.FilePath LIKE ''%.EVE'' OR
    DataFile.FilePath LIKE ''%.CEV''

DECLARE @url VARCHAR(MAX) = (SELECT Value FROM DashSettings WHERE Name = ''System.URL'')

SELECT
    (
        SELECT ID AS [@id]
        FROM #lineEvent
        FOR XML PATH(''Event''), TYPE
    ) AS [Events],
    (
        SELECT
            FaultNumber AS [@num],
            (
                SELECT
                    MeterKey,
                    MeterName,
                    StationKey,
                    StationName,
                    LineKey,
                    LineName,
                    FaultType,
                    Inception,
                    DurationCycles,
                    DurationMilliseconds,
                    PrefaultCurrent,
                    PostfaultCurrent,
                    ReactanceRatio,
                    FaultCurrent,
                    Algorithm,
                    SingleEndedDistance,
                    DoubleEndedDistance,
                    DoubleEndedAngle,
                    EventStartTime,
                    EventEndTime,
                    FileName,
                    EventID,
                    FaultSummaryID AS FaultID,
                    CASE WHEN ABS(Reactance/COALESCE(Simple,1)) > 0.6 THEN ''LOW''
                            WHEN ABS(Reactance/COALESCE(Simple,1)) < 0.4 THEN ''HIGH''
                            ELSE ''MEDIUM''
                    END AS Ratio
                FROM #summaryData
                WHERE FaultNumber = Fault.FaultNumber
                FOR XML PATH(''SummaryData''), TYPE
            )
        FROM
        (
            SELECT DISTINCT FaultNumber
            FROM #summaryData
        ) Fault
        FOR XML PATH(''Fault''), TYPE
    ) AS [Faults],
    MeterLine.LineName AS [Line/Name],
    Line.AssetKey AS [Line/AssetKey],
    FORMAT(Line.Length, ''0.##########'') AS [Line/Length],
    FORMAT(SQRT(LineImpedance.R1 * LineImpedance.R1 + LineImpedance.X1 * LineImpedance.X1), ''0.##########'') AS [Line/Z1],
    CASE LineImpedance.R1 WHEN 0 THEN ''0'' ELSE FORMAT(ATN2(LineImpedance.X1, LineImpedance.R1) * 180 / PI(), ''0.##########'') END AS [Line/A1],
    FORMAT(LineImpedance.R1, ''0.##########'') AS [Line/R1],
    FORMAT(LineImpedance.X1, ''0.##########'') AS [Line/X1],
    FORMAT(SQRT(LineImpedance.R0 * LineImpedance.R0 + LineImpedance.X0 * LineImpedance.X0), ''0.##########'') AS [Line/Z0],
    CASE LineImpedance.R0 WHEN 0 THEN ''0'' ELSE FORMAT(ATN2(LineImpedance.X0, LineImpedance.R0) * 180 / PI(), ''0.##########'') END AS [Line/A0],
    FORMAT(LineImpedance.R0, ''0.##########'') AS [Line/R0],
    FORMAT(LineImpedance.X0, ''0.##########'') AS [Line/X0],
    FORMAT(SQRT(POWER((2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0, 2) + POWER((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, 2)), ''0.##########'') AS [Line/ZS],
    CASE 2.0 * LineImpedance.R1 + LineImpedance.R0 WHEN 0 THEN ''0'' ELSE FORMAT(ATN2((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, (2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0) * 180 / PI(), ''0.##########'') END AS [Line/AS],
    FORMAT((2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0, ''0.##########'') AS [Line/RS],
    FORMAT((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, ''0.##########'') AS [Line/XS],
    @url AS [PQDashboard]
FROM
    Event JOIN
    Line ON Event.LineID = Line.ID JOIN
    MeterLine ON
        MeterLine.MeterID = Event.MeterID AND
        MeterLine.LineID = Event.LineID JOIN
    LineImpedance ON LineImpedance.LineID = Line.ID
WHERE Event.ID = {0}
FOR XML PATH(''EventDetail''), TYPE'
WHERE EventEmailParameters.ID = 2
GO

UPDATE EventEmailParameters
SET TriggersEmailSQL = 'SELECT CASE WHEN (SELECT COUNT(RP.ID) FROM RelayPerformance RP LEFT OUTER JOIN
	EVENT EV ON EV.ID = RP.EventID LEFT OUTER JOIN
	METERLINE ML ON EV.MeterID = ML.ID LEFT OUTER JOIN
	LINE LN ON LN.ID = EV.LineID INNER JOIN
	RelayAlertSetting RA ON RA.LineID = LN.ID
	WHERE RP.EventID = {0}
		AND ((RP.TripTime > 10*RA.TripTime AND RA.TripTime > 0) 
			OR (RP.PickupTime > 10*RA.PickupTime AND RA.PickupTime > 0)
			OR (RP.TripCoilCondition > RA.TripCoilCondition AND RA.TripCoilCondition > 0))
	) > 0 THEN 1 ELSE 0 END'
WHERE EventEmailParameters.ID = 3
GO		
		
UPDATE EventEmailParameters
SET EventDetailSQL = 'DECLARE @url VARCHAR(MAX) = (SELECT Value FROM DashSettings WHERE Name = ''System.URL'')

/* Temporary Tables */
/* Breaker */
SELECT LN.ID AS LineID, ML.LineName AS Name, LN.AssetKey AS AssetKey, 
	RP.TripTime / 10 AS TT, RP.PickupTime / 10 AS PT, RP.TripCoilCondition AS TCC, RP.TripInitiate AS TI, RP.Imax1 AS L1, RP.Imax2 AS L2,
	( SELECT CASE WHEN (RP.TripTime > 10*RA.TripTime AND RA.TripTime > 0) THEN 1 ELSE 0 END ) AS TTAlert,
	( SELECT CASE WHEN (RP.PickupTime > 10*RA.PickupTime AND RA.PickupTime > 0) THEN 1 ELSE 0 END ) AS PTAlert,
	( SELECT CASE WHEN (RP.TripCoilCondition > RA.TripCoilCondition AND RA.TripCoilCondition > 0) THEN 1 ELSE 0 END ) AS TCCAlert,
	( SELECT CASE WHEN
		(RP.TripCoilCondition > RA.TripCoilCondition AND RA.TripCoilCondition > 0) OR
		(RP.PickupTime > 10 * RA.PickupTime AND RA.PickupTime > 0) OR
		(RP.TripTime > 10 * RA.TripTime AND RA.TripTime > 0)
		THEN 1 ELSE 0 END 
	) AS Alert
	INTO #Breaker 
	FROM RelayPerformance RP LEFT OUTER JOIN
	EVENT EV ON EV.ID = RP.EventID LEFT OUTER JOIN
	METERLINE ML ON EV.MeterID = ML.ID LEFT OUTER JOIN
	LINE LN ON LN.ID = EV.LineID LEFT OUTER JOIN
	RelayAlertSetting RA ON RA.LineID = LN.ID
	WHERE RP.EventID = {0}

/* Alert */
SELECT ML.LineName AS Name, LN.AssetKey AS AssetKey
	INTO #Alert 
	FROM RelayPerformance RP LEFT OUTER JOIN
	EVENT EV ON EV.ID = RP.EventID LEFT OUTER JOIN
	METERLINE ML ON EV.MeterID = ML.ID LEFT OUTER JOIN
	LINE LN ON LN.ID = EV.LineID INNER JOIN
	RelayAlertSetting RA ON RA.LineID = LN.ID
	WHERE RP.EventID = {0}
		AND ((RP.TripTime > 10 * RA.TripTime AND RA.TripTime > 0) 
			OR (RP.PickupTime > 10 * RA.PickupTime AND RA.PickupTime > 0)
			OR (RP.TripCoilCondition > RA.TripCoilCondition AND RA.TripCoilCondition > 0))

/* History */
SELECT Line.ID AS LineID, Relay.EventID AS EventID, Relay.PickupTime / 10 AS PT, Relay.TripTime / 10 AS TT, Relay.TripCoilCondition AS TCC,  Relay.Imax1 AS L1, Relay.Imax2 AS L2, Relay.TripInitiate AS TI,
	( SELECT CASE WHEN (Relay.TripTime >10 * RelayAlert.TripTime AND RelayAlert.TripTime > 0) THEN 1 ELSE 0 END ) AS TTAlert,
	( SELECT CASE WHEN (Relay.PickupTime > 10 * RelayAlert.PickupTime AND RelayAlert.PickupTime > 0) THEN 1 ELSE 0 END ) AS PTAlert,
	( SELECT CASE WHEN (Relay.TripCoilCondition > RelayAlert.TripCoilCondition AND RelayAlert.TripCoilCondition > 0) THEN 1 ELSE 0 END ) AS TCCAlert,
	( SELECT CASE WHEN
		(Relay.TripCoilCondition > RelayAlert.TripCoilCondition AND RelayAlert.TripCoilCondition > 0) OR
		(Relay.PickupTime > RelayAlert.PickupTime AND RelayAlert.PickupTime > 0) OR
		(Relay.TripTime > RelayAlert.TripTime AND RelayAlert.TripTime > 0)
		THEN 1 ELSE 0 END 
	) AS Alert
	INTO #History FROM  
	RelayPerformance Relay LEFT OUTER JOIN
	Channel ON Relay.ChannelID = Channel.ID LEFT OUTER JOIN
	Line ON Channel.LineID = Line.ID LEFT OUTER JOIN
	RelayAlertSetting RelayAlert ON RelayAlert.LineID = Line.ID
	WHERE Relay.EventID <> {0}

/* Event */
SELECT EV.StartTime, EV.EndTime, STAT.IA2t, STAT.IB2t, STAT.IC2t,
	EVT.Description AS EventType,
	(SELECT CASE 
		WHEN ((SELECT COUNT(FaultSummary.ID) FROM FaultSummary WHERE FaultSummary.IsSelectedAlgorithm <> 0 AND FaultSummary.EventID = {0}) > 0) 
			THEN (SELECT Fault.DurationSeconds * 1000) 
			ELSE (SELECT DATEDIFF(millisecond, EV.StartTime, EV.EndTime)) 
		END
	) AS EventDuration
	INTO #EventDetails FROM 
	EVENT EV LEFT OUTER JOIN 
	EventStat STAT ON STAT.EventID = EV.ID LEFT OUTER JOIN
	EventType EVT ON EVT.ID = EV.EventTypeID LEFT OUTER JOIN
	FaultSummary Fault ON Fault.EventID = EV.ID
	WHERE EV.Id = {0} AND (Fault.IsSelectedAlgorithm <> 0 OR Fault.IsSelectedAlgorithm IS NULL)


SELECT @url AS [PQDashboard],
	(SELECT * FROM #ALERT FOR XML PATH(''Breaker''), TYPE) AS [Alerts],
	( SELECT 
		Name, AssetKey, 
		TT, PT, TCC, TI, L1, L2,
		Alert, TTAlert, PTAlert, TCCAlert,
		(
			SELECT TT, PT, TCC, TI, L1, L2, Alert, TTAlert, PTAlert, TCCAlert FROM #History H WHERE H.LineID = B.LineID FOR XML PATH(''Data''), TYPE
		) AS History FROM #Breaker B FOR XML PATH(''Breaker''), TYPE
	) AS Breakers,
	( SELECT * FROM #EventDetails FOR XML PATH(''Event''), TYPE) AS EventDetail
	FOR XML PATH(''AlertDetail'')'
WHERE EventEmailParameters.ID = 3
GO



UPDATE XSLTemplate
SET Template = '<?xml version="1.0"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" />

<xsl:template match="/">
    <html>
    <head>
        <title>Fault detected on <xsl:value-of select="/EventDetail/Line/Name" /> (<xsl:value-of select="/EventDetail/Line/AssetKey" />)</title>

        <style>
            th, td {
                border-spacing: 0;
                border-collapse: collapse;
                padding-left: 5px;
                padding-right: 5px
            }

            .fault-details {
                margin-left: 1cm
            }

            .fault-header {
                font-size: 120%;
                font-weight: bold;
                text-decoration: underline
            }

            table {
                border-spacing: 0;
                border-collapse: collapse
            }

            table.left tr th, table.left tr td {
                border: 1px solid black
            }

            table.center tr th, table.center tr td {
                border: 1px solid black;
                text-align: center
            }
        </style>
    </head>
    <body>
        <xsl:for-each select="/EventDetail/Faults/Fault">
            <span class="fault-header">Fault <xsl:value-of select="@num" /></span> - <format type="System.DateTime" spec="yyyy-MM-dd HH:mm:ss.fffffff"><xsl:value-of select="SummaryData[1]/Inception" /></format>
            <div class="fault-details">
                <xsl:variable name="deDistance" select="SummaryData/DoubleEndedDistance" />

                <table>
                    <xsl:for-each select="SummaryData">
                        <tr>
                            <td><xsl:if test="position() = 1">DFRs:</xsl:if></td>
                            <td><xsl:value-of select="MeterKey" /> at <xsl:value-of select="StationName" /> triggered at <format type="System.DateTime" spec="HH:mm:ss.fffffff"><xsl:value-of select="EventStartTime" /></format> (<a><xsl:attribute name="href"><xsl:value-of select="/EventDetail/PQDashboard" />/Main/OpenSEE?eventid=<xsl:value-of select="EventID" />&amp;faultcurves=1</xsl:attribute>click for waveform</a>)</td>
                        </tr>
                    </xsl:for-each>

                    <tr>
                        <td>&amp;nbsp;</td>
                        <td>&amp;nbsp;</td>
                    </tr>

                    <xsl:for-each select="SummaryData">
                        <tr>
                            <td><xsl:if test="position() = 1">Files:</xsl:if></td>
                            <td><xsl:value-of select="FileName" /></td>
                        </tr>
                    </xsl:for-each>

                    <tr>
                        <td>&amp;nbsp;</td>
                        <td>&amp;nbsp;</td>
                    </tr>

                    <tr>
                        <td>Line:</td>
                        <td><xsl:value-of select="/EventDetail/Line/Name" /> (<format type="System.Double" spec="0.00"><xsl:value-of select="/EventDetail/Line/Length" /></format> miles)</td>
                    </tr>
                </table>

                <br />

                <table class="left">
                    <tr>
                        <td style="border: 0"></td>
                        <xsl:for-each select="SummaryData">
                            <td style="text-align: center"><xsl:value-of select="StationName" /> - <xsl:value-of select="MeterKey" /></td>
                        </xsl:for-each>
                    </tr>
                    <tr>
                        <td style="border: 0; text-align: right">Fault Type:</td>
                        <xsl:for-each select="SummaryData">
                            <td><xsl:value-of select="FaultType" /></td>
                        </xsl:for-each>
                    </tr>
                    <tr>
                        <td style="border: 0; text-align: right">Inception Time:</td>
                        <xsl:for-each select="SummaryData">
                            <td><format type="System.DateTime" spec="HH:mm:ss.fffffff"><xsl:value-of select="Inception" /></format></td>
                        </xsl:for-each>
                    </tr>
                    <tr>
                        <td style="border: 0; text-align: right">Fault Duration:</td>
                        <xsl:for-each select="SummaryData">
                            <td><format type="System.Double" spec="0.000"><xsl:value-of select="DurationMilliseconds" /></format> msec (<format type="System.Double" spec="0.00"><xsl:value-of select="DurationCycles" /></format> cycles)</td>
                        </xsl:for-each>
                    </tr>
                    <tr>
                        <td style="border: 0; text-align: right">Fault Current:</td>
                        <xsl:for-each select="SummaryData">
                            <td><format type="System.Double" spec="0.0"><xsl:value-of select="FaultCurrent" /></format> Amps (RMS)</td>
                        </xsl:for-each>
                    </tr>
                    <tr>
                        <td style="border: 0; text-align: right">Distance Method:</td>
                        <xsl:for-each select="SummaryData">
                            <td><xsl:value-of select="Algorithm" /></td>
                        </xsl:for-each>
                    </tr>
                    <tr>
                        <td style="border: 0; text-align: right">Single-ended Distance:</td>
                        <xsl:for-each select="SummaryData">
                            <td><format type="System.Double" spec="0.000"><xsl:value-of select="SingleEndedDistance" /></format> miles</td>
                        </xsl:for-each>
                    </tr>
                    <xsl:if test="count($deDistance) &gt; 0">
                        <tr>
                            <td style="border: 0; text-align: right">Double-ended Distance:</td>
                            <xsl:for-each select="SummaryData">
                                <td>
                                    <xsl:if test="DoubleEndedDistance">
                                        <format type="System.Double" spec="0.000"><xsl:value-of select="DoubleEndedDistance" /></format> miles
                                    </xsl:if>
                                </td>
                            </xsl:for-each>
                        </tr>
                        <tr>
                            <td style="border: 0; text-align: right">Double-ended Angle:</td>
                            <xsl:for-each select="SummaryData">
                                <td>
                                    <xsl:if test="DoubleEndedDistance">
                                        <format type="System.Double" spec="0.000"><xsl:value-of select="DoubleEndedAngle" /></format>&amp;deg;
                                    </xsl:if>
                                </td>
                            </xsl:for-each>
                        </tr>
                    </xsl:if>
                    <tr>
                        <td style="border: 0; text-align: right">openXDA Event ID:</td>
                        <xsl:for-each select="SummaryData">
                            <td><xsl:value-of select="EventID" /></td>
                        </xsl:for-each>
                    </tr>
                </table>
            </div>
        </xsl:for-each>

        <hr />

        <table class="center" style="width: 600px">
            <tr>
                <th style="border: 0; border-bottom: 1px solid black; border-right: 1px solid black; text-align: center">Line Parameters:</th>
                <th>Value:</th>
                <th>Per Mile:</th>
            </tr>
            <tr>
                <th>Length (Mi)</th>
                <td><pre><format type="System.Double" spec="0.##"><xsl:value-of select="/EventDetail/Line/Length" /></format></pre></td>
                <td><pre>1.0</pre></td>
            </tr>
            <tr>
                <th>
                    Pos-Seq Imp<br />
                    Z1 (Ohm)<br />
                    (LLL,LLLG,LL,LLG)
                </th>
                <td>
                    <pre><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/Z1" /></format>&amp;ang;<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/A1" /></format>&amp;deg;<br /><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/R1" /></format>+j<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/X1" /></format></pre>
                </td>
                <td>
                    <pre><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/Z1 div /EventDetail/Line/Length" /></format>&amp;ang;<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/A1" /></format>&amp;deg;<br /><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/R1 div /EventDetail/Line/Length" /></format>+j<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/Z1 div /EventDetail/Line/Length" /></format></pre>
                </td>
            </tr>
            <tr>
                <th>
                    Zero-Seq Imp<br />
                    Z0 (Ohm)
                </th>
                <td>
                    <pre><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/Z0" /></format>&amp;ang;<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/A0" /></format>&amp;deg;<br /><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/R0" /></format>+j<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/X0" /></format></pre>
                </td>
                <td>
                    <pre><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/Z0 div /EventDetail/Line/Length" /></format>&amp;ang;<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/A0" /></format>&amp;deg;<br /><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/R0 div /EventDetail/Line/Length" /></format>+j<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/X0 div /EventDetail/Line/Length" /></format></pre>
                </td>
            </tr>
            <tr>
                <th>
                    Loop Imp<br />
                    ZS (Ohm)<br />
                    (LG)
                </th>
                <td>
                    <pre><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/ZS" /></format>&amp;ang;<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/AS" /></format>&amp;deg;<br /><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/RS" /></format>+j<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/XS" /></format></pre>
                </td>
                <td>
                    <pre><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/ZS div /EventDetail/Line/Length" /></format>&amp;ang;<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/AS" /></format>&amp;deg;<br /><format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/RS div /EventDetail/Line/Length" /></format>+j<format type="System.Double" spec="0.####"><xsl:value-of select="/EventDetail/Line/XS div /EventDetail/Line/Length" /></format></pre>
                </td>
            </tr>
        </table>

        <hr />

        <p style="font-size: .8em">
            If you would like receive a different set of emails or unsubscribe, you can <a><xsl:attribute name="href"><xsl:value-of select="/EventDetail/PQDashboard" />/Email/UpdateSettings</xsl:attribute>manage your subscriptions</a>.
        </p>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet>'
WHERE Name = 'Default Fault'
GO


UPDATE XSLTemplate
SET Template = '<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" />

<xsl:template match="/">
    <html>
    <head>
        <title>Breaker <xsl:value-of select="/AlertDetail/Alerts/Breaker[1]/Name" /> (<xsl:value-of select="/AlertDetail/Alerts/Breaker[1]/AssetKey" />) trip not within Limits</title>

        <style>
            td {
                border: 1px solid #ddd;
                border-collapse: collapse;
                padding: 8px;
                text-align: center;
            }

            .Trip-details {
            }

            .section-header {
                font-size: 120%;
                font-weight: bold;
                text-decoration: underline
            }

            table {
                border-spacing: 0;
                border-collapse: collapse;
                
            }

            tr:nth-child(even){background-color: #f2f2f2;}
            
            th {
                padding-top: 12px;
                padding-bottom: 12px;
                text-align: center;
                background-color: #4CAF50;
                color: white;
            }
            
            .alert {
                color: #ff0000;
            }
            
            .normal {
                color: #000000;
            }
            
			table.tblstyle-center tr th, table.tblstyle-center tr td
			{
				border-spacing: 0;
                border-collapse: collapse
				border: 1px solid black
				text-align: center

			}
			table.tblstyle-left tr th, table.tblstyle-left tr td,
			{
				border-spacing: 0;
                border-collapse: collapse
				border: 1px solid black
				text-align: left
			}

        </style>
    </head>
    <body>
        <span class="section-header">Tripped Breakers</span>
        <br/><br/>
        <div class="Trip-details">
            <table class="table.tblstyle-left">
                <tr>
                  <th>Breaker</th>
                  <th>Trip Initiated</th>
                  <th>Pickup Time <br />(micros)</th>
                  <th>Trip Time <br />(micros)</th>
                  <th>Imax 1 <br />(A)</th>
                  <th>Imax 2 <br />(A)</th>
                  <th>Trip Coil Condition <br />(A/s)</th>
                </tr>
                <xsl:for-each select="/AlertDetail/Breakers/Breaker">
                    <tr>
                        <xsl:choose>
                            <xsl:when test="Alert = 1">
                                <td class = "alert"> <xsl:value-of select="Name" /> <br/> (<xsl:value-of select="AssetKey" />) </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <xsl:value-of select="Name" /> <br/> (<xsl:value-of select="AssetKey" />) </td>
                            </xsl:otherwise>
                        </xsl:choose>
                        <td> <format type="System.DateTime" spec="HH:mm:ss.ffffff"> <xsl:value-of select="TI" /> </format> </td>
                        <xsl:choose>
                            <xsl:when test="PTAlert = 1">
                                <td class = "alert"> <format type="System.Double" spec="#####"> <xsl:value-of select="PT" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.Double" spec="#####"> <xsl:value-of select="PT" /> </format> </td>
                            </xsl:otherwise>
                        </xsl:choose>
                        <xsl:choose>
                            <xsl:when test="TTAlert = 1">
                                <td class = "alert"> <format type="System.Double" spec="#####"> <xsl:value-of select="TT" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.Double" spec="#####"> <xsl:value-of select="TT" /> </format> </td>
                            </xsl:otherwise>
                        </xsl:choose>
                        <td> <format type="System.Double" spec="#0.###"> <xsl:value-of select="L1" /> </format> </td>
                        <td> <format type="System.Double" spec="#0.###"> <xsl:value-of select="L2" /> </format></td>
                        
                        <xsl:choose>
                            <xsl:when test="TCCAlert = 1">
                                <td class = "alert"> <format type="System.Double" spec="##0.###"> <xsl:value-of select="TCC" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.Double" spec="##0.###"> <xsl:value-of select="TCC" /> </format> </td>
                            </xsl:otherwise>
                        </xsl:choose>
                    </tr>
                </xsl:for-each>
                
            </table>
        </div>
        <hr />
        <span class="section-header">Event Details </span>
        <br/><br/>
		<div class="Event-details">
			<table class="table.tblstyle-left">
				<tr>
                    <td> Start Time:</td>
					<td><format type="System.DateTime" spec="MM/dd/yyyy"> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/StartTime" /> </format>
								<br/> <format type="System.DateTime" spec="HH:mm:ss:ffffff"> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/StartTime" /> </format></td>
                </tr>
				<tr>
                    <td> Event:</td>
					<td> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/EventType" /> </td>
                </tr>
				<tr>
                    <td> Event Duration:</td>
					<td> <format type="System.Double" spec="#####"> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/EventDuration" /></format> ms</td>
                </tr>
                <xsl:if test="/AlertDetail/EventDetail/Event[1]/IA2t">
    				<tr>
                        <td> I2(t) Phase A :</td>
    					<td> <format type="System.Double" spec="#####"> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/IA2t" /> </format>  </td>
                    </tr>
                </xsl:if>
                <xsl:if test="/AlertDetail/EventDetail/Event[1]/IB2t">
    				<tr>
                        <td> I2(t) Phase B :</td>
    					<td> <format type="System.Double" spec="#####"> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/IB2t" /> </format>  </td>
                    </tr>
                </xsl:if>
                <xsl:if test="/AlertDetail/EventDetail/Event[1]/IC2t">
        			<tr>
                        <td> I2(t) Phase C :</td>
        				<td> <format type="System.Double" spec="#####"> <xsl:value-of select="/AlertDetail/EventDetail/Event[1]/IC2t" /> </format> </td>
                    </tr>
                </xsl:if>
            </table>
		</div>
        <hr />
        <xsl:for-each select="/AlertDetail/Breakers/Breaker">
            <span class="section-header">Breaker <xsl:value-of select="Name" /> (<xsl:value-of select="AssetKey" />) History</span>
            <br/><br/>
            <div class="Breaker-details">
                <table>
                <tr>
                  <th>Trip Initiated</th>
                  <th>Pickup Time <br/> (micros)</th>
                  <th>Trip Time <br/> (micros)</th>
                  <th>Imax 1 <br/> (A)</th>
                  <th>Imax 2 <br/> (A)</th>
                  <th>Trip Coil Condition <br/> (A/s)</th>
                </tr>
                <xsl:for-each select="History/Data">
                    <tr>                  
                        <xsl:choose>
                            <xsl:when test="Alert = 1">
                                <td class = "alert"> <format type="System.DateTime" spec="MM/dd/yyyy"> <xsl:value-of select="TI" /> </format>
								<br/> <format type="System.DateTime" spec="HH:mm:ss:ffffff"> <xsl:value-of select="TI" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.DateTime" spec="MM/dd/yyyy"> <xsl:value-of select="TI" /> </format>
								<br/> <format type="System.DateTime" spec="HH:mm:ss:ffffff"> <xsl:value-of select="TI" /> </format> </td>
                            </xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
                            <xsl:when test="PTAlert = 1">
                                <td class = "alert"> <format type="System.Double" spec="#####"> <xsl:value-of select="PT" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.Double" spec="#####"> <xsl:value-of select="PT" /> </format> </td>
                            </xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
                            <xsl:when test="TTAlert = 1">
                                <td class = "alert"> <format type="System.Double" spec="#####"> <xsl:value-of select="TT" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.Double" spec="#####"> <xsl:value-of select="TT" /> </format> </td>
                            </xsl:otherwise>
						</xsl:choose>

                        <td> <format type="System.Double" spec="#0.###"> <xsl:value-of select="L1" /> </format> </td>
                        <td> <format type="System.Double" spec="#0.###"> <xsl:value-of select="L2" /> </format></td>
						<xsl:choose>
                            <xsl:when test="TCCAlert = 1">
                                <td class = "alert"> <format type="System.Double" spec="##0.###"> <xsl:value-of select="TCC" /> </format> </td>
                            </xsl:when>
                            <xsl:otherwise>
                                <td class = "normal"> <format type="System.Double" spec="##0.###"> <xsl:value-of select="TCC" /> </format> </td>
                            </xsl:otherwise>
						</xsl:choose>
                    </tr>
                </xsl:for-each>
                
            </table>
            </div>
        </xsl:for-each>
        <p style="font-size: .8em">
            If you would like receive a different set of emails or unsubscribe, you can <a><xsl:attribute name="href"><xsl:value-of select="/AlertDetail/PQDashboard" />/Email/UpdateSettings</xsl:attribute>manage your subscriptions</a>.
        </p>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet>'
WHERE Name = 'Default Breaker'
GO

----- PQInvestigator Integration -----

-- The following commented statements are used to create a link to the PQInvestigator database server.
-- If the PQI databases are on a separate instance of SQL Server, be sure to associate the appropriate
-- local login with a remote login that has db_owner privileges on both IndustrialPQ and UserIndustrialPQ.

--EXEC sp_addlinkedserver PQInvestigator, N'', N'SQLNCLI', N'localhost\SQLEXPRESS'
--GO
--EXEC sp_addlinkedsrvlogin PQInvestigator, 'FALSE', [LocalLogin], [PQIAdmin], [PQIPassword]
--GO

---------------- System Center TableSpace -------------
CREATE TABLE [ValueListGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
)

CREATE TABLE [ValueList](
    [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [GroupID] [int] NOT NULL FOREIGN KEY REFERENCES ValueListGroup(ID),
    [Text] [varchar](200) NULL,
    [Value] [int] NULL,
    [Description] [varchar](max) NULL,
    [SortOrder] [int] NULL,
)
GO


CREATE TABLE SystemCenter.AdditionalField(
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTable varchar(100) NOT NULL,
	FieldName varchar(100) NOT NULL,
	Type varchar(max) NULL DEFAULT ('string'),
	ExternalDB varchar(max) NULL,
	ExternalDBTable varchar(max) NULL,
	ExternalDBTableKey varchar(max) NULL,
	IsSecure bit NULL DEFAULT(0)
	Constraint UC_AdditonaField UNIQUE(OpenXDAParentTable, FieldName)
)
GO

CREATE TABLE SystemCenter.AdditionalFieldValue(
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTableID int NOT NULL,
	AdditionalFieldID int NOT NULL FOREIGN KEY REFERENCES SystemCenter.AdditionalField(ID),
	Value varchar(max) NULL,
    UpdatedOn DATE NULL DEFAULT (SYSDATETIME()),
	Constraint UC_AdditonaFieldValue UNIQUE(ParentTableID, AdditionalFieldID)
)
GO

CREATE TABLE SystemCenter.ExternalOpenXDAField(
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTable varchar(100) NOT NULL,
	FieldName varchar(100) NOT NULL,
	ExternalDB varchar(max) NULL,
	ExternalDBTable varchar(max) NULL,
	ExternalDBTableKey varchar(max) NULL,
	Constraint UC_ExternalOpenXDAField UNIQUE(ParentTable, FieldName)
)
GO

CREATE Table SystemCenter.extDBTables (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TableName varchar(200) NOT NULL,
    ExternalDB varchar(200) NOT NULL,
	Query varchar(max) NULL,
)
GO

CREATE TABLE SystemCenter.TSC (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(200) NOT NULL,
	Description varchar(max) NULL,
	DepartmentNumber varchar(6) NOT NULL
)
GO

CREATE TABLE SystemCenter.CustomerAccess(
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(ID),
	PQViewSiteID int NOT NULL
)
GO

CREATE TABLE SystemCenter.CustomerAccessPQDigest(
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(ID),
	OpenXDAMeterID INT FOREIGN KEY REFERENCES Meter(ID)
)
GO


CREATE TABLE SystemCenter.Role (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(200) NOT NULL,
	Description varchar(max) NULL,
)
GO

-- Note PQMark might not work properly with this schema --