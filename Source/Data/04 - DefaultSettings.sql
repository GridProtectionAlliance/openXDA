INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.LateBreakerThreshold', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.OpenBreakerThreshold', '20.0', '20.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('COMTRADEMinWaitTime', '15.0', '15.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DbTimeout', '120', '120')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DefaultMeterTimeZone', 'UTC', 'UTC')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.EnableSSL', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.FromAddress', 'openXDA@gridprotectionalliance.org', 'openXDA@gridprotectionalliance.org')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.Password', '', '')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.SMTPServer', '', '')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.Username', '', '')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EventEmail.WaitPeriod', '10.0', '10.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.MaxFaultDistanceMultiplier', '1.05', '1.05')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.MinFaultDistanceMultiplier', '-0.05', '-0.05')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.PrefaultTrigger', '5.0', '5.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.UseDefaultFaultDetectionLogic', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.WarnMissingDetectionLogic', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FilePattern', '(?<AssetKey>[^\\]+)\\[^\\]+$', '(?<AssetKey>[^\\]+)\\[^\\]+$')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherBufferSize', '65536', '65536')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherEnumerationStrategy', 'ParallelSubdirectories', 'ParallelSubdirectories')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherInternalThreadCount', '0', '0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherMaxFragmentation', '10', '10')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Historian.InstanceName', 'XDA', 'XDA')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Historian.Server', '127.0.0.1:38402', '127.0.0.1:38402')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Historian.URL', 'http://127.0.0.1:8180', 'http://127.0.0.1:8180')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('InterruptionThreshold', '0.1', '0.1')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('LengthUnits', 'miles', 'miles')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxCurrent', '1000000.0', '1000000.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxFileCreationTimeOffset', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxFileDuration', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxQueuedFileCount', '10', '10')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxTimeOffset', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxVoltage', '2.0', '2.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MinTimeOffset', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('ProcessingThreadCount', '0', '0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('ResultsPath', 'Results', 'Results')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SagThreshold', '0.9', '0.9')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SwellThreshold', '1.1', '1.1')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SystemFrequency', '60.0', '60.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('TimeTolerance', '0.5', '0.5')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('WatchDirectories', 'Watch', 'Watch')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('XDATimeZone', 'UTC', 'UTC')
GO