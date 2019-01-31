-- The following commented statements are used to create a database
-- from scratch and create a new user with access to the database.
--
--  * To change the database name, replace all [openXDA] with the desired database name.
--  * To change the username, replace all NewUser with the desired username.
--  * To change the password, replace all MyPassword with the desired password.

--USE [master]
--GO
--CREATE DATABASE [openXDA]
--GO
--USE [openXDA]
--GO

--USE [master]
--GO
--IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'NewUser')
--CREATE LOGIN [NewUser] WITH PASSWORD=N'MyPassword', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
--GO
--USE [openXDA]
--GO
--CREATE USER [NewUser] FOR LOGIN [NewUser]
--GO
--CREATE ROLE [openXDARole] AUTHORIZATION [dbo]
--GO
--EXEC sp_addrolemember N'openXDAAdmin', N'NewUser'
--GO
--EXEC sp_addrolemember N'db_datareader', N'openXDAAdmin'
--GO
--EXEC sp_addrolemember N'db_datawriter', N'openXDAAdmin'
--GO

----- TABLES -----

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
    Error INT NOT NULL DEFAULT 0,
	FileHash INT
)
GO

CREATE NONCLUSTERED INDEX IX_FileGroup_FileHash
ON FileGroup(FileHash ASC)
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

CREATE TABLE MeterLocation
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetKey VARCHAR(50) NOT NULL UNIQUE,
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
    MeterLocationID INT NOT NULL REFERENCES MeterLocation(ID),
    Name VARCHAR(200) NOT NULL,
    Alias VARCHAR(200) NULL,
    ShortName VARCHAR(50) NULL,
    Make VARCHAR(200) NOT NULL,
    Model VARCHAR(200) NOT NULL,
    TimeZone VARCHAR(200) NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE MeterFileGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID)
)
GO

CREATE TABLE MeterFacility
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    FacilityID INT NOT NULL
)
GO


CREATE TABLE Line
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetKey VARCHAR(50) NOT NULL UNIQUE,
    VoltageKV FLOAT NOT NULL,
    ThermalRating FLOAT NOT NULL,
    Length FLOAT NOT NULL,
    Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE Structure
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssetKey VARCHAR(50) NOT NULL UNIQUE,
    LineID INT NOT NULL REFERENCES Line(ID),
    Latitude FLOAT NOT NULL DEFAULT 0.0,
    Longitude FLOAT NOT NULL DEFAULT 0.0
)
GO

CREATE TABLE MeterLine
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    LineID INT NOT NULL REFERENCES Line(ID),
    LineName VARCHAR(200) NOT NULL
)
GO

CREATE TABLE MeterLocationLine
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterLocationID INT NOT NULL REFERENCES MeterLocation(ID),
    LineID INT NOT NULL REFERENCES Line(ID)
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
    LineID INT NOT NULL REFERENCES Line(ID),
    MeasurementTypeID INT NOT NULL REFERENCES MeasurementType(ID),
    MeasurementCharacteristicID INT NOT NULL REFERENCES MeasurementCharacteristic(ID),
    PhaseID INT NOT NULL REFERENCES Phase(ID),
    Name VARCHAR(200) NOT NULL,
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

CREATE NONCLUSTERED INDEX IX_Channel_LineID
ON Channel(LineID ASC)
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

CREATE TABLE BreakerChannel
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    BreakerNumber VARCHAR(120) NOT NULL
)
GO

CREATE TABLE AssetGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE MeterAssetGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    AssetGroupID INT NOT NULL REFERENCES AssetGroup(ID)
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

CREATE TABLE LineAssetGroup
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LineID INT NOT NULL REFERENCES Line(ID),
    AssetGroupID INT NOT NULL REFERENCES AssetGroup(ID),
)
GO

CREATE NONCLUSTERED INDEX IX_LineAssetGroup_LineID
ON LineAssetGroup(LineID ASC)
GO

