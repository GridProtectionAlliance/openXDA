-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 3, 2015>
-- Description: <Description, Selects All Events in the database>
-- selectBreakersForCalendar
-- =============================================
CREATE PROCEDURE [dbo].[selectBreakersForCalendar]
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

DECLARE @counter INT = 0
DECLARE @eventDate DATE = (Select max(CAST(StartTime AS Date)) from Event) --'2015-03-23'
DECLARE @numberOfDays INT = DATEDIFF ( day , (Select min(CAST(StartTime AS Date)) from Event), @eventDate) --365*5

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT Date as thedate, Normal as normal, late as late, indeterminate as indeterminate
FROM
(
    SELECT #temp.Date, [BreakerOperationType].Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    FROM
        #temp CROSS JOIN
        [BreakerOperationType] LEFT OUTER JOIN
        (
            SELECT CAST([TripCoilEnergized] AS Date) AS EventDate, [BreakerOperationTypeID] as EventTypeID, COUNT(*) AS EventCount
            FROM [BreakerOperation] join [Event] on [BreakerOperation].[EventID] = [Event].[ID] and [Event].[MeterID] in (select * from authMeters(@username))
            GROUP BY CAST([TripCoilEnergized] AS Date), [BreakerOperationTypeID]
        ) AS Event ON #temp.Date = Event.EventDate AND [BreakerOperationType].ID = Event.EventTypeID
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN ( normal , late , indeterminate )
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 3, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectBreakersForMeterIDByDateRange '01/10/2013', '05/10/2015', '0'
-- selectBreakersForMeterIDByDateRange '05/07/2014' , '05/07/2014', '52', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectBreakersForMeterIDByDateRange]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT 
Date as thedate, 
COALESCE(Normal,0) as normal, 
COALESCE(late,0) as late, 
COALESCE(indeterminate,0) as indeterminate
FROM
(
    SELECT #temp.Date, [BreakerOperationType].Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    FROM
        #temp CROSS JOIN
        [BreakerOperationType] LEFT OUTER JOIN
        (
            SELECT CAST([TripCoilEnergized] AS Date) AS EventDate, [BreakerOperationTypeID] as EventTypeID, COUNT(*) AS EventCount
            FROM [BreakerOperation] join [Event] on [BreakerOperation].[EventID] = [Event].[ID]
            
             where MeterID in ( Select * from @MeterIDs)
            GROUP BY CAST([TripCoilEnergized] AS Date), [BreakerOperationTypeID]
        ) AS Event ON #temp.Date = Event.EventDate AND [BreakerOperationType].ID = Event.EventTypeID
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN ( normal , late , indeterminate )
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 3, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectBreakersForMeterIDsByDate '01/10/2013', '05/10/2015', '0'
-- selectBreakersForMeterIDsByDate '03/30/2015', '03/30/2015', '13,17,46,40,6,52,15,16,28,57,19,55,53,12,58,54,8,59,18,20,9,11,21,1,41,39,23,51,14,45,47,2,50,56,30,42,32,10,22,29,48,24,43,34,4,37,26,36,25,31,44,49,3,7,27,35,33,38,5,'
-- =============================================
CREATE PROCEDURE [dbo].[selectBreakersForMeterIDsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp (thesiteid int, thesitename varchar(100))

INSERT INTO #temp Select [dbo].[Meter].[ID], [dbo].[Meter].[Name] from [dbo].[Meter] where [dbo].[Meter].[ID] in (select * from authMeters(@username))

SELECT thesiteid as siteid, thesitename as sitename , Normal as normal, Late as late, Indeterminate as indeterminate
FROM
(
    SELECT #temp.thesiteid, #temp.thesitename , [BreakerOperationType].Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    
    FROM
        #temp CROSS JOIN
        [BreakerOperationType] LEFT OUTER JOIN
        (
            SELECT MeterID, [BreakerOperationTypeID], COUNT(*) AS EventCount
            FROM [BreakerOperation] join [Event] on [BreakerOperation].[EventID] = [Event].[ID]
            where MeterID in (Select * from @MeterIDs)
            and (CAST([TripCoilEnergized] as Date) between @EventDateFrom and @EventDateTo)
            GROUP BY [BreakerOperationTypeID], MeterID
        ) AS E ON [BreakerOperationType].ID = E.[BreakerOperationTypeID] and E.MeterID = #temp.thesiteid
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN (normal, late, indeterminate)
) as pvt
ORDER BY sitename asc

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Aug 31, 2015>
-- Description: <Description, Selects Correctness for a MeterID by Date for date range>
-- selectCompletenessForMeterIDByDateRange '01/10/2013', '05/10/2015', '0'
-- selectCompletenessForMeterIDByDateRange '01/01/2014', '01/01/2015', '108,109,86,87,110,118,13,17,167,168,77,78,79,80,70,46,169,185,186,40,6,170,88,89,52,192,15,16,91,92,93,94,95,96,97,28,98,99,100,101,102,171,57,172,125,173,63,19,104,174,55,105,106,107,103,111,112,113,60,53,160,114,115,116,117,68,12,58,54,8,59,18,20,9,193,119,120,81,164,121,175,156,157,176,177,11,122,123,21,65,1,41,39,23,51,127,128,129,130,165,76,131,132,133,134,135,136,137,14,124,45,47,73,64,2,138,139,140,141,142,143,82,83,62,50,144,145,56,30,42,146,147,148,149,150,151,32,152,126,74,67,10,66,22,178,179,29,180,48,181,153,154,155,194,24,43,34,4,69,37,158,26,182,36,159,161,162,163,71,166,25,31,44,49,72,61,75,187,188,189,190,191,3,84,85,7,195,90,183,27,196,184,35,33,197,198,199,200,201,38,5,', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectCompletenessForMeterIDByDateRange]
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',');

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT
    Date,
    COALESCE(First, 0) AS First,
    COALESCE(Second, 0) AS Second,
    COALESCE(Third, 0) AS Third,
    COALESCE(Fourth, 0) AS Fourth,
    COALESCE(Fifth, 0) AS Fifth,
    COALESCE(Sixth, 0) AS Sixth
FROM
(
    SELECT #temp.Date, CompletenessLevel, COALESCE(MeterCount, 0) AS MeterCount
    FROM
        #temp LEFT OUTER JOIN
        (
            SELECT Date, CompletenessLevel, COUNT(*) AS MeterCount
            FROM
            (
                SELECT
                    Date,
                    CASE
                        WHEN Completeness >= 100.0 THEN 'First'
                        WHEN 98.0 <= Completeness AND Completeness < 100.0 THEN 'Second'
                        WHEN 90.0 <= Completeness AND Completeness < 98.0 THEN 'Third'
                        WHEN 70.0 <= Completeness AND Completeness < 90.0 THEN 'Fourth'
                        WHEN 50.0 <= Completeness AND Completeness < 70.0 THEN 'Fifth'
                        WHEN 0.0 < Completeness AND Completeness < 50.0 THEN 'Sixth'
                    END AS CompletenessLevel
                FROM
                (
                    SELECT Date, 100.0 * CAST(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints AS FLOAT) / CAST(NULLIF(ExpectedPoints, 0) AS FLOAT) AS Completeness
                    FROM MeterDataQualitySummary
                    WHERE Date BETWEEN @EventDateFrom AND @EventDateTo AND MeterID IN (SELECT * FROM @MeterIDs)
                ) MeterDataQualitySummary
            ) MeterDataQualitySummary
            GROUP BY Date, CompletenessLevel
        ) MeterDataQualitySummary ON #temp.Date = MeterDataQualitySummary.Date
) MeterDataQualitySummary
PIVOT
(
    SUM(MeterCount)
    FOR MeterDataQualitySummary.CompletenessLevel IN (First, Second, Third, Fourth, Fifth, Sixth)
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, August 31, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectCompletenessForMeterIDsByDate '01/10/2013', '05/10/2015', '0', 'jwalker'
-- selectCompletenessForMeterIDsByDate '08/01/2006', '08/31/2015', '32,33,10,11,34,42,91,92,1,2,3,4,93,109,110,94,12,13,116,15,16,17,18,19,20,21,22,23,24,25,26,95,96,49,97,28,98,29,30,31,27,35,36,37,84,38,39,40,41,117,43,44,5,88,45,99,80,81,100,101,46,47,51,52,53,54,89,55,56,57,58,59,60,61,48,62,63,64,65,66,67,6,7,68,69,70,71,72,73,74,75,76,50,102,103,104,105,77,78,79,118,82,106,83,85,86,87,90,111,112,113,114,115,8,9,119,14,107,120,108,121,122,123,124,125,', 'jwalker'
-- selectCompletenessForMeterIDsByDate '08/01/2015', '08/31/2015', '32,33,10,11,34,42,91,92,1,2,3,4,93,109,110,94,12,13,116,15,16,17,18,19,20,21,22,23,24,25,26,95,96,49,97,28,98,29,30,31,27,35,36,37,84,38,39,40,41,117,43,44,5,88,45,99,80,81,100,101,46,47,51,52,53,54,89,55,56,57,58,59,60,61,48,62,63,64,65,66,67,6,7,68,69,70,71,72,73,74,75,76,50,102,103,104,105,77,78,79,118,82,106,83,85,86,87,90,111,112,113,114,115,8,9,119,14,107,120,108,121,122,123,124,125,', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectCompletenessForMeterIDsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #meters (
thesiteid int, 
thesitename varchar(100)
)

INSERT INTO #meters Select 
[dbo].[Meter].[ID] as thesiteid, 
[dbo].[Meter].[Name] as thesitename
from [dbo].[Meter] where Meter.ID in (Select * from @MeterIDs)

DECLARE @thesiteid int
DECLARE @thesitename varchar(100)

CREATE TABLE #temp (
siteId int, 
siteName varchar(100),
ExpectedPoints int,
GoodPoints int,
LatchedPoints int,
UnreasonablePoints int,
NoncongruentPoints int,
DuplicatePoints int
)

DECLARE db_cursor CURSOR FOR  
SELECT thesiteid, thesitename FROM #meters

OPEN db_cursor   
FETCH NEXT FROM db_cursor INTO @thesiteid , @thesitename

