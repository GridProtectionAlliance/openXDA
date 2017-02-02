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
    Error INT NOT NULL DEFAULT 0
)
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
	MeterTypeID INT NULL,
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

CREATE TABLE MeterType
(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name VARCHAR(25) NOT NULL
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

CREATE TABLE SeriesType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Description VARCHAR(MAX) NULL
)
GO

INSERT INTO SeriesType(Name, Description) VALUES('Values', 'Instantaneous data values')
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

CREATE TABLE MeterGroup
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE MeterMeterGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    MeterID INT NOT NULL REFERENCES Meter(ID),
    MeterGroupID INT NOT NULL REFERENCES MeterGroup(ID)
)
GO

CREATE TABLE LineGroup
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)

GO

CREATE TABLE LineLineGroup
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LineID] [int] NOT NULL,
	[LineGroupID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)

GO

ALTER TABLE [dbo].[LineLineGroup]  WITH CHECK ADD FOREIGN KEY([LineID])
REFERENCES [dbo].[Line] ([ID])
GO

ALTER TABLE [dbo].[LineLineGroup]  WITH CHECK ADD FOREIGN KEY([LineGroupID])
REFERENCES [dbo].[LineGroup] ([ID])
GO

CREATE TABLE EmailGroupLineGroup
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmailGroupID] [int] NOT NULL,
	[LineGroupID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)

GO

ALTER TABLE [dbo].[EmailGroupLineGroup]  WITH CHECK ADD FOREIGN KEY([LineGroupID])
REFERENCES [dbo].[LineGroup] ([ID])
GO



CREATE TABLE [dbo].[AuditLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](200) NOT NULL,
	[PrimaryKeyColumn] [varchar](200) NOT NULL,
	[PrimaryKeyValue] [varchar](max) NOT NULL,
	[ColumnName] [varchar](200) NOT NULL,
	[OriginalValue] [varchar](max) NULL,
	[NewValue] [varchar](max) NULL,
	[Deleted] [bit] NOT NULL,
	[UpdatedBy] [varchar](200) NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[AuditLog] ADD  CONSTRAINT [DF_AuditLog_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[AuditLog] ADD  CONSTRAINT [DF_AuditLog_UpdatedBy]  DEFAULT (suser_name()) FOR [UpdatedBy]
GO

ALTER TABLE [dbo].[AuditLog] ADD  CONSTRAINT [DF_AuditLog_UpdatedOn]  DEFAULT (getutcdate()) FOR [UpdatedOn]
GO


CREATE NONCLUSTERED INDEX IX_MeterMeterGroup_MeterID
ON MeterMeterGroup(MeterID ASC)
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

INSERT INTO DataWriter(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataWriters.XMLWriter', 1)
GO

INSERT INTO DataWriter(AssemblyName, TypeName, LoadOrder) VALUES('FaultData.dll', 'FaultData.DataWriters.EventEmailWriter', 1)
GO

INSERT INTO MeterGroup(Name) VALUES('AllMeters')
GO

INSERT MeterType (Name) VALUES (N'Breaker')
GO

INSERT MeterType (Name) VALUES (N'Capacitor')
GO

INSERT MeterType (Name) VALUES (N'Line')
GO

INSERT MeterType (Name) VALUES (N'Reactor')
GO

INSERT MeterType (Name) VALUES (N'Other')
GO

CREATE TRIGGER Meter_AugmentAllMetersGroup
ON Meter
AFTER INSERT
AS BEGIN
    SET NOCOUNT ON;

    INSERT INTO MeterMeterGroup(MeterID, MeterGroupID)
    SELECT Meter.ID, MeterGroup.ID
    FROM inserted Meter CROSS JOIN MeterGroup
    WHERE MeterGroup.Name = 'AllMeters'
END
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
    Name VARCHAR(200) NOT NULL,
    Password VARCHAR(200) NULL,
    FirstName VARCHAR(200) NULL,
    LastName VARCHAR(200) NULL,
    DefaultNodeID UNIQUEIDENTIFIER NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    Phone VARCHAR(200) NULL,
    Email VARCHAR(200) NULL,
    LockedOut BIT NOT NULL DEFAULT 0,
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

CREATE TABLE UserAccountMeterGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID),
    MeterGroupID INT NOT NULL REFERENCES MeterGroup(ID)
)
GO

CREATE TRIGGER UserAccount_AugmentAllMetersGroup
ON UserAccount
AFTER INSERT
AS BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO UserAccountMeterGroup(UserAccountID, MeterGroupID)
    SELECT UserAccount.ID, MeterGroup.ID
    FROM inserted UserAccount CROSS JOIN MeterGroup
    WHERE MeterGroup.Name = 'AllMeters'
