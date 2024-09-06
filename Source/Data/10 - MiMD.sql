---------------- MiMD TableSpace -------------
CREATE TABLE [MiMD.Setting]
(
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
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
	Constraint UC_ConfigFileChanges UNIQUE(MeterID, [FileName], LastWriteTime),
	ValidChange BIT NOT NULL Default 1
)
GO

CREATE TABLE [MiMD.ConfigFileRules](
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Pattern VARCHAR(500) NOT NULL,
	Field VARCHAR(255) NOT NULL,
	[Value] VARCHAR(255) NOT NULL,
	Comparison VARCHAR(50) NOT NULL,
	FieldType VARCHAR(50) NOT NULL,
	AdditionalFieldID INT NULL REFERENCES AdditionalField(ID),
	PreVal VARCHAR(MAX) NOT NULL DEFAULT('0')
);
GO

CREATE TABLE [MiMD.ColorConfig] (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Color VARCHAR(255) NOT NULL,
    Threshold INT NOT NULL,
);
GO

INSERT INTO [MiMD.ColorConfig] (Color, Threshold) VALUES ('#FF0000', 1);
GO
INSERT INTO [MiMD.ColorConfig] (Color, Threshold) VALUES ('#FFA500', 7);  
GO
INSERT INTO [MiMD.ColorConfig] (Color, Threshold) VALUES ('#FFFF00', 30);
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

CREATE TABLE [MiMD.DiagnosticFileRules](
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FilePattern VARCHAR(255) NOT NULL DEFAULT '',
	Field VARCHAR(255) NOT NULL DEFAULT '',
	RegexPattern VARCHAR(500) NOT NULL DEFAULT '',
	[Text] VARCHAR(MAX) NOT NULL DEFAULT '',
	Severity INT NOT NULL DEFAULT 0,
	ReverseRule BIT NOT NULL DEFAULT 0,
	SQLQuery VARCHAR(500) NOT NULL DEFAULT '',
	AdditionalFieldID INT NULL REFERENCES AdditionalField(ID)
);
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'dfr', 'online', 'MiMD Parsing Alarm: DFR not set to ONLINE.', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'time_mark_source', '\b(?:irig-b|pc)\b', 'MiMD Parsing Alarm: TIME_MARK_SOURCE not set to IRIG-B or PC.', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', '', '\b(?:alarmon|anfail)\b', 'MiMD Parsing Alarm: ALARMON or ANFAIL field present.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'pc_time', '^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/([0-9]{4})-(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])\s*$', 'MiMD Parsing Alarm: Incorrect date format for PC_Time.', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', '', 'chassis_not_comm', 'MiMD Parsing Alarm: CHASSIS_NOT_COMM field present.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'data_drive', '^(\d+)gb\/(\d+)gb\s*$', 'MiMD Parsing Alarm: Incorrect format for Data_Drive.', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'timemark', '^(\d{2}:\d{2}:\d{2})(?:,(\1))*(,)?\s*$', 'MiMD Parsing Warning: TimeMark values not equal.', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'timemark', '', 'MiMD Parsing Alarm: Time_Mark_Source Set to PC and Clock set to UNSYNC(unknown) in consecutive files.', '0', '0', 
'with cte as 
(
    select top 14 * 
    from appstatusfilechanges 
    where meterid = {MeterID}
    order by LastWriteTime desc
)
select 
case 
    when (select count(*) from cte where text like ''%timeMark values not equal%'') = 14 then 1
    else 0
end
')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', '', '', '', '0', '0', 
'DECLARE @ConditionFlag INT = {variables["time_mark_source"] = "pc" };

IF @ConditionFlag = 1
BEGIN
    WITH cte AS 
    (
        SELECT TOP 1 * FROM AppStatusFileChanges 
        WHERE meterid = {MeterID} 
        ORDER BY LastWriteTime DESC
    )
    SELECT 
    CASE 
        WHEN (SELECT COUNT(*) FROM cte WHERE Text LIKE ''%TIME_MARK_SOURCE=PC%'' AND Text LIKE ''%Clock=UNSYNC(unknown)%'') > 0 THEN 1
        ELSE 0
    END
END
ELSE
BEGIN
    SELECT 0