WHILE @@FETCH_STATUS = 0   
BEGIN
INSERT INTO #temp
SELECT
    @thesiteid, 
    @thesitename,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[ExpectedPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as ExpectedPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[GoodPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as GoodPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[LatchedPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as LatchedPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[UnreasonablePoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as UnreasonablePoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[NoncongruentPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as NoncongruentPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[DuplicatePoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as DuplicatePoints

    FETCH NEXT FROM db_cursor INTO @thesiteid , @thesitename
END   

CLOSE db_cursor   
DEALLOCATE db_cursor 

select * from #temp

drop Table #temp
drop Table #meters

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Aug 31, 2015>
-- Description: <Description, Selects Correctness for a MeterID by Date for date range>
-- [selectCorrectnessForMeterIDByDateRange] '01/10/2013', '05/10/2015', '0'
-- [selectCorrectnessForMeterIDByDateRange] '08/05/2007', '09/04/2008', '108,109,86,87,110,118,13,17,167,168,77,78,79,80,70,46,169,185,186,40,6,170,88,89,52,192,15,16,91,92,93,94,95,96,97,28,98,99,100,101,102,171,57,172,125,173,63,19,104,174,55,105,106,107,103,111,112,113,60,53,160,114,115,116,117,68,12,58,54,8,59,18,20,9,193,119,120,81,164,121,175,156,157,176,177,11,122,123,21,65,1,41,39,23,51,127,128,129,130,165,76,131,132,133,134,135,136,137,14,124,45,47,73,64,2,138,139,140,141,142,143,82,83,62,50,144,145,56,30,42,146,147,148,149,150,151,32,152,126,74,67,10,66,22,178,179,29,180,48,181,153,154,155,194,24,43,34,4,69,37,158,26,182,36,159,161,162,163,71,166,25,31,44,49,72,61,75,187,188,189,190,191,3,84,85,7,195,90,183,27,196,184,35,33,197,198,199,200,201,38,5,', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectCorrectnessForMeterIDByDateRange]
    @EventDateFrom as DateTime,
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',');

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT
    Date,
    COALESCE(First, 0) AS First,
    COALESCE(Second, 0) AS Second,
    COALESCE(Third, 0) AS Third,
    COALESCE(Fourth, 0) AS Fourth,
    COALESCE(Fifth, 0) AS Fifth,
    COALESCE(Sixth, 0) AS Sixth
FROM
(
    SELECT #temp.Date, CorrectnessLevel, COALESCE(MeterCount, 0) AS MeterCount
    FROM
        #temp LEFT OUTER JOIN
        (
            SELECT Date, CorrectnessLevel, COUNT(*) AS MeterCount
            FROM
            (
                SELECT
                    Date,
                    CASE
                        WHEN Correctness >= 100.0 THEN 'First'
                        WHEN 98.0 <= Correctness AND Correctness < 100.0 THEN 'Second'
                        WHEN 90.0 <= Correctness AND Correctness < 98.0 THEN 'Third'
                        WHEN 70.0 <= Correctness AND Correctness < 90.0 THEN 'Fourth'
                        WHEN 50.0 <= Correctness AND Correctness < 70.0 THEN 'Fifth'
                        WHEN 0.0 < Correctness AND Correctness < 50.0 THEN 'Sixth'
                    END AS CorrectnessLevel
                FROM
                (
                    SELECT Date, 100.0 * CAST(GoodPoints AS FLOAT) / CAST(NULLIF(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints, 0) AS FLOAT) AS Correctness
                    FROM MeterDataQualitySummary
                    WHERE Date BETWEEN @EventDateFrom AND @EventDateTo AND MeterID IN (SELECT * FROM @MeterIDs)
                ) MeterDataQualitySummary
            ) MeterDataQualitySummary
            GROUP BY Date, CorrectnessLevel
        ) MeterDataQualitySummary ON #temp.Date = MeterDataQualitySummary.Date
) MeterDataQualitySummary
PIVOT
(
    SUM(MeterCount)
    FOR MeterDataQualitySummary.CorrectnessLevel IN (First, Second, Third, Fourth, Fifth, Sixth)
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, August 31, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectCorrectnessForMeterIDsByDate '01/10/2013', '05/10/2015', '0', 'jwalker'
-- selectCorrectnessForMeterIDsByDate '01/01/2007', '01/01/2008', '1,2,3,4,5,6,7,8,9,', 'jwalker'
-- selectCorrectnessForMeterIDsByDate '08/01/2015', '08/31/2015', '32,33,10,11,34,42,91,92,1,2,3,4,93,109,110,94,12,13,116,15,16,17,18,19,20,21,22,23,24,25,26,95,96,49,97,28,98,29,30,31,27,35,36,37,84,38,39,40,41,117,43,44,5,88,45,99,80,81,100,101,46,47,51,52,53,54,89,55,56,57,58,59,60,61,48,62,63,64,65,66,67,6,7,68,69,70,71,72,73,74,75,76,50,102,103,104,105,77,78,79,118,82,106,83,85,86,87,90,111,112,113,114,115,8,9,119,14,107,120,108,121,122,123,124,125,', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectCorrectnessForMeterIDsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #meters (
thesiteid int, 
thesitename varchar(100)
)

INSERT INTO #meters Select 
[dbo].[Meter].[ID] as thesiteid, 
[dbo].[Meter].[Name] as thesitename
from [dbo].[Meter] where Meter.ID in (Select * from @MeterIDs)

DECLARE @thesiteid int
DECLARE @thesitename varchar(100)

CREATE TABLE #temp (
siteId int, 
siteName varchar(100),
ExpectedPoints int,
GoodPoints int,
LatchedPoints int,
UnreasonablePoints int,
NoncongruentPoints int,
DuplicatePoints int
)

DECLARE db_cursor CURSOR FOR  
SELECT thesiteid, thesitename FROM #meters

OPEN db_cursor   
FETCH NEXT FROM db_cursor INTO @thesiteid , @thesitename

WHILE @@FETCH_STATUS = 0   
BEGIN
INSERT INTO #temp
SELECT
    @thesiteid, 
    @thesitename,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[ExpectedPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as ExpectedPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[GoodPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as GoodPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[LatchedPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as LatchedPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[UnreasonablePoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as UnreasonablePoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[NoncongruentPoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as NoncongruentPoints,
    (Select Coalesce(SUM([dbo].[MeterDataQualitySummary].[DuplicatePoints]), 0) from [MeterDataQualitySummary] where @thesiteid = MeterID and CAST([Date] as Date) between @EventDateFrom and @EventDateTo) as DuplicatePoints

    FETCH NEXT FROM db_cursor INTO @thesiteid , @thesitename
END   

CLOSE db_cursor   
DEALLOCATE db_cursor 

select * from #temp

drop Table #temp
drop Table #meters

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, August 27, 2015>
-- Description: <Description, Selects All Events in the database>
-- selectDashSettings 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectDashSettings]
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

    Select * from [DashSettings]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, April 30, 2015>
-- Description: <Description, Selects Double Ended Fault Distance by EventID>
-- selectDoubleEndedFaultDistanceForEventID '1'

-- =============================================
CREATE PROCEDURE [dbo].[selectDoubleEndedFaultDistanceForEventID]
    -- Add the parameters for the stored procedure here
    @EventID as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

    select Distance, Angle from DoubleEndedFaultSummary where EventID = @EventID

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Nov 10, 2014>
-- Description: <Description, Selects tMeasurementCharacteristic for a MeterID by Date for day>
-- [selectEventInstance] '11/17/2008', 10, 2
-- =============================================
CREATE PROCEDURE [dbo].[selectEventInstance]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000),
    @Type as int

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  SELECT [dbo].[Event].[ID] as value, [dbo].[Event].[StartTime] as text, [dbo].[MeterLine].[LineName] as linename FROM [dbo].[Event] 
  join [dbo].[Line] on [dbo].[Event].[LineID] = [dbo].[Line].[ID]
  join [dbo].[MeterLine] on [dbo].[MeterLine].[LineID] = [dbo].[Line].[ID] and [dbo].[MeterLine].[MeterID] = [dbo].[Event].[MeterID]
  where (CAST([dbo].[Event].[StartTime] as Date) = @theDate) and 
  [dbo].[Event].[EventTypeID] = @Type and 
  [dbo].[Event].[MeterID] = @MeterID
  order by [dbo].[Event].[StartTime]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jan 12, 2015>
-- Description: <Description, Selects EventInstances for a MeterID by Date for day>
-- [selectEventInstancesByMeterDate] '11/17/2008', 10
-- =============================================
CREATE PROCEDURE [dbo].[selectEventInstancesByMeterDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  SELECT [dbo].[Event].[ID] as value, [dbo].[Event].[StartTime] as text FROM [dbo].[Event] 
  where (CAST([dbo].[Event].[StartTime] as Date) = @theDate) and 
  [dbo].[Event].[MeterID] = @MeterID
  order by [dbo].[Event].[StartTime]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jan 12, 2015>
-- Description: <Description, Selects EventInstances for a MeterID and LineID by Date for day>
-- [selectEventInstancesByMeterLineDate] '06/17/2013', 7 , 30
-- =============================================
CREATE PROCEDURE [dbo].[selectEventInstancesByMeterLineDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000),
    @LineID as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  SELECT [dbo].[Event].[ID] as value, [dbo].[Event].[StartTime] as text , [dbo].[EventType].[Name] as type FROM [dbo].[Event] 
  join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID]
  where (CAST([dbo].[Event].[StartTime] as Date) = @theDate) and 
  [dbo].[Event].[LineID] = @LineID --and
  --[dbo].[Event].[MeterID] = @MeterID
  order by [dbo].[Event].[EventTypeID] , [dbo].[Event].[StartTime]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects All Events in the database>
-- selectEventsForCalendar 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectEventsForCalendar]
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

DECLARE @counter INT = 0
DECLARE @eventDate DATE = (Select max(CAST(StartTime AS Date)) from Event) --'2015-03-23'
DECLARE @numberOfDays INT = DATEDIFF ( day , (Select min(CAST(StartTime AS Date)) from Event), @eventDate) --365*5

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT Date as thedate, Fault as faults, Interruption as interruptions, Sag as sags, Swell as swells, Other as others
FROM
(
    SELECT #temp.Date, EventType.Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    FROM
        #temp CROSS JOIN
        EventType LEFT OUTER JOIN
        (
            SELECT CAST(StartTime AS Date) AS EventDate, EventTypeID, COUNT(*) AS EventCount
            FROM Event where Event.MeterID in (select * from authMeters(@username))
            GROUP BY CAST(StartTime AS Date), EventTypeID
        ) AS Event ON #temp.Date = Event.EventDate AND EventType.ID = Event.EventTypeID
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN (Fault, Interruption, Sag, Swell, Other)
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectEventsForMeterIDByDateRange '01/10/2013', '05/10/2015', '0'
-- selectEventsForMeterIDByDateRange '03/12/2014', '03/12/2015', '0', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectEventsForMeterIDByDateRange]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',');

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT Date as thedate, Fault as faults, Interruption as interruptions, Sag as sags, Swell as swells, Other as others, Transient as transients
FROM
(
    SELECT #temp.Date, EventType.Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    FROM
        #temp CROSS JOIN
        EventType LEFT OUTER JOIN
        (
            SELECT CAST(StartTime AS Date) AS EventDate, EventTypeID, COUNT(*) AS EventCount
            FROM Event where ( (@MeterID = '0' and MeterID = MeterID) or (MeterID in ( Select * from @MeterIDs) ) )
            and MeterID in (select * from authMeters(@username))
            GROUP BY CAST(StartTime AS Date), EventTypeID
        ) AS Event ON #temp.Date = Event.EventDate AND EventType.ID = Event.EventTypeID
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN (Fault, Interruption, Sag, Swell, Other, Transient)
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectEventsForMeterIDsByDate '01/10/2013', '05/10/2015', '58,', 'jwalker'
-- selectEventsForMeterIDsByDate '03/12/2013', '03/12/2014', '13,17,46,40,6,52,15,16,28,57,19,55,53,12,58,54,8,59,18,20,9,11,21,1,41,39,23,51,14,45,47,2,50,56,30,42,32,10,22,29,48,24,43,34,4,37,26,36,25,31,44,49,3,7,27,35,33,38,5,' , ''
-- =============================================
CREATE PROCEDURE [dbo].[selectEventsForMeterIDsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp (thesiteid int, thesitename varchar(100))

INSERT INTO #temp Select [dbo].[Meter].[ID], [dbo].[Meter].[Name] from [dbo].[Meter] where Meter.ID in (Select * from @MeterIDs)

SELECT thesiteid as siteid, thesitename as sitename , Fault as faults, Interruption as interruptions, Sag as sags, Swell as swells, Other as others, Transient as transients
FROM
(
    SELECT #temp.thesiteid, #temp.thesitename , EventType.Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    FROM
        #temp CROSS JOIN
        EventType LEFT OUTER JOIN
        (
            SELECT MeterID, EventTypeID, COUNT(*) AS EventCount FROM Event where
            (CAST([StartTime] as Date) between @EventDateFrom and @EventDateTo)
            GROUP BY EventTypeID, MeterID
        ) AS E ON EventType.ID = E.EventTypeID and E.MeterID = #temp.thesiteid 
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN (Fault, Interruption, Sag, Swell, Other, Transient)
) as pvt
ORDER BY sitename asc

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Nov 10, 2014>
-- Description: <Description, Selects MeasurementTypes for a MeterID by Date for a day>
-- [selectEventType] '11/09/2008', 10

-- =============================================
CREATE PROCEDURE [dbo].[selectEventType]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  -- EventType
  SELECT distinct [dbo].[EventType].[ID] as value, [dbo].[EventType].[Name] as text, [dbo].[Event].[StartTime] from [dbo].[EventType]
  
  join [dbo].[Event] on 
  [dbo].[Event].[EventTypeID] = [dbo].[EventType].[ID] and 
  CAST([dbo].[Event].[StartTime] as Date) = @theDate and 
  [dbo].[Event].[MeterID] = @MeterID

  order by [dbo].[Event].[StartTime]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 11, 2014>
-- Description: <Description, Fault Counts and Location of all meters for a day>
-- =============================================

--[selectFaultHeatmapForDate] '06/01/2008'

CREATE PROCEDURE [dbo].[selectFaultHeatmapForDate]
    @EventDate DateTime
AS
BEGIN

    SET NOCOUNT ON;

        SELECT [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        (

        Select count([dbo].[Event].[ID]) from [dbo].[Event] 
        inner join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID] and [dbo].[EventType].[Name] = 'Fault'
        where [dbo].[Meter].[ID] = [dbo].[Event].[MeterID] and CAST([StartTime] as Date) = CAST(@EventDate as Date)) as Event_Count

        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] --where [Latitude] <> 0 and [Longitude] <> 0

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, June 11, 2015>
-- Description: <Description, Returns count of faults of type>
-- =============================================

--[selectFaultHistoryByLineIDType] 262,'AB'

CREATE PROCEDURE [dbo].[selectFaultHistoryByLineIDType]
    @LineID as int,
    @FaultType as varchar(20)
AS
BEGIN

declare @test as float
declare @test2 as float

Select
  (SELECT count ([FaultSummary].[ID])  FROM [FaultSummary] join [Event] on [Event].[ID] = [FaultSummary].[EventID]
  where [IsSelectedAlgorithm] = 1 and [IsValid] = 1 and [IsSuppressed] = 0 and [LineID] = @LineID) total,

  (SELECT count ([FaultSummary].[ID]) FROM [FaultSummary] join [Event] on [Event].[ID] = [FaultSummary].[EventID]
  where [IsSelectedAlgorithm] = 1 and [IsValid] = 1 and [IsSuppressed] = 0 and [LineID] = @LineID and [FaultType] = @FaultType) as faultcount
  
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Mar 2, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for @NumberOfDays days back>
-- [selectFaultsForMeterIDByDateRange] '01/12/2011', '03/12/2015', '52', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectFaultsForMeterIDByDateRange]
    @EventDateFrom as DateTime,
    @EventDateTo as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    declare @counter int = 0
    declare @EventDate as DateTime
    declare @NumberOfDays as INT

    set @NumberOfDays = DateDiff ( day  , @EventDateFrom , @EventDateTo )

    set @EventDate = @EventDateFrom

    PRINT @NumberOfDays
    PRINT @EventDate

    CREATE TABLE #temp(Date DATE)

    WHILE (@counter <= @numberOfDays)
    BEGIN
        INSERT INTO #temp VALUES(@eventDate)
        SET @eventDate = DATEADD(DAY, 1, @eventDate)
        SET @counter = @counter + 1
    END

    SELECT
        #temp.Date AS thedate,
        COALESCE(Event.FaultCount, 0) AS thecount,
        Line.VoltageKV AS theclass
    FROM
        #temp CROSS JOIN
        (
            SELECT DISTINCT VoltageKV
            FROM Line
        ) Line LEFT OUTER JOIN
        (
            SELECT
                CAST(Event.StartTime AS Date) AS Date,
                Line.VoltageKV,
                COUNT(*) AS FaultCount
            FROM
                Event JOIN
                EventType ON Event.EventTypeID = EventType.ID JOIN
                Line ON Event.LineID = Line.ID
            WHERE
                EventType.Name = 'Fault' AND
                Event.MeterID IN (SELECT * FROM @MeterIDs)
            GROUP BY
                CAST(Event.StartTime AS Date),
                Line.VoltageKV
        ) Event ON Event.Date = #temp.Date AND Event.VoltageKV = Line.VoltageKV
    ORDER BY
        #temp.Date,
        Line.VoltageKV DESC

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectFaultsForMeterIDsByDate '03/12/2013', '03/12/2014', '13,17,46,40,6,52,15,16,28,57,19,55,53,12,58,54,8,59,18,20,9,11,21,1,41,39,23,51,14,45,47,2,50,56,30,42,32,10,22,29,48,24,43,34,4,37,26,36,25,31,44,49,3,7,27,35,33,38,5,' , 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectFaultsForMeterIDsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);

INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp (thesiteid int, thesitename varchar(100))

INSERT INTO #temp Select [dbo].[Meter].[ID], [dbo].[Meter].[Name] from [dbo].[Meter] where [dbo].[Meter].[ID] in (Select * from @MeterIDs)

SELECT thesiteid as siteid, thesitename as sitename , Fault as faults
FROM
(
    SELECT #temp.thesiteid, #temp.thesitename , EventType.Name AS EventTypeName, COALESCE(EventCount, 0) AS EventCount
    FROM
        #temp CROSS JOIN
        EventType LEFT OUTER JOIN
        (
            SELECT MeterID, EventTypeID, COUNT(*) AS EventCount FROM Event
            where MeterID in (Select * from @MeterIDs)
            and (CAST([StartTime] as Date) between @EventDateFrom and @EventDateTo)
            
            GROUP BY EventTypeID, MeterID
        ) AS E ON EventType.ID = E.EventTypeID and E.MeterID = #temp.thesiteid and EventType.Name = 'Fault'
) AS EventDate
PIVOT
(
    SUM(EventCount)
    FOR EventDate.EventTypeName IN (Fault, Interruption, Sag, Swell, Other)
) as pvt
ORDER BY sitename asc

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Oct 2, 2015>
-- Description: <Description, Selects Double Ended Fault Distance by EventID>
-- [selectHasICFResult] '1'

-- =============================================
CREATE PROCEDURE [dbo].[selectHasICFResult]
    -- Add the parameters for the stored procedure here
    @EventID as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

    return dbo.HasICFResult(@EventID)

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects Trending Data for a ChannelID by Date for a day>
-- selectHeatmapMeterLocationsTrending2 '09/29/2014 00:00:00.0000000', '5,6,7,8,21,22,23,24,25,38,39,40,41,55,56,57,58,72,73,74,75,88,89,90,91,105,106,107,108,122,123,124,125,138,139,140,141,142,155,156,157,158,172,173,174,175,189,190,191,192,13,14,15,16,30,31,32,33,46,47,48,49,50,63,64,65,66,80,81,82,83,97,98,99,100,113,114,115,116,130,131,132,133,147,148,149,150,163,164,165,166,167,180,181,182,183,197,198,199,200,2,4,9,11,18,20,27,29,34,36,43,45,52,54,59,61,68,70,77,79,84,86,93,95,102,104,109,111,118,120,127,129,134,136,143,145,152,154,159,161,168,170,177,179,184,186,188,193,195,1,3,10,12,17,19,26,28,35,37,42,44,51,53,60,62,67,69,71,76,78,85,87,92,94,96,101,103,110,112,117,119,121,126,128,135,137,144,146,151,153,160,162,169,171,176,178,185,187,194,196,201,','jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectHeatmapMeterLocationsTrending]
    -- Add the parameters for the stored procedure here
    @EventDate DateTime2,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @ChannelList VARCHAR(MAX)

--SELECT @ChannelList = CAST([dbo].[Channel].[ID] as Varchar(Max)) + ',' + @ChannelList

SELECT @ChannelList = Coalesce(@ChannelList+ ',','') + CAST([dbo].[Channel].[ID] as Varchar(Max))

  FROM [dbo].[Channel] join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[Channel].[MeterID] and [dbo].[Meter].[ID] in (Select ID from @MeterIDs)
  where [dbo].[Channel].[MeasurementCharacteristicID] = 
  (
  SELECT [ID]
  FROM [dbo].[MeasurementCharacteristic] where Name = 'TotalTHD'
  )
  and
  [dbo].[Channel].[PhaseID] = 
  (
  SELECT [ID]
  FROM [dbo].[Phase] where Name = 'AN'
  )

  --Print @ChannelList

BEGIN

WITH TrendingDataPoint AS
(
    SELECT
        ChannelID,
        SeriesType,
        Time,
        Value
    FROM
        GetTrendingData(@EventDate, DATEADD(NANOSECOND, -100, DATEADD(minute, 5, @EventDate)), @ChannelList, default) AS TrendingData JOIN
        (
            SELECT 0 AS ID, 'Minimum' AS SeriesType UNION
            SELECT 1 AS ID, 'Maximum' AS SeriesType UNION
            SELECT 2 AS ID, 'Average' AS SeriesType
        ) AS Series ON TrendingData.SeriesID = Series.ID
),
TrendingData AS
(
    SELECT
        ChannelID,
        Time,
        Maximum,
        Minimum,
        Average
    FROM TrendingDataPoint
    PIVOT
    (
        SUM(Value)
        FOR SeriesType IN (Maximum, Minimum, Average)
    ) AS TrendingData
)
SELECT
Coalesce([dbo].[Meter].[ID],0) as MeterID,
[dbo].[MeterLocation].[Longitude] as Longitude, 
[dbo].[MeterLocation].[Latitude] as Latitude,
[TrendingData].[Time] as thedate,
Cast(Round(([TrendingData].[Maximum] * 100),0) as Int) as Value

FROM [TrendingData] 
    left outer join [dbo].[AlarmRangeLimit] on [dbo].[AlarmRangeLimit].[ChannelID] = [TrendingData].[ChannelID]
    left outer join [dbo].[HourOfWeekLimit] on [dbo].[HourOfWeekLimit].[ChannelID] = [TrendingData].[ChannelID]
    join [dbo].[Channel] on [dbo].[Channel].[ID] = TrendingData.ChannelID
    join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[Channel].[MeterID]
    join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] 
  where
      ([dbo].[HourOfWeekLimit].[HourOfWeek] = 0 or [dbo].[HourOfWeekLimit].[HourOfWeek] is null)
    order by [TrendingData].[Time]

END

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jan 12, 2015>
-- Description: <Description, Selects Lines for Events at a MeterID by Date for a day>
-- [selectLineNames] '11/09/2008', 10

-- =============================================
CREATE PROCEDURE [dbo].[selectLineNames]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  -- EventType
  SELECT distinct [dbo].[Event].[LineID] as value, [dbo].[MeterLine].[LineName] as text, [dbo].[Event].[StartTime] from [dbo].[Event]
  
  join [dbo].[Line] on [dbo].[Event].[LineID] = [dbo].[Line].[ID] and CAST([dbo].[Event].[StartTime] as Date) = @theDate 
  join [dbo].[MeterLine] on [dbo].[MeterLine].[LineID] = [dbo].[Event].[LineID]
  order by [dbo].[Event].[StartTime]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects tMeasurementCharacteristic for a MeterID by Date for day>
-- selectMeasurementCharacteristic '04/07/2008', 61, 1

-- =============================================
CREATE PROCEDURE [dbo].[selectMeasurementCharacteristic]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000),
    @Type as int

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  SELECT distinct [dbo].[MeasurementCharacteristic].[ID] as value,  [dbo].[MeasurementCharacteristic].[Name] as text FROM [dbo].[DailyTrendingSummary] 

  join [dbo].[Channel] on [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[Channel].[ID] and [DailyTrendingSummary].[Date] = @theDate
  join [dbo].[Meter] on [dbo].[Channel].[MeterID] = [dbo].[Meter].[ID] and [dbo].[Meter].[ID] = @MeterID
  join [dbo].[MeasurementCharacteristic] on [dbo].[Channel].[MeasurementCharacteristicID] = [dbo].[MeasurementCharacteristic].[ID]
  join [dbo].[MeasurementType] on [dbo].[Channel].[MeasurementTypeID] = @Type
  order by [dbo].[MeasurementCharacteristic].[ID]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects MeasurementTypes for a MeterID by Date for a day>
-- selectMeasurementTypes '09/08/2007', 98

-- =============================================
CREATE PROCEDURE [dbo].[selectMeasurementTypes]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  -- MeasurementType
  SELECT distinct [dbo].[MeasurementType].[ID] as value,  [dbo].[MeasurementType].[Name] as text FROM [dbo].[MeasurementType]
  join [dbo].[Channel] on [dbo].[Channel].[MeasurementTypeID] = [dbo].[MeasurementType].[ID]
  join [dbo].[DailyTrendingSummary] on [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[Channel].[ID] and [Date] = @theDate
  join [dbo].[Meter] on [dbo].[Channel].[MeterID] = [dbo].[Meter].[ID] and [dbo].[Meter].[ID] = @MeterID
  order by [dbo].[MeasurementType].[ID]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, June 16, 2014>
-- Description: <Description, Selects meters within rectangular geocoordinate set>
-- selectMeterIDsForArea -91.40624523544129 , 40.539939877519885 , -83.474116329193464 , 37.117668162290165

-- =============================================
CREATE PROCEDURE [dbo].[selectMeterIDsForArea]
    -- Add the parameters for the stored procedure here
    @ax as float,
    @ay as float,
    @bx as float,
    @by as float,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT distinct [dbo].[Meter].[ID] as TheMeterID from [dbo].[Meter] inner join [dbo].[MeterLocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID]
        
        where ( [dbo].[MeterLocation].[Latitude] between @by and @ay and [dbo].[MeterLocation].[Longitude] between @ax and @bx )
        and [dbo].[Meter].[ID] in (select * from authMeters(@username))

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for Day>
-- =============================================

--[selectMeterLocations] '04/07/2008'

CREATE PROCEDURE [dbo].[selectMeterLocations]
    @EventDate DateTime
AS
BEGIN

    SET NOCOUNT ON;

        SELECT [dbo].[Meter].[ID], [dbo].[Meter].[Name], [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        (
        Select count([dbo].[Event].[ID]) from [dbo].[Event]
        inner join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID]
        where CAST([StartTime] as Date) = CAST(@EventDate as Date) and
        [dbo].[Event].[MeterID] = [dbo].[Meter].[ID]
        
        
        ) as Event_Count

        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] --where [Latitude] <> 0 and [Longitude] <> 0

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 03, 2015>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================

--[selectMeterLocationsBreakers] '05/07/2014' , '05/07/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsBreakers]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT [dbo].[Meter].[ID], [dbo].[Meter].[Name], [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        (
        select count (BreakerOperation.ID) from BreakerOperation 
        join Event on BreakerOperation.EventID = Event.ID
        where (CAST([TripCoilEnergized] as Date) between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)) 
        and Event.MeterID = [dbo].[Meter].[ID]
        ) as Event_Count

        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID]
        where [dbo].[Meter].[ID] in (select * from authMeters(@username))
        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Aug 31, 2015>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================