CREATE NONCLUSTERED INDEX IX_LineAssetGroup_AssetGroupID
ON LineAssetGroup(AssetGroupID ASC)
GO

CREATE TRIGGER Line_AugmentAllAssetsGroup
ON Line
AFTER INSERT
AS BEGIN
    SET NOCOUNT ON;

    INSERT INTO LineAssetGroup(LineID, AssetGroupID)
    SELECT Line.ID, AssetGroup.ID
    FROM inserted Line CROSS JOIN AssetGroup
    WHERE AssetGroup.Name = 'AllAssets'
END
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

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.TrendingDataSummaryOperation', 6)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.DailySummaryOperation', 7)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.DataQualityOperation', 8)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.AlarmOperation', 9)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.StatisticOperation', 10)
GO


INSERT INTO AssetGroup(Name) VALUES('AllAssets')
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

INSERT INTO SecurityGroup(Name, Description) VALUES('BUILTIN\Users', 'All Windows authenticated users')
GO

INSERT INTO ApplicationRoleSecurityGroup(ApplicationRoleID, SecurityGroupID) VALUES((SELECT ID FROM ApplicationRole), (SELECT ID FROM SecurityGroup))
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('Engineer', 'Engineer Role')
GO

INSERT INTO ApplicationRole(Name, Description) VALUES('Viewer', 'Viewer Role')
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

CREATE TABLE DeviceFilter
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    UserAccount VARCHAR(500) NOT NULL,
    Name NVARCHAR(500) NOT NULL,
    FilterExpression NVARCHAR(MAX) NOT NULL,
    AssetGroupID INT NOT NULL
)
GO

CREATE TABLE SavedViews
(
    ID INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    UserAccount VARCHAR(500) NOT NULL,
    Name NVARCHAR(500) NOT NULL,
	DateRange INT NOT NULL,
    FromDate DATETIME NOT NULL,
    ToDate DATETIME NOT NULL,
    Tab NVARCHAR(20) NOT NULL,
    DeviceFilterID INT NOT NULL,
    MapGrid NVARCHAR(5) NOT NULL,
	IsDefault BIT NOT NULL
)
GO

INSERT INTO SavedViews(UserAccount, Name, DateRange, FromDate, ToDate, Tab, DeviceFilterID, MapGrid, IsDefault) VALUES('Default', 'Home', 2, GETDATE(), GETDATE(), 'Events', 0, 'Grid', 'true')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Daily', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Disturbance', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Fault', '')
GO

INSERT INTO XSLTemplate(Name, Template) VALUES('Default Alarm', '')
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

INSERT INTO EventEmailParameters(EmailTypeID) VALUES(2)
GO

INSERT INTO EventEmailParameters(EmailTypeID) VALUES(3)
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
    LineID INT NOT NULL REFERENCES Line(ID),
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
    UpdatedBy VARCHAR(200) NULL
)
GO