END
'
)
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'clock', 'sync\(lock\)\s*', 'MiMD Parsing Alarm: Clock not set to SYNC(lock).', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', 'time_mark_time', '^(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d-(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]\.\d{6}\s*$',
'MiMD Parsing Alarm: Incorrect date format for Time_Mark_Time.', '0', '0', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', '', '','MiMD Parsing Alarm: Time_Mark_Time and PC_Time difference greater than 2 seconds.', '0', '0', 
'DECLARE @ConditionFlag INT;

SET @ConditionFlag = CASE 
    WHEN DATEDIFF(second, 
                  CONVERT(DATETIME2, REPLACE({variables["pc_time"]}, ''-'', '' ''), 101), 
                  CONVERT(DATETIME2, REPLACE({variables["time_mark_time"]}, ''-'', '' ''), 101)) > 2 
    THEN 1 
    ELSE 0 
END;

SELECT @ConditionFlag;
')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppStatus', '', '','MiMD Parsing Alarm: DFR time set in the future.', '0', '0', 
'DECLARE @ConditionFlag INT = {LastWriteTime > ConvertTimeFromUtc(UtcNow, FindSystemTimeZoneById("Central Standard Time")) };

SELECT @ConditionFlag;')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', '','', '0', '0', 
'DECLARE @ConditionFlag INT = {Time > ConvertTimeFromUtc(UtcNow, FindSystemTimeZoneById("Central Standard Time")) };

SELECT @ConditionFlag;')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'unsync<invalid\(no signal\)>','MiMD Parsing Alarm: unsync<invalid(no signal)> text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', '\[alarmon\]','MiMD Parsing Alarm: [alarmon] text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'sync loss','MiMD Parsing Alarm: sync loss text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'chassis comm\. error','MiMD Parsing Alarm: chassis comm. error text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'disk full','MiMD Parsing Alarm: disk full text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'master comm\. error','MiMD Parsing Alarm: master comm. error text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'dsp board temperature','MiMD Parsing Alarm: dsp board temperature text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'analog fail','MiMD Parsing Alarm: analog fail text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'pc health','MiMD Parsing Alarm: pc health text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'offline','MiMD Parsing Alarm: offline text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('AppTrace', '', 'time in future','MiMD Parsing Alarm: time in future field text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', 'offline\. system offline','MiMD Parsing Alarm: offline. system offline text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', 'buffer full','MiMD Parsing Alarm: buffer full text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', 'error','MiMD Parsing Alarm: error text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', '^(?=.*\balarm\b)(?=.*\btime sync failed\b).*$','MMiMD Parsing Alarm: ALARM and Time Sync Failed text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', 'alarm','MiMD Parsing Alarm: Alarm text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', '^^(?=.*\balarm\b)(?=.*\btime sync failed\b)(?=.*\bsystem started\b).*$','MiMD Parsing Alarm: Alarm, Time Sync Failed, and System Started text found in file.', '0', '1', '')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', '','', '0', '0', 
'DECLARE @ConditionFlag INT = {Time > ConvertTimeFromUtc(UtcNow, FindSystemTimeZoneById("Central Standard Time")) };

SELECT @ConditionFlag;')
GO

INSERT INTO [MiMD.DiagnosticFileRules] ([FilePattern], [Field], [RegexPattern], [Text], [Severity], [ReverseRule], [SQLQuery])
VALUES ('EmaxEventHis', '', '','MiMD Parsing Alarm: Time - SystemStartedTime is greater than 60 seconds', '0', '0',
'DECLARE @ConditionFlag INT;

SET @ConditionFlag = CASE 
    WHEN {variables["systemstarted"] = "true"} = 1
         AND DATEDIFF(second, CAST({variables["systemstartedtime"]} AS datetime), {Time}) > 60 
    THEN 1 
    ELSE 0 
END;

SELECT @ConditionFlag;
')
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
	Description VARCHAR(MAX) NULL,
	PreVal VARCHAR(MAX) NOT NULL DEFAULT('0')
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
	PreVal VARCHAR(MAX) NOT NULL DEFAULT '0'
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
    MAX([MiMD.ComplianceField].Label) AS FieldLabel,
	MAX([MiMD.ComplianceFieldValue].PreVal) AS PreVal
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