--[selectMeterLocationsCompleteness] '05/07/2014' , '05/07/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsCompleteness]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT 
        [dbo].[Meter].[ID], 
        [dbo].[Meter].[Name], 
        [dbo].[MeterLocation].[Longitude], 
        [dbo].[MeterLocation].[Latitude],
        (
        Select Cast(Coalesce(cast(SUM(goodPoints) as float) / NULLIF(cast(SUM(expectedPoints) as float),0) * 100 , 0 ) as int) from MeterDataQualitySummary
            where [dbo].[MeterDataQualitySummary].[MeterID] = [dbo].[Meter].[ID] and [Date] between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)
        ) as Event_Count

        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID]
        where [dbo].[Meter].[ID] in (select * from authMeters(@username))
        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Aug 31, 2015>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================
--[selectMeterLocationsCorrectness] '08/14/2008' , '08/16/2008', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsCorrectness]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT 
        [dbo].[Meter].[ID], 
        [dbo].[Meter].[Name], 
        [dbo].[MeterLocation].[Longitude], 
        [dbo].[MeterLocation].[Latitude], 
        (
        Select Cast(Coalesce(cast(SUM(goodPoints) as float) / NULLIF(cast(SUM(expectedPoints) as float),0) * 100 , 0 ) as int) from MeterDataQualitySummary
            where [dbo].[MeterDataQualitySummary].[MeterID] = [dbo].[Meter].[ID] and [Date] between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)
        ) as Event_Count

        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] --where [Latitude] <> 0 and [Longitude] <> 0
        and [dbo].[Meter].[ID] in (select * from authMeters(@username))
        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================
