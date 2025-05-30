﻿---------------- System Center TableSpace -------------
CREATE TABLE [SystemCenter.Setting](
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

CREATE TABLE [SystemCenter.AccessLog](
    ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserName varchar(200) NOT NULL,
    AccessGranted bit NOT NULL,
    CreatedOn datetime NOT NULL CONSTRAINT [DF_SC_AccessLog_Timestamp]  DEFAULT (getutcdate())
)
GO

CREATE TABLE [SystemCenter.ApplicationRole]
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

CREATE TABLE [SystemCenter.SecurityGroup]
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

CREATE TABLE [SystemCenter.UserAccount]
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
    TSCID INT NULL,
    RoleID INT NULL,
    Title varchar(200) NULL,
    Department varchar(200) NULL,
    DepartmentNumber varchar(200) NULL,
    MobilePhone VARCHAR(200) NULL,
    ReceiveNotifications BIT NOT NULL DEFAULT 1,
    UseADAuthentication BIT NOT NULL DEFAULT 1,
    ChangePasswordOn DATETIME NULL DEFAULT DATEADD(DAY, 90, GETUTCDATE()),
    CreatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    UpdatedOn DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy VARCHAR(50) NOT NULL DEFAULT SUSER_NAME()
)
GO

CREATE TABLE [SystemCenter.ApplicationRoleSecurityGroup]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ApplicationRoleID UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.ApplicationRole](ID),
    SecurityGroupID UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.SecurityGroup](ID)
)
GO

CREATE TABLE [SystemCenter.ApplicationRoleUserAccount]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    ApplicationRoleID UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.ApplicationRole](ID),
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.UserAccount](ID)
)
GO

CREATE TABLE [SystemCenter.SecurityGroupUserAccount]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    SecurityGroupID UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.ApplicationRole](ID),
    UserAccountID UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.UserAccount](ID)
)
GO

CREATE TABLE AdditionalUserField(
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FieldName] [varchar](100) NOT NULL,
	[Type] [varchar](max) NOT NULL DEFAULT ('string'),
	[IsSecure] [bit] NOT NULL DEFAULT (0),
) 
GO

CREATE TABLE AdditionalUserFieldValue(
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserAccountID] UNIQUEIDENTIFIER NOT NULL REFERENCES [UserAccount](ID),
	[AdditionalUserFieldID] [int] NOT NULL FOREIGN KEY References [AdditionalUserField](ID),
	[Value] [varchar](max) NULL,

)
GO

CREATE Table [ExternalDatabases] (
    ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name varchar(200) NOT NULL,
    Schedule varchar(50) NULL DEFAULT(NULL),
    ConnectionString Varchar(MAX) NOT NULL,
    DataProviderString VARCHAR(MAX) NULL,
    Encrypt bit NOT NULL DEFAULT(0),
    LastDataUpdate DATETIME2 NULL DEFAULT(NULL),
    Constraint UC_ExternalDatabase UNIQUE(Name)
)
GO

CREATE Table [extDBTables] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TableName varchar(200) NOT NULL,
    ExtDBID INT NOT NULL FOREIGN KEY References [ExternalDatabases](ID),
	Query varchar(max) NOT NULL,
    Constraint UC_ExternalDatabaseTable UNIQUE(TableName, ExtDBID)
)
GO

CREATE TABLE [AdditionalField] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTable varchar(100) NOT NULL,
	FieldName varchar(100) NOT NULL,
	Type varchar(max) NULL DEFAULT ('string'),
	ExternalDBTableID INT NULL FOREIGN KEY REFERENCES [extDBTables](ID),
	IsSecure bit NULL DEFAULT(0),
    Searchable bit NULL DEFAULT(0),
    IsInfo bit NOT NULL DEFAULT(0),
    IsKey bit NOT NULL DEFAULT(0),
	Constraint UC_AdditonaField UNIQUE(ParentTable, FieldName)
)
GO

