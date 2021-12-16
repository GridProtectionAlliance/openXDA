---------------- SEBrowser TableSpace -------------
CREATE TABLE [SEBrowser.Setting]
(
   	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Scope] [varchar](64) NULL,
	[Name] [varchar](64) NULL,
	[Value] [varchar](512) NULL,
	[ApplicationInstance] [bit] NOT NULL,
	[Roles] [varchar](200) NULL,
)
GO

/*
	Required Fields are:
		EventID for matching,
		DisturbanceID for matching
		Time for showing the Time of the Event,
		Asset
	Everything else can be customized to appear in the UI.
*/

CREATE VIEW [dbo].[SEBrowser.EventSearchEventView] AS
	SELECT
		Event.ID AS EventID,
		FORMAT(Event.StartTime,'MM/dd/yyyy <br> HH:mm:ss.fffffff') AS Time,
		Meter.AssetKey AS [Meter Key],
		Meter.Name AS [Meter],
		Meter.Alias AS [Meter Alias],
		Meter.ShortName AS [Meter ShortName],
		Meter.Make AS [Meter Make],
		Meter.Model AS [Meter Model],
		Meter.TimeZone AS [Meter TimeZone],
		Meter.Description AS [Meter Desc],
		--Meter Sector
--Meter Firmware Version
--Meter Template Version
--Meter Connection Type
		Location.Name AS [Station],
		Location.LocationKey AS [Station Key],
		Location.ShortName AS [Station ShortName],
		Location.Alias AS [Station Alias],
		Location.Description AS [Station Desc],
--TSC
		Asset.AssetName AS [Asset Name],
		AssetType.Name AS [Asset Type],
		Asset.VoltageKV AS [Nom Voltage (kV)],
		Asset.Description AS [Asset Desc]
--Asset Manufacturer
--Asset Model
	FROM Event LEFT JOIN
		Meter ON Meter.ID = Event.MeterID LEFT JOIN 
		Location ON Meter.LocationID = Location.ID LEFT JOIN
		Asset ON Asset.ID = Event.AssetID LEFT JOIN
		AssetType ON Asset.AssetTypeID = AssetType.ID
GO

CREATE VIEW [dbo].[SEBrowser.EventSearchDetailsView] AS
	SELECT
		EventWorstDisturbance.EventID AS EventID,
		NULL AS FaultID,
		Disturbance.ID AS DisturbanceID,
		Phase.Name AS [Phase],
		FORMAT(Disturbance.DurationCycles, 'F2') AS [Duration (cycles)],
		Format(Disturbance.DurationSeconds, 'F4') AS [Duration (sec)],
		Disturbance.PerUnitMagnitude AS [MagDurMagnitude],
		Disturbance.DurationSeconds AS [MagDurDuration],
		Format((SELECT D.PerUnitMagnitude FROM Disturbance D WHERE D.ID = EventWorstDisturbance.WorstLLDisturbanceID )*100.0,'F2') as [Worst LL Magnitude (%nominal)],
        Format((SELECT D.PerUnitMagnitude FROM Disturbance D WHERE D.ID = EventWorstDisturbance.WorstLNDisturbanceID )*100.0,'F2')  as [Worst LN Magnitude (%nominal)],
		EventType.Name AS [Event Type],
		NULL AS [Fault Dist Alg],
		NULL AS [Fault Dist],
		NULL AS [Fault Current Mag],
		NULL AS [Fault Inception]
	FROM EventWorstDisturbance  LEFT JOIN
		Disturbance ON EventWorstDisturbance.WorstDisturbanceID = Disturbance.ID LEFT JOIN
		Phase ON Disturbance.PhaseID = Phase.ID LEFT JOIN
		EventType ON Disturbance.EventTypeID = EventType.ID
	UNION
	SELECT 
		Event.ID AS EventID,
		NULL AS FaultID,
		NULL AS DisturbanceID,
		NULL AS [Phase],
		NULL AS [Duration (cycles)],
		NULL AS [Duration (sec)],
		NULL AS [MagDurMagnitude],
		NULL AS [MagDurDuration],
		NULL as [Worst LL Magnitude (%nominal)],
        NULL  as [Worst LN Magnitude (%nominal)],
		EventType.Name AS [Event Type],
		NULL AS [Fault Dist Alg],
		NULL AS [Fault Dist],
		NULL AS [Fault Current Mag],
		NULL AS [Fault Inception]
	FROM Event LEFT JOIN
		EventType ON EventType.ID = Event.EventTypeID 
	WHERE EventTypeID IN (SELECT ID FROM EventType WHERE Name IN ('BreakerOpen','Other'))
	UNION 
	SELECT 
		Event.ID AS EventID,
		FaultSummary.FaultNumber AS FaultID,
		NULL AS DisturbanceID,
		FaultSummary.FaultType AS [Phase],
		FORMAT(FaultSummary.DurationCycles, 'F2')  AS [Duration (cycles)],
		FORMAT(FaultSummary.DurationSeconds, 'F4')  AS [Duration (sec)],
		NULL AS [MagDurMagnitude],
		NULL AS [MagDurDuration],
		NULL as [Worst LL Magnitude (%nominal)],
        NULL  as [Worst LN Magnitude (%nominal)],
		EventType.Name AS [Event Type],
		FaultSummary.Algorithm AS [Fault Dist Alg],
		FORMAT(FaultSummary.Distance, 'F2') AS [Fault Dist],
		FORMAT(FaultSummary.CurrentMagnitude, 'F0') AS [Fault Current Mag],
		FORMAT(FaultSummary.Inception,'HH:mm:ss.fffffff') AS [Fault Inception]
	FROM Event LEFT JOIN
		EventType ON EventType.ID = Event.EventTypeID LEFT JOIN
		FaultSummary ON FaultSummary.IsSelectedAlgorithm <> 0 AND
			FaultSummary.IsValid <> 0 AND FaultSummary.IsSuppressed = 0 AND
			FaultSummary.EventID = Event.ID
	WHERE EventTypeID IN (SELECT ID FROM EventType WHERE Name IN ('Fault','RecloseIntoFault'))