--[selectMeterLocationsEvents] '04/07/2008' , '04/07/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsEvents]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT [dbo].[Meter].[ID], [dbo].[Meter].[Name], [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        (
        Select count([dbo].[Event].[ID]) from [dbo].[Event]
        inner join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID] 
        where (CAST([StartTime] as Date) between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)) and
        [dbo].[Event].[MeterID] = [dbo].[Meter].[ID] 
        ) as Event_Count
        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] --where [Latitude] <> 0 and [Longitude] <> 0
        and [dbo].[Meter].[ID] in (select * from authMeters(@username))
        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================

--[selectMeterLocationsFaults] '04/07/2008' , '04/07/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsFaults]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        declare @thedatefrom as Date;
        declare @thedateto as Date;

        set @thedatefrom = CAST(@EventDateFrom as Date);
        set @thedateto = CAST(@EventDateTo as Date);

        SELECT [dbo].[Meter].[ID], [dbo].[Meter].[Name], [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        (
        Select count([dbo].[Event].[ID]) from [dbo].[Event]
        inner join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID] and [dbo].[EventType].[Name] = 'Fault'
        where (CAST([StartTime] as Date) between @thedatefrom and @thedateto) and
        [dbo].[Event].[MeterID] = [dbo].[Meter].[ID] 
        ) as Event_Count

        from [dbo].[Meter] 
        inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID]
        where [dbo].[Meter].[ID] in (select * from authMeters(@username))

        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================
--[selectMeterLocationsMaximumSwell] '08/22/2014' , '09/21/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsMaximumSwell]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT [dbo].[Meter].[ID], [dbo].[Meter].[Name], [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        cast(Coalesce((
        SELECT (10.0/9.0) * ((max([PerUnitMagnitude]) - 1.1)) * 100  from [Disturbance] 
        join [Event] on [Disturbance].[EventID] = [Event].[ID]
        join [EventType] on [EventType].[ID] = [Event].[EventTypeID] and [EventType].[Name] = 'Swell'
        where (CAST([Event].[StartTime] as Date) between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)) and
        [dbo].[Event].[MeterID] = [dbo].[Meter].[ID] and 
        [PerUnitMagnitude] is not null and [PerUnitMagnitude] >= 1.1
        ),0) as int) as Event_Count
        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] --where [Latitude] <> 0 and [Longitude] <> 0
        and [dbo].[Meter].[ID] in (select * from authMeters(@username))
        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================
--[selectMeterLocationsMinimumSags] '09/07/2014' , '09/07/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsMinimumSags]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        SELECT [dbo].[Meter].[ID], [dbo].[Meter].[Name], [dbo].[MeterLocation].[Longitude], [dbo].[MeterLocation].[Latitude], 
        cast(Coalesce((

        SELECT (10.0/8.0) * ( .9 - (min([PerUnitMagnitude]))) * 100  from [Disturbance] 
        join [Event] on [Disturbance].[EventID] = [Event].[ID]
        join [EventType] on [EventType].[ID] = [Event].[EventTypeID] and [EventType].[Name] = 'Sag'
        where (CAST([Event].[StartTime] as Date) between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)) and
        [dbo].[Event].[MeterID] = [dbo].[Meter].[ID] and 
        [PerUnitMagnitude] is not null and [PerUnitMagnitude] <= .9
        ),0) as int) as Event_Count
        from [dbo].[Meter] inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] --where [Latitude] <> 0 and [Longitude] <> 0
        and [dbo].[Meter].[ID] in (select * from authMeters(@username))
        order by [dbo].[Meter].[Name] asc
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 24, 2014>
-- Description: <Description, Selects Meter Identification Data, Alarm Count and Location for Day>
-- =============================================

--update meterlocation set Longitude = (select (cast(abs(checksum(NewId())) % 10000 as float) /1000) + (-90))
--update meterlocation set Latitude = (select (cast(abs(checksum(NewId())) % 10000 as float) /1000) + 30)