END
GO

INSERT INTO UserAccount(Name, UseADAuthentication) VALUES('External', 0)
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
    Name VARCHAR(200) NOT NULL,
    Template VARCHAR(MAX) NOT NULL
)
GO

CREATE TABLE EmailGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL
)
GO

ALTER TABLE [dbo].[EmailGroupLineGroup]  WITH CHECK ADD FOREIGN KEY([EmailGroupID])
REFERENCES [dbo].[EmailGroup] ([ID])
GO


CREATE TABLE EmailGroupUserAccount
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID),
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES UserAccount(ID)
)
GO

CREATE TABLE EmailGroupSecurityGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID),
    SecurityGroupID UNIQUEIDENTIFIER NOT NULL REFERENCES SecurityGroup(ID)
)
GO

CREATE TABLE EmailGroupMeterGroup
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID),
    MeterGroupID INT NOT NULL REFERENCES MeterGroup(ID)
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
    XSLTemplateID INT NOT NULL REFERENCES XSLTemplate(ID)
)
GO

CREATE TABLE EmailGroupType
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID),
    EmailTypeID INT NOT NULL REFERENCES EmailType(ID)
)
GO

CREATE TABLE DisturbanceEmailCriterion
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID),
    SeverityCode INT NOT NULL
)
GO

CREATE TABLE FaultEmailCriterion
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID)
)
GO

CREATE TABLE AlarmEmailCriterion
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    EmailGroupID INT NOT NULL REFERENCES EmailGroup(ID),
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
    FrequencyDomainData VARBINARY(MAX) NOT NULL,
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
    EventDataID INT NOT NULL REFERENCES EventData(ID),
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

CREATE TABLE [dbo].[WorkbenchFilter](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[TimeRange] [varchar](512) NOT NULL,
	[Meters] [varchar](max) NULL,
	[Lines] [varchar](max) NULL,
	[EventTypes] [varchar](50) NOT NULL,
	[IsDefault] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

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

INSERT INTO EventType(Name, Description) VALUES ('Fault', 'Fault')
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

INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('ITIC Upper',1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 2, 0.001, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 1.4, 0.003, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 1.2, 0.003, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 1.2, 0.5, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 1.1, 0.5, 5)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 5, 0.0001667, 0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (1, 1.1, 1000000, 6)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('ITIC Lower',1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0, 0.02, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0.7, 0.02, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0.7, 0.5, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0.8, 0.5, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0.8, 10, 5)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0.9, 10, 6)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (2, 0.9, 1000000, 7)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('SEMI',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0, 0.02, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.5, 0.02, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.5, 0.2, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.7, 0.2, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.7, 0.5, 5)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.8, 0.5, 6)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.8, 10, 7)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.9, 10, 8)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (3, 0.9, 1000000, 9)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1668 Recommended Type I & II',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.5, 0.01, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.5, 0.2, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.7, 0.2, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.7, 0.5, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.8, 0.5, 5)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.8, 2, 6)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 1, 2, 7)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 1, 0.01, 8)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (4, 0.5, 0.01, 9)
GO

INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1668 Recommended Type III',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.5, 0.01, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.5, 0.05, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.7, 0.05, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.7, 0.1, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.8, 0.1, 5)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.8, 2, 6)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 1, 2, 7)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 1, 0.01, 8)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (5, 0.5, 0.01, 9)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 1.0 Transients',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (6, 0, 1E-06, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (6, 0, 0.01, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (6, 5, 0.01, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (6, 5, 1E-06, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (6, 0, 1E-06, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.1.1 Instantaneous Sag',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (7, 0.1, 0.01, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (7, 0.1, 0.5, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (7, 0.9, 0.5, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (7, 0.9, 0.01, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (7, 0.1, 0.01, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.1.2 Instantaneous Swell',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (8, 1.1, 0.01, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (8, 1.1, 0.5, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (8, 1.8, 0.5, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (8, 1.8, 0.01, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (8, 1.1, 0.01, 5)
GO

INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.2.1 Mom. Interruption',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (9, 0, 0.01, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (9, 0, 3, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (9, 0.1, 3, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (9, 0.1, 0.01, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (9, 0, 0.01, 5)
GO

INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.2.2 Momentary Sag',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (10, 0.1, 0.5, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (10, 0.1, 3, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (10, 0.9, 3, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (10, 0.9, 0.5, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (10, 0.1, 0.5, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.2.3 Momentary Swell',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (11, 1.1, 0.5, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (11, 1.1, 3, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (11, 1.4, 3, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (11, 1.4, 0.5, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (11, 1.1, 0.5, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.3.1 Temp. Interruption',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (12, 0, 3, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (12, 0, 60, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (12, 0.1, 60, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (12, 0.1, 3, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (12, 0, 3, 5)
GO

INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.3.2 Temporary Sag',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (13, 0.1, 3, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (13, 0.1, 60, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (13, 0.9, 60, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (13, 0.9, 3, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (13, 0.1, 3, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 2.3.3 Temporary Swell',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (14, 1.1, 3, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (14, 1.1, 60, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (14, 1.2, 60, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (14, 1.2, 3, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (14, 1.1, 3, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 3.1 Sustained Int.',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (15, 0, 60, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (15, 0, 1000000, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (15, 0.1, 1000000, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (15, 0.1, 60, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (15, 0, 60, 5)
GO

INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 3.2 Undervoltage',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (16, 0.8, 60, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (16, 0.8, 1000000, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (16, 0.9, 1000000, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (16, 0.9, 60, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (16, 0.8, 60, 5)
GO


INSERT [dbo].[WorkbenchVoltageCurve] ([Name],[Visible]) VALUES ('IEEE 1159 3.3 Overvoltage',0)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (17, 1.1, 60, 1)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (17, 1.1, 1000000, 2)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (17, 1.2, 1000000, 3)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (17, 1.2, 60, 4)
GO
INSERT [dbo].[WorkbenchVoltageCurvePoint] ([VoltageCurveID], [PerUnitMagnitude], [DurationSeconds], [LoadOrder]) VALUES (17, 1.1, 60, 5)
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
	HasResultFunction VARCHAR(50) NOT NULL
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

----- FUNCTIONS -----

CREATE FUNCTION ComputeHash
(
    @eventID INT,
    @templateID INT
)
RETURNS BIGINT
BEGIN
    DECLARE @md5Hash BINARY(16)

    SELECT @md5Hash = master.sys.fn_repl_hash_binary(CONVERT(VARBINARY(MAX), EventDetail))
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

    SELECT @minStartTime = MIN(dbo.AdjustDateTime2(StartTime, -@timeTolerance)), @maxEndTime = MAX(dbo.AdjustDateTime2(EndTime, @timeTolerance))
    FROM Event
    WHERE
        StartTime <= @adjustedEndTime AND
        @adjustedStartTime <= EndTime

    WHILE @startTime != @minStartTime OR @endTime != @maxEndTime
    BEGIN
        SET @startTime = @minStartTime
        SET @endTime = @maxEndTime
        SET @adjustedStartTime = dbo.AdjustDateTime2(@startTime, -@timeTolerance)
        SET @adjustedEndTime = dbo.AdjustDateTime2(@endTime, @timeTolerance)

        SELECT @minStartTime = MIN(dbo.AdjustDateTime2(StartTime, -@timeTolerance)), @maxEndTime = MAX(dbo.AdjustDateTime2(EndTime, @timeTolerance))
        FROM Event
        WHERE
            StartTime <= @adjustedEndTime AND
            @adjustedStartTime <= EndTime
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

CREATE VIEW MeterDetail
AS
SELECT
    Meter.ID,
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
    CASE COALESCE(Meter.TimeZone, '')
        WHEN '' THEN COALESCE(Setting.Value, 'UTC')
        ELSE Meter.TimeZone
    END AS TimeZone,
    Meter.Description, 
	MeterType.Name AS MeterType, 
	Meter.MeterTypeID
FROM
    Meter JOIN
    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID  LEFT OUTER JOIN
    dbo.MeterType ON dbo.Meter.MeterTypeID = dbo.MeterType.ID LEFT OUTER JOIN
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

CREATE VIEW MeterMeterGroupView
AS
SELECT
    MeterMeterGroup.ID,
    Meter.Name AS MeterName,
    Meter.ID AS MeterID,
    MeterGroupID,
    MeterLocation.Name AS Location
FROM
    MeterMeterGroup JOIN
    Meter ON MeterMeterGroup.MeterID = Meter.ID JOIN
    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
GO

CREATE VIEW UserAccountMeterGroupView
AS
SELECT
    UserAccountMeterGroup.ID,
    UserAccountMeterGroup.UserAccountID,
    UserAccountMeterGroup.MeterGroupID,
    UserAccount.Name AS Username,
    MeterGroup.Name AS GroupName
FROM
    UserAccountMeterGroup JOIN
    UserAccount ON UserAccountMeterGroup.UserAccountID = UserAccount.ID JOIN
    MeterGroup ON UserAccountMeterGroup.MeterGroupID = MeterGroup.ID
GO

CREATE VIEW UserMeter
AS
SELECT
	UserAccount.Name AS UserName,
	Meter.ID AS MeterID
FROM
	Meter JOIN
	MeterMeterGroup ON MeterMeterGroup.MeterID = Meter.ID JOIN
	UserAccountMeterGroup ON MeterMeterGroup.MeterGroupID = UserAccountMeterGroup.MeterGroupID JOIN
	UserAccount ON UserAccountMeterGroup.UserAccountID = UserAccount.ID
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

CREATE VIEW EventDetail AS
WITH TimeTolerance AS
(
    SELECT
        COALESCE(CAST(Value AS FLOAT), 0.5) AS Tolerance
    FROM
        (SELECT 'TimeTolerance' AS Name) AS SettingName LEFT OUTER JOIN
        Setting ON SettingName.Name = Setting.Name
),
SelectedDisturbance AS
(
    SELECT Disturbance.*
    FROM
        Disturbance JOIN
        Phase ON Disturbance.PhaseID = Phase.ID JOIN
        Disturbance WorstDisturbance ON
            Disturbance.EventID = WorstDisturbance.EventID AND
            Disturbance.Magnitude = WorstDisturbance.Magnitude AND
            Disturbance.DurationCycles = WorstDisturbance.DurationCycles JOIN
        Phase WorstPhase ON WorstDisturbance.PhaseID = WorstPhase.ID
    WHERE
        Phase.Name <> 'Worst' AND
        WorstPhase.Name = 'Worst'
),
DisturbanceData AS
(
    SELECT
        SelectedDisturbance.ID AS DisturbanceID,
        Meter.AssetKey AS MeterKey,
        MeterLocation.Name AS StationName,
        MeterLine.LineName,
        EventType.Name AS DisturbanceType,
        Phase.Name AS Phase,
        SelectedDisturbance.StartTime,
        SelectedDisturbance.EndTime,
        SelectedDisturbance.DurationCycles,
        SelectedDisturbance.DurationSeconds * 1000.0 AS DurationMilliseconds,
        SelectedDisturbance.Magnitude,
        SelectedDisturbance.PerUnitMagnitude,
        RIGHT(DataFile.FilePath, CHARINDEX('\', REVERSE(DataFile.FilePath)) - 1) AS FileName,
        SelectedDisturbance.EventID,
        Event.StartTime AS EventStartTime,
        Event.EndTime AS EventEndTime
    FROM
        SelectedDisturbance JOIN
        Event ON SelectedDisturbance.EventID = Event.ID JOIN
        EventType ON SelectedDisturbance.EventTypeID = EventType.ID JOIN
        DataFile ON DataFile.FileGroupID = Event.FileGroupID JOIN
        Meter ON Event.MeterID = Meter.ID JOIN
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
        MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Event.LineID JOIN
        Phase ON SelectedDisturbance.PhaseID = Phase.ID
    WHERE
        DataFile.FilePath LIKE '%.DAT' OR
        DataFile.FilePath LIKE '%.D00' OR
        DataFile.FilePath LIKE '%.PQD' OR
        DataFile.FilePath LIKE '%.RCD' OR
        DataFile.FilePath LIKE '%.RCL'
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
        SelectedSummary.PrefaultCurrent,
        SelectedSummary.PostfaultCurrent,
        SelectedSummary.CurrentMagnitude AS FaultCurrent,
        SelectedSummary.Algorithm,
        SelectedSummary.Distance AS SingleEndedDistance,
        DoubleEndedFaultSummary.Distance AS DoubleEndedDistance,
        DoubleEndedFaultSummary.Angle AS DoubleEndedAngle,
        RIGHT(DataFile.FilePath, CHARINDEX('\', REVERSE(DataFile.FilePath)) - 1) AS FileName,
        SelectedSummary.EventID,
        Event.StartTime AS EventStartTime,
        Event.EndTime AS EventEndTime
    FROM
        SelectedSummary JOIN
        Event ON SelectedSummary.EventID = Event.ID JOIN
        DataFile ON DataFile.FileGroupID = Event.FileGroupID JOIN
        Meter ON Event.MeterID = Meter.ID JOIN
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
        MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Event.LineID LEFT OUTER JOIN
        DoubleEndedFaultDistance ON DoubleEndedFaultDistance.LocalFaultSummaryID = SelectedSummary.ID LEFT OUTER JOIN
        DoubleEndedFaultSummary ON DoubleEndedFaultSummary.ID = DoubleEndedFaultDistance.ID
    WHERE
        DataFile.FilePath LIKE '%.DAT' OR
        DataFile.FilePath LIKE '%.D00' OR
        DataFile.FilePath LIKE '%.PQD' OR
        DataFile.FilePath LIKE '%.RCD' OR
        DataFile.FilePath LIKE '%.RCL'
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
                    SequenceNumber AS [@num],
                    (
                        SELECT
                            MeterKey,
                            StationName,
                            LineName,
                            DisturbanceType,
                            Phase,
                            StartTime,
                            EndTime,
                            DurationCycles,
                            DurationMilliseconds,
                            Magnitude,
                            PerUnitMagnitude,
                            EventStartTime,
                            EventEndTime,
                            FileName,
                            EventID,
                            DisturbanceID
                        FROM DisturbanceData
                        WHERE DisturbanceID IN
                        (
                            SELECT PhaseDisturbance.ID
                            FROM
                                dbo.GetDisturbancesInSystemEvent(Event.StartTime, Event.EndTime, (SELECT * FROM TimeTolerance)) InnerSequenceNumber JOIN
                                Disturbance ON Disturbance.ID = InnerSequenceNumber.ID JOIN
                                Disturbance PhaseDisturbance ON
                                    Disturbance.EventID = PhaseDisturbance.EventID AND
                                    Disturbance.StartTime = PhaseDisturbance.StartTime AND
                                    Disturbance.PerUnitMagnitude = PhaseDisturbance.PerUnitMagnitude
                            WHERE InnerSequenceNumber.SequenceNumber = OuterSequenceNumber.SequenceNumber
                        )
                        ORDER BY DisturbanceData.StartTime
                        FOR XML PATH('Disturbance'), TYPE
                    )
                FROM
                (
                    SELECT DISTINCT SequenceNumber
                    FROM dbo.GetDisturbancesInSystemEvent(Event.StartTime, Event.EndTime, (SELECT * FROM TimeTolerance))
                ) OuterSequenceNumber
                FOR XML PATH('DisturbanceGroup'), TYPE
            ) AS [DisturbanceGroups],
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
                            PrefaultCurrent,
                            PostfaultCurrent,
                            FaultCurrent,
                            Algorithm,
                            SingleEndedDistance,
                            DoubleEndedDistance,
                            DoubleEndedAngle,
                            EventStartTime,
                            EventEndTime,
                            FileName,
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
            CASE LineImpedance.R1 WHEN 0 THEN '0' ELSE FORMAT(ATN2(LineImpedance.X1, LineImpedance.R1) * 180 / PI(), '0.##########') END AS [Line/A1],
            FORMAT(LineImpedance.R1, '0.##########') AS [Line/R1],
            FORMAT(LineImpedance.X1, '0.##########') AS [Line/X1],
            FORMAT(SQRT(LineImpedance.R0 * LineImpedance.R0 + LineImpedance.X0 * LineImpedance.X0), '0.##########') AS [Line/Z0],
            CASE LineImpedance.R0 WHEN 0 THEN '0' ELSE FORMAT(ATN2(LineImpedance.X0, LineImpedance.R0) * 180 / PI(), '0.##########') END AS [Line/A0],
            FORMAT(LineImpedance.R0, '0.##########') AS [Line/R0],
            FORMAT(LineImpedance.X0, '0.##########') AS [Line/X0],
            FORMAT(SQRT(POWER((2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0, 2) + POWER((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, 2)), '0.##########') AS [Line/ZS],
            CASE 2.0 * LineImpedance.R1 + LineImpedance.R0 WHEN 0 THEN '0' ELSE FORMAT(ATN2((2.0 * LineImpedance.X1 + LineImpedance.X0) / 3.0, (2.0 * LineImpedance.R1 + LineImpedance.R0) / 3.0) * 180 / PI(), '0.##########') END AS [Line/AS],
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

CREATE VIEW [dbo].[EventView]
AS
SELECT	dbo.Event.ID, dbo.Event.FileGroupID, dbo.Event.MeterID, dbo.Event.LineID, dbo.Event.EventTypeID, dbo.Event.EventDataID, dbo.Event.Name, dbo.Event.Alias, dbo.Event.ShortName, dbo.Event.StartTime, 
		dbo.Event.EndTime, dbo.Event.Samples, dbo.Event.TimeZoneOffset, dbo.Event.SamplesPerSecond, dbo.Event.SamplesPerCycle, dbo.Event.Description,
		    (SELECT        TOP (1) LineName
		      FROM            dbo.MeterLine
		      WHERE        (LineID = dbo.Line.ID)) AS LineName, dbo.Meter.Name AS MeterName, dbo.Line.Length, dbo.EventType.Name AS EventTypeName
FROM	dbo.Event INNER JOIN
		dbo.Line ON dbo.Event.LineID = dbo.Line.ID INNER JOIN
		dbo.Meter ON dbo.Event.MeterID = dbo.Meter.ID INNER JOIN
		dbo.EventType ON dbo.Event.EventTypeID = dbo.EventType.ID

GO

CREATE VIEW [dbo].[DisturbanceView]
AS
SELECT dbo.Disturbance.ID, dbo.Disturbance.EventID, dbo.Disturbance.EventTypeID, dbo.Disturbance.PhaseID, dbo.Disturbance.Magnitude, dbo.Disturbance.PerUnitMagnitude, dbo.Disturbance.StartTime, 
       dbo.Disturbance.EndTime, dbo.Disturbance.DurationSeconds, dbo.Disturbance.DurationCycles, dbo.Disturbance.StartIndex, dbo.Disturbance.EndIndex, dbo.Event.MeterID,
           (SELECT        MAX(SeverityCode) AS Expr1
             FROM            dbo.DisturbanceSeverity
             WHERE        (DisturbanceID = dbo.Disturbance.ID)) AS SeverityCode, dbo.Meter.Name AS MeterName, dbo.Phase.Name AS PhaseName, dbo.Event.LineID
FROM   dbo.Disturbance INNER JOIN
       dbo.Event ON dbo.Disturbance.EventID = dbo.Event.ID INNER JOIN
       dbo.Meter ON dbo.Event.MeterID = dbo.Meter.ID INNER JOIN
       dbo.Phase ON dbo.Disturbance.PhaseID = dbo.Phase.ID

GO

CREATE VIEW [dbo].[BreakerView]
AS
SELECT dbo.BreakerOperation.ID, dbo.Meter.ID AS MeterID, dbo.Event.ID AS EventID, dbo.EventType.Name AS EventType, dbo.BreakerOperation.TripCoilEnergized AS Energized, dbo.BreakerOperation.BreakerNumber, 
       dbo.MeterLine.LineName, dbo.Phase.Name AS PhaseName, CAST(dbo.BreakerOperation.BreakerTiming AS DECIMAL(16, 5)) AS Timing, dbo.BreakerOperation.BreakerSpeed AS Speed, 
       dbo.BreakerOperationType.Name AS OperationType
FROM   dbo.BreakerOperation INNER JOIN
       dbo.Event ON dbo.BreakerOperation.EventID = dbo.Event.ID INNER JOIN
       dbo.EventType ON dbo.EventType.ID = dbo.Event.EventTypeID INNER JOIN
       dbo.Meter ON dbo.Meter.ID = dbo.Event.MeterID INNER JOIN
       dbo.Line ON dbo.Line.ID = dbo.Event.LineID INNER JOIN
       dbo.MeterLine ON dbo.MeterLine.LineID = dbo.Event.LineID AND dbo.MeterLine.MeterID = dbo.Meter.ID INNER JOIN
       dbo.BreakerOperationType ON dbo.BreakerOperation.BreakerOperationTypeID = dbo.BreakerOperationType.ID INNER JOIN
       dbo.Phase ON dbo.BreakerOperation.PhaseID = dbo.Phase.ID

GO

CREATE VIEW [dbo].[FaultView]
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
    CASE WHEN FaultSummary.Distance = '-1E308' THEN 'NaN' ELSE CAST(CAST(FaultSummary.Distance AS DECIMAL(16,2)) AS NVARCHAR(19)) END AS CurrentDistance,
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

CREATE VIEW [dbo].[WorkbenchVoltageCurveView]
AS
SELECT	dbo.WorkbenchVoltageCurve.ID, dbo.WorkbenchVoltageCurve.Name, dbo.WorkbenchVoltageCurvePoint.ID AS CurvePointID, dbo.WorkbenchVoltageCurvePoint.PerUnitMagnitude, 
		dbo.WorkbenchVoltageCurvePoint.DurationSeconds, dbo.WorkbenchVoltageCurvePoint.LoadOrder, dbo.WorkbenchVoltageCurve.Visible
FROM	dbo.WorkbenchVoltageCurve INNER JOIN
		dbo.WorkbenchVoltageCurvePoint ON dbo.WorkbenchVoltageCurve.ID = dbo.WorkbenchVoltageCurvePoint.VoltageCurveID AND 
		dbo.WorkbenchVoltageCurve.ID = dbo.WorkbenchVoltageCurvePoint.VoltageCurveID AND dbo.WorkbenchVoltageCurve.ID = dbo.WorkbenchVoltageCurvePoint.VoltageCurveID

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
        EmailGroupUserAccount.UserAccountID,
        EmailType.XSLTemplateID AS TemplateID
    FROM
        GetSystemEventIDs(@startTime, @endTime, @timeTolerance) SystemEventID JOIN
        Event ON SystemEventID.EventID = Event.ID JOIN
        Meter ON Event.MeterID = Meter.ID JOIN
        MeterMeterGroup ON MeterMeterGroup.MeterID = Meter.ID JOIN
        EmailGroupMeterGroup ON MeterMeterGroup.MeterGroupID = EmailGroupMeterGroup.MeterGroupID JOIN
        (
            SELECT
                EmailGroupID,
                UserAccountID
            FROM EmailGroupUserAccount
            UNION ALL
            SELECT
                EmailGroupSecurityGroup.EmailGroupID,
                UserAccount.ID
            FROM
                EmailGroupSecurityGroup JOIN
                SecurityGroupUserAccount ON EmailGroupSecurityGroup.SecurityGroupID = SecurityGroupUserAccount.SecurityGroupID JOIN
                UserAccount on SecurityGroupUserAccount.UserAccountID = UserAccount.ID
        ) EmailGroupUserAccount ON EmailGroupMeterGroup.EmailGroupID = EmailGroupUserAccount.EmailGroupID JOIN
        EmailGroupType ON EmailGroupMeterGroup.EmailGroupID = EmailGroupType.EmailGroupID JOIN
        EmailType ON EmailGroupType.EmailTypeID = EmailType.ID JOIN
        EmailCategory ON
            EmailType.EmailCategoryID = EmailCategory.ID AND
            EmailCategory.Name = 'Event'
    WHERE
        Event.LineID = @lineID AND
        (
            NOT EXISTS
            (
                SELECT *
                FROM FaultEmailCriterion
                WHERE EmailGroupID = EmailGroupMeterGroup.EmailGroupID
            )
            OR EXISTS
            (
                SELECT *
                FROM FaultGroup
                WHERE
                    EventID = Event.ID AND
                    (
                        (
                            FaultDetectionLogicResult IS NOT NULL AND
                            FaultDetectionLogicResult <> 0
                        )
                        OR
                        (
                            FaultDetectionLogicResult IS NULL AND
                            FaultValidationLogicResult <> 0 AND
                            (
                                SELECT COALESCE(Value, 1)
                                FROM
                                    (VALUES('UseDefaultFaultDetectionLogic')) SettingName(Name) LEFT OUTER JOIN
                                    Setting ON SettingName.Name = Setting.Name
                                WHERE SettingName.Name = 'UseDefaultFaultDetectionLogic'
                            ) <> 0
                        )
                    )
            )
        )
        AND
        (
            NOT EXISTS
            (
                SELECT *
                FROM DisturbanceEmailCriterion
                WHERE EmailGroupID = EmailGroupMeterGroup.EmailGroupID
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
                    DisturbanceEmailCriterion.EmailGroupID = EmailGroupMeterGroup.EmailGroupID
            )
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
-- Author:		<Author, William Ernest/ Stephen Wills>
-- Create date: <Create Date,12/1/2016>
-- Description:	<Description, Calls usp_delete_cascade to perform cascading deletes for a table>
-- =============================================
CREATE PROCEDURE [dbo].[UniversalCascadeDelete]
	-- Add the parameters for the stored procedure here
	@tableName VARCHAR(200),
	@baseCriteria NVARCHAR(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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

CREATE PROCEDURE [dbo].[InsertIntoAuditLog] (@tableName VARCHAR(128), @primaryKeyColumn VARCHAR(128), @primaryKeyValue NVARCHAR(MAX), @deleted BIT = '0', @inserted BIT = '0') AS	
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
		
		IF @deleted = '0' AND @inserted = '1'
			SET @sql = 'INSERT INTO AuditLog (TableName, PrimaryKeyColumn, PrimaryKeyValue, ColumnName, OriginalValue, NewValue, Deleted, UpdatedBy) ' +
					'SELECT ''' + @tableName + ''', ''' + @primaryKeyColumn + ''', ''' + @primaryKeyValue + ''', ''' + @columnName + ''', ' +
					'NULL, CONVERT(NVARCHAR(MAX), #inserted.' + @columnName + '), ''0'', #inserted.UpdatedBy FROM #inserted'
		ELSE IF @deleted = '1' AND @inserted = '0'
			BEGIN
				DECLARE @context VARCHAR(128)
				SELECT @context = CASE WHEN CONTEXT_INFO() IS NULL THEN SUSER_NAME() ELSE CAST(CONTEXT_INFO() AS VARCHAR(128)) END
				SET @sql = 'INSERT INTO AuditLog (TableName, PrimaryKeyColumn, PrimaryKeyValue, ColumnName, OriginalValue, NewValue, Deleted, UpdatedBy) ' +
						'SELECT ''' + @tableName + ''', ''' + @primaryKeyColumn + ''', ''' + @primaryKeyValue + ''', ''' + @columnName + ''', ' +
						'CONVERT(NVARCHAR(MAX), #deleted.' + @columnName + '), NULL, ''1'', ''' + @context + ''' FROM #deleted'
			END
		ELSE
			SET @sql = 'DECLARE @oldVal NVARCHAR(MAX) ' +
					'DECLARE @newVal NVARCHAR(MAX) ' +
					'SELECT @oldVal = CONVERT(NVARCHAR(MAX), #deleted.' + @columnName + '), @newVal = CONVERT(NVARCHAR(MAX), #inserted.' + @columnName + ') FROM #deleted, #inserted ' +
					'IF @oldVal <> @newVal BEGIN ' +			
					'INSERT INTO AuditLog (TableName, PrimaryKeyColumn, PrimaryKeyValue, ColumnName, OriginalValue, NewValue, Deleted, UpdatedBy) ' +
					'SELECT ''' + @tableName + ''', ''' + @primaryKeyColumn + ''', ''' + @primaryKeyValue + ''', ''' + @columnName + ''', ' +
					'CONVERT(NVARCHAR(MAX), #deleted.' + @columnName + '), CONVERT(NVARCHAR(MAX), #inserted.' + @columnName + '), ''0'', #inserted.UpdatedBy ' +
					'FROM #inserted, #deleted ' +
					'END'

		EXECUTE (@sql)
		
		FETCH NEXT FROM @cursorColumnNames INTO @columnName 
	END 

	CLOSE @cursorColumnNames 
	DEALLOCATE @cursorColumnNames
	
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER Event_AuditInsert 
   ON  Event
   AFTER INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT * INTO #inserted FROM inserted
	
	DECLARE @id NVARCHAR(MAX)		
	SELECT @id = CONVERT(NVARCHAR(MAX), ID) FROM #inserted	

	EXEC InsertIntoAuditLog 'Event', 'ID', @id, '0', '1'
	
	DROP TABLE #inserted

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
	
	DECLARE @id NVARCHAR(MAX)		
	SELECT @id = CONVERT(NVARCHAR(MAX), ID) FROM #deleted	

	EXEC InsertIntoAuditLog 'Event', 'ID', @id
	
	DROP TABLE #inserted
	DROP TABLE #deleted

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER Event_AuditDelete
   ON  Event
   AFTER DELETE
AS 
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT * INTO #deleted FROM deleted
		
	DECLARE @id NVARCHAR(MAX)		
	SELECT @id = CONVERT(NVARCHAR(MAX), ID) FROM #deleted	

	EXEC InsertIntoAuditLog 'Event', 'ID', @id, '1'
		
	DROP TABLE #deleted

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER Disturbance_AuditInsert 
   ON  Disturbance
   AFTER INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT * INTO #inserted FROM inserted

	DECLARE @id NVARCHAR(MAX)		
	SELECT @id = CONVERT(NVARCHAR(MAX), ID) FROM #inserted
	
	EXEC InsertIntoAuditLog 'Disturbance', 'ID', @id, '0', '1'
	
	DROP TABLE #inserted

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER Disturbance_AuditUpdate 
   ON  Disturbance
   AFTER UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT * INTO #deleted  FROM deleted
	SELECT * INTO #inserted FROM inserted

	DECLARE @id NVARCHAR(MAX)		
	SELECT @id = CONVERT(NVARCHAR(MAX), ID) FROM #deleted	
	
	EXEC InsertIntoAuditLog 'Disturbance', 'ID', @id
	
	DROP TABLE #inserted
	DROP TABLE #deleted

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER Disturbance_AuditDelete
   ON  Disturbance
   AFTER DELETE
AS 
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT * INTO #deleted FROM deleted
		
	DECLARE @id NVARCHAR(MAX)		
	SELECT @id = CONVERT(NVARCHAR(MAX), ID) FROM #deleted	

	EXEC InsertIntoAuditLog 'Disturbance', 'ID', @id, '1'
		
	DROP TABLE #deleted

END
GO


----- Email Templates -----

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
                            <td><xsl:value-of select="MeterKey" /> at <xsl:value-of select="StationName" /> triggered at <format type="System.DateTime" spec="HH:mm:ss.fffffff"><xsl:value-of select="EventStartTime" /></format> (<a><xsl:attribute name="href">http://pqserver/pqdashboard/openSeeStack.aspx?eventid=<xsl:value-of select="EventID" /></xsl:attribute>click for waveform</a>)</td>
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