CREATE NONCLUSTERED INDEX IX_Event_FileGroupID
ON Event(FileGroupID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_MeterID
ON Event(MeterID ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_LineID
ON Event(LineID ASC)
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

CREATE NONCLUSTERED INDEX IX_Disturbance_StartTime
ON Disturbance(StartTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_EndTime
ON Disturbance(EndTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Disturbance_StartTime_ID_EventID_PhaseID
ON Disturbance ( StartTime ASC ) INCLUDE ( ID, EventID, PhaseID)
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

INSERT INTO EventType(Name, Description) VALUES ('RecloseIntoFault', 'RecloseIntoFault')
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

INSERT INTO EventType(Name, Description) VALUES ('Breaker', 'Breaker')
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
    MeterLocationLineID INT NOT NULL REFERENCES MeterLocationLine(ID),
    RSrc FLOAT NOT NULL,
    XSrc FLOAT NOT NULL
)
GO

CREATE TABLE LineImpedance
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    LineID INT NOT NULL UNIQUE REFERENCES Line(ID),
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
    MeterLineID INT NOT NULL REFERENCES MeterLine(ID),
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
    Data VARBINARY(MAX) NOT NULL
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

CREATE TABLE NearestStructure
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FaultSummaryID INT NOT NULL REFERENCES FaultSummary(ID),
    StructureID INT NOT NULL REFERENCES Structure(ID)
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
	Setting float NULL
)

CREATE NONCLUSTERED INDEX IX_StepChangeMeasurement_PQMeasurement
ON StepChangeMeasurement(PQMeasurementID ASC)
GO

CREATE TABLE StepChangeStat(
	ID INT PRIMARY KEY IDENTITY(1,1),
	MeterID INT FOREIGN KEY REFERENCES Meter(ID) NOT NULL,
	Date Date NOT NULL,
	StepChangeMeasurementID INT FOREIGN KEY REFERENCES StepChangeMeasurement(ID) NOT NULL,
	Value float NULL
)

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

CREATE TABLE AlarmLog
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ChannelID INT NOT NULL REFERENCES Channel(ID),
    AlarmTypeID INT NOT NULL REFERENCES AlarmType(ID),
    Time DATETIME NOT NULL,
    Severity INT NOT NULL,
    LimitHigh FLOAT NULL,
    LimitLow FLOAT NULL,
    Value FLOAT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_AlarmLog_ChannelID
ON AlarmLog(ChannelID ASC)
GO

CREATE NONCLUSTERED INDEX IX_AlarmLog_AlarmTypeID
ON AlarmLog(AlarmTypeID ASC)
GO

CREATE NONCLUSTERED INDEX IX_AlarmLog_Time
ON AlarmLog(Time ASC)
GO

CREATE NONCLUSTERED INDEX IX_AlarmLog_Severity
ON AlarmLog(Severity ASC)
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Latched', 'Latched value')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Non-congruent', 'Average value outside of minimum and maximum')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Unreasonable', 'Value outside of reasonable limits')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('OffNormal', 'Value was outside of normal range for a given hour of the week')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Alarm', 'Value exceeded regulatory limits')
GO

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

CREATE TABLE LinesToDataPush
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LocalXDALineID INT NOT NULL,
    RemoteXDALineID INT NULL,
    LocalXDAAssetKey VARCHAR(200) NOT NULL,
    RemoteXDAAssetKey VARCHAR(200) NOT NULL,
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

CREATE TABLE PQMarkCompany
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE PQMarkCompanyMeter
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PQMarkCompanyID INT NOT NULL REFERENCES PQMarkCompany(ID),
    MeterID INT NOT NULL REFERENCES Meter(ID),
    DisplayName VARCHAR(200) NOT NULL,
    Enabled BIT NOT NULL
)
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
	CONSTRAINT UC_Report UNIQUE(ID, MeterID, Month, Year)
)
GO

CREATE TABLE EventStat
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	EventID INT NOT NULL REFERENCES Event(ID),
	IMin float NULL,
	IMax float NULL,
    VMin float NULL,
	VMax float NULL,
	I2t float NULL,
	CONSTRAINT UC_EventStat_EventID UNIQUE(EventID)
)
GO

----- FUNCTIONS -----

CREATE FUNCTION ComputeHash
(
    @eventID INT,
    @templateID INT
)
RETURNS BIGINT
BEGIN
    DECLARE @md5Hash BINARY(16)

    DECLARE @eventDetailSQL VARCHAR(MAX) =
    (
        SELECT EventDetailSQL
        FROM
            EmailType JOIN
            EventEmailParameters ON EventEmailParameters.EmailTypeID = EmailType.ID
        WHERE EmailType.XSLTemplateID = @templateID
    )

    DECLARE @eventDetail VARCHAR(MAX)
    EXEC sp_executesql @eventDetailSQL, @eventID, @eventDetail OUT

    SELECT @md5Hash = master.sys.fn_repl_hash_binary(CONVERT(VARBINARY(MAX), @eventDetail))
    FROM EventDetail
    WHERE EventID = @eventID

    SELECT @md5Hash = master.sys.fn_repl_hash_binary(@md5Hash + CONVERT(VARBINARY(MAX), Template))
    FROM XSLTemplate
    WHERE ID = @templateID

    RETURN CONVERT(BIGINT, SUBSTRING(@md5Hash, 0, 8)) ^ CONVERT(BIGINT, SUBSTRING(@md5Hash, 8, 8))