--[selectMeterLocationsTrending] '08/19/2008','08/20/2008', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsTrending]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

        declare @thedatefrom as Date;
        declare @thedateto as Date;

        set @thedatefrom = CAST(@EventDateFrom as Date);
        set @thedateto = CAST(@EventDateTo as Date);

        SELECT 
        [dbo].[Meter].[ID], 
        [dbo].[Meter].[Name], 
        [dbo].[MeterLocation].[Longitude], 
        [dbo].[MeterLocation].[Latitude],

        (Select count([dbo].[AlarmLog].[ID]) from [dbo].[AlarmLog] 
            inner join [dbo].[Channel] on [dbo].[AlarmLog].[ChannelID] = [dbo].[Channel].[ID]
            --inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[Channel].[MeterID]
            --and [dbo].[Meter].[ID] in (select * from authMeters(@username))
            inner join [dbo].[AlarmType] on [dbo].[AlarmType].[ID] = [dbo].[AlarmLog].[AlarmTypeID] and
            ([dbo].[AlarmType].[Name] = 'OffNormal' or [dbo].[AlarmType].[Name] = 'Alarm')
            where 
            CAST([Time] as Date) between @thedatefrom and @thedateto
            and [dbo].[Meter].[ID] = [dbo].[Channel].[MeterID]
            
            )
            as Event_Count
            
        from [dbo].[Meter]
        inner join [dbo].[Meterlocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID] 
        where [dbo].[Meter].[ID] in (select * from authMeters(@username))

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Feb 25, 2015>
-- Description: <Description, Selects meters>
-- selectMeters 'jwalker'

-- =============================================
CREATE PROCEDURE [dbo].[selectMeters]
    -- Add the parameters for the stored procedure here
        @username as nvarchar(4000)
    

AS
BEGIN

    SET NOCOUNT ON;
        SELECT distinct [dbo].[Meter].[ID] as id, [dbo].[Meter].[Name] 
        from [dbo].[Meter] where [dbo].[Meter].[ID] in (Select * from authMeters(@username) )
        order by [dbo].[Meter].[Name]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects Phase for Meter, MeasurementType, MeasurementCharacteristic for date >
-- selectPhase '04/07/2008', 61 , 1 , 1

-- =============================================
CREATE PROCEDURE [dbo].[selectPhase]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime, 
    @MeterID as nvarchar(4000),
    @Type as int,
    @Characteristic as int

AS
BEGIN

    SET NOCOUNT ON;

    declare @theDate as Date

    set @theDate = CAST(@EventDate as Date)

  SELECT distinct [dbo].[Phase].[ID] as value,  [dbo].[Phase].[Name] as text from [dbo].[Phase]
    join [dbo].[Channel] on [dbo].[Channel].[PhaseID] = [dbo].[Phase].[ID]
    join [dbo].[DailyTrendingSummary] on [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[Channel].[ID] and [DailyTrendingSummary].[Date] = @theDate
    join [dbo].[Meter] on [dbo].[Channel].[MeterID] = [dbo].[Meter].[ID] and [dbo].[Meter].[ID] = @MeterID
    join [dbo].[MeasurementType] on [dbo].[Channel].[MeasurementTypeID] = @Type
    join [dbo].[MeasurementCharacteristic] on [dbo].[Channel].[MeasurementCharacteristicID] = @Characteristic
    order by [dbo].[Phase].[ID]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Mar 02, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for @NumberOfDays days back>
-- selectPowerLineClasses 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectPowerLineClasses]
    @username as nvarchar(4000) 
AS
BEGIN

    SET NOCOUNT ON;

select distinct VoltageKV as class from [dbo].[Line] 
join Channel on Line.ID = Channel.LineID
join Meter on Channel.MeterID = Meter.ID
where Meter.ID in (select * from authMeters(@username))

order by VoltageKV Desc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Sep 17, 2015>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSiteChannelCompletenessDetailsByDate2 '09/17/2014', '77'
-- =============================================
CREATE PROCEDURE [dbo].[selectSiteChannelCompletenessDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    select
    Distinct [dbo].[Channel].[ID] as channelid,
    [dbo].[Channel].[Name] as channelname,
    @EventDate as date,
    [dbo].[Meter].[ID] as meterid,
    [dbo].[MeasurementType].[Name] as measurementtype,
    [dbo].[MeasurementCharacteristic].[Name] as characteristic,
    [dbo].[Phase].[Name] as phasename,

    COALESCE(CAST( cast(LatchedPoints as float) / NULLIF(cast(expectedPoints as float),0) as float),0) * 100 as Latched,
    COALESCE(CAST( cast(UnreasonablePoints as float) / NULLIF(cast(expectedPoints as float),0) as float),0) * 100 as Unreasonable,
    COALESCE(CAST( cast(NoncongruentPoints as float) / NULLIF(cast(expectedPoints as float),0) as float),0) * 100 as Noncongruent,
    COALESCE(CAST( cast(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints as float) / NULLIF(cast(expectedPoints as float),0) as float),0) * 100 as completeness
            
    from [dbo].[ChannelDataQualitySummary] 
    join [dbo].[Channel] on [dbo].[ChannelDataQualitySummary].[ChannelID] = [dbo].[Channel].[ID]
    join [dbo].[Meter] on [dbo].[Channel].[MeterID] = @MeterID
    join [dbo].[MeasurementType] on [dbo].[MeasurementType].[ID] = [dbo].[Channel].[MeasurementTypeID]
    join [dbo].[MeasurementCharacteristic] on [dbo].[MeasurementCharacteristic].[ID] = [dbo].[Channel].[MeasurementCharacteristicID]
    join [dbo].[Phase] on [dbo].[Phase].[ID] = [dbo].[Channel].[PhaseID]
    where [dbo].[ChannelDataQualitySummary].[Date] = @EventDate and [dbo].[Meter].[ID] = @MeterID

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Sep 1, 2015>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSiteChannelDataQualityDetailsByDate '09/01/2008', '1'

-- =============================================
CREATE PROCEDURE [dbo].[selectSiteChannelDataQualityDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    select
    Distinct [dbo].[Channel].[ID] as channelid,
    @EventDate as date,
    [dbo].[Channel].[Name] as channelname,
    [dbo].[Meter].[ID] as meterid,
    [dbo].[MeasurementType].[Name] as measurementtype,
    [dbo].[MeasurementCharacteristic].[Name] as characteristic,
    [dbo].[Phase].[Name] as phasename,
    [dbo].[ChannelDataQualitySummary].[ExpectedPoints] as ExpectedPoints,
    [dbo].[ChannelDataQualitySummary].[GoodPoints] as GoodPoints,
    [dbo].[ChannelDataQualitySummary].[LatchedPoints] as LatchedPoints,
    [dbo].[ChannelDataQualitySummary].[UnreasonablePoints] as UnreasonablePoints,
    [dbo].[ChannelDataQualitySummary].[NoncongruentPoints] as NoncongruentPoints,
    [dbo].[ChannelDataQualitySummary].[DuplicatePoints] as DuplicatePoints
    from [dbo].[ChannelDataQualitySummary] 
    join [dbo].[Channel] on [dbo].[ChannelDataQualitySummary].[ChannelID] = [dbo].[Channel].[ID]
    join [dbo].[Meter] on [dbo].[Channel].[MeterID] = @MeterID
    join [dbo].[MeasurementType] on [dbo].[MeasurementType].[ID] = [dbo].[Channel].[MeasurementTypeID]
    join [dbo].[MeasurementCharacteristic] on [dbo].[MeasurementCharacteristic].[ID] = [dbo].[Channel].[MeasurementCharacteristicID]
    join [dbo].[Phase] on [dbo].[Phase].[ID] = [dbo].[Channel].[PhaseID]
    where [dbo].[ChannelDataQualitySummary].[Date] = @EventDate and [dbo].[Meter].[ID] = @MeterID

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSiteLinesDetailsByDate '10/05/2014', '15'

-- =============================================
CREATE PROCEDURE [dbo].[selectSiteLinesDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
    [Event].[LineID] as thelineid, 
    [Event].[ID] as theeventid, 
    [EventType].Name as theeventtype,
    CAST(Coalesce(CAST([FaultSummary].[Inception] AS datetime2(7)), CAST([Event].[StartTime] AS datetime2(7))) as varchar(26)) as theinceptiontime,
    [MeterLine].[LineName] + ' ' + [Line].[AssetKey] as thelinename,
    [VoltageKV] as voltage,
    COALESCE([FaultSummary].[FaultType],'N/A') as thefaulttype,
    COALESCE([FaultSummary].[Distance],0) as thecurrentdistance,
    [dbo].[EventHasImpactedComponents]([Event].[ID]) as pqiexists,
    [dbo].[HasICFResult]([Event].[ID]) as easexists

    from Event 
    join [Line] on [Event].[LineID] = [Line].[ID]
    left outer join [FaultSummary] on [FaultSummary].[EventID] = [Event].[ID] and [IsSelectedAlgorithm] = 1
    join [Meter] on [Meter].[ID] = @MeterID
    join [MeterLine] on [MeterLine].MeterID = @MeterID and [MeterLine].LineID = [Line].[ID]
    join [EventType] on [Event].[EventTypeID] = [EventType].ID

    where Cast([Event].[StartTime] as Date) = @EventDate and [Event].[MeterID] = @MeterID
    order by [Event].[StartTime] ASC

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 7, 2015>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesBreakersDetailsByDate '03/30/2015', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,'

-- =============================================
CREATE PROCEDURE [dbo].[selectSitesBreakersDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

        SELECT 

        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Event].[ID] as theeventid, 
        [dbo].[EventType].[Name] as eventtype, 

        Cast(Cast([dbo].[BreakerOperation].[TripCoilEnergized] as Time) as nvarchar(100)) as energized,

        --[dbo].[BreakerOperation].[TripCoilEnergized] as energized,
        [dbo].[BreakerOperation].[BreakerNumber] as breakernumber,
        [dbo].[MeterLine].[LineName] as linename,
        [dbo].[Phase].[Name] as phasename,

        CAST([dbo].[BreakerOperation].[BreakerTiming] as decimal(16,5)) as timing,

        [dbo].[BreakerOperation].[BreakerSpeed] as speed,
        [dbo].[BreakerOperationType].[Name] as operationtype 
        
        FROM [dbo].[BreakerOperation] 

        join [dbo].[Event] on [BreakerOperation].[EventID] = [Event].[ID]
        join [dbo].[EventType] on [EventType].[ID] = [Event].[EventTypeID]
        join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[Event].[MeterID]
        join [dbo].[Line] on [dbo].[Line].[ID] = [dbo].[Event].[LineID]
        join [dbo].[MeterLine] on [dbo].[MeterLine].LineID = [dbo].[Event].[LineID] and [dbo].[MeterLine].MeterID = Meter.ID
        join [dbo].[BreakerOperationType] on [dbo].[BreakerOperation].[BreakerOperationTypeID] = [dbo].[BreakerOperationType].[ID]
        join [dbo].[Phase] on [BreakerOperation].[PhaseID] = [Phase].[ID]

        where [dbo].[Meter].[ID] in ( Select * from @MeterIDs)
        and CAST([TripCoilEnergized] as Date) = CAST(@EventDate as Date)
        

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesCompletenessDetailsByDate '07/19/2014', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,','jwalker'
-- selectSitesCompletenessDetailsByDate2 '09/03/2014', '108,109,86,87,110,118,13,17,167,168,77,78,79,80,70,46,169,185,186,40,6,170,88,89,52,192,15,16,91,92,93,94,95,96,97,28,98,99,100,101,102,171,57,172,125,173,63,19,104,174,55,105,106,107,103,111,112,113,60,53,160,114,115,116,117,68,12,58,54,8,59,18,20,9,193,119,120,81,164,121,175,156,157,176,177,11,122,123,21,65,1,41,39,23,51,127,128,129,130,165,76,131,132,133,134,135,136,137,14,124,45,47,73,64,2,138,139,140,141,142,143,82,83,62,50,144,145,56,30,42,146,147,148,149,150,151,32,152,126,74,67,10,66,22,178,179,29,180,48,181,153,154,155,194,24,43,34,4,69,37,158,26,182,36,159,161,162,163,71,166,25,31,44,49,72,61,75,187,188,189,190,191,3,84,85,7,195,90,183,27,196,184,35,33,197,198,199,200,201,38,5,','jwalker'
-- selectSitesCompletenessDetailsByDate '09/17/2008', '86', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectSitesCompletenessDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    declare  @TempTable TABLE ( themeterid int, thesite varchar(100), thecount float, thename varchar(100));

    insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT 

        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (
        Select  COALESCE((Select CAST( cast((GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) as float) / NULLIF(cast(expectedPoints as float),0) as float) as completenessPercentage from MeterDataQualitySummary
            where 
             MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate), 0)) as thecount,

        'Completeness' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]

        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]

        where [MeterID] in (Select * from @MeterIDs)
        
        and CAST([Date] as Date) = CAST(@EventDate as Date)

        insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT 

        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (
        Select  COALESCE((Select CAST( NULLIF(cast(expectedPoints as float),0) as float) as completenessPercentage from MeterDataQualitySummary
            where 
             MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate), 0)) as thecount,

        'Expected' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]

        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]

        where [MeterID] in (Select * from @MeterIDs)
        
        and CAST([Date] as Date) = CAST(@EventDate as Date)

        insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT 

        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (
        Select  COALESCE((Select CAST( cast((GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints + DuplicatePoints) as float) / NULLIF(cast(expectedPoints as float),0) as float) as completenessPercentage from MeterDataQualitySummary
            where 
             MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate), 0)) as thecount,

        'Received' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]

        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]

        where [MeterID] in (Select * from @MeterIDs)
        
        and CAST([Date] as Date) = CAST(@EventDate as Date)

                insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT 

        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (
        Select  COALESCE((Select CAST( cast((DuplicatePoints) as float) / NULLIF(cast(expectedPoints as float),0) as float) as completenessPercentage from MeterDataQualitySummary
            where 
             MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate), 0)) as thecount,

        'Duplicate' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]

        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]

        where [MeterID] in (Select * from @MeterIDs)
        
        and CAST([Date] as Date) = CAST(@EventDate as Date)

        --select * from @TempTable

    declare @composite TABLE (theeventid int, themeterid int, thesite varchar(100), Expected float, Received float, Duplicate float, Completeness float);

    DECLARE @sitename varchar(100)
    DECLARE @themeterid int
    DECLARE @theeventid int

    DECLARE site_cursor CURSOR FOR Select distinct (themeterid), thesite from @TempTable

    OPEN site_cursor

    FETCH NEXT FROM site_cursor into @themeterid , @sitename

    WHILE @@FETCH_STATUS = 0
    BEGIN
        insert @composite VALUES (

            (Select top 1 [dbo].[MeterDataQualitySummary].[ID] FROM [dbo].[MeterDataQualitySummary]

                
            where [dbo].[MeterDataQualitySummary].[MeterID] = @themeterid

            
            and CAST([Date] as Date) = CAST(@EventDate as Date)),

            
            @themeterid,
            @sitename,
            (Select thecount * 100 from @TempTable where thename = 'Expected' and thesite = @sitename ),
            (Select thecount * 100 from @TempTable where thename = 'Received' and thesite = @sitename ),
            (Select thecount * 100 from @TempTable where thename = 'Duplicate' and thesite = @sitename ),
            (Select thecount * 100 from @TempTable where thename = 'Completeness' and thesite = @sitename )

            )

    FETCH NEXT FROM site_cursor into @themeterid , @sitename
    END
 
    CLOSE site_cursor;
    DEALLOCATE site_cursor;

    select * from @composite
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesCorrectnessDetailsByDate '07/19/2014', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,','jwalker'
-- selectSitesCorrectnessDetailsByDate '09/03/2007', '108,109,86,87,110,118,13,17,167,168,77,78,79,80,70,46,169,185,186,40,6,170,88,89,52,192,15,16,91,92,93,94,95,96,97,28,98,99,100,101,102,171,57,172,125,173,63,19,104,174,55,105,106,107,103,111,112,113,60,53,160,114,115,116,117,68,12,58,54,8,59,18,20,9,193,119,120,81,164,121,175,156,157,176,177,11,122,123,21,65,1,41,39,23,51,127,128,129,130,165,76,131,132,133,134,135,136,137,14,124,45,47,73,64,2,138,139,140,141,142,143,82,83,62,50,144,145,56,30,42,146,147,148,149,150,151,32,152,126,74,67,10,66,22,178,179,29,180,48,181,153,154,155,194,24,43,34,4,69,37,158,26,182,36,159,161,162,163,71,166,25,31,44,49,72,61,75,187,188,189,190,191,3,84,85,7,195,90,183,27,196,184,35,33,197,198,199,200,201,38,5,','jwalker'

