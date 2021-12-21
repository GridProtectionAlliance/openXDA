---------------- MiMD TableSpace -------------
CREATE TABLE [MiMD.Setting]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NULL,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

CREATE TABLE [MiMD.SummaryEmail]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Template VARCHAR(MAX) NOT NULL,
    DataSQL VARCHAR(MAX) NOT NULL DEFAULT 'SELECT '''' FOR XML PATH(''Email''), TYPE',
    Subject VARCHAR(200) NOT NULL
)
GO

CREATE TABLE [MiMD.DBCleanUpTask]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    SQLCommand VARCHAR(MAX) NOT NULL DEFAULT 'SELECT 1',
    Schedule VARCHAR(50) NOT NULL,
	Name VARCHAR(200) NOT NULL
)
GO

CREATE TABLE [MiMD.DataReader]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FilePattern VARCHAR(500) NOT NULL,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

INSERT INTO [MiMD.DataReader] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\Config\*', 'MiMD.exe', 'MiMD.FileParsing.DataReaders.ConfigFileReader', 1)
GO
INSERT INTO [MiMD.DataReader] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\Diagnostic\EVENTHIS.txt', 'MiMD.exe', 'MiMD.FileParsing.DataReaders.EMAXEventHisFileReader', 2)
GO
INSERT INTO [MiMD.DataReader] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\Diagnostic\ALARMS.txt', 'MiMD.exe', 'MiMD.FileParsing.DataReaders.EMAXEventHisFileReader', 6)
GO
INSERT INTO [MiMD.DataReader] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\Diagnostic\Trace*.wri', 'MiMD.exe', 'MiMD.FileParsing.DataReaders.AppTraceFileReader', 3)
GO
INSERT INTO [MiMD.DataReader] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\Diagnostic\Status*.txt', 'MiMD.exe', 'MiMD.FileParsing.DataReaders.AppStatusFileReader', 4)
GO
INSERT INTO [MiMD.DataReader] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\BEN\**\Event\*.cfg', 'MiMD.exe', 'MiMD.FileParsing.DataReaders.BENConfigFileReader', 5)
GO

CREATE TABLE [MiMD.DataOperation]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.ConfigOperation', 1)
GO
INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.EmaxEventHisOperation', 2)
GO
INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.AppTraceOperation', 3)
GO
INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.AppStatusOperation', 4)
GO
INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.BENConfigOperation', 5)
GO
INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.PRC002Operation', 6)
GO
INSERT INTO [MiMD.DataOperation] (AssemblyName, TypeName, LoadOrder) VALUES('MiMD.exe', 'MiMD.FileParsing.DataOperations.DailyStatisticOperation', 7)
GO



INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.AdminAddress', 'MiMD-admin@gridprotectionalliance.org', 'MiMD-admin@gridprotectionalliance.org')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.BlindCopyAddress', '', '')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.EnableSSL', 'False', 'False')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.FromAddress', 'MiMD@gridprotectionalliance.org', 'MiMD@gridprotectionalliance.org')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.Password', '', '')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.SMTPServer', '', '')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.Username', '', '')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('MiMD.Url', 'http://localhost:8986', '')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FilePattern', '(?<AssetKey>[^\\]+)\\[^\\]+$', '(?<AssetKey>[^\\]+)\\[^\\]+$')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileProcessorID', '4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D', '4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileWatcherBufferSize', '65536', '65536')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileWatcherEnabled', 'True', 'True')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileWatcherEnumerationStrategy', 'ParallelSubdirectories', 'ParallelSubdirectories')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileWatcherOrderedEnumeration', 'False', 'False')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileWatcherInternalThreadCount', '0', '0')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FileWatcherMaxFragmentation', '10', '10')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('FolderExclusion', '', '')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('WatchDirectories', 'Watch', 'Watch')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('Email.SummaryEmailSchedule', '0 7 * * *', '0 7 * * *')
GO

INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('SystemCenter.Url', 'http://localhost:8987', '')
GO
INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('SystemCenter.Credential', 'MiMDUser', '')
GO
INSERT INTO [MiMD.Setting] (Name, Value, DefaultValue) VALUES('SystemCenter.Password', '', '')
GO

CREATE TABLE ConfigFileChanges(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MeterID INT NOT NULL,
	[FileName] VARCHAR(500) NOT NULL,
	LastWriteTime DATETIME NOT NULL,
	Changes INT NOT NULL,
	Html VARCHAR(MAX) NOT NULL,
    Text VARCHAR(MAX) NOT NULL,
	Constraint UC_ConfigFileChanges UNIQUE(MeterID, [FileName], LastWriteTime)
)
GO

CREATE TABLE AppStatusFileChanges(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MeterID INT NOT NULL,
	[FileName] VARCHAR(500) NOT NULL,
	LastWriteTime DATETIME NOT NULL,
	FileSize INT NOT NULL,
	Version VARCHAR(10) NOT NULL,
	DFR VARCHAR(10) NOT NULL,
	PCTime DateTime2 NOT NULL,
	TimeMarkSource VARCHAR(20) NOT NULL,
	TimeMarkTime DateTime2 NOT NULL,
	DataDriveUsage FLOAT NOT NULL,
	DSPBoard VARCHAR(300) NULL,
	DSPRevision VARCHAR(300) NULL,
	Packet VARCHAR(300) NULL,
	Recovery VARCHAR(300) NULL,
	BoardTemp VARCHAR(300) NULL,
	SpeedFan VARCHAR(300) NULL,
	Text VARCHAR(MAX) NOT NULL,
	Html VARCHAR(MAX) NOT NULL,
	Alarms INT NOT NULL,
	Constraint UC_AppStatusFileChanges UNIQUE(MeterID, [FileName], LastWriteTime)
)
GO

CREATE TABLE AppTraceFileChanges(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MeterID INT NOT NULL,
	[FileName] VARCHAR(500) NOT NULL,
	LastWriteTime DATETIME NOT NULL,
    LastFaultTime DATETIME NULL,
    FaultCount48hr INT NOT NULL,
	FileSize INT NOT NULL,
	Span INT NOT NULL,
	NewRecords INT NOT NULL,
	Alarms INT NOT NULL,
	Html VARCHAR(MAX) NOT NULL,
	Constraint UC_AppTraceFileChanges UNIQUE(MeterID, [FileName], LastWriteTime)
)
GO

CREATE TABLE EmaxDiagnosticFileChanges(
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MeterID INT NOT NULL,
	[FileName] VARCHAR(500) NOT NULL,
	LastWriteTime DATETIME NOT NULL,
	FileSize INT NOT NULL,
	Span INT NOT NULL,
	NewRecords INT NOT NULL,
	Alarms INT NOT NULL,
	Html VARCHAR(MAX) NOT NULL,
	Constraint UC_EmaxDiagnosticFileChanges UNIQUE(MeterID, [FileName], LastWriteTime)
)
GO

CREATE VIEW [MiMD.MaxFileChanges] AS
	SELECT
		ID,
		MeterID,
		FileName,
		LastWriteTime,
		LastFaultTime,
		FaultCount48hr,
		row_number() over (partition by MeterID order by LastWriteTime desc,Alarms desc) as RowNum
	FROM
	(
		SELECT ID,MeterID, FileName, LastWriteTime,Alarms, NULL as LastFaultTime, 0 as FaultCount48hr FROM AppStatusFileChanges UNION
		SELECT ID,MeterID, FileName, LastWriteTime,Alarms, LastFaultTime, FaultCount48hr FROM AppTraceFileChanges UNION
		SELECT ID,MeterID, FileName, LastWriteTime,Alarms, NULL as LastFaultTime, 0 as FaultCount48hr FROM EmaxDiagnosticFileChanges
	) t
GO

CREATE VIEW [MiMD.MaxAlarmChanges] AS
	SELECT
		ID,
		MeterID,
		FileName,
		LastWriteTime,
		Alarms,
		row_number() over (partition by MeterID order by LastWriteTime desc,Alarms desc) as RowNum
	FROM
		(
			SELECT ID,MeterID, FileName, LastWriteTime, Alarms FROM AppStatusFileChanges UNION
			SELECT ID,MeterID, FileName, LastWriteTime, Alarms FROM AppTraceFileChanges UNION
			SELECT ID,MeterID, FileName, LastWriteTime, Alarms FROM EmaxDiagnosticFileChanges
		) t
	WHERE Alarms > 0
GO

CREATE TABLE [MiMD.ComplianceMeter] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	MeterID INT NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
    Reviewed Bit NOT NULL Default 0,
)
GO

CREATE TABLE [MiMD.BaseConfig] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	MeterId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceMeter](ID),
    NAME VARCHAR(200) NOT NULL,
    Pattern VARCHAR(200) NOT NULL
)
GO

CREATE TABLE [MiMD.ComplianceState] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
  Description VARCHAR(MAX) NOT NULL,
	Color VARCHAR(10) NOT NULL,
	TextColor VARCHAR(10) NOT NULL,
	Priority INT NOT NULL
)
GO


CREATE TABLE [MiMD.ComplianceField] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	BaseConfigId INT  NOT NULL FOREIGN KEY REFERENCES [MiMD.BaseConfig](ID),
  Name VARCHAR(MAX) NOT NULL,
  Label VARCHAR(MAX) NULL,
  Category VARCHAR(MAX) NULL,
	Value VARCHAR(MAX) NOT NULL,
	Comparison VARCHAR(2) NOT NULL,
	FieldType VARCHAR(10) NOT NULL DEFAULT('string'),
  Description VARCHAR(MAX) NULL
)
GO

CREATE TABLE [MiMD.ComplianceRecord] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	MeterId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceMeter](ID),
	BaseConfigId INT FOREIGN KEY REFERENCES [MiMD.BaseConfig](ID),
	TimerOffset INT NOT NULL DEFAULT(0)
)
GO

CREATE TABLE [MiMD.ComplianceRecordFields] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	RecordId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceRecord](ID),
	FieldId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceField](ID),
)
GO

CREATE TABLE [MiMD.ComplianceAction] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	RecordId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceRecord](ID),
	Note VARCHAR(MAX) NOT NULL,
  UserAccount VARCHAR(MAX) NULL DEFAULT 'MiMD',
  Timestamp DATETIME NOT NULL DEFAULT GETUTCDATE(),
	StateId INT FOREIGN KEY REFERENCES [MiMD.ComplianceState](ID)
)
GO

CREATE TABLE [MiMD.ComplianceFieldValue] (
	ID int not null IDENTITY(1,1) PRIMARY KEY,
	ActionId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceAction](ID),
	FieldId INT NOT NULL  FOREIGN KEY REFERENCES [MiMD.ComplianceField](ID),
	Value VARCHAR(MAX) NOT NULL,
)
GO

CREATE VIEW [MiMD.ComplianceRecordView] AS
SELECT
	[MiMD.ComplianceRecord].ID,
	[MiMD.ComplianceRecord].MeterID AS MeterId,
	[MiMD.ComplianceRecord].BaseConfigId AS BaseConfigId,
	[MiMD.ComplianceRecord].TimerOffset AS TimerOffset,
	(SELECT Top 1 UserAccount FROM [MiMD.ComplianceAction] WHERE TimeStamp = (SELECT MAX(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID) AND RecordId = [MiMD.ComplianceRecord].ID) AS [User],
	(SELECT MAX(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID) AS [Timestamp],
	(SELECT StateId FROM [MiMD.ComplianceAction] WHERE TimeStamp = (SELECT MAX(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID AND [MiMD.ComplianceAction].StateId IS NOT NULL) AND RecordId = [MiMD.ComplianceRecord].ID) AS [Status],
	(SELECT MIN(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID) AS Created,
	(CASE
	WHEN (SELECT StateId FROM [MiMD.ComplianceAction] WHERE TimeStamp = (SELECT MAX(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID) AND RecordId = [MiMD.ComplianceRecord].ID) = (SELECT ID FROM [MiMD.ComplianceState] WHERE Description = 'In Compliance')
	THEN
		(DATEDIFF(DAY,(SELECT MIN(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID),
		(SELECT MAX(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID)))
	ELSE DATEDIFF(DAY,(SELECT MIN(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID), GETUTCDATE())
	END  + [MiMD.ComplianceRecord].TimerOffset
	) AS Timer,
	(SELECT ID FROM [MiMD.ComplianceAction] WHERE TimeStamp = (SELECT MAX(TimeStamp) FROM [MiMD.ComplianceAction]  WHERE [MiMD.ComplianceAction].RecordId = [MiMD.ComplianceRecord].ID) AND RecordId = [MiMD.ComplianceRecord].ID) AS LastActionID
FROM
	[MiMD.ComplianceRecord]
GO

CREATE VIEW [MiMD.ComplianceFieldValueView] AS
SELECT
	[MiMD.ComplianceFieldValue].FieldID AS FieldID,
	(SELECT TOP 1 CFV.Value FROM [MiMD.ComplianceFieldValue] CFV LEFT JOIN [MiMD.ComplianceAction] CA ON CA.ID = CFV.ActionID WHERE CFV.FieldId = MAX([MiMD.ComplianceFieldValue].FieldID) ORDER BY CA.Timestamp DESC )AS Value,
	[MiMD.ComplianceAction].RecordId AS RecordID,
	MAX([MiMD.ComplianceField].Name) AS FieldName,
    MAX([MiMD.ComplianceField].Category) AS FieldCategory,
    MAX([MiMD.ComplianceField].Label) AS FieldLabel
	FROM [MiMD.ComplianceFieldValue] Left JOIN
		[MiMD.ComplianceAction] ON [MiMD.ComplianceFieldValue].ActionId = [MiMD.ComplianceAction].ID LEFT JOIN
		[MiMD.ComplianceField] ON [MiMD.ComplianceField].ID = [MiMD.ComplianceFieldValue].FieldId LEFT JOIN
		[MiMD.ComplianceRecordView] ON [MiMD.ComplianceRecordView].ID = [MiMD.ComplianceAction].RecordId
	WHERE
	[MiMD.ComplianceRecordView].Status NOT IN (SELECT ID FROM [MiMD.ComplianceState] WHERE Description = 'In Compliance')
	GROUP BY [MiMD.ComplianceFieldValue].FieldId, [MiMD.ComplianceAction].RecordId
GO



CREATE TABLE [MiMD.ComplianceOperation]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    FilePattern VARCHAR(500) NOT NULL,
    AssemblyName VARCHAR(200) NOT NULL,
    TypeName VARCHAR(200) NOT NULL,
    LoadOrder INT NOT NULL
)
GO

INSERT INTO [MiMD.ComplianceOperation] (FilePattern, AssemblyName, TypeName, LoadOrder) VALUES('**\Config\*','MiMD.exe', 'MiMD.FileParsing.ComplianceOperation.INIParser', 0)
GO