END
GO

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

CREATE FUNCTION GetLineEventIDs
(
    @lineID INT,
    @startTime DATETIME2,
    @endTime DATETIME2,
    @timeTolerance FLOAT
)
RETURNS @lineEvent TABLE
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
        LineID = @lineID AND
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
            LineID = @lineID AND
            StartTime <= @adjustedEndTime AND
            @adjustedStartTime <= EndTime AND
            ProcessingEndTime > '0001-01-01'
    END

    INSERT INTO @lineEvent
    SELECT ID
    FROM Event
    WHERE
        LineID = @lineID AND
        @adjustedStartTime <= StartTime AND
        EndTime <= @adjustedEndTime

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

CREATE VIEW MeterDetail
AS
SELECT	Meter.ID,
		Meter.AssetKey,
		Meter.MeterLocationID,
		MeterLocation.AssetKey AS LocationKey,
		MeterLocation.Name AS Location,
		MeterLocation.Latitude,
		MeterLocation.Longitude,
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
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT OUTER JOIN
        Setting ON Setting.Name = 'DefaultMeterTimeZone'

GO

CREATE VIEW LineView
AS
SELECT
    Line.ID,
    Line.AssetKey,
    Line.VoltageKV,
    Line.ThermalRating,
    Line.Length,
    Line.Description,
    (
        SELECT TOP 1 LineName
        FROM MeterLine
        WHERE LineID = Line.ID
    ) AS TopName,
    LineImpedance.R0,
    LineImpedance.X0,
    LineImpedance.R1,
    LineImpedance.X1,
    LineImpedance.ID AS LineImpedanceID
FROM
    Line LEFT OUTER JOIN
    LineImpedance ON Line.ID = LineImpedance.LineID
GO

CREATE VIEW MeterLineDetail
AS
SELECT
    MeterLine.ID,
    MeterLine.MeterID,
    Meter.AssetKey AS MeterKey,
    Meter.Name AS MeterName,
    MeterLine.LineID,
    Line.AssetKey AS LineKey,
    MeterLine.LineName
FROM
    MeterLine JOIN
    Meter ON MeterLine.MeterID = Meter.ID JOIN
    Line ON MeterLIne.LineID = Line.ID
GO

CREATE VIEW ChannelDetail
AS
SELECT
    Channel.ID,
    Channel.MeterID,
    Meter.AssetKey AS MeterKey,
    Meter.Name AS MeterName,
    Channel.LineID,
    Line.AssetKey AS LineKey,
    MeterLine.LineName,
    Channel.MeasurementTypeID,
    MeasurementType.Name AS MeasurementType,
    Channel.MeasurementCharacteristicID,
    MeasurementCharacteristic.Name AS MeasurementCharacteristic,
    Channel.PhaseID,
    Phase.Name AS Phase,
    Channel.Name,
    Channel.SamplesPerHour,
    Channel.PerUnitValue,
    Channel.HarmonicGroup,
    Series.SourceIndexes AS Mapping,
    Channel.Description,
    Channel.Enabled,
    Series.SeriesTypeID,
    SeriesType.Name AS SeriesType
FROM
    Channel JOIN
    Meter ON Channel.MeterID = Meter.ID JOIN
    Line ON Channel.LineID = Line.ID JOIN
    MeterLine ON
        MeterLine.MeterID = Meter.ID AND
        MeterLine.LineID = Line.ID JOIN
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
    Channel.LineID,
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
    MeterLocation.Name AS Location