-- =============================================
CREATE PROCEDURE [dbo].[selectSitesCorrectnessDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    declare  @TempTable TABLE ( themeterid int, thesite varchar(100), thecount float, thename varchar(100));

    insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT
        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (Select 
        

        COALESCE(Round(CAST(
        sum(LatchedPoints)
        as float) / NULLIF(cast(sum(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) as float),0) * 100 , 0),0)
        
        as correctnessPercentage from MeterDataQualitySummary
            where MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate) as thecount,

        'Latched' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]
        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]
        where [MeterID] in (Select * from @MeterIDs)
        and CAST([Date] as Date) = CAST(@EventDate as Date)

    insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT
        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (Select 

        COALESCE(Round(CAST(
        sum(UnreasonablePoints)
        as float) / NULLIF(cast(sum(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) as float),0) * 100 , 0),0)

        
        as correctnessPercentage from MeterDataQualitySummary
            where MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate) as thecount,

        'Unreasonable' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]
        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]
        where [MeterID] in (Select * from @MeterIDs)
        and CAST([Date] as Date) = CAST(@EventDate as Date)

    insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT
        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 

        (Select 
        
        COALESCE(Round(CAST(
        sum(NoncongruentPoints)
        as float) / NULLIF(cast(sum(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) as float),0) * 100 , 0),0)

        as correctnessPercentage from MeterDataQualitySummary
            where MeterID = [dbo].[Meter].[ID]
            and [Date] = @eventDate) as thecount,

        'Noncongruent' as thename 
        
        FROM [dbo].[MeterDataQualitySummary]
        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[MeterDataQualitySummary].[MeterID]
        where [MeterID] in (Select * from @MeterIDs)
        and CAST([Date] as Date) = CAST(@EventDate as Date)

        --select * from @TempTable

    declare @composite TABLE (theeventid int, themeterid int, thesite varchar(100), Latched float, Unreasonable float , Noncongruent float);

    DECLARE @sitename varchar(100)
    DECLARE @themeterid int
    DECLARE @theeventid int

    DECLARE site_cursor CURSOR FOR Select distinct (themeterid), thesite from @TempTable

    OPEN site_cursor

    FETCH NEXT FROM site_cursor into @themeterid , @sitename

    WHILE @@FETCH_STATUS = 0
    BEGIN
        insert @composite VALUES (

            (Select top 1 [dbo].[MeterDataQualitySummary].[ID] FROM [dbo].[MeterDataQualitySummary]
            where [dbo].[MeterDataQualitySummary].[MeterID] = @themeterid
            and CAST([Date] as Date) = CAST(@EventDate as Date)),
            @themeterid,
            @sitename,
            (Select thecount from @TempTable where thename = 'Latched' and thesite = @sitename ),
            (Select thecount from @TempTable where thename = 'Unreasonable' and thesite = @sitename ),
            (Select thecount from @TempTable where thename = 'Noncongruent' and thesite = @sitename )
            )

    FETCH NEXT FROM site_cursor into @themeterid , @sitename
    END
 
    CLOSE site_cursor;
    DEALLOCATE site_cursor;

    select * from @composite
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesEventsDetailsByDate '07/19/2014', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,','jwalker'
-- selectSitesEventsDetailsByDate '07/19/2014', '0','jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectSitesEventsDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    declare  @TempTable TABLE ( themeterid int, thesite varchar(100), thecount int, thename varchar(100));

    insert into @TempTable ( themeterid, thesite , thecount , thename )

        SELECT 

        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite, 
        [dbo].[Event].[EventTypeID] as thecount, 
        [dbo].[EventType].[Name] as thename 
        
        FROM [dbo].[Event]

        inner join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID]    
        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[Event].[MeterID]

        where [MeterID] in (Select * from @MeterIDs)
        
        and CAST([StartTime] as Date) = CAST(@EventDate as Date)

        --select * from @TempTable

    declare @composite TABLE (theeventid int, themeterid int, thesite varchar(100), faults int, interruptions int, sags int, swells int, others int );

    DECLARE @sitename varchar(100)
    DECLARE @themeterid int
    DECLARE @theeventid int

    DECLARE site_cursor CURSOR FOR Select distinct (themeterid), thesite from @TempTable

    OPEN site_cursor

    FETCH NEXT FROM site_cursor into @themeterid , @sitename

    WHILE @@FETCH_STATUS = 0
    BEGIN
        insert @composite VALUES (

            (Select top 1 [dbo].[Event].[ID] FROM [dbo].[Event]
            where [dbo].[Event].[MeterID] = @themeterid
            and CAST([StartTime] as Date) = CAST(@EventDate as Date)),
            @themeterid,
            @sitename,
            (Select count(thecount) from @TempTable where thename = 'Fault' and thesite = @sitename ),
            (Select count(thecount) from @TempTable where thename = 'Interruption' and thesite = @sitename ),
            (Select count(thecount) from @TempTable where thename = 'Sag' and thesite = @sitename ),
            (Select count(thecount) from @TempTable where thename = 'Swell' and thesite = @sitename ),
            (Select count(thecount) from @TempTable where thename = 'Other' and thesite = @sitename )
            )

    FETCH NEXT FROM site_cursor into @themeterid , @sitename
    END
 
    CLOSE site_cursor;
    DEALLOCATE site_cursor;

    select * from @composite
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date Range>
-- =============================================
CREATE PROCEDURE [dbo].[selectSitesEventsDetailsByDateRange]
    @EventDateFrom as DateTime,
    @EventDateTo as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    SELECT 

    CAST(CAST([dbo].[Event].[StartTime] AS datetime2(7))as varchar(26)) as starttime,
    --CAST(Coalesce(CAST([FaultSummary].[Inception] AS datetime2(7)), CAST([Event].[StartTime] AS datetime2(7))) as varchar(26)) as inceptiontime,
    CAST(CAST([dbo].[Event].[EndTime] AS datetime2(7))as varchar(26)) as endtime,

        [dbo].[Event].[ID] as eventid,
        [dbo].[Meter].[ID] as meterid, 
        [dbo].[Meter].[Name] as thesite,
        [dbo].[EventType].[Name] as thename 
        
        FROM [dbo].[Event]
        --left outer join [dbo].[FaultSummary] on [dbo].[Event].[ID] = [dbo].[FaultSummary].[EventID] 
        inner join [dbo].[EventType] on [dbo].[EventType].[ID] = [dbo].[Event].[EventTypeID]    
        inner join [dbo].[Meter] on [dbo].[Meter].[ID] = [dbo].[Event].[MeterID]

        where [MeterID] in (Select * from @MeterIDs)
        
        and CAST([StartTime] as Date) between CAST(@EventDateFrom as Date) and CAST(@EventDateTo as Date)

        order by [StartTime]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Mar 06, 2015>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesFaultsDetailsByDate '09/07/2014', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,'
