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
    Event.ID EventID,
    FaultSummary.FaultNumber FaultID,
    WorstDisturbance.ID DisturbanceID,
    DisturbancePhase.Name Phase,
    FORMAT(COALESCE(WorstDisturbance.DurationCycles, FaultSummary.DurationCycles), 'F2') [Duration (cycles)],
    FORMAT(COALESCE(WorstDisturbance.DurationSeconds, FaultSummary.DurationSeconds), 'F4') [Duration (sec)],
    WorstDisturbance.PerUnitMagnitude MagDurMagnitude,
    WorstDisturbance.DurationSeconds MagDurDuration,
    CASE WHEN WorstLLDisturbance.PerUnitMagnitude <> -1E308
        THEN FORMAT(WorstLLDisturbance.PerUnitMagnitude * 100.0, 'F2')
        ELSE 'NaN'
    END [Worst LL Magnitude (%nominal)],
    CASE WHEN WorstLNDisturbance.PerUnitMagnitude <> -1E308
        THEN FORMAT(WorstLNDisturbance.PerUnitMagnitude * 100.0, 'F2')
        ELSE 'NaN'
    END [Worst LN Magnitude (%nominal)],
    EventType.Name [Event Type],
    FaultSummary.Algorithm [Fault Dist Alg],
    FORMAT(FaultSummary.Distance, 'F2') [Fault Dist],
    FORMAT(FaultSummary.CurrentMagnitude, 'F0') [Fault Current Mag],
    FORMAT(FaultSummary.Inception,'HH:mm:ss.fffffff') [Fault Inception]
FROM
    Event JOIN
    EventType ON Event.EventTypeID = EventType.ID LEFT OUTER JOIN
    EventWorstDisturbance ON
        EventWorstDisturbance.EventID = Event.ID AND
        EventType.Name IN ('Sag', 'Swell', 'Interruption', 'Transient') LEFT OUTER JOIN
    Disturbance WorstDisturbance ON EventWorstDisturbance.WorstDisturbanceID = WorstDisturbance.ID LEFT OUTER JOIN
    Disturbance WorstLLDisturbance ON EventWorstDisturbance.WorstLLDisturbanceID = WorstLLDisturbance.ID LEFT OUTER JOIN
    Disturbance WorstLNDisturbance ON EventWorstDisturbance.WorstLNDisturbanceID = WorstLNDisturbance.ID LEFT OUTER JOIN
    Phase DisturbancePhase ON WorstDisturbance.PhaseID = DisturbancePhase.ID LEFT OUTER JOIN
    EventType DisturbanceType ON WorstDisturbance.EventTypeID = DisturbanceType.ID LEFT OUTER JOIN
    FaultGroup ON
        FaultGroup.EventID = Event.ID AND
        COALESCE(FaultGroup.FaultDetectionLogicResult, 0) <> 0 LEFT OUTER JOIN
    FaultSummary ON
        FaultSummary.EventID = Event.ID AND
        FaultSummary.IsSelectedAlgorithm <> 0 AND
        (
            FaultGroup.ID IS NOT NULL OR
            (
                FaultSummary.IsValid <> 0 AND
                FaultSummary.IsSuppressed = 0
            )
        ) AND
        EventType.Name IN ('Fault', 'RecloseIntoFault')
WHERE
    EventWorstDisturbance.ID IS NOT NULL OR
    FaultSummary.ID IS NOT NULL OR
    EventType.Name IN ('BreakerOpen', 'Other')
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

CREATE TABLE [SEBrowser.WidgetCategory] (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL,
    OrderBy INT NOT NULL
)

INSERT [SEBrowser.WidgetCategory] (Name, OrderBy) VALUES ('Waveform Analysis', 1)
GO
INSERT [SEBrowser.WidgetCategory] (Name, OrderBy) VALUES ('Fault', 2)
GO
INSERT [SEBrowser.WidgetCategory] (Name, OrderBy) VALUES ('Correlating Events', 3)
GO
INSERT [SEBrowser.WidgetCategory] (Name, OrderBy) VALUES ('Configuration', 4)
GO
INSERT [SEBrowser.WidgetCategory] (Name, OrderBy) VALUES ('All', 5)
GO

CREATE TABLE [SEBrowser.Widget] (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL,
    Enabled BIT NOT NULL
)

INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchOpenSEE', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchFaultSegments', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchAssetVoltageDisturbances', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchEsriMap', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchAssetHistory', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('Lightning', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('StructureInfo', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchHistory', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('InterruptionReport', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('SIDA', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('SOE', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('LSC', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('PQWeb', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchFileInfo', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchNoteWindow', 1)
GO
INSERT [SEBrowser.Widget] (Name, Enabled) VALUES ('EventSearchPQI', 1)
GO

CREATE TABLE [SEBrowser.WidgetWidgetCategory] (
    WidgetID INT NOT NULL REFERENCES [SEBrowser.Widget](ID),
    CategoryID INT NOT NULL REFERENCES [SEBrowser.WidgetCategory](ID),
    CONSTRAINT UC_WidgetWidgetCategory UNIQUE(WidgetID, CategoryID)
)

INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (1, 1)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (2, 1)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (3, 1)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (4, 2)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (5, 2)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (6, 2)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (7, 2)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (8, 3)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (9, 3)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (10, 3)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (11, 3)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (12, 3)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (13, 3)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (14, 4)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (15, 4)
GO
INSERT [SEBrowser.WidgetWidgetCategory] (WidgetID, CategoryID) VALUES (16, 4)
GO