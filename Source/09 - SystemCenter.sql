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
    CreatedOn datetime NOT NULL CONSTRAINT [DF_AccessLog_Timestamp]  DEFAULT (getutcdate())
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


CREATE TABLE [SystemCenter.ValueListGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
)

CREATE TABLE [SystemCenter.ValueList](
    [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [GroupID] [int] NOT NULL FOREIGN KEY REFERENCES [SystemCenter.ValueListGroup](ID),
    [Text] [varchar](200) NULL,
    [Value] [int] NULL,
    [Description] [varchar](max) NULL,
    [SortOrder] [int] NULL,
)
GO


CREATE TABLE [SystemCenter.AdditionalField] (
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

CREATE TABLE [SystemCenter.AdditionalFieldValue] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTableID int NOT NULL,
	AdditionalFieldID int NOT NULL FOREIGN KEY REFERENCES [SystemCenter.AdditionalField](ID),
	Value varchar(max) NULL,
    UpdatedOn DATE NULL DEFAULT (SYSDATETIME()),
	Constraint UC_AdditonaFieldValue UNIQUE(ParentTableID, AdditionalFieldID)
)
GO

CREATE TABLE [SystemCenter.ExternalOpenXDAField] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ParentTable varchar(100) NOT NULL,
	FieldName varchar(100) NOT NULL,
	ExternalDB varchar(max) NULL,
	ExternalDBTable varchar(max) NULL,
	ExternalDBTableKey varchar(max) NULL,
	Constraint UC_ExternalOpenXDAField UNIQUE(ParentTable, FieldName)
)
GO

CREATE Table [SystemCenter.extDBTables] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TableName varchar(200) NOT NULL,
    ExternalDB varchar(200) NOT NULL,
	Query varchar(max) NULL,
)
GO

CREATE TABLE [SystemCenter.TSC] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(200) NOT NULL,
	Description varchar(max) NULL,
	DepartmentNumber varchar(6) NOT NULL
)
GO

CREATE TABLE [SystemCenter.CustomerAccess] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(ID),
	PQViewSiteID int NOT NULL
)
GO

CREATE TABLE [SystemCenter.CustomerAccessPQDigest] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(ID),
	OpenXDAMeterID INT FOREIGN KEY REFERENCES Meter(ID)
)
GO


CREATE TABLE [SystemCenter.Role] (
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(200) NOT NULL,
	Description varchar(max) NULL,
)
GO

CREATE TABLE [SystemCenter.NoteApplication](
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	Name varchar(max) not null,
)
GO

INSERT INTO [SystemCenter.NoteApplication] (Name) VALUES ('OpenMIC')
GO
INSERT INTO [SystemCenter.NoteApplication] (Name) VALUES ('OpenXDA')
GO
INSERT INTO [SystemCenter.NoteApplication] (Name) VALUES ('MiMD')
GO
INSERT INTO [SystemCenter.NoteApplication] (Name) VALUES ('SystemCenter')
GO
INSERT INTO [SystemCenter.NoteApplication] (Name) VALUES ('OpenHistorian')
GO
INSERT INTO [SystemCenter.NoteApplication] (Name) VALUES ('All')
GO

CREATE TABLE [SystemCenter.NoteTable] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	Name varchar(max) not null
)
GO

INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('Meter')
GO
INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('Event')
GO
INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('Asset')
GO
INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('Location')
GO
INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('Customer')
GO
INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('UserAccount')
GO
INSERT INTO [SystemCenter.NoteTable] (Name) VALUES ('Company')
GO

CREATE TABLE [SystemCenter.NoteType] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	Name varchar(max) not null
)
GO

INSERT INTO [SystemCenter.NoteType] (Name) VALUES ('Configuration')
GO
INSERT INTO [SystemCenter.NoteType] (Name) VALUES ('Diagnostic')
GO
INSERT INTO [SystemCenter.NoteType] (Name) VALUES ('General')
GO



CREATE TABLE [SystemCenter.Note] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    NoteApplicationID int Not NULL REFERENCES [SystemCenter.NoteApplication](ID),
	NoteTableID int Not NULL REFERENCES [SystemCenter.NoteTable](ID),
    NoteTypeID int Not NULL REFERENCES [SystemCenter.NoteType](ID),
	ReferenceTableID INT NOT NULL,
    Note VARCHAR(MAX) NOT NULL,
    UserAccount VARCHAR(MAX) NOT NULL DEFAULT SUSER_NAME(),
    Timestamp DATETIME NOT NULL DEFAULT GETUTCDATE(),
)
GO

CREATE TABLE [SystemCenter.OpenMICDailyStatistic] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    [Date] VARCHAR(10) not null,
    Meter VARCHAR(50) not null,
    LastSuccessfulConnection DATETIME NULL,
    LastUnsuccessfulConnection DATETIME NULL,
    LastUnsuccessfulConnectionExplanation VARCHAR(MAX) NULL,
    TotalConnections INT NOT NULL,
    TotalUnsuccessfulConnections INT NOT NULL,
    TotalSuccessfulConnections INT NOT NULL
    CONSTRAINT UC_SystemCenter_OpenMICDailyStatistic_Date_Meter UNIQUE ([Date],Meter)
)
GO

CREATE TABLE [SystemCenter.OpenXDADailyStatistic] (
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
    AverageTotalEmailLatency FLOAT NULL
    CONSTRAINT UC_SystemCenter_OpenXDADailyStatistic_Date_Meter UNIQUE ([Date],Meter)
)
GO

CREATE TABLE [SystemCenter.MiMDDailyStatistic] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
    [Date] VARCHAR(10) not null,
    Meter VARCHAR(50) not null,
    LastSuccessfulFileProcessed DATETIME NULL,
    LastUnsuccessfulFileProcessed DATETIME NULL,
    LastUnsuccessfulFileProcessedExplanation VARCHAR(MAX) NULL,
    TotalFilesProcessed INT NOT NULL,
    TotalUnsuccessfulFilesProcessed INT NOT NULL,
    TotalSuccessfulFilesProcessed INT NOT NULL, 
    ConfigChanges INT NOT NULL,
    DiagnosticAlarms INT NOT NULL,
    ComplianceIssues INT NOT NULL,
    CONSTRAINT UC_SystemCenter_MiMDDailyStatistic_Date_Meter UNIQUE ([Date],Meter)
)
GO