-- =============================================
CREATE PROCEDURE [dbo].[selectSitesFaultsDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare  @MeterIDs TABLE (ID int);
    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));
    declare @TheEventDate as Date = CAST(@EventDate as Date)

    ; WITH FaultDetail AS
    (
        SELECT
            Meter.Name AS thesite,
            Meter.ShortName AS theshortsite,
            MeterLocation.ShortName AS locationname,
            Meter.ID AS themeterid,
            Line.ID AS thelineid,
            Event.ID AS theeventid,
            MeterLine.LineName AS thelinename,
            Line.VoltageKV AS voltage,
            CAST(CAST(Event.StartTime AS TIME) AS NVARCHAR(100)) AS theinceptiontime,
            FaultSummary.FaultType AS thefaulttype,
            CASE WHEN FaultSummary.Distance= '-1E308' THEN 'NaN' ELSE CAST(CAST(FaultSummary.Distance AS DECIMAL(16,2)) AS NVARCHAR(19)) END AS thecurrentdistance,
            ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY FaultSummary.IsSuppressed, FaultSummary.IsSelectedAlgorithm DESC) AS rk
        FROM
            FaultSummary JOIN
            Event ON FaultSummary.EventID = Event.ID JOIN
            EventType ON Event.EventTypeID = EventType.ID JOIN
            Meter ON Event.MeterID = Meter.ID JOIN
            MeterLocation ON Meter.MeterLocationID = MeterLocation.ID JOIN
            Line ON Event.LineID = Line.ID JOIN
            MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Line.ID
        WHERE
            EventType.Name = 'Fault' AND
            CAST(Event.StartTime AS DATE) = @TheEventDate AND
            Meter.ID IN (SELECT * FROM @MeterIDs)
    )
    SELECT *
    FROM FaultDetail
    WHERE rk = 1
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 29, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesTrendingDetailsByDate '04/08/2008', '52','jwalker'

-- =============================================
CREATE PROCEDURE [dbo].[selectSitesTrendingDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    declare @theDate as Date
    declare  @MeterIDs TABLE (ID int);

    set @theDate = CAST(@EventDate as Date)

    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

        Select 
        Meter.ID as meterid, 
        Channel.ID as channelid, 
        Meter.Name as sitename, 
        [dbo].[AlarmType].[Name] as eventtype, 
        [dbo].[MeasurementCharacteristic].[Name] as characteristic,
        [dbo].[MeasurementType].[Name] as measurementtype,
        [dbo].[Phase].[Name] as phasename,
        count (AlarmLog.ID) as eventcount,
        @theDate as date
        
        from Channel
        
        join AlarmLog on AlarmLog.ChannelID = Channel.ID and CAST(AlarmLog.Time as Date) = @theDate
        join Meter on Channel.MeterID = Meter.ID and [MeterID] in ( Select * from @MeterIDs)
        join [dbo].[AlarmType] on 
            [dbo].[AlarmType].[ID] = [dbo].[AlarmLog].[AlarmTypeID] and
            ([dbo].[AlarmType].[Name] = 'OffNormal' or [dbo].[AlarmType].[Name] = 'Alarm')

        join [dbo].[MeasurementCharacteristic] on Channel.MeasurementCharacteristicID = [dbo].[MeasurementCharacteristic].[ID]
        join [dbo].[MeasurementType] on Channel.MeasurementTypeID =  [dbo].[MeasurementType].ID
        join [dbo].[Phase] on Channel.PhaseID = [dbo].[Phase].ID

        Group By Meter.ID , Channel.ID , Meter.Name , [dbo].[AlarmType].[Name], [MeasurementCharacteristic].[Name] , [MeasurementType].[Name] , [dbo].[Phase].[Name]
        Order By Meter.ID

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2015>
-- Description: <Description, Selects Trending Data for a MeterID by Date, MeasurementCharacteristic, MeqasurementType, and Phase for a day>
-- selectTrendingData2 '03/22/2008', 1 , 20 , 1 , 3

-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingData]
    -- Add the parameters for the stored procedure here
    @EventDate DateTime2,
    @MeterID int,
    @MeasurementCharacteristicID int,
    @MeasurementTypeID int,
    @PhaseID int

AS
BEGIN

    SET NOCOUNT ON;

    Declare @ChannelID as int

    Select @ChannelID = Channel.ID from Channel where MeterID = @MeterID and MeasurementCharacteristicID = @MeasurementCharacteristicID and [MeasurementTypeID] = @MeasurementTypeID and [PhaseID] = @PhaseID;

    exec selectTrendingDataByChannelIDDate @EventDate, @ChannelID;

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects Trending Data for a ChannelID by Date for a day>
-- selectTrendingDataByChannelIDDate '9/15/2015', 988

-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingDataByChannelIDDate]
    -- Add the parameters for the stored procedure here
    @EventDate DateTime2,
    @ChannelID int
AS
BEGIN

    SET NOCOUNT ON;

WITH TrendingDataPoint AS
(
    SELECT
        ChannelID,
        SeriesType,
        Time,
        Value
    FROM
        GetTrendingData(@EventDate, DATEADD(NANOSECOND, -100, DATEADD(DAY, 1, @EventDate)), @ChannelID, default) AS TrendingData JOIN
        (
            SELECT 0 AS ID, 'Minimum' AS SeriesType UNION
            SELECT 1 AS ID, 'Maximum' AS SeriesType UNION
            SELECT 2 AS ID, 'Average' AS SeriesType
        ) AS Series ON TrendingData.SeriesID = Series.ID
),
TrendingData AS
(
    SELECT
        ChannelID,
        Time,
        Maximum,
        Minimum,
        Average
    FROM TrendingDataPoint
    PIVOT
    (
        SUM(Value)
        FOR SeriesType IN (Maximum, Minimum, Average)
    ) AS TrendingData
)
SELECT
[TrendingData].[Time] as thedate,
[TrendingData].[Minimum] as theminimum,
[TrendingData].[Maximum] as themaximum,
[TrendingData].[Average] as theaverage,
Coalesce([dbo].[AlarmRangeLimit].[High],0) as alarmlimithigh,
Coalesce([dbo].[AlarmRangeLimit].[Low],0) as alarmlimitlow,

Coalesce((Select [dbo].[HourOfWeekLimit].[High] from [dbo].[HourOfWeekLimit] where [dbo].[HourOfWeekLimit].[HourOfWeek] = 
(select (24 * ((DatePart(dw,[TrendingData].[Time])-1))) + DatePart(hh,[TrendingData].[Time]))
and [TrendingData].[ChannelID] = [dbo].[HourOfWeekLimit].[ChannelID]
),0) as offlimithigh,

 Coalesce((Select[dbo].[HourOfWeekLimit].[Low] from [dbo].[HourOfWeekLimit] where [dbo].[HourOfWeekLimit].[HourOfWeek] = 
(select (24 * ((DatePart(dw,[TrendingData].[Time])-1))) + DatePart(hh,[TrendingData].[Time]))
and [TrendingData].[ChannelID] = [dbo].[HourOfWeekLimit].[ChannelID]
),0) as offlimitlow

FROM [TrendingData] 
  left outer join [dbo].[AlarmRangeLimit] on [dbo].[AlarmRangeLimit].[ChannelID] = [TrendingData].[ChannelID]
  left outer join [dbo].[HourOfWeekLimit] on [dbo].[HourOfWeekLimit].[ChannelID] = [TrendingData].[ChannelID]

  where 
      [TrendingData].[ChannelID] = @ChannelID 
      and
      ([dbo].[HourOfWeekLimit].[HourOfWeek] = 0 or [dbo].[HourOfWeekLimit].[HourOfWeek] is null)
    order by [TrendingData].[Time]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects Trending Data for a MeterID by Date, MeasurementCharacteristic, MeqasurementType, and Phase for 30 days back>
-- selectTrendingDataMonthly '03/22/2008', 1 , 20 , 1 , 3

-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingDataMonthly]
    -- Add the parameters for the stored procedure here
    @EventDate DateTime,
    @MeterID int,
    @MeasurementCharacteristicID int,
    @MeasurementTypeID int,
    @PhaseID int

AS
BEGIN

    SET NOCOUNT ON;

SELECT
[dbo].[DailyTrendingSummary].[Date] as thedate,
[dbo].[DailyTrendingSummary].[Minimum] as theminimum,
[dbo].[DailyTrendingSummary].[Maximum] as themaximum,
[dbo].[DailyTrendingSummary].[Average] as theaverage,
[dbo].[AlarmRangeLimit].[High] as alarmlimithigh,
[dbo].[AlarmRangeLimit].[Low] as alarmlimitlow,