FROM
    MeterAssetGroup JOIN
    Meter ON MeterAssetGroup.MeterID = Meter.ID JOIN
    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
GO

CREATE VIEW LineAssetGroupView
AS
SELECT
    LineAssetGroup.ID,
    Line.AssetKey AS LineName,
	(SELECT TOP 1 LineName FROM MeterLine Where LineID = Line.ID) AS LongLineName,
    Line.ID AS LineID,
    AssetGroupID
FROM
    LineAssetGroup JOIN
    Line ON LineAssetGroup.LineID = Line.ID
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

CREATE VIEW UserMeter
AS
SELECT DISTINCT
    UserAccount.Name AS UserName,
    Meter.ID AS MeterID
FROM
    UserAccount JOIN
    UserAccountAssetGroup ON UserAccountAssetGroup.UserAccountID = UserAccount.ID LEFT OUTER JOIN
    MeterAssetGroup ON MeterAssetGroup.AssetGroupID = UserAccountAssetGroup.AssetGroupID LEFT OUTER JOIN
    LineAssetGroup ON LineAssetGroup.AssetGroupID = UserAccountAssetGroup.AssetGroupID LEFT OUTER JOIN
    MeterLine ON MeterLine.LineID = LineAssetGroup.LineID JOIN
    Meter ON
        MeterAssetGroup.MeterID = Meter.ID OR
        MeterLine.MeterID = Meter.ID
WHERE
    UserAccount.Approved <> 0 AND
    UserAccountAssetGroup.Dashboard <> 0
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
    Event.LineID,
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
    MeterLine.LineName,
    Meter.Name AS MeterName,
    MeterLocation.Name AS StationName,
    Line.Length,
    EventType.Name AS EventTypeName
FROM
    Event JOIN
    Meter ON Event.MeterID = Meter.ID JOIN
    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
    Line ON Event.LineID = Line.ID JOIN
    MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Line.ID JOIN
    EventType ON Event.EventTypeID = EventType.ID
GO

CREATE VIEW DisturbanceView
AS
SELECT
    Disturbance.ID,
    Disturbance.EventID,
    Disturbance.EventTypeID,
    Disturbance.PhaseID,
    Disturbance.Magnitude,
    Disturbance.PerUnitMagnitude,
    Disturbance.StartTime,
    Disturbance.EndTime,
    Disturbance.DurationSeconds,
    Disturbance.DurationCycles,
    Disturbance.StartIndex,
    Disturbance.EndIndex,
    Event.MeterID,
    (
        SELECT MAX(SeverityCode) AS Expr1
        FROM DisturbanceSeverity
        WHERE (DisturbanceID = Disturbance.ID)
    ) AS SeverityCode,
    Meter.Name AS MeterName,
    Phase.Name AS PhaseName,
    Event.LineID
FROM
    Disturbance JOIN
    Event ON Disturbance.EventID = Event.ID JOIN
    Meter ON Event.MeterID = Meter.ID JOIN
    Phase ON Disturbance.PhaseID = Phase.ID
GO

CREATE VIEW BreakerView
AS
SELECT
    BreakerOperation.ID,
    Meter.ID AS MeterID,
    Event.ID AS EventID,
    EventType.Name AS EventType,
    BreakerOperation.TripCoilEnergized AS Energized,
    BreakerOperation.BreakerNumber,
    MeterLine.LineName,
    Phase.Name AS PhaseName,
    CAST(BreakerOperation.BreakerTiming AS DECIMAL(16, 5)) AS Timing,
    BreakerOperation.BreakerSpeed AS Speed,
    BreakerOperationType.Name AS OperationType,
    BreakerOperation.UpdatedBy
