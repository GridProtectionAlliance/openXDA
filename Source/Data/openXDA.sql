USE [master]
GO

CREATE DATABASE [openXDA]
GO

-- The following commented statements are used to create a user with access to openXDA.
-- Be sure to change the username and password.
-- Replace-all from NewUser to the desired username is the recommended method of changing the username.

--IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'NewUser')
--CREATE LOGIN [NewUser] WITH PASSWORD=N'MyPassword', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
--GO
USE [openXDA]
GO
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
    Name VARCHAR(200) NOT NULL,
    Value VARCHAR(MAX) NOT NULL
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
    FileExtension VARCHAR(10) NOT NULL,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL
)
GO

CREATE TABLE DataOperation
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    TransactionOrder INT NOT NULL,
    LoadOrder INT NOT NULL
)
GO

CREATE TABLE DataWriter
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL
)
GO

CREATE TABLE FileGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    DataStartTime DATETIME2 NOT NULL,
    DataEndTime DATETIME2 NOT NULL,
    ProcessingStartTime DATETIME2 NOT NULL,
    ProcessingEndTime DATETIME2 NOT NULL,
    Error INT NOT NULL DEFAULT 0
)
GO

CREATE TABLE DataFile
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FileGroupID INT NOT NULL REFERENCES FileGroup(ID),
    FilePath VARCHAR(MAX) NOT NULL,
    FileSize BIGINT NOT NULL,
    CreationTime DATETIME NOT NULL,
    LastWriteTime DATETIME NOT NULL,
    LastAccessTime DATETIME NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_DataFile_FileGroupID
ON DataFile(FileGroupID ASC)
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
    SamplesPerHour INT NOT NULL,
    PerUnitValue FLOAT NULL,
    HarmonicGroup INT NOT NULL,
    Description VARCHAR(MAX) NULL
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

CREATE TABLE SeriesType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
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

CREATE TABLE Recipient
(
	ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Email VARCHAR(200) NOT NULL
)
GO

CREATE TABLE MeterRecipient
(
	ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	MeterID INT NOT NULL REFERENCES Meter(ID),
	RecipientID INT NOT NULL REFERENCES Recipient(ID)
)
GO

INSERT INTO DataReader(FileExtension, AssemblyName, TypeName) VALUES('DAT', 'FaultData.dll', 'FaultData.DataReaders.COMTRADEReader')
GO

INSERT INTO DataReader(FileExtension, AssemblyName, TypeName) VALUES('D00', 'FaultData.dll', 'FaultData.DataReaders.COMTRADEReader')
GO

INSERT INTO DataReader(FileExtension, AssemblyName, TypeName) VALUES('RCD', 'FaultData.dll', 'FaultData.DataReaders.EMAXReader')
GO

INSERT INTO DataReader(FileExtension, AssemblyName, TypeName) VALUES('RCL', 'FaultData.dll', 'FaultData.DataReaders.EMAXReader')
GO

INSERT INTO DataOperation(AssemblyName, TypeName, TransactionOrder, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.ConfigurationOperation', 0, 1)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, TransactionOrder, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.EventOperation', 0, 2)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, TransactionOrder, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.FaultLocationOperation', 0, 3)
GO

INSERT INTO DataOperation(AssemblyName, TypeName, TransactionOrder, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataOperations.DoubleEndedFaultOperation', 1, 1)
GO

INSERT INTO DataWriter(AssemblyName, TypeName) VALUES('FaultData.dll', 'FaultData.DataWriters.XMLWriter')
GO

INSERT INTO DataWriter(AssemblyName, TypeName) VALUES('FaultData.dll', 'FaultData.DataWriters.COMTRADEWriter')
GO

INSERT INTO DataWriter(AssemblyName, TypeName) VALUES('FaultData.dll', 'FaultData.DataWriters.EmailWriter')
GO

-- ------ --
-- Events --
-- ------ --

CREATE TABLE EventType
(
	ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	Name VARCHAR(200) NOT NULL,
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
    Name VARCHAR(200) NOT NULL,
    Alias VARCHAR(200) NULL,
    ShortName VARCHAR(50) NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    TimeZoneOffset INT NOT NULL,
    Magnitude FLOAT NOT NULL,
    Duration FLOAT NOT NULL,
    HasImpactedComponents INT NOT NULL,
    Description VARCHAR(MAX) NULL,
    Data VARBINARY(MAX) NOT NULL
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

CREATE NONCLUSTERED INDEX IX_Event_StartTime
ON Event(StartTime ASC)
GO

CREATE NONCLUSTERED INDEX IX_Event_EndTime
ON Event(EndTime ASC)
GO

CREATE TABLE CycleData
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    Data VARBINARY(MAX) NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_CycleData_EventID
ON CycleData(EventID ASC)
GO

CREATE TABLE BreakerOperationType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
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
    APhaseCleared DATETIME2 NOT NULL,
    BPhaseCleared DATETIME2 NOT NULL,
    CPhaseCleared DATETIME2 NOT NULL,
    BreakerTiming FLOAT NOT NULL,
    APhaseBreakerTiming FLOAT NOT NULL,
    BPhaseBreakerTiming FLOAT NOT NULL,
    CPhaseBreakerTiming FLOAT NOT NULL,
    BreakerSpeed FLOAT NOT NULL
)
GO

CREATE NONCLUSTERED INDEX IX_BreakerOperation_EventID
ON BreakerOperation(EventID ASC)
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
    LineID INT NOT NULL REFERENCES Line(ID),
    R0 FLOAT NOT NULL,
    X0 FLOAT NOT NULL,
    R1 FLOAT NOT NULL,
    X1 FLOAT NOT NULL
)
GO

