INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.ApplyDCOffsetLogic', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.DCOffsetWindowSize', '1.125', '1.125')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.LateBreakerThreshold', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.MaxCyclesBeforeRestrike', '2.0', '2.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.MinCyclesBeforeOpen', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.MinCyclesBeforeRestrike', '0.125', '0.125')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.MinWaitBeforeReclose', '15.0', '15.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Breakers.OpenBreakerThreshold', '20.0', '20.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('COMTRADE.MinWaitTime', '15.0', '15.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('COMTRADE.UseRelaxedValidation', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('COMTRADE.WaitForINF', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DataPusher.Enabled', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DataPusher.OnlyValidFaults', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DataPusher.TimeWindow', '72', '72')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DbTimeout', '120', '120')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DefaultDisturbanceEnvelope', '1', '1')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('DefaultMeterTimeZone', 'UTC', 'UTC')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.AdminAddress', 'xda-admin@gridprotectionalliance.org', 'xda-admin@gridprotectionalliance.org')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('Email.BlindCopyAddress', '', '')
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

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EMAX.ApplyTimestampCorrection', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EMAX.ApplyValueCorrection', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EMAX.COMTRADEExportDirectory', '', '')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EventEmail.Enabled', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EventEmail.MaxEmailCount', '0', '0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('EventEmail.MaxEmailSpan', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.MaxFaultDistanceMultiplier', '1.05', '1.05')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.MinFaultDistanceMultiplier', '-0.05', '-0.05')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.PrefaultTrigger', '5.0', '5.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.PrefaultTriggerAdjustment', '50.0', '50.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.FaultCalculationCycleMethod', 'MaxCurrent', 'MaxCurrent')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.FaultClearingAdjustmentSamples', '10', '10')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.FaultedVoltageThreshold', '0.8', '0.8')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.GroundedFaultVoltageThreshold', '0.001', '0.001')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.MinFaultSegmentCycles', '1.0', '1.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.UseDefaultFaultDetectionLogic', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FaultLocation.WarnMissingDetectionLogic', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FilePattern', '(?<AssetKey>[^\\]+)\\[^\\]+$', '(?<AssetKey>[^\\]+)\\[^\\]+$')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileProcessorID', '4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D', '4E3D3A90-6E7E-4AB7-96F3-3A5899081D0D')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherBufferSize', '65536', '65536')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherEnabled', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherEnumerationStrategy', 'ParallelSubdirectories', 'ParallelSubdirectories')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherOrderedEnumeration', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherInternalThreadCount', '0', '0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FileWatcherMaxFragmentation', '10', '10')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('FolderExclusion', '', '')
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

INSERT INTO Setting (Name, Value, DefaultValue)VALUES ('LocalXDAInstance', 'http://127.0.0.1:8989', 'http://127.0.0.1:8989')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxCurrent', '1000000.0', '1000000.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxEventDuration', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxFileCreationTimeOffset', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxFileDuration', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxTimeOffset', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MaxVoltage', '2.0', '2.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('MinTimeOffset', '0.0', '0.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('ProcessingThreadCount', '0', '0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('PQMarkAggregation.Enabled', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('PQMarkAggregation.Frequency', '0 0 * * *', '0 0 1 * *')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('PQTrendingWebReport.Enabled', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('PQTrendingWebReport.Frequency', '0 2 * * *', '0 2 * * *')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('PQTrendingWebReport.Verbose', 'False', 'False')
GO

INSERT INTO Setting (Name, Value, DefaultValue)VALUES ('RemoteXDAInstance', 'http://127.0.0.1:8989', 'http://127.0.0.1:8989')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('ResultsPath', 'Results', 'Results')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SagThreshold', '0.9', '0.9')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SkipOnCRCHashMatch', 'True', 'True')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SwellThreshold', '1.1', '1.1')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('StepChangeWebReport.Enabled', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('StepChangeWebReport.Frequency', '0 2 * * *', '0 2 * * *')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('StepChangeWebReport.Verbose', 'False', 'False')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SystemFrequency', '60.0', '60.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('SystemMVABase', '100.0', '100.0')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('TimeTolerance', '0.5', '0.5')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('UseDefaultFaultDetectionLogic', '1', '1')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('WatchDirectories', 'Watch', 'Watch')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('WorkbenchTimeRangeInSeconds', '60', '60')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('XDATimeZone', 'UTC', 'UTC')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('TrendingData.RMS.FolderPath', 'Trms.dat', 'Trms.dat')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('TrendingData.Flicker.FolderPath', 'FkrR[0-9]*\.dat', 'FkrR[0-9]*\.dat')
GO

INSERT INTO Setting(Name, Value, DefaultValue) VALUES('TrendingData.Trigger.FolderPath', 'TrR[0-9]*\.dat', 'TrR[0-9]*\.dat')
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsEvents', 1)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsDisturbances', 1)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsFaults', 1)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsBreakers', 0)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsExtensions', 0)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsTrending', 1)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsTrendingData', 1)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsCompleteness', 1)
GO