FROM
    BreakerOperation JOIN
    Event ON BreakerOperation.EventID = Event.ID JOIN
    EventType ON EventType.ID = Event.EventTypeID JOIN
    Meter ON Meter.ID = Event.MeterID JOIN
    Line ON Line.ID = Event.LineID JOIN
    MeterLine ON MeterLine.LineID = Event.LineID AND MeterLine.MeterID = Meter.ID JOIN
    BreakerOperationType ON BreakerOperation.BreakerOperationTypeID = BreakerOperationType.ID JOIN
    Phase ON BreakerOperation.PhaseID = Phase.ID
GO

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
    MeterLocation.ShortName AS LocationName,
    Meter.ID AS MeterID,
    Line.ID AS LineID,
    MeterLine.LineName AS LineName,
    Line.VoltageKV AS Voltage,
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
    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
    Line ON Event.LineID = Line.ID JOIN
    MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Line.ID
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
CREATE VIEW OpenSEEScalarStatView as
SELECT *
FROM
(
SELECT
	Event.ID as EventID,
	MeterLocation.Name as Station,
	Meter.Name as Meter,
	Line.AssetKey as Line,
	EventType.Name as EventType,
	DATEDIFF(MILLISECOND, Event.StartTime, Event.EndTime)/1000.0 as FileDuration,
	FaultSummary.Distance,
	FaultSummary.DurationCycles,
	FaultSummary.IsSelectedAlgorithm,
	EventStat.I2t,
	EventStat.VMax,
	EventStat.VMin,
	EventStat.IMax
FROM
	Event JOIN
	Meter ON Event.MeterID = Meter.ID JOIN
	MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
	Line ON Event.LineID = Line.ID JOIN
	EventType ON Event.EventTypeID = EventType.ID LEFT JOIN
	FaultSummary ON Event.ID = FaultSummary.EventID LEFT JOIN
	EventStat ON Event.ID = EventStat.EventID
	)as sub
Where IsSelectedAlgorithm IS NULL OR IsSelectedAlgorithm = 1
GO
----- PROCEDURES -----

CREATE PROCEDURE GetEventEmailRecipients
(
    @eventID INT
)
AS BEGIN
    DECLARE @lineID INT
    DECLARE @startTime DATETIME2
    DECLARE @endTime DATETIME2
    DECLARE @timeTolerance FLOAT

    SELECT
        @lineID = LineID,
        @startTime = StartTime,
        @endTime = EndTime,
        @timeTolerance = COALESCE(Value, 0.5)
    FROM
        Event LEFT OUTER JOIN
        Setting ON Setting.Name = 'TimeTolerance'
    WHERE Event.ID = @eventID

    SELECT DISTINCT
        UserAccountAssetGroup.UserAccountID,
        EmailType.XSLTemplateID AS TemplateID
    FROM
        GetLineEventIDs(@lineID, @startTime, @endTime, @timeTolerance) LineEventID JOIN
        Event ON LineEventID.EventID = Event.ID JOIN
        EventType ON Event.EventTypeID = EventType.ID LEFT OUTER JOIN
        MeterAssetGroup ON MeterAssetGroup.MeterID = Event.MeterID LEFT OUTER JOIN
        LineAssetGroup ON LineAssetGroup.LineID = Event.LineID JOIN
        UserAccountAssetGroup ON
            UserAccountAssetGroup.AssetGroupID = MeterAssetGroup.AssetGroupID OR
            UserAccountAssetGroup.AssetGroupID = LineAssetGroup.AssetGroupID JOIN
        UserAccountEmailType ON UserAccountAssetGroup.UserAccountID = UserAccountEmailType.UserAccountID JOIN
        EmailType ON UserAccountEmailType.EmailTypeID = EmailType.ID JOIN
        EmailCategory ON
            EmailType.EmailCategoryID = EmailCategory.ID AND
            EmailCategory.Name = 'Event'
    WHERE
        (
            EventType.Name = 'Fault' AND
            EXISTS
            (
                SELECT *
                FROM FaultEmailCriterion
                WHERE FaultEmailCriterion.EmailTypeID = EmailType.ID
            )
        )
        OR
        (
            EventType.Name = 'RecloseIntoFault' AND
            EXISTS
            (
                SELECT *
                FROM FaultEmailCriterion
                WHERE
                    FaultEmailCriterion.EmailTypeID = EmailType.ID AND
                    FaultEmailCriterion.EmailOnReclose <> 0
            )
        )
        OR EXISTS
        (
            SELECT *
            FROM
                Disturbance JOIN
                DisturbanceSeverity ON DisturbanceSeverity.DisturbanceID = Disturbance.ID JOIN
                DisturbanceEmailCriterion ON DisturbanceSeverity.SeverityCode = DisturbanceEmailCriterion.SeverityCode
            WHERE
                Disturbance.EventID = Event.ID AND
                DisturbanceEmailCriterion.EmailTypeID = EmailType.ID
        )