CREATE TABLE SegmentType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
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

CREATE TABLE FaultLocationAlgorithm
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(1024) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    MethodName VARCHAR(80) NOT NULL,
    ExecutionOrder INT NOT NULL
)
GO

CREATE TABLE FaultCurve
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    Algorithm VARCHAR(80) NOT NULL,
    Data VARBINARY(MAX) NOT NULL
)
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
    LocalFaultSummaryID INT NOT NULL REFERENCES FaultSummary(ID),
    RemoteFaultSummaryID INT NOT NULL REFERENCES FaultSummary(ID),
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

CREATE TABLE FaultEmailTemplate
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Template VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE FaultEmailTemplateRecipient
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    RecipientID INT NOT NULL REFERENCES Recipient(ID),
    FaultEmailTemplateID INT NOT NULL REFERENCES FaultEmailTemplate(ID)
)
GO

CREATE NONCLUSTERED INDEX IX_FaultEmailTemplateRecipient_RecipientID
ON FaultEmailTemplateRecipient(RecipientID ASC)
GO

CREATE NONCLUSTERED INDEX IX_FaultEmailTemplateRecipient_FaultEmailTemplateID
ON FaultEmailTemplateRecipient(FaultEmailTemplateID ASC)
GO

CREATE TABLE FaultEmail
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    TimeSent DATETIME NOT NULL,
    ToLine VARCHAR(MAX) NOT NULL,
    Subject VARCHAR(500) NOT NULL,
    Message VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE EventFaultEmail
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EventID INT NOT NULL REFERENCES Event(ID),
    FaultEmailID INT NOT NULL REFERENCES FaultEmail(ID)
)
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

INSERT INTO AlarmType(Name, Description) VALUES ('Off-normal-168H', 'Value was outside of normal range for a given hour of the week')
GO

INSERT INTO AlarmType(Name, Description) VALUES ('Regulatory', 'Value exceeded regulatory limits')
GO

----- FUNCTIONS -----

CREATE FUNCTION AdjustDateTime2
(
	@dateTime2 DATETIME2,
	@timeTolerance FLOAT
)
RETURNS DATETIME2
BEGIN
    DECLARE @adjustedDateTime DATETIME2 = DATEADD(SECOND, @timeTolerance, @dateTime2)
	RETURN DATEADD(NANOSECOND, (@timeTolerance - ROUND(@timeTolerance, 0, 1)) * 1000000000, @adjustedDateTime)
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
	DECLARE @minStartTime DATETIME2
	DECLARE @maxEndTime DATETIME2

	SELECT @minStartTime = MIN(dbo.AdjustDateTime2(StartTime, -@timeTolerance)), @maxEndTime = MAX(dbo.AdjustDateTime2(EndTime, @timeTolerance))
	FROM Event
	WHERE
		(dbo.AdjustDateTime2(StartTime, -@timeTolerance) <= @startTime AND @startTime <= dbo.AdjustDateTime2(EndTime, @timeTolerance)) OR
		(@startTime <= dbo.AdjustDateTime2(StartTime, -@timeTolerance) AND dbo.AdjustDateTime2(StartTime, -@timeTolerance) <= @endTime)

	WHILE @startTime != @minStartTime OR @endTime != @maxEndTime
	BEGIN
		SET @startTime = @minStartTime
		SET @endTime = @maxEndTime

		SELECT @minStartTime = MIN(dbo.AdjustDateTime2(StartTime, -@timeTolerance)), @maxEndTime = MAX(dbo.AdjustDateTime2(EndTime, @timeTolerance))
		FROM Event
		WHERE
			(dbo.AdjustDateTime2(StartTime, -@timeTolerance) <= @startTime AND @startTime <= dbo.AdjustDateTime2(EndTime, @timeTolerance)) OR
			(@startTime <= dbo.AdjustDateTime2(StartTime, -@timeTolerance) AND dbo.AdjustDateTime2(StartTime, -@timeTolerance) <= @endTime)
	END

	INSERT INTO @systemEvent
    SELECT ID
    FROM Event
	WHERE @startTime <= dbo.AdjustDateTime2(StartTime, -@timeTolerance) AND dbo.AdjustDateTime2(EndTime, @timeTolerance) <= @endTime
    
    RETURN