CREATE TABLE [AdditionalFieldValue] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTableID int NOT NULL,
	AdditionalFieldID int NOT NULL FOREIGN KEY REFERENCES [AdditionalField](ID),
	Value varchar(max) NULL,
    UpdatedOn DATE NULL DEFAULT (SYSDATETIME()),
	Constraint UC_AdditonaFieldValue UNIQUE(ParentTableID, AdditionalFieldID)
)
GO

CREATE TRIGGER [dbo].[UO_AdditonaFieldValue] 
ON [dbo].[AdditionalFieldValue]
AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE original SET UpdatedOn = SYSDATETIME()
		FROM dbo.[AdditionalFieldValue] as original INNER JOIN deleted ON 
		original.ID = deleted.ID AND 
		(
			original.Value <> deleted.Value OR
			(original.Value IS NULL AND deleted.Value IS NOT NULL) OR
			(original.Value IS NOT NULL AND deleted.Value IS NULL)
		);
END
GO

CREATE VIEW AdditionalFieldSearch AS 
	SELECT 
		AdditionalFieldValue.ParentTableID,
		AdditionalFieldValue.Value,
		AdditionalField.ParentTable,
		AdditionalField.FieldName
		FROM 
			AdditionalField LEFT JOIN
			AdditionalFieldValue ON AdditionalFieldValue.AdditionalFieldID = AdditionalField.ID
GO

CREATE TABLE [ExternalOpenXDAField] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTable varchar(100) NOT NULL,
	FieldName varchar(100) NOT NULL,
    ExternalDBTableID INT NOT NULL FOREIGN KEY References [extDBTables](ID),
	Constraint UC_ExternalOpenXDAField UNIQUE(ParentTable, FieldName)
)
GO

CREATE TABLE [ADRole] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(200) NOT NULL,
	Description varchar(max) NULL,
)
GO

CREATE TABLE [OpenMICDailyStatistic] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    [Date] VARCHAR(10) not null,
    Meter VARCHAR(50) not null,
    LastSuccessfulConnection DATETIME NULL,
    LastUnsuccessfulConnection DATETIME NULL,
    LastUnsuccessfulConnectionExplanation VARCHAR(MAX) NULL,
    TotalConnections INT NOT NULL,
    TotalUnsuccessfulConnections INT NOT NULL,
    TotalSuccessfulConnections INT NOT NULL,
    BadDays INT NOT NULL,
    [Status] VARCHAR(7) NULL,
    CONSTRAINT UC_SystemCenter_OpenMICDailyStatistic_Date_Meter UNIQUE ([Date],Meter)
)
GO

CREATE TABLE [OpenXDADailyStatistic] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    [Date] VARCHAR(10) not null,
    Meter VARCHAR(50) not null,
    LastSuccessfulFileProcessed DATETIME NULL,
    LastUnsuccessfulFileProcessed DATETIME NULL,
    LastUnsuccessfulFileProcessedExplanation VARCHAR(MAX) NULL,
    TotalFilesProcessed INT NOT NULL,
    TotalUnsuccessfulFilesProcessed INT NOT NULL,
    TotalSuccessfulFilesProcessed INT NOT NULL,
	TotalEmailsSent INT NOT NULL,
    AverageDownloadLatency FLOAT NULL,
    AverageProcessingStartLatency FLOAT NULL,
    AverageProcessingEndLatency FLOAT NULL,
    AverageEmailLatency FLOAT NULL,
    AverageTotalProcessingLatency FLOAT NULL,
    AverageTotalEmailLatency FLOAT NULL,
    BadDays INT NOT NULL,
    [Status] VARCHAR(7) NULL,
    CONSTRAINT UC_SystemCenter_OpenXDADailyStatistic_Date_Meter UNIQUE ([Date],Meter)
)
GO

