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