END
GO

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

CREATE PROCEDURE GetLineEvent
    @lineID INT,
    @startTime DATETIME2,
    @endTime DATETIME2,
    @timeTolerance FLOAT
AS BEGIN
    SELECT *
    FROM Event
    WHERE ID IN (SELECT * FROM dbo.GetLineEventIDs(@lineID, @startTime, @endTime, @timeTolerance))
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

    CREATE TABLE #temp
    (
        Facility VARCHAR(64),
        Area VARCHAR(256),
        SectionTitle VARCHAR(256),
        SectionRank INT,
        ComponentModel VARCHAR(64),
        ManufacturerName VARCHAR(64),
        SeriesName VARCHAR(64),
        ComponentTypeName VARCHAR(32)
    )

    DECLARE dbCursor CURSOR FOR
    SELECT
        FacilityID,
        PerUnitMagnitude,
        DurationSeconds
    FROM
        Disturbance JOIN
        Event ON Disturbance.EventID = Event.ID JOIN
        MeterFacility ON MeterFacility.MeterID = Event.MeterID
    WHERE
        EventID = @eventID

    OPEN dbCursor
    FETCH NEXT FROM dbCursor INTO @facilityID, @magnitude, @duration

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #temp
        EXEC GetImpactedComponents @facilityID, @magnitude, @duration

        FETCH NEXT FROM dbCursor INTO @facilityID, @magnitude, @duration
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
    FaultSummary.PrefaultCurrent,
    FaultSummary.PostfaultCurrent,
    FaultSummary.ReactanceRatio,
    FaultSummary.CurrentMagnitude AS FaultCurrent,
    FaultSummary.Algorithm,
    FaultSummary.Distance AS SingleEndedDistance,
    DoubleEndedFaultSummary.Distance AS DoubleEndedDistance,
    DoubleEndedFaultSummary.Angle AS DoubleEndedAngle,
    RIGHT(DataFile.FilePath, CHARINDEX(''\'', REVERSE(DataFile.FilePath)) - 1) AS FileName,
    FaultSummary.EventID,
    Event.StartTime AS EventStartTime,
    SimpleSummary.Distance AS Simple,
    ReactanceSummary.Distance AS Reactance,
    Event.EndTime AS EventEndTime
INTO #summaryData
FROM
    #lineEvent Event JOIN
    FaultSummary ON
        FaultSummary.EventID = Event.ID AND
        FaultSummary.IsSelectedAlgorithm <> 0 AND
        FaultSummary.IsSuppressed = 0 LEFT OUTER JOIN
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

----- PQInvestigator Integration -----

-- The following commented statements are used to create a link to the PQInvestigator database server.
-- If the PQI databases are on a separate instance of SQL Server, be sure to associate the appropriate
-- local login with a remote login that has db_owner privileges on both IndustrialPQ and UserIndustrialPQ.

--EXEC sp_addlinkedserver PQInvestigator, N'', N'SQLNCLI', N'localhost\SQLEXPRESS'
--GO
--EXEC sp_addlinkedsrvlogin PQInvestigator, 'FALSE', [LocalLogin], [PQIAdmin], [PQIPassword]
--GO
