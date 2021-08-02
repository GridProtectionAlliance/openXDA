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
    Schedule VARCHAR(50) NOT NULL
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
