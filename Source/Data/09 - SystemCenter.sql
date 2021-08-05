---------------- System Center TableSpace -------------
CREATE TABLE [SystemCenter.Setting](
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NULL,
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

CREATE TABLE [dbo].[SystemCenter.AdditionalUserField](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FieldName] [varchar](100) NOT NULL,
	[Type] [varchar](max) NOT NULL DEFAULT ('string'),
	[IsSecure] [bit] NOT NULL DEFAULT (0),
) 
GO

CREATE TABLE [dbo].[SystemCenter.AdditionalUserFieldValue](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserAccountID] UNIQUEIDENTIFIER NOT NULL REFERENCES [SystemCenter.UserAccount](ID),
	[AdditionalUserFieldID] [int] NOT NULL FOREIGN KEY References [SystemCenter.AdditionalUserField](ID),
	[Value] [varchar](max) NULL,

)

GO


CREATE TABLE [ValueListGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
)

CREATE TABLE [ValueList](
    [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [GroupID] [int] NOT NULL FOREIGN KEY REFERENCES [ValueListGroup](ID),
    [Value] [varchar](200) NULL,
    [AltValue] [varchar](200) NULL,
    [SortOrder] [int] NULL,
)
GO


CREATE TABLE [AdditionalField] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTable varchar(100) NOT NULL,
	FieldName varchar(100) NOT NULL,
	Type varchar(max) NULL DEFAULT ('string'),
	ExternalDB varchar(max) NULL,
	ExternalDBTable varchar(max) NULL,
	ExternalDBTableKey varchar(max) NULL,
	IsSecure bit NULL DEFAULT(0)
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
	ExternalDB varchar(max) NULL,
	ExternalDBTable varchar(max) NULL,
	ExternalDBTableKey varchar(max) NULL,
	Constraint UC_ExternalOpenXDAField UNIQUE(ParentTable, FieldName)
)
GO

CREATE Table [extDBTables] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TableName varchar(200) NOT NULL,
    ExternalDB varchar(200) NOT NULL,
	Query varchar(max) NULL,
)
GO

CREATE TABLE [CustomerAccess] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(ID),
	PQViewSiteID int NOT NULL
)
GO

CREATE TABLE [CustomerAccessPQDigest] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(ID),
	OpenXDAMeterID INT FOREIGN KEY REFERENCES Meter(ID)
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
    Description VARCHAR(max) NULL
)
GO

-- Default Settings --
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