(Select [dbo].[HourOfWeekLimit].[High] from [dbo].[HourOfWeekLimit] where [dbo].[HourOfWeekLimit].[HourOfWeek] = 0
and [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[HourOfWeekLimit].[ChannelID]) as offlimithigh,

(Select [dbo].[HourOfWeekLimit].[Low] from [dbo].[HourOfWeekLimit] where [dbo].[HourOfWeekLimit].[HourOfWeek] = 0
and [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[HourOfWeekLimit].[ChannelID]) as offlimitlow

FROM [dbo].[DailyTrendingSummary] 
    join [dbo].[Channel] on [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[Channel].[ID]
    join [dbo].[Meter] on [dbo].[Channel].[MeterID] = [dbo].[Meter].[ID]
    join [dbo].[MeasurementType] on [dbo].[Channel].[MeasurementTypeID] = [dbo].[MeasurementType].[ID]
    join [dbo].[MeasurementCharacteristic] on [dbo].[Channel].[MeasurementCharacteristicID] = [dbo].[MeasurementCharacteristic].[ID]
    join [dbo].[Phase] on [dbo].[Channel].[PhaseID] = [dbo].[Phase].[ID]
    join [dbo].[MeterLocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID]
    join [dbo].[AlarmRangeLimit] on [dbo].[AlarmRangeLimit].[ChannelID] = [dbo].[Channel].[ID]
    join [dbo].[HourOfWeekLimit] on [dbo].[HourOfWeekLimit].[ChannelID] = [dbo].[Channel].[ID]

  where 
      [dbo].[Meter].[ID] = @MeterID and 
      [DailyTrendingSummary].[Date] between DATEADD( month , -1 , @EventDate) and @EventDate and
      [dbo].[MeasurementCharacteristic].[ID] = @MeasurementCharacteristicID and
      [dbo].[MeasurementType].[ID] = @MeasurementTypeID and
      [dbo].[Phase].[ID] = @PhaseID and
      [dbo].[HourOfWeekLimit].[HourOfWeek] = 0

    order by [dbo].[DailyTrendingSummary].[Date]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2014>
-- Description: <Description, Selects Trending Data for a MeterID by Date, MeasurementCharacteristic, MeqasurementType, and Phase for 30 days back>
-- [selectTrendingDataWeekly] '03/22/2008', 1 , 20 , 1 , 3

-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingDataWeekly]
    -- Add the parameters for the stored procedure here
    @EventDate DateTime,
    @MeterID int,
    @MeasurementCharacteristicID int,
    @MeasurementTypeID int,
    @PhaseID int

AS
BEGIN

    SET NOCOUNT ON;

SELECT
[dbo].[DailyTrendingSummary].[Date] as thedate,
[dbo].[DailyTrendingSummary].[Minimum] as theminimum,
[dbo].[DailyTrendingSummary].[Maximum] as themaximum,
[dbo].[DailyTrendingSummary].[Average] as theaverage,
[dbo].[AlarmRangeLimit].[High] as alarmlimithigh,
[dbo].[AlarmRangeLimit].[Low] as alarmlimitlow,

(Select [dbo].[HourOfWeekLimit].[High] from [dbo].[HourOfWeekLimit] where [dbo].[HourOfWeekLimit].[HourOfWeek] = 0
and [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[HourOfWeekLimit].[ChannelID]) as offlimithigh,

(Select [dbo].[HourOfWeekLimit].[Low] from [dbo].[HourOfWeekLimit] where [dbo].[HourOfWeekLimit].[HourOfWeek] = 0
and [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[HourOfWeekLimit].[ChannelID]) as offlimitlow

FROM [dbo].[DailyTrendingSummary] 
    join [dbo].[Channel] on [dbo].[DailyTrendingSummary].[ChannelID] = [dbo].[Channel].[ID]
    join [dbo].[Meter] on [dbo].[Channel].[MeterID] = [dbo].[Meter].[ID]
    join [dbo].[MeasurementType] on [dbo].[Channel].[MeasurementTypeID] = [dbo].[MeasurementType].[ID]
    join [dbo].[MeasurementCharacteristic] on [dbo].[Channel].[MeasurementCharacteristicID] = [dbo].[MeasurementCharacteristic].[ID]
    join [dbo].[Phase] on [dbo].[Channel].[PhaseID] = [dbo].[Phase].[ID]
    join [dbo].[MeterLocation] on [dbo].[Meter].[MeterLocationID] = [dbo].[MeterLocation].[ID]
    join [dbo].[AlarmRangeLimit] on [dbo].[AlarmRangeLimit].[ChannelID] = [dbo].[Channel].[ID]
    join [dbo].[HourOfWeekLimit] on [dbo].[HourOfWeekLimit].[ChannelID] = [dbo].[Channel].[ID]

  where 
      [dbo].[Meter].[ID] = @MeterID and 
      [DailyTrendingSummary].[Date] between DATEADD( week , -1 , @EventDate) and @EventDate and
      [dbo].[MeasurementCharacteristic].[ID] = @MeasurementCharacteristicID and
      [dbo].[MeasurementType].[ID] = @MeasurementTypeID and
      [dbo].[Phase].[ID] = @PhaseID and
      [dbo].[HourOfWeekLimit].[HourOfWeek] = 0

    order by [dbo].[DailyTrendingSummary].[Date]

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects All Trending in the database>
-- selectTrendingForCalendar 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingForCalendar]
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

DECLARE @counter INT = 0
DECLARE @eventDate DATE = (Select max(CAST(Time AS Date)) from AlarmLog)
DECLARE @numberOfDays INT = DATEDIFF ( day , (Select min(CAST(Time AS Date)) from AlarmLog), @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Date DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT 
Date as thedate, 
Offnormal as offnormal, 
Alarm as alarm
FROM
(
    SELECT #temp.Date, AlarmType.Name AS AlarmTypeName, COALESCE(AlarmCount, 0) AS AlarmCount
    FROM
        #temp CROSS JOIN
        AlarmType LEFT OUTER JOIN
        (
            SELECT CAST(Time AS Date) AS AlarmDate, AlarmTypeID, COUNT(*) AS AlarmCount
            FROM AlarmLog
            join Channel on AlarmLog.ChannelID = Channel.ID and Channel.MeterID in (select * from authMeters(@username))
            GROUP BY CAST(Time AS Date), AlarmTypeID
        ) AS Alarm ON #temp.Date = Alarm.AlarmDate AND AlarmType.ID = Alarm.AlarmTypeID
) AS AlarmDate
PIVOT
(
    SUM(AlarmCount)
    FOR AlarmDate.AlarmTypeName IN (Offnormal,Alarm)
) as pvt
ORDER BY Date

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- [selectTrendingForMeterIDByDateRange] '03/12/2008', '03/12/2014', '52', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingForMeterIDByDateRange]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp(Thedate DATE)

WHILE (@counter <= @numberOfDays)
BEGIN
    INSERT INTO #temp VALUES(@eventDate)
    SET @eventDate = DATEADD(DAY, 1, @eventDate)
    SET @counter = @counter + 1
END

SELECT 
Thedate as thedate, 
COALESCE(OffNormal,0) as offnormal, 
COALESCE(Alarm,0) as alarm
FROM
(
    SELECT #temp.Thedate, AlarmType.Name AS AlarmTypeName, COALESCE(AlarmCount, 0) AS AlarmCount
    FROM
        #temp CROSS JOIN
        AlarmType LEFT OUTER JOIN
        (
            SELECT CAST(Time AS Date) AS AlarmDate, AlarmTypeID, COUNT(*) AS AlarmCount
            FROM AlarmLog join Channel on AlarmLog.ChannelID = Channel.ID
            where MeterID in ( Select * from @MeterIDs)
            GROUP BY CAST(Time AS Date), AlarmTypeID
        ) AS Alarm ON #temp.Thedate = AlarmDate AND [AlarmType].[ID] = AlarmTypeID
) AS AlarmDate
PIVOT
(
    SUM(AlarmCount)
    FOR AlarmDate.AlarmTypeName IN (alarm, offnormal )
) as pvt
ORDER BY thedate

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, June 19, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectTrendingForMeterIDsByDate '05/20/2008', '06/19/2015', '32,33,10,11,34,42,91,92,1,2,3,4,93,109,110,94,12,13,116,15,16,17,18,19,20,21,22,23,24,25,26,95,96,49,97,28,98,29,30,31,27,35,36,37,84,38,39,40,41,117,43,44,5,88,45,99,80,81,100,101,46,47,51,52,53,54,89,55,56,57,58,59,60,61,48,62,63,64,65,66,67,6,7,68,69,70,71,72,73,74,75,76,50,102,103,104,105,77,78,79,118,82,106,83,85,86,87,90,111,112,113,114,115,8,9,119,14,107,120,108,121,122,123,124,125,', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingForMeterIDsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)

AS
BEGIN

    SET NOCOUNT ON;

--  declare @tablecount int = (Select count(*) from AlarmLog);

--if ( @tablecount = 0 )
--Begin
--return;
--End

declare  @MeterIDs TABLE (ID int);
INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

DECLARE @counter INT = 0
DECLARE @eventDate DATE = CAST(@EventDateTo AS Date)
DECLARE @numberOfDays INT = DATEDIFF ( day , CAST(@EventDateFrom AS Date) , @eventDate)

SET @eventDate = DATEADD(DAY, -@numberOfDays, @eventDate)

CREATE TABLE #temp (thesiteid int, thesitename varchar(100))

INSERT INTO #temp Select [dbo].[Meter].[ID], [dbo].[Meter].[Name] from [dbo].[Meter] where [dbo].[Meter].[ID] in (Select * from @MeterIDs)

SELECT 
thesiteid as siteid, 
thesitename as sitename ,
COALESCE(Offnormal,0) as offnormal, 
COALESCE(Alarm,0) as alarm

FROM
(
    SELECT #temp.thesiteid, #temp.thesitename , AlarmType.Name AS AlarmTypeName, COALESCE(AlarmCount, 0) AS AlarmCount
    FROM
        #temp CROSS JOIN
        AlarmType LEFT OUTER JOIN
        (
            SELECT MeterID, AlarmTypeID, COUNT(*) AS AlarmCount FROM AlarmLog
            join Channel on AlarmLog.ChannelID = Channel.ID
            join Meter on Channel.MeterID = Meter.ID
            where MeterID in (Select * from @MeterIDs)
            and (CAST([Time] as Date) between @EventDateFrom and @EventDateTo)
            GROUP BY AlarmTypeID, MeterID
        ) AS E ON AlarmType.ID = E.AlarmTypeID and E.MeterID = #temp.thesiteid
) AS AlarmDate
PIVOT
(
    SUM(AlarmCount)
    FOR AlarmDate.AlarmTypeName IN ( Alarm, Offnormal)
) as pvt
ORDER BY sitename asc

DROP TABLE #temp

END
GO

CREATE FUNCTION [dbo].[authMeters](@username varchar(100))
RETURNS @authMeters TABLE 
(
    ID int 
)
AS 
BEGIN

insert into @authMeters Select distinct [dbo].[Meter].[ID] 
from [dbo].[Meter]
join [dbo].[GroupMeter] on [dbo].[GroupMeter].[meterID] = [dbo].[Meter].[ID]
join [dbo].[UserGroup] on [dbo].[UserGroup].[groupID] = [dbo].[GroupMeter].[groupID]
join dbo.[User] on  UserGroup.userID = dbo.[User].ID
where dbo.[User].Name = @username

return;

END
GO

CREATE FUNCTION [dbo].[String_To_Int_Table]
(
         @list NVARCHAR(MAX)
       , @delimiter NCHAR(1) = ',' --Defaults to CSV
)
RETURNS
    @tableList TABLE(
       value INT
       )
AS

BEGIN
   DECLARE @value NVARCHAR(11)
   DECLARE @position INT

   SET @list = LTRIM(RTRIM(@list))+ ','
   set @list = REPLACE(@list, ',,', ',')
   SET @position = CHARINDEX(@delimiter, @list, 1)

   IF REPLACE(@list, @delimiter, '') <> ''
   BEGIN
          WHILE @position > 0
          BEGIN 
                 SET @value = LTRIM(RTRIM(LEFT(@list, @position - 1)));
                 INSERT INTO @tableList (value)
                 VALUES (cast(@value as int));
                 SET @list = RIGHT(@list, LEN(@list) - @position);
                 SET @position = CHARINDEX(@delimiter, @list, 1);

          END
   END   
   RETURN
END
GO