GO



INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'applicationName', N'SEBrowser', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'applicationDescription', N'System Event Browser', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'applicationKeywords', N'open source, utility, browser, power quality, management', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'bootstrapTheme', N'~/Content/bootstrap-theme.css', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'XDAInstance', N'http://localhost:8989', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'SCInstance', N'http://localhost:8987', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting] ([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'app.setting', N'OpenSEEInstance', N'http://localhost/OpenSEE', 0, N'Administrator')
GO
INSERT [dbo].[SEBrowser.Setting]([Scope], [Name], [Value], [ApplicationInstance], [Roles]) VALUES (N'eventPreviewPane.widgetSetting', N'OpenSEEInstance', N'http://localhost/OpenSEE', 0, N'Administrator')
GO


Insert into ValueListGroup (Name, Description) VALUES('CustomReports', 'Custom Reports to list on the SEBrowser nav bar')
GO
Insert into ValueList (GroupID, Value, AltValue, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'CustomReports'), 'Breaker Report', 'breakerreport',1)
GO
Insert into ValueList (GroupID, Value, AltValue, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'CustomReports'), 'TripCoil Report', 'relayreport',2)
GO
Insert into ValueList (GroupID, Value, AltValue, SortOrder) VALUES((SELECT ID FROM ValueListGroup WHERE name = 'CustomReports'), 'CapBank Report', 'capbankreport',3)
GO


CREATE TABLE [SEBrowser.EventPreviewPaneSetting](
	ID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(200) NOT NULL,
	Show bit NOT NULL DEFAULT (1),
	OrderBy int NOT NULL
)

INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchOpenSEE', 1,1)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchFaultSegments', 1,2)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchAssetVoltageDisturbances', 1,3)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchCorrelatedSags', 1,4)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('TVAESRIMap', 0,5)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchFileInfo', 0,6)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchHistory', 1,7)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchRelayPerformance', 0,8)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchBreakerPerformance', 0,9)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchNoteWindow', 1,11)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('TVALightning', 0,12)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('TVAFaultInfo', 1,13)
GO
INSERT [SEBrowser.EventPreviewPaneSetting] (Name, Show, OrderBy) VALUES ('EventSearchCapBankAnalyticOverview', 0,10)
GO

CREATE TABLE [SEBrowser.Links] (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(100) NOT NULL UNIQUE,
	Display varchar(100) NOT NULL,
	Value varchar(max) NOT NULL
)
GO

INSERT INTO [SEBrowser.Links] (Name, Display,Value) VALUES
	('Breaker Report',0,'breakerreport'),
	('TCE  Report',0,'relayreport'),
	('CapBank Report',0,'capbankreport')
GO
