---------------- PQDigest TableSpace -------------
CREATE TABLE [PQDigest.Setting](
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL UNIQUE,
    Value VARCHAR(MAX) NULL,
    DefaultValue VARCHAR(MAX) NULL
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.Url', 'http://localhost:8989', '')
GO
INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.APIKey', '', '')
GO
INSERT INTO [PQDigest.Setting](Name, Value, DefaultValue) VALUES('XDA.APIToken', '', '')
GO

CREATE TABLE [PQBrowser.EventViewWidget] (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL DEFAULT(1),
    Setting varchar(max) NOT NULL Default(''),
    Type varchar(max) NOT NULL Default('OpenSEE')
)
GO

INSERT [PQBrowser.EventViewWidget] (Name, Type) VALUES ('OpenSEE','OpenSEE')
GO

INSERT [PQBrowser.EventViewWidget] (Name, Type) VALUES ('TrendGraph','TrendGraph')
GO

INSERT [PQBrowser.EventViewWidget] (Name, Type) VALUES ('PQICurves','PQICurves')
GO

INSERT [PQBrowser.EventViewWidget] (Name, Type) VALUES ('pqi','pqi')
GO


CREATE TABLE [PQBrowser.HomeScreenWidget] (
    ID INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL DEFAULT(1),
    Setting varchar(max) NOT NULL Default(''),
    Type varchar(max) NOT NULL Default('OpenSEE'),
    TimeFrame INT NOT NULL Default(30)
)

INSERT [PQBrowser.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('EPRI PQ Health Index - Last 30 Days','PQHealthIndex', 30)
GO

INSERT [PQBrowser.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('Historical Event Counts - Last 30 Days','EventCountChart', 30)
GO

INSERT [PQBrowser.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('Magnitude Duration Chart - Last 30 Days','MagDurChart', 30)
GO

INSERT [PQBrowser.HomeScreenWidget] (Name, Type, TimeFrame) VALUES ('Meter Activity - Last 30 Days','EventCountTable', 30)
GO