INSERT INTO DashSettings(Name, Value, Enabled) VALUES('DashTab', '#tabsCorrectness', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Fault', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'RecloseIntoFault', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'BreakerOpen', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Sag', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Swell', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Interruption', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Transient', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Other', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Test', 0)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Breaker', 0)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChart', 'Snapshot', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Fault,#FF2800', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'RecloseIntoFault,#323232', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'BreakerOpen,#B245BA', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Sag,#FF9600', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Swell,#00FFF4', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Interruption,#C00000', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Transient,#FFFF00', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Other,#0000FF', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Test,#A9A9A9', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Breaker,#A500FF', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('EventsChartColors', 'Snapshot,#9db087', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChart', '5', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChart', '4', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChart', '3', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChart', '2', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChart', '1', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChart', '0', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChartColors', '5,#C00000', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChartColors', '4,#FF2800', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChartColors', '3,#FF9600', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChartColors', '2,#00FFF4', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChartColors', '1,#FFFF00', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('DisturbancesChartColors', '0,#0000FF', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '0', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '0.208', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '12', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '13.8', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '46', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '69', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '115', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '135', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '161', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '200', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '230', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '300', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChart', '500', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '0,#90ed7d', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '0.208,#78E35C', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '12,#806283', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '13.8,#DC14B2', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '46,#434348', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '69,#ff0000', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '115,#f7a35c', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '135,#8085e9', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '161,#f15c80', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '200,#e4d354', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '230,#2b908f', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '300,#f45b5b', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('FaultsChartColors', '500,#91e8e1', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChart', 'Normal', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChart', 'Late', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChart', 'Indeterminate', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChart', 'No Operation', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChartColors', 'Normal,#FF0000', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChartColors', 'Late,#434348', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChartColors', 'Indeterminate,#90ED7D', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('BreakersChartColors', 'No Operation,#FC8EBA', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('TrendingChart', 'Alarm', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('TrendingChart', 'Offnormal', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('TrendingChartColors', 'Alarm,#ff0000', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('TrendingChartColors', 'Offnormal,#434348', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '> 100%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '98% - 100%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '90% - 97%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '70% - 89%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '50% - 69%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '>0% - 49%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChart', '0%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '> 100%,#00FFF4', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '98% - 100%,#00C80E', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '90% - 97%,#FFFF00', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '70% - 89%,#FF9600', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '50% - 69%,#FF2800', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '>0% - 49%,#FF0EF0', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CompletenessChartColors', '0%,#0000FF', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '> 100%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '98% - 100%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '90% - 97%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '70% - 89%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '50% - 69%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '>0% - 49%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChart', '0%', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '> 100%,#00FFF4', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '98% - 100%,#00C80E', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '90% - 97%,#FFFF00', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '70% - 89%,#FF9600', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '50% - 69%,#FF2800', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '>0% - 49%,#FF0EF0', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('CorrectnessChartColors', '0%,#0000FF', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('System.XDAInstance', 'http://localhost:8989', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('System.TimeWindow', '1', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('System.URL', 'http://localhost/PQDashboard', 1)
GO

INSERT INTO DashSettings (Name, Value, Enabled) VALUES ('YearBeginDate', 'January 1', 1)
GO