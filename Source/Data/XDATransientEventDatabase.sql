USE [master]
GO

CREATE DATABASE [XDATransientEventDatabase];
GO

-- The following commented statements are used to create a user with access to the XDATransientEventDatabase.
-- Be sure to change the username and password.
-- Replace-all from NewUser to the desired username is the preferred method of changing the username.

--IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'NewUser')
--CREATE LOGIN [NewUser] WITH PASSWORD=N'MyPassword', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
--GO
USE [XDATransientEventDatabase]
GO
--CREATE USER [NewUser] FOR LOGIN [NewUser]
--GO
--CREATE ROLE [openPDCManagerRole] AUTHORIZATION [dbo]
--GO
--EXEC sp_addrolemember N'openPDCManagerRole', N'NewUser'
--GO
--EXEC sp_addrolemember N'db_datareader', N'openPDCManagerRole'
--GO
--EXEC sp_addrolemember N'db_datawriter', N'openPDCManagerRole'
--GO


----- TABLES -----

CREATE TABLE [dbo].[Device]
(
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[FLEID] [varchar](200) NOT NULL,
	[Make] [varchar](200) NOT NULL,
	[Model] [varchar](200) NOT NULL,
	[StationID] [varchar](200) NOT NULL,
	[StationName] [varchar](200) NOT NULL,
	
	CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Line]
(
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[DeviceID] [int] NOT NULL,
	[FLEID] [varchar](200) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Voltage] [varchar](200) NOT NULL,
	[Rating50F] [float] NOT NULL,
	[Length] [float] NOT NULL,
	[EndStationID] [varchar](200) NOT NULL,
	[EndStationName] [varchar](200) NOT NULL,
	[R1] [float] NOT NULL,
	[X1] [float] NOT NULL,
	[R0] [float] NOT NULL,
	[X0] [float] NOT NULL,
	
	CONSTRAINT [PK_Line] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[FileGroup]
(
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[DeviceID] [int] NOT NULL,
	
	CONSTRAINT [PK_FileGroup] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DisturbanceFile]
(
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[FileGroupID] [int] NOT NULL,
	[SourcePath] [varchar](max) NOT NULL,
	[DestinationPath] [varchar](max) NOT NULL,
	[FileSize] [int] NOT NULL,
	[CreationTime] [datetime] NOT NULL,
	[LastWriteTime] [datetime] NOT NULL,
	[LastAccessTime] [datetime] NOT NULL,
	[FileWatcherStartTime] [datetime] NOT NULL,
	[FileWatcherEndTime] [datetime] NOT NULL,
	[FLEStartTime] [datetime] NOT NULL,
	[FLEEndTime] [datetime] NOT NULL,
	
	CONSTRAINT [PK_DisturbanceFile] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Event]
(
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[TimeStart] [datetime] NOT NULL,
	[TimeEnd] [datetime] NOT NULL,
	
	CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LineDisturbance]
(
	[ID] [int] IDENTITY(1, 1) NOT NULL,
	[LineID] [int] NOT NULL,
	[FileGroupID] [int] NOT NULL,
	[FaultType] [varchar](3) NOT NULL,
    [LargestCurrentIndex] [int] NOT NULL,
    [FaultDistance] [float] NOT NULL,
	[IAMax] [float] NOT NULL,
	[IBMax] [float] NOT NULL,
	[ICMax] [float] NOT NULL,
	[VAMin] [float] NOT NULL,
	[VBMin] [float] NOT NULL,
	[VCMin] [float] NOT NULL,
	
	CONSTRAINT [PK_LineDisturbance] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Cycle]