CREATE TABLE [MiMDDailyStatistic] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    [Date] VARCHAR(10) not null,
    Meter VARCHAR(50) not null,
    LastSuccessfulFileProcessed DATETIME NULL,
    LastUnsuccessfulFileProcessed DATETIME NULL,
    LastConfigFileChange DATETIME NULL,
    LastUnsuccessfulFileProcessedExplanation VARCHAR(MAX) NULL,
    TotalFilesProcessed INT NOT NULL,
    TotalUnsuccessfulFilesProcessed INT NOT NULL,
    TotalSuccessfulFilesProcessed INT NOT NULL, 
    ConfigChanges INT NOT NULL,
    DiagnosticAlarms INT NOT NULL,
    ComplianceIssues INT NOT NULL,
    BadDays INT NOT NULL,
    [Status] VARCHAR(7) NULL,
    CONSTRAINT UC_SystemCenter_MiMDDailyStatistic_Date_Meter UNIQUE ([Date],Meter)
)
GO


CREATE TABLE [LocationDrawing] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    LocationID INT not null FOREIGN KEY REFERENCES Location(ID),
    Name VARCHAR(200) NOT NUll,
    Link VARCHAR(max) NOT NUll,
    Description VARCHAR(max) NULL,
    Number VARCHAR(200) NULL,
    Category VARCHAR(max) NULL
)
GO

CREATE TABLE [LSCVSAccount] (
    ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    AccountID VARCHAR(200) NOT NULL,
    ChannelID int NOT NULL DEFAULT(1),
    CustomerID int NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(ID)
)
GO

-- Default Settings --
Insert into ValueListGroup (Name, Description) VALUES('SpareChannel', 'List of spare channel names and descriptions for filtering in new meter wizard, case insensitive.')
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Spare',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Virtual Spare',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Spare Virtual',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Current Spare',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Spare Current',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Voltage Spare',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Spare Voltage',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Spare Trigger',0)
GO
Insert into ValueList (GroupID, Value, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'SpareChannel'), 'Spare Channel',0)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.AdminAddress', 'SystemCenter-admin@gridprotectionalliance.org', 'SystemCenter-admin@gridprotectionalliance.org')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.BlindCopyAddress', '', '')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.EnableSSL', 'False', 'False')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.FromAddress', 'SystemCenter@gridprotectionalliance.org', 'SystemCenter@gridprotectionalliance.org')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.Password', '', '')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.SMTPServer', '', '')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('Email.Username', '', '')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('SystemCenter.Url', 'http://localhost:8987', '')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('PQBrowser.Url', 'http://localhost', 'http://localhost')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('XDA.Url', 'http://localhost:8989', '')
GO
INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('XDA.APIKey', '', '')
GO
INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('XDA.APIToken', '', '')
GO
INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('XDA.ClientID', 'LocalXDAClient', 'LocalXDAClient')
GO

INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('FAWG.Enabled', 'False', 'False')
GO
INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('MiMD.Url', 'http://localhost:8989', '')
GO
INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('MiMD.APIKey', '', '')
GO
INSERT INTO [SystemCenter.Setting](Name, Value, DefaultValue) VALUES('MiMD.APIToken', '', '')
GO

---------------- OpenSEE TableSpace -------------
CREATE TABLE [OpenSEE.Setting](
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('applicationName', 'OpenSEE', 'OpenSEE')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('applicationDescription', 'Event Viewing Engine', 'Event Viewing Engine')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('applicationKeywords', 'open source, utility, browser, power quality, management', 'open source, utility, browser, power quality, management')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('bootstrapTheme', '~/Content/bootstrap-theme.css', '~/Content/bootstrap-theme.css')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('useLLVoltage', 'false', 'false')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('SlidingCacheExpiration', '2.0', '2.0')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('maxSampleRate', '-1', '-1')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('minSampleRate', '-1', '-1')
GO
INSERT INTO [OpenSEE.Setting](Name, Value, DefaultValue) VALUES('maxFFTHarmonic', '50', '50')
GO