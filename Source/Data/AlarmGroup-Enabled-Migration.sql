-- Migration: Add Enabled flag to AlarmGroup
-- This script adds the Enabled column to the AlarmGroup table and updates
-- the ActiveAlarmView and AlarmGroupView to reflect the new column.
-- Existing rows default to Enabled = 1 (true) to preserve existing behavior.

-- Add Enabled column to AlarmGroup if it does not already exist
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID('AlarmGroup')
      AND name = 'Enabled'
)
BEGIN
    ALTER TABLE AlarmGroup
    ADD Enabled BIT NOT NULL DEFAULT(1)
END
GO

-- Drop and recreate ActiveAlarmView to filter disabled AlarmGroups
IF OBJECT_ID('ActiveAlarmView', 'V') IS NOT NULL
    DROP VIEW ActiveAlarmView
GO

CREATE VIEW ActiveAlarmView AS
SELECT
    Alarm.ID AS AlarmID,
    Alarm.AlarmGroupID AS AlarmGroupID,
    AlarmGroup.AlarmTypeID AS AlarmTypeID,
    AlarmFactor.ID AS AlarmFactorID,
    AlarmFactor.SeverityID,
    Alarm.SeriesID AS SeriesID,
    AlarmFactor.Factor AS Value
FROM
    (
        SELECT AF.ID, AF.Factor, AF.AlarmGroupID, AF.SeverityID
        FROM AlarmFactor AF
        INNER JOIN AlarmGroup AG ON AF.AlarmGroupID = AG.ID AND AG.Enabled = 1
        UNION
        SELECT
            NULL AS ID,
            1.0 AS Factor,
            AlarmGroup.ID AS AlarmGroupID,
            AlarmGroup.SeverityID
        FROM AlarmGroup
        WHERE AlarmGroup.Enabled = 1
    ) AlarmFactor LEFT JOIN
    Alarm ON AlarmFactor.AlarmGroupID = alarm.AlarmGroupID LEFT JOIN
    AlarmGroup ON Alarm.AlarmGroupID = AlarmGroup.ID
GO

-- Drop and recreate AlarmGroupView to include Enabled column
IF OBJECT_ID('AlarmGroupView', 'V') IS NOT NULL
    DROP VIEW AlarmGroupView
GO

CREATE VIEW AlarmGroupView AS 
SELECT 
    AlarmGroup.ID,
    AlarmGroup.Name,
    AlarmSeverity.Name AlarmSeverity,
    CountStats.ChannelCount Channels,
    CountStats.MeterCount Meters,
    LastAlarm.StartTime LastAlarmStart,
    LastAlarm.EndTime LastAlarmEnd,
    LastAlarm.ChannelName LastChannel,
    LastAlarm.MeterName LastMeter,
    AlarmType.Description AS AlarmType,
    AlarmGroup.Enabled
FROM 
    AlarmGroup LEFT JOIN
    AlarmSeverity ON AlarmGroup.SeverityID = AlarmSeverity.ID LEFT JOIN
    AlarmType ON AlarmGroup.AlarmTypeID = AlarmType.ID OUTER APPLY
    (
        SELECT
            COUNT(DISTINCT Channel.ID) ChannelCount,
            COUNT(DISTINCT Channel.MeterID) MeterCount
        FROM
            Channel JOIN
            Series ON Series.ChannelID = Channel.ID JOIN
            Alarm ON Alarm.SeriesID = Series.ID
        WHERE Alarm.AlarmGroupID = AlarmGroup.ID
    ) CountStats OUTER APPLY
    (
        SELECT TOP 1
            LatestAlarmLog.StartTime,
            LatestAlarmLog.EndTime,
            Channel.Name ChannelName,
            Meter.Name MeterName
        FROM
            Alarm JOIN
            Series ON Alarm.SeriesID = Series.ID JOIN
            Channel ON Series.ChannelID = Channel.ID JOIN
            Meter ON Channel.MeterID = Meter.ID JOIN
            LatestAlarmLog ON LatestAlarmLog.AlarmID = Alarm.ID
        WHERE Alarm.AlarmGroupID = AlarmGroup.ID
        ORDER BY
            LatestAlarmLog.StartTime DESC,
            LatestAlarmLog.AlarmLogID DESC
    ) LastAlarm
GO