(
    [ID] [int] IDENTITY(1, 1) NOT NULL,
    [LineDisturbanceID] [int] NOT NULL,
    [TimeStart] [datetime] NOT NULL,
    [CycleIndex] [int] NOT NULL,
    
    [ANVoltagePeak] [float] NOT NULL,
    [ANVoltageRMS] [float] NOT NULL,
    [ANVoltagePhase] [float] NOT NULL,
    [ANCurrentPeak] [float] NOT NULL,
    [ANCurrentRMS] [float] NOT NULL,
    [ANCurrentPhase] [float] NOT NULL,
    
    [BNVoltagePeak] [float] NOT NULL,
    [BNVoltageRMS] [float] NOT NULL,
    [BNVoltagePhase] [float] NOT NULL,
    [BNCurrentPeak] [float] NOT NULL,
    [BNCurrentRMS] [float] NOT NULL,
    [BNCurrentPhase] [float] NOT NULL,
    
    [CNVoltagePeak] [float] NOT NULL,
    [CNVoltageRMS] [float] NOT NULL,
    [CNVoltagePhase] [float] NOT NULL,
    [CNCurrentPeak] [float] NOT NULL,
    [CNCurrentRMS] [float] NOT NULL,
    [CNCurrentPhase] [float] NOT NULL,
    
    [PositiveVoltageMagnitude] [float] NOT NULL,
    [PositiveVoltageAngle] [float] NOT NULL,
    [PositiveCurrentMagnitude] [float] NOT NULL,
    [PositiveCurrentAngle] [float] NOT NULL,
    
    [NegativeVoltageMagnitude] [float] NOT NULL,
    [NegativeVoltageAngle] [float] NOT NULL,
    [NegativeCurrentMagnitude] [float] NOT NULL,
    [NegativeCurrentAngle] [float] NOT NULL,
    
    [ZeroVoltageMagnitude] [float] NOT NULL,
    [ZeroVoltageAngle] [float] NOT NULL,
    [ZeroCurrentMagnitude] [float] NOT NULL,
    [ZeroCurrentAngle] [float] NOT NULL,
    
    CONSTRAINT [PK_Cycle] PRIMARY KEY CLUSTERED
    (
        [ID] ASC
    ) WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[FaultDistance]
(
    [CycleID] [int] NOT NULL,
    [Algorithm] [varchar](200) NOT NULL,
    [Distance] [float] NOT NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[EventDisturbance]
(
	[EventID] [int] NOT NULL,
	[LineDisturbanceID] [int] NOT NULL
) ON [PRIMARY]
GO


--- FOREIGN KEYS ---

ALTER TABLE [dbo].[Line] WITH CHECK ADD CONSTRAINT [FK_Line_Device] FOREIGN KEY ([DeviceID])
REFERENCES [dbo].[Device] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FileGroup] WITH CHECK ADD CONSTRAINT [FK_FileGroup_Device] FOREIGN KEY ([DeviceID])
REFERENCES [dbo].[Device] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DisturbanceFile] WITH CHECK ADD CONSTRAINT [FK_DisturbanceFile_FileGroup] FOREIGN KEY ([FileGroupID])
REFERENCES [dbo].[FileGroup] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LineDisturbance] WITH CHECK ADD CONSTRAINT [FK_LineDisturbance_Line] FOREIGN KEY ([LineID])
REFERENCES [dbo].[Line] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LineDisturbance] WITH CHECK ADD CONSTRAINT [FK_LineDisturbance_FileGroup] FOREIGN KEY ([FileGroupID])
REFERENCES [dbo].[FileGroup] ([ID])
GO

ALTER TABLE [dbo].[Cycle] WITH CHECK ADD CONSTRAINT [FK_Cycle_LineDisturbance] FOREIGN KEY ([LineDisturbanceID])
REFERENCES [dbo].[LineDisturbance] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FaultDistance] WITH CHECK ADD CONSTRAINT [FK_FaultDistance_Cycle] FOREIGN KEY ([CycleID])
REFERENCES [dbo].[Cycle] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EventDisturbance] WITH CHECK ADD CONSTRAINT [FK_EventDisturbance_Event] FOREIGN KEY ([EventID])
REFERENCES [dbo].[Event] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EventDisturbance] WITH CHECK ADD CONSTRAINT [FK_EventDisturbance_LineDisturbance] FOREIGN KEY ([LineDisturbanceID])
REFERENCES [dbo].[LineDisturbance] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Line] CHECK CONSTRAINT [FK_Line_Device]
GO

ALTER TABLE [dbo].[FileGroup] CHECK CONSTRAINT [FK_FileGroup_Device]
GO

ALTER TABLE [dbo].[DisturbanceFile] CHECK CONSTRAINT [FK_DisturbanceFile_FileGroup]
GO

ALTER TABLE [dbo].[LineDisturbance] CHECK CONSTRAINT [FK_LineDisturbance_Line]
GO

ALTER TABLE [dbo].[LineDisturbance] CHECK CONSTRAINT [FK_LineDisturbance_FileGroup]
GO

ALTER TABLE [dbo].[Cycle] CHECK CONSTRAINT [FK_Cycle_LineDisturbance]
GO

ALTER TABLE [dbo].[FaultDistance] CHECK CONSTRAINT [FK_FaultDistance_Cycle]
GO

ALTER TABLE [dbo].[EventDisturbance] CHECK CONSTRAINT [FK_EventDisturbance_Event]
GO

ALTER TABLE [dbo].[EventDisturbance] CHECK CONSTRAINT [FK_EventDisturbance_LineDisturbance]
GO