END
GO

----- VIEWS -----

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

CREATE VIEW EventDetail AS
WITH TimeTolerance AS
(
	SELECT
		COALESCE(CAST(Value AS FLOAT), 0.5) AS Tolerance
	FROM
		(SELECT 'TimeTolerance' AS Name) AS SettingName LEFT OUTER JOIN
		Setting ON SettingName.Name = Setting.Name
),
SelectedSummary AS
(
	SELECT *
	FROM FaultSummary
	WHERE IsSelectedAlgorithm <> 0 AND IsSuppressed = 0
),
SummaryData AS
(
	SELECT
		SelectedSummary.ID AS FaultSummaryID,
		Meter.AssetKey AS MeterKey,
		MeterLocation.Name AS StationName,
		MeterLine.LineName,
		SelectedSummary.FaultType,
		SelectedSummary.Inception,
		SelectedSummary.DurationCycles,
		SelectedSummary.DurationSeconds * 1000.0 AS DurationMilliseconds,
		SelectedSummary.CurrentMagnitude AS FaultCurrent,
		SelectedSummary.Algorithm,
		SelectedSummary.Distance AS SingleEndedDistance,
		DoubleEndedFaultSummary.Distance AS DoubleEndedDistance,
		SelectedSummary.EventID,
		Event.StartTime AS EventStartTime,
		Event.EndTime AS EventEndTime
	FROM
		SelectedSummary JOIN
		Event ON SelectedSummary.EventID = Event.ID JOIN
		Meter ON Event.MeterID = Meter.ID JOIN
		MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
		MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Event.LineID LEFT OUTER JOIN
		DoubleEndedFaultDistance ON DoubleEndedFaultDistance.LocalFaultSummaryID = SelectedSummary.ID LEFT OUTER JOIN
		DoubleEndedFaultSummary ON DoubleEndedFaultSummary.ID = DoubleEndedFaultDistance.ID
),
SummaryIDs AS
(
	SELECT
		SelectedSummary.ID AS FaultSummaryID,
		EventID,
		LineID,
		MeterID AS PartitionID,
		Inception AS OrderID
	FROM
		SelectedSummary JOIN
		Event ON SelectedSummary.EventID = Event.ID
)
SELECT
    Event.ID AS EventID,
    (
        SELECT
            Event.ID AS [Event/ID],
            Event.StartTime AS [Event/StartTime],
            Event.EndTime AS [Event/EndTime],
            EventType.Name AS [Event/Type],
			(
				SELECT
					FaultNumber AS [@num],
					(
						SELECT
							MeterKey,
							StationName,
							LineName,
							FaultType,
							Inception,
							DurationCycles,
							DurationMilliseconds,
							FaultCurrent,
							Algorithm,
							SingleEndedDistance,
							DoubleEndedDistance,
							EventStartTime,
							EventEndTime,
							EventID,
							FaultSummaryID AS FaultID
						FROM SummaryData
						WHERE FaultSummaryID IN
						(
							SELECT FaultSummaryID
							FROM
							(
								SELECT FaultSummaryID, ROW_NUMBER() OVER(PARTITION BY PartitionID ORDER BY OrderID) AS FaultNumber
								FROM SummaryIDs
								WHERE SummaryIDs.LineID = Event.LineID AND SummaryIDs.EventID IN (SELECT * FROM dbo.GetSystemEventIDs(Event.StartTime, Event.EndTime, (SELECT * FROM TimeTolerance)))
							) InnerFaultNumber
							WHERE InnerFaultNumber.FaultNumber = OuterFaultNumber.FaultNumber
						)
						FOR XML PATH('SummaryData'), TYPE
					)
				FROM
				(
					SELECT DISTINCT ROW_NUMBER() OVER(PARTITION BY PartitionID ORDER BY OrderID) AS FaultNumber
					FROM SummaryIDs
					WHERE SummaryIDs.LineID = Event.LineID AND SummaryIDs.EventID IN (SELECT * FROM dbo.GetSystemEventIDs(Event.StartTime, Event.EndTime, (SELECT * FROM TimeTolerance)))
				) OuterFaultNumber
				FOR XML PATH('Fault'), TYPE
			) AS [Faults],
            Meter.AssetKey AS [Meter/AssetKey],
            Meter.Name AS [Meter/Name],
            Meter.ShortName AS [Meter/ShortName],
            Meter.Alias AS [Meter/Alias],
            Meter.Make AS [Meter/Make],
            Meter.Model AS [Meter/Model],
            MeterLocation.AssetKey AS [MeterLocation/AssetKey],
            MeterLocation.Name AS [MeterLocation/Name],
            MeterLocation.ShortName AS [MeterLocation/ShortName],
            MeterLocation.Alias AS [MeterLocation/Alias],
            SourceImpedance.RSrc AS [MeterLocation/RSrc],
            SourceImpedance.XSrc AS [MeterLocation/XSrc],
            Line.AssetKey AS [Line/AssetKey],
            MeterLine.LineName AS [Line/Name],
			FORMAT(Line.Length, '0.##########') AS [Line/Length],
            FORMAT(SQRT(LineImpedance.R1 * LineImpedance.R1 + LineImpedance.X1 * LineImpedance.X1), '0.##########') AS [Line/Z1],
            FORMAT(ATN2(LineImpedance.X1, LineImpedance.R1) * 180 / PI(), '0.##########') AS [Line/A1],
            FORMAT(LineImpedance.R1, '0.##########') AS [Line/R1],
            FORMAT(LineImpedance.X1, '0.##########') AS [Line/X1],
            FORMAT(SQRT(LineImpedance.R0 * LineImpedance.R0 + LineImpedance.X0 * LineImpedance.X0), '0.##########') AS [Line/Z0],
            FORMAT(ATN2(LineImpedance.X0, LineImpedance.R0) * 180 / PI(), '0.##########') AS [Line/A0],
            FORMAT(LineImpedance.R0, '0.##########') AS [Line/R0],
            FORMAT(LineImpedance.X0, '0.##########') AS [Line/X0],
            FORMAT(SQRT(POWER((2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0, 2) + POWER((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, 2)), '0.##########') AS [Line/ZS],
            FORMAT(ATN2((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, (2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0) * 180 / PI(), '0.##########') AS [Line/AS],
            FORMAT((2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0, '0.##########') AS [Line/RS],
            FORMAT((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, '0.##########') AS [Line/XS],
			(
				CAST((SELECT '<TimeTolerance>' + CAST((SELECT * FROM TimeTolerance) AS VARCHAR) + '</TimeTolerance>') AS XML)
			) AS [Settings]
        FROM
            Meter CROSS JOIN
            Line LEFT OUTER JOIN
            MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT OUTER JOIN
            MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Line.ID LEFT OUTER JOIN
            MeterLocationLine ON MeterLocationLine.MeterLocationID = MeterLocation.ID AND MeterLocationLine.LineID = Line.ID LEFT OUTER JOIN
            SourceImpedance ON SourceImpedance.MeterLocationLineID = MeterLocationLine.ID LEFT OUTER JOIN
            LineImpedance ON LineImpedance.LineID = Line.ID LEFT OUTER JOIN
            EventType ON Event.EventTypeID = EventType.ID
        WHERE
            Event.MeterID = Meter.ID AND
            Event.LineID = Line.ID
        FOR XML PATH('EventDetail'), TYPE
    ) AS EventDetail
FROM Event
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

----- PQInvestigator Integration -----

-- The following commented statements and procedures are used to create a link to the PQInvestigator database.
-- If the PQI databases are on a separate instance of SQL Server, be sure to associate the appropriate
-- local login with a remote login that has db_owner privileges on both IndustrialPQ and UserIndustrialPQ.

--EXEC sp_addlinkedserver PQInvestigator, N'', N'SQLNCLI', N'localhost\SQLEXPRESS'
--GO
--EXEC sp_addlinkedsrvlogin PQInvestigator, 'FALSE', [LocalLogin], [PQIAdmin], [PQIPassword]
--GO
--
--CREATE PROCEDURE GetPQIFacility
--    @facilityID INT
--AS BEGIN
--    SELECT
--        Facility.FacilityName,
--        Facility.FacilityVoltage,
--        Facility.UtilitySupplyVoltage,
--        Address.Address1,
--        Address.Address2,
--        Address.City,
--        Address.StateOrProvince,
--        Address.PostalCode,
--        Address.Country,
--        Company.CompanyName,
--        Company.Industry
--    FROM
--        PQInvestigator.UserIndustrialPQ.dbo.Facility JOIN
--        PQInvestigator.UserIndustrialPQ.dbo.Address ON Facility.AddressID = Address.AddressID JOIN
--        PQInvestigator.UserIndustrialPQ.dbo.Company ON Address.CompanyID = Company.CompanyID
--    WHERE
--        Facility.FacilityID = @facilityID
--END
--GO
--
--CREATE PROCEDURE GetImpactedComponents
--    @facilityID INT,
--    @magnitude FLOAT,
--    @duration FLOAT
--AS BEGIN
--    WITH EPRITolerancePoint AS
--    (
--        SELECT
--            ROW_NUMBER() OVER(PARTITION BY TestCurve.TestCurveID ORDER BY Y, X) AS RowNumber,
--            ComponentID,
--            TestCurve.TestCurveID,
--            Y AS Magnitude,
--            X AS Duration
--        FROM
--            PQInvestigator.IndustrialPQ.dbo.TestCurvePoint JOIN
--            PQInvestigator.IndustrialPQ.dbo.TestCurve ON TestCurvePoint.TestCurveID = TestCurve.TestCurveID
--    ),
--    EPRIToleranceSegment AS
--    (
--        SELECT
--            P1.RowNumber,
--            P1.ComponentID,
--            P1.TestCurveID,
--            CASE WHEN @duration < P1.Duration THEN 0
--                 WHEN @magnitude < P1.Magnitude THEN 1
--                 WHEN @duration > P2.Duration THEN 0
--                 WHEN P1.Duration = P2.Duration THEN 0
--                 WHEN @duration <= (((P2.Magnitude - P1.Magnitude) / (P2.Duration - P1.Duration)) * (@duration - P1.Duration) + P1.Magnitude) THEN 1
--                 ELSE 0
--            END AS IsUnder,
--            CASE WHEN P1.Duration > P2.Duration THEN 1
--                 ELSE 0
--            END AS IsBackwards
--        FROM
--            EPRITolerancePoint P1 LEFT OUTER JOIN
--            EPRITolerancePoint P2 ON P1.TestCurveID = P2.TestCurveID AND P1.RowNumber = P2.RowNumber - 1
--        WHERE
--            P1.Duration <> P2.Duration
--    ),
--    EPRIToleranceCurve AS
--    (
--        SELECT
--            ComponentID,
--            TestCurveID,
--            IsUnder
--        FROM
--        (
--            SELECT
--                ROW_NUMBER() OVER(PARTITION BY TestCurveID ORDER BY RowNumber) AS RowNumber,
--                ComponentID,
--                TestCurveID,
--                1 - IsBackwards AS IsUnder
--            FROM
--                EPRIToleranceSegment
--            WHERE
--                IsUnder <> 0
--        ) AS Temp
--        WHERE
--            RowNumber = 1
--    ),
--    USERTolerancePoint AS
--    (
--        SELECT
--            ROW_NUMBER() OVER(PARTITION BY TestCurve.TestCurveID ORDER BY Y, X) AS RowNumber,
--            ComponentID,
--            TestCurve.TestCurveID,
--            Y AS Magnitude,
--            X AS Duration
--        FROM
--            PQInvestigator.UserIndustrialPQ.dbo.TestCurvePoint JOIN
--            PQInvestigator.UserIndustrialPQ.dbo.TestCurve ON TestCurvePoint.TestCurveID = TestCurve.TestCurveID
--    ),
--    USERToleranceSegment AS
--    (
--        SELECT
--            P1.RowNumber,
--            P1.ComponentID,
--            P1.TestCurveID,
--            CASE WHEN @duration < P1.Duration THEN 0
--                 WHEN @magnitude < P1.Magnitude THEN 1
--                 WHEN @duration > P2.Duration THEN 0
--                 WHEN P1.Duration = P2.Duration THEN 0
--                 WHEN @duration <= (((P2.Magnitude - P1.Magnitude) / (P2.Duration - P1.Duration)) * (@duration - P1.Duration) + P1.Magnitude) THEN 1
--                 ELSE 0
--            END AS IsUnder,
--            CASE WHEN P1.Duration > P2.Duration THEN 1
--                 ELSE 0
--            END AS IsBackwards
--        FROM
--            USERTolerancePoint P1 LEFT OUTER JOIN
--            USERTolerancePoint P2 ON P1.TestCurveID = P2.TestCurveID AND P1.RowNumber = P2.RowNumber - 1
--        WHERE
--            P1.Duration <> P2.Duration
--    ),
--    USERToleranceCurve AS
--    (
--        SELECT
--            ComponentID,
--            TestCurveID,
--            IsUnder
--        FROM
--        (
--            SELECT
--                ROW_NUMBER() OVER(PARTITION BY TestCurveID ORDER BY RowNumber) AS RowNumber,
--                ComponentID,
--                TestCurveID,
--                1 - IsBackwards AS IsUnder
--            FROM
--                USERToleranceSegment
--            WHERE
--                IsUnder <> 0
--        ) AS Temp
--        WHERE
--            RowNumber = 1
--    ),
--    FacilityCurve AS
--    (
--        SELECT
--            CurveID,
--            CurveDB,
--            SectionTypeName,
--            Title AS SectionTitle,
--            Rank AS SectionRank,
--            Content AS SectionContent
--        FROM
--            PQInvestigator.UserIndustrialPQ.dbo.FacilityAudit JOIN
--            PQInvestigator.UserIndustrialPQ.dbo.AuditSection ON AuditSection.FacilityAuditID = FacilityAudit.FacilityAuditID JOIN
--            PQInvestigator.UserIndustrialPQ.dbo.SectionType ON AuditSection.SectionTypeID = SectionType.SectionTypeID JOIN
--            PQInvestigator.UserIndustrialPQ.dbo.AuditCurve ON AuditCurve.AuditSectionID = AuditSection.AuditSectionID
--        WHERE
--            AuditCurve.CurveType = 'TOLERANCE' AND
--            FacilityAudit.FacilityID = @facilityID
--    )
--    SELECT
--        FacilityCurve.SectionTypeName,
--        FacilityCurve.SectionTitle,
--        FacilityCurve.SectionRank,
--        FacilityCurve.SectionContent,
--        Component.ComponentModel,
--        Component.ComponentDescription,
--        Manufacturer.ManufacturerName,
--        Series.SeriesName,
--        ComponentType.ComponentTypeName
--    FROM
--        PQInvestigator.IndustrialPQ.dbo.Component JOIN
--        PQInvestigator.IndustrialPQ.dbo.Series ON Component.SeriesID = Series.SeriesID JOIN
--        PQInvestigator.IndustrialPQ.dbo.Manufacturer ON Series.ManufacturerID = Manufacturer.ManufacturerID JOIN
--        PQInvestigator.IndustrialPQ.dbo.ComponentType ON Component.ComponentTypeID = ComponentType.ComponentTypeID JOIN
--        EPRIToleranceCurve ON EPRIToleranceCurve.ComponentID = Component.ComponentID JOIN
--        FacilityCurve ON FacilityCurve.CurveID = EPRIToleranceCurve.TestCurveID
--    WHERE
--        CurveDB = 'EPRI' AND
--        IsUnder <> 0
--    UNION
--    SELECT
--        FacilityCurve.SectionTypeName,
--        FacilityCurve.SectionTitle,
--        FacilityCurve.SectionRank,
--        FacilityCurve.SectionContent,
--        Component.ComponentModel,
--        Component.ComponentDescription,
--        Manufacturer.ManufacturerName,
--        Series.SeriesName,
--        ComponentType.ComponentTypeName
--    FROM
--        PQInvestigator.UserIndustrialPQ.dbo.Component JOIN
--        PQInvestigator.UserIndustrialPQ.dbo.Series ON Component.SeriesID = Series.SeriesID JOIN
--        PQInvestigator.UserIndustrialPQ.dbo.Manufacturer ON Series.ManufacturerID = Manufacturer.ManufacturerID JOIN
--        PQInvestigator.UserIndustrialPQ.dbo.ComponentType ON Component.ComponentTypeID = ComponentType.ComponentTypeID JOIN
--        USERToleranceCurve ON USERToleranceCurve.ComponentID = Component.ComponentID JOIN
--        FacilityCurve ON FacilityCurve.CurveID = USERToleranceCurve.TestCurveID
--    WHERE
--        CurveDB = 'USER' AND
--        IsUnder <> 0
--END
--GO
