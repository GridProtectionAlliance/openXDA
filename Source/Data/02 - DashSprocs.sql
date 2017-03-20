CREATE TYPE [dbo].[SiteLineDetailsByDate] AS TABLE(
    [thelineid] [int] NULL,
    [theeventid] [int] NULL,
    [theeventtype] [varchar](200) NULL,
    [theinceptiontime] [varchar](26) NULL,
    [thelinename] [varchar](251) NULL,
    [voltage] [float] NULL,
    [thefaulttype] [varchar](200) NULL,
    [thecurrentdistance] [nvarchar](22) NULL,
    [pqiexists] [int] NULL,
	[UpdatedBy] [varchar](200) NULL
)
GO

CREATE PROCEDURE [dbo].[GetPreviousAndNextEventIds]
    @EventID as INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @currentTime DATETIME2,
            @meterID INT,
            @lineID INT

    SELECT @currentTime = StartTime, @meterID = MeterID, @lineID = LineID
    FROM Event
    WHERE ID = @EventID

    SELECT evt2.ID as previd, evt3.ID as nextid
    FROM Event evt1 LEFT OUTER JOIN 
         Event evt2 ON evt2.StartTime = (SELECT MAX(StartTime)
         FROM Event
         WHERE StartTime < @currentTime AND MeterID = @meterID AND LineID = @lineID) AND evt2.MeterID = @meterID AND evt2.LineID = @lineID 
         LEFT OUTER JOIN 
         Event evt3 ON evt3.StartTime = (SELECT MIN(StartTime)
         FROM Event
         WHERE StartTime > @currentTime AND MeterID = @meterID AND LineID = @lineID) 
         AND evt3.MeterID = @meterID AND evt3.LineID = @lineID
    WHERE evt1.ID = @EventID
END
GO

CREATE PROCEDURE [dbo].[GetPreviousAndNextEventIdsNonMeter]
    @EventID as INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @currentTime DATETIME2

	SELECT @currentTime = StartTime
	FROM Event
	WHERE ID = @EventID

	SELECT evt2.ID as previd, evt3.ID as nextid
	FROM Event evt1 LEFT OUTER JOIN 
		 Event evt2 ON evt2.StartTime = 
			 (SELECT MAX(StartTime)
			 FROM Event
			 WHERE StartTime < @currentTime ) LEFT OUTER JOIN 
		 Event evt3 ON evt3.StartTime = (SELECT MIN(StartTime)
			 FROM Event
			 WHERE StartTime > @currentTime )
	WHERE evt1.ID = @EventID
END
GO

-- =============================================
-- Author:      <Author, William Ernest>
-- Create date: <Create Date, Aug 2, 2016>
-- Description: <Description, Selects Trending Data for a ChannelID by Date for a day for purpose of creating smart alarms in openXDA/Historian>
-- [dbo].[selectAlarmDataByChannelByDate] '05/28/2008', '05/29/2008', 8026 
-- =============================================
CREATE PROCEDURE [dbo].[selectAlarmDataByChannelByDate]
    @StartDate DateTime2,
	@EndDate DateTime2,
    @ChannelID int
AS
BEGIN
    SET NOCOUNT ON;

    -- Trending Data
    WITH TrendingData AS
    (
        SELECT
           *
        FROM
            GetTypedTrendingData(@StartDate, @EndDate, @ChannelID, default) AS TrendingData
    )
    SELECT
		*
    FROM
        TrendingData
    WHERE
        TrendingData.ChannelID = @ChannelID
    ORDER BY TrendingData.Time
END
GO

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

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

DECLARE @PivotColumns NVARCHAR(MAX) = N''
DECLARE @ReturnColumns NVARCHAR(MAX) = N''
DECLARE @SQLStatement NVARCHAR(MAX) = N''

create table #TEMP (Name varchar(max))
insert into #TEMP SELECT Name FROM (Select Distinct Name FROM BreakerOperationType) as t

SELECT @PivotColumns = @PivotColumns + '[' + COALESCE(CAST(Name as varchar(max)), '') + '],' 
FROM #TEMP ORDER BY Name desc

SELECT @ReturnColumns = @ReturnColumns + ' COALESCE([' + COALESCE(CAST(Name as varchar(max)), '') + '], 0) AS [' + COALESCE(CAST(Name as varchar(max)), '') + '],' 
FROM #TEMP ORDER BY Name desc

DROP TABLE #TEMP

SET @SQLStatement =
' SELECT Date as thedate, ' + SUBSTRING(@ReturnColumns,0, LEN(@ReturnColumns)) +
' FROM (																									 ' +	
'	SELECT CAST(TripCoilEnergized AS Date) AS Date,                                                          ' +
'		   BreakerOperationType.Name, 																		 ' +
'		   COUNT(*) AS thecount																				 ' +
'	FROM BreakerOperation JOIN																				 ' +
'		 BreakerOperationType ON BreakerOperation.BreakerOperationTypeID = BreakerOperationType.ID JOIN		 ' +
'		 Event ON Event.ID = BreakerOperation.EventID														 ' +
'	WHERE MeterID in (select * from authMeters(@username)) AND 												 ' +
'		  MeterID IN (SELECT * FROM String_To_Int_Table(@MeterID, '','')) AND 								 ' +
'		  TripCoilEnergized >= @startDate AND TripCoilEnergized < @endDate									 ' +
'	GROUP BY CAST(TripCoilEnergized AS Date), BreakerOperationType.Name										 ' +
') as table1																								 ' +
' PIVOT(																									 ' +
'		SUM(table1.thecount)																				 ' +
'		FOR table1.Name IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ')							 ' +
' ) as pvt																									 ' +
' ORDER BY Date                                                                                   '

exec sp_executesql @SQLStatement, N'@username nvarchar(4000), @MeterID nvarchar(MAX), @startDate DATE, @endDate DATE ', @username = @username, @MeterID = @MeterID, @startDate = @startDate, @endDate = @endDate

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

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

SELECT	Date, COALESCE(First, 0) AS First, COALESCE(Second, 0) AS Second, COALESCE(Third, 0) AS Third, COALESCE(Fourth, 0) AS Fourth, COALESCE(Fifth, 0) AS Fifth, COALESCE(Sixth, 0) AS Sixth
FROM
    (
        SELECT Date, CompletenessLevel, COUNT(*) AS MeterCount
        FROM
        (
            SELECT Date,
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
                WHERE Date BETWEEN @startDate AND @endDate AND MeterID IN (SELECT * FROM String_To_Int_Table(@MeterID, ',')) AND MeterID IN (SELECT * FROM authMeters(@username))
            ) MeterDataQualitySummary
        ) MeterDataQualitySummary
        GROUP BY Date, CompletenessLevel
) MeterDataQualitySummary
PIVOT
(
    SUM(MeterDataQualitySummary.MeterCount)
    FOR MeterDataQualitySummary.CompletenessLevel IN (First, Second, Third, Fourth, Fifth, Sixth)
) as pvt
ORDER BY Date

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

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

SELECT	Date, COALESCE(First, 0) AS First, COALESCE(Second, 0) AS Second, COALESCE(Third, 0) AS Third, COALESCE(Fourth, 0) AS Fourth, COALESCE(Fifth, 0) AS Fifth, COALESCE(Sixth, 0) AS Sixth
FROM
    (
        SELECT Date, CompletenessLevel, COUNT(*) AS MeterCount
        FROM
        (
            SELECT Date,
					CASE
						WHEN Correctness >= 100.0 THEN 'First'
						WHEN 98.0 <= Correctness AND Correctness < 100.0 THEN 'Second'
						WHEN 90.0 <= Correctness AND Correctness < 98.0 THEN 'Third'
						WHEN 70.0 <= Correctness AND Correctness < 90.0 THEN 'Fourth'
						WHEN 50.0 <= Correctness AND Correctness < 70.0 THEN 'Fifth'
						WHEN 0.0 < Correctness AND Correctness < 50.0 THEN 'Sixth'
					END AS CompletenessLevel
            FROM
            (
				SELECT Date, 100.0 * CAST(GoodPoints AS FLOAT) / CAST(NULLIF(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints, 0) AS FLOAT) AS Correctness                
				FROM MeterDataQualitySummary
                WHERE Date BETWEEN @startDate AND @endDate AND MeterID IN (SELECT * FROM String_To_Int_Table(@MeterID, ',')) AND MeterID IN (SELECT * FROM authMeters(@username))
            ) MeterDataQualitySummary
        ) MeterDataQualitySummary
        GROUP BY Date, CompletenessLevel
) MeterDataQualitySummary
PIVOT
(
    SUM(MeterDataQualitySummary.MeterCount)
    FOR MeterDataQualitySummary.CompletenessLevel IN (First, Second, Third, Fourth, Fifth, Sixth)
) as pvt
ORDER BY Date

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
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectDisturbancesDetailsByDate '07/19/2006', '07/05/2016', 'External'
-- selectSitesDisturbancesDetailsByDate '07/19/2014', '0','External'
-- =============================================
CREATE PROCEDURE [dbo].[selectDisturbancesDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime,
    @EventDateTo as DateTime,
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)
    
    SELECT
        Meter.ID AS themeterid,
        Meter.Name AS thesite,
        MeterLocation.Latitude AS latitude,
        MeterLocation.Longitude AS longitude,
        COALESCE(Disturbance_Count, 0) AS Disturbance_Count,
        COALESCE([5], 0) AS [5],
        COALESCE([4], 0) AS [4],
        COALESCE([3], 0) AS [3],
        COALESCE([2], 0) AS [2],
        COALESCE([1], 0) AS [1],
        COALESCE([0], 0) AS [0]
    FROM
        Meter JOIN
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT OUTER JOIN
        (
            SELECT
                MeterID,
                6*[5] + 5*[4] + 4*[3] + 3*[2] + 2*[1] + 1*[0] AS Disturbance_Count,
                [5],
                [4],
                [3],
                [2],
                [1],
                [0]
            FROM
            (
                SELECT
                    Event.MeterID,
                    DisturbanceSeverity.SeverityCode
                FROM
                    DisturbanceSeverity JOIN
                    Disturbance ON DisturbanceSeverity.DisturbanceID = Disturbance.ID JOIN
                    Event ON Disturbance.EventID = Event.ID
                WHERE CAST(Disturbance.StartTime AS DATE) BETWEEN @startDate AND @endDate
            ) AS MeterSeverityCode
            PIVOT
            (
                COUNT(SeverityCode)
                FOR SeverityCode IN ([5], [4], [3], [2], [1], [0])
            ) AS PivotTable
        ) AS SeverityCount ON SeverityCount.MeterID = Meter.ID
    WHERE Meter.ID IN (SELECT * FROM authMeters(@username))
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectDisturbancesForMeterIDByDate '01/10/2013', '05/10/2015', '0'
-- selectDisturbancesForMeterIDByDate '03/05/2007', '03/06/2015', '13,17,46,40,6,52,15,16,28,57,19,55,53,12,58,54,8,59,18,20,9,11,21,1,41,39,23,51,14,45,47,2,50,56,30,42,32,10,22,29,48,24,43,34,4,37,26,36,25,31,44,49,3,7,27,35,33,38,5,', 'External'
-- =============================================
CREATE PROCEDURE [dbo].[selectDisturbancesForMeterIDByDate]
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

CREATE TABLE #temp (thesiteid int, thesitename varchar(100))

INSERT INTO #temp Select [dbo].[Meter].[ID], [dbo].[Meter].[Name] from [dbo].[Meter] where Meter.ID in (Select * from @MeterIDs)

SELECT thesiteid as siteid, thesitename as sitename, [5], [4], [3], [2], [1], [0]
FROM
(
    SELECT #temp.thesiteid, #temp.thesitename, SeverityCodes.SeverityCode AS SeverityCode, COALESCE(DisturbanceCount, 0) AS DisturbanceCount
    FROM
        #temp Cross JOIN
        ( Select 5 as SeverityCode UNION 
          SELECT 4 as SeverityCode UNION 
          SELECT 3 as SeverityCode UNION 
          SELECT 2 as SeverityCode UNION 
          SELECT 1 as SeverityCode UNION 
          SELECT 0 as SeverityCode
        ) AS SeverityCodes LEFT OUTER JOIN
        (
            SELECT MeterID, SeverityCode, COUNT(*) AS DisturbanceCount
            FROM DisturbanceSeverity JOIN
                 Disturbance ON Disturbance.ID = DisturbanceSeverity.DisturbanceID Join
                 Event ON Event.ID = Disturbance.EventID
            Where ( (CAST( Event.StartTime as Date) between @EventDateFrom and @EventDateTo))
            and MeterID in (select * from authMeters(@username))
            and Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst')
            GROUP BY SeverityCode, MeterID
        ) AS Disturbances ON #temp.thesiteid = Disturbances.MeterID AND Disturbances.SeverityCode = SeverityCodes.SeverityCode
) AS DisturbanceData
PIVOT
(
    SUM(DisturbanceData.DisturbanceCount)
    FOR DisturbanceData.SeverityCode IN ([5], [4], [3], [2], [1], [0])
) as pvt
ORDER BY sitename asc

DROP TABLE #temp

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectDisturbancesForMeterIDByDateRange '01/10/2013', '05/10/2015', '0'
-- selectDisturbancesForMeterIDByDateRange '03/05/2015', '03/06/2015', '0', 'External'
-- =============================================
CREATE PROCEDURE [dbo].[selectDisturbancesForMeterIDByDateRange]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime, 
    @EventDateTo as DateTime, 
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN

    SET NOCOUNT ON;

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

DECLARE @PivotColumns NVARCHAR(MAX) = N''
DECLARE @ReturnColumns NVARCHAR(MAX) = N''
DECLARE @SQLStatement NVARCHAR(MAX) = N''

create table #TEMP (Name varchar(max))
insert into #TEMP SELECT SeverityCode FROM (Select Distinct SeverityCode FROM DisturbanceSeverity) as t

SELECT @PivotColumns = @PivotColumns + '[' + COALESCE(CAST(Name as varchar(5)), '') + '],' 
FROM #TEMP ORDER BY Name desc
IF @PivotColumns = ''
	SET @PivotColumns = '[0],'

SELECT @ReturnColumns = @ReturnColumns + ' COALESCE([' + COALESCE(CAST(Name as varchar(5)), '') + '], 0) AS [' + COALESCE(CAST(Name as varchar(5)), '') + '],' 
FROM #TEMP ORDER BY Name desc
IF @ReturnColumns = ''
	SET @ReturnColumns = ' COALESCE([0],0) AS [0],'

SET @SQLStatement =
' SELECT DisturbanceDate as thedate, ' + SUBSTRING(@ReturnColumns,0, LEN(@ReturnColumns)) +
' FROM (																		  ' +
'	SELECT                                                                        ' + 
'		CAST(Disturbance.StartTime AS DATE) AS DisturbanceDate,                	  ' + 
'		SeverityCode,															  ' + 
'		COUNT(*) AS DisturbanceCount											  ' + 
'	FROM																		  ' + 
'		DisturbanceSeverity JOIN												  ' + 
'		Disturbance ON Disturbance.ID = DisturbanceSeverity.DisturbanceID JOIN	  ' + 
'		Event ON Event.ID = Disturbance.EventID JOIN							  ' + 
'		Phase ON Disturbance.PhaseID = Phase.ID									  ' + 
'	WHERE																		  ' + 
'		(																		  ' + 
'			@MeterID = ''0'' OR													  ' + 
'			MeterID IN (SELECT * FROM String_To_Int_Table(@MeterID, '',''))	      ' + 
'		) AND																	  ' + 
'		MeterID IN (SELECT * FROM authMeters(@username)) AND					  ' + 
'		Phase.Name = ''Worst'' AND												  ' + 
'		Disturbance.StartTime BETWEEN @startDate AND @endDate AND				  ' + 
'		Disturbance.StartTime <> @endDate										  ' + 
'	GROUP BY CAST(Disturbance.StartTime AS DATE), SeverityCode 					  ' + 
'	) As DisturbanceDate														  ' + 
' PIVOT( ' +
'		SUM(DisturbanceDate.DisturbanceCount) ' +
'		FOR DisturbanceDate.SeverityCode IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') ' +
' ) as pvt ' +
' ORDER BY DisturbanceDate '

exec sp_executesql @SQLStatement, N'@username nvarchar(4000), @MeterID nvarchar(MAX), @startDate DATE, @endDate DATE ', @username = @username, @MeterID = @MeterID, @startDate = @startDate, @endDate = @endDate

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
-- selectEventsForMeterIDByDateRange '03/05/2015', '03/06/2015', '0', 'External'
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

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

DECLARE @PivotColumns NVARCHAR(MAX) = N''
DECLARE @ReturnColumns NVARCHAR(MAX) = N''
DECLARE @SQLStatement NVARCHAR(MAX) = N''

SELECT @PivotColumns = @PivotColumns + '[' + t.Name + '],' 
FROM (Select Name FROM EventType) AS t

SELECT @ReturnColumns = @ReturnColumns + ' COALESCE([' + t.Name + '], 0) AS [' + t.Name + '],' 
FROM (Select Name FROM EventType) AS t

SET @SQLStatement =
' SELECT Date as thedate, ' + SUBSTRING(@ReturnColumns,0, LEN(@ReturnColumns)) +
' FROM ( ' +
'		SELECT CAST(StartTime AS Date) as Date, COUNT(*) AS EventCount, EventType.Name as Name ' +
'		FROM Event JOIN '+
'		EventType ON Event.EventTypeID = EventType.ID ' +
'       WHERE ' +
'			MeterID in (select * from authMeters(@username)) AND MeterID IN (SELECT * FROM String_To_Int_Table( @MeterID,  '','')) AND StartTime >= @startDate AND StartTime < @endDate  ' +
'       GROUP BY CAST(StartTime AS DATE), EventType.Name ' +
'       ) as ed ' +
' PIVOT( ' +
'		SUM(ed.EventCount) ' +
'		FOR ed.Name IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') ' +
' ) as pvt ' +
' ORDER BY Date '

exec sp_executesql @SQLStatement, N'@username nvarchar(4000), @MeterID nvarchar(MAX), @startDate DATE, @endDate DATE ', @username = @username, @MeterID = @MeterID, @startDate = @startDate, @endDate = @endDate
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, March 25, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectEventsForMeterIDsByDate '01/10/2013', '05/10/2015', '58,', 'jwalker'
-- selectEventsForMeterIDsByDate '03/12/2013', '03/12/2014', '13,17,46,40,6,52,15,16,28,57,19,55,53,12,58,54,8,59,18,20,9,11,21,1,41,39,23,51,14,45,47,2,50,56,30,42,32,10,22,29,48,24,43,34,4,37,26,36,25,31,44,49,3,7,27,35,33,38,5,' , 'External'
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
            SELECT MeterID, EventTypeID, COUNT(*) AS EventCount 
            FROM Event 
            WHERE (CAST([StartTime] as Date) between @EventDateFrom and @EventDateTo)
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
    
    DECLARE @thedate DATE = CAST(@EventDate AS DATE)

    SELECT
        MeterLocation.Longitude,
        MeterLocation.Latitude, 
        (
            SELECT COUNT(Event.ID)
            FROM
                Event JOIN
                EventType ON EventType.ID = Event.EventTypeID AND EventType.Name = 'Fault'
            WHERE
                Meter.ID = Event.MeterID AND
                CAST([StartTime] AS DATE) = @thedate
        ) AS Event_Count
    FROM
        Meter JOIN
        Meterlocation ON Meter.MeterLocationID = MeterLocation.ID

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

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

DECLARE @PivotColumns NVARCHAR(MAX) = N''
DECLARE @ReturnColumns NVARCHAR(MAX) = N''
DECLARE @SQLStatement NVARCHAR(MAX) = N''

SELECT @PivotColumns = @PivotColumns + '[' + COALESCE(CAST(t.VoltageKV as varchar(5)), '') + '],' 
FROM (Select Distinct Line.VoltageKV 
		FROM Line) AS t
IF @PivotColumns = ''
	SET @PivotColumns = '[0],'

SELECT @ReturnColumns = @ReturnColumns + ' COALESCE([' + COALESCE(CAST(t.VoltageKV as varchar(5)), '') + '], 0) AS [' + COALESCE(CAST(t.VoltageKV as varchar(5)), '') + '],' 
FROM (Select Distinct Line.VoltageKV 
		FROM Line) AS t
IF @ReturnColumns = ''
	SET @ReturnColumns = ' COALESCE([0],0) AS [0],'

SET @SQLStatement =
' SELECT Date as thedate, ' + SUBSTRING(@ReturnColumns,0, LEN(@ReturnColumns)) +
' FROM ( ' +
'		SELECT CAST(Event.StartTime AS DATE) AS Date, Line.VoltageKV, COUNT(*) AS thecount ' +
'		FROM Event JOIN '+
'		EventType ON Event.EventTypeID = EventType.ID JOIN ' +
'       Line ON Event.LineID = Line.ID ' +
'       WHERE EventType.Name = ''Fault'' AND ' +
'		MeterID in (select * from authMeters(@username)) AND MeterID IN (SELECT * FROM String_To_Int_Table( @MeterID,  '','')) AND StartTime >= @startDate AND StartTime < @endDate  ' +
'       GROUP BY CAST(Event.StartTime AS DATE), EventType.Name, Line.VoltageKV ' +
'       ) as eventtable ' +
' PIVOT( ' +
'		SUM(eventtable.thecount) ' +
'		FOR eventtable.VoltageKV IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') ' +
' ) as pvt ' +
' ORDER BY Date '

exec sp_executesql @SQLStatement, N'@username nvarchar(4000), @MeterID nvarchar(MAX), @startDate DATE, @endDate DATE ', @username = @username, @MeterID = @MeterID, @startDate = @startDate, @endDate = @endDate
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
    
    DECLARE @thedate DATE = CAST(@EventDate AS DATE)

    SELECT
        Meter.ID,
        Meter.Name,
        MeterLocation.Longitude,
        MeterLocation.Latitude, 
        (
            SELECT COUNT(Event.ID)
            FROM
                Event JOIN
                EventType ON EventType.ID = Event.EventTypeID
            WHERE
                CAST([StartTime] AS DATE) = @thedate AND
                Event.MeterID = Meter.ID
        ) AS Event_Count
        FROM
            Meter JOIN
            MeterLocation on Meter.MeterLocationID = MeterLocation.ID

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 03, 2015>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================

--[selectMeterLocationsBreakers] '05/07/2014' , '05/07/2014', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsBreakers]
    @EventDateFrom DATETIME,
    @EventDateTo DATETIME,
    @meterGroup AS int
AS
BEGIN

    SET NOCOUNT ON;
    
    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)

    SELECT
        Meter.ID,
        Meter.Name,
        MeterLocation.Latitude,
        MeterLocation.Longitude, 
	    COALESCE(pvttable.Normal, 0) AS Normal,
        COALESCE(pvttable.Late, 0) AS Late,
        COALESCE(pvttable.Indeterminate, 0) AS Indeterminate, 
	    (COALESCE(pvttable.Normal, 0) + COALESCE(pvttable.Late, 0) + COALESCE(pvttable.indeterminate, 0)) AS Event_Count
    FROM
	    Meter JOIN
	    MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT JOIN
	    (
		    SELECT
                pvt.Normal,
                pvt.Late,
                pvt.Indeterminate,
                MeterID
		    FROM
		    (
		        SELECT
                    COUNT(*) as EventCount,
                    BreakerOperationType.Name as TypeName,
                    Event.MeterID as MeterID
		        FROM
                    BreakerOperation JOIN
				    Event ON BreakerOperation.EventID = Event.ID JOIN
				    BreakerOperationType ON BreakerOperationType.ID = BreakerOperation.BreakerOperationTypeID
			    WHERE CAST(BreakerOperation.TripCoilEnergized AS DATE) BETWEEN @startDate AND @endDate
			    GROUP BY BreakerOperationType.Name, Event.MeterID
		    ) AS EventStuff
 		    PIVOT
            (
			    SUM(EventCount)
			    FOR EventStuff.TypeName IN (normal, late, indeterminate)
		    ) AS pvt	 
	    ) AS pvttable ON Meter.ID = pvttable.MeterID
		WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
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
    @meterGroup AS int
AS
BEGIN

    SET NOCOUNT ON;
    
    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)

    SELECT
        Meter.ID, 
        Meter.Name, 
        MeterLocation.Longitude, 
        MeterLocation.Latitude,
        (
            SELECT CAST(COALESCE(CAST(SUM(goodPoints) AS FLOAT) / NULLIF(CAST(SUM(expectedPoints) AS FLOAT), 0) * 100 , 0) AS INT)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS Event_Count,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.ExpectedPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS ExpectedPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.GoodPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS GoodPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.LatchedPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS LatchedPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.UnreasonablePoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS UnreasonablePoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.NoncongruentPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS NoncongruentPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.DuplicatePoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS DuplicatePoints
        FROM
            Meter JOIN
            MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
		WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
        ORDER BY Meter.Name
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Aug 31, 2015>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- =============================================
--[selectMeterLocationsCorrectness] '08/14/2008' , '08/16/2008', 'jwalker'

CREATE PROCEDURE [dbo].[selectMeterLocationsCorrectness]
    @EventDateFrom DATETIME,
    @EventDateTo DATETIME,
    @meterGroup AS int
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)

    SELECT 
        Meter.ID, 
        Meter.Name, 
        MeterLocation.Longitude, 
        MeterLocation.Latitude, 
        (
            SELECT CAST(COALESCE(CAST(SUM(goodPoints) AS FLOAT) / NULLIF(CAST(SUM(expectedPoints) AS FLOAT), 0) * 100 , 0) AS INT)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS Event_Count,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.ExpectedPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS ExpectedPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.GoodPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS GoodPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.LatchedPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS LatchedPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.UnreasonablePoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS UnreasonablePoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.NoncongruentPoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS NoncongruentPoints,
		(
            SELECT COALESCE(SUM(MeterDataQualitySummary.DuplicatePoints), 0)
            FROM MeterDataQualitySummary
            WHERE MeterDataQualitySummary.MeterID = Meter.ID AND [Date] BETWEEN @startDate AND @endDate
        ) AS DuplicatePoints
        FROM
            Meter JOIN
            MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
		WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
        ORDER BY Meter.Name
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectMeterLocationsDisturbances '10/06/2014', '10/06/2014', 'External'
-- selectMeterLocationsDisturbances '07/19/2014', '0','External'
-- =============================================
CREATE PROCEDURE [dbo].[selectMeterLocationsDisturbances]
    -- Add the parameters for the stored procedure here
    @EventDateFrom as DateTime,
    @EventDateTo as DateTime,
    @meterGroup as int
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

    -- Force the optimizer to generate a more intelligent query plan by storing
    -- the results of this simple query in a temp table before using it
    SELECT
        Event.MeterID,
        DisturbanceSeverity.SeverityCode
    INTO #MeterSeverityCode
    FROM
        DisturbanceSeverity JOIN
        Disturbance ON DisturbanceSeverity.DisturbanceID = Disturbance.ID JOIN
        Event ON Disturbance.EventID = Event.ID JOIN
        Phase ON Disturbance.PhaseID = Phase.ID
    WHERE
        Disturbance.StartTime BETWEEN @startDate AND @endDate AND
        Disturbance.StartTime <> @endDate AND
        Phase.Name = 'Worst'
    
    ; WITH SeverityCount AS
    (
        SELECT
            MeterID,
            6*[5] + 5*[4] + 4*[3] + 3*[2] + 2*[1] + 1*[0] AS Disturbance_Count,
            [5],
            [4],
            [3],
            [2],
            [1],
            [0]
        FROM #MeterSeverityCode
        PIVOT
        (
            COUNT(SeverityCode)
            FOR SeverityCode IN ([5], [4], [3], [2], [1], [0])
        ) AS PivotTable
    )
    SELECT
        Meter.ID AS ID,
        Meter.Name AS Name,
        MeterLocation.Latitude AS Latitude,
        MeterLocation.Longitude AS Longitude,
        COALESCE(Disturbance_Count, 0) AS Disturbance_Count,
        COALESCE([5], 0) AS [5],
        COALESCE([4], 0) AS [4],
        COALESCE([3], 0) AS [3],
        COALESCE([2], 0) AS [2],
        COALESCE([1], 0) AS [1],
        COALESCE([0], 0) AS [0]
    FROM
        Meter JOIN
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT OUTER JOIN
        SeverityCount ON SeverityCount.MeterID = Meter.ID
    WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
    ORDER BY Meter.Name

    DROP TABLE #MeterSeverityCode
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Update date: <07/08/2016>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- [selectMeterLocationsEvents] '04/07/2008' , '04/07/2014', 'External'
-- =============================================
CREATE PROCEDURE [dbo].[selectMeterLocationsEvents]
    @EventDateFrom DATETIME,
    @EventDateTo DATETIME,
    @meterGroup AS INT
AS
BEGIN

    SET NOCOUNT ON;
    
    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)

    SELECT
        Meter.ID,
        Meter.Name,
        MeterLocation.Longitude,
        MeterLocation.Latitude,
		COALESCE(Event_Count, 0) AS Event_Count,
		COALESCE(Interruption, 0) AS Interruption,
		COALESCE(Fault, 0) AS Fault,
		COALESCE(Sag, 0) AS Sag,
		COALESCE(Transient, 0) AS Transient,
		COALESCE(Swell, 0) AS Swell,
		COALESCE(Other, 0) AS Other

        FROM
            Meter JOIN
            MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT OUTER JOIN
			(
				SELECT 
					6*Interruption + 5*Fault + 4*Sag + 3*Transient + 2*Swell + 1*Other AS Event_Count, 
					MeterID,
					Interruption,
					Fault,
					Sag,
					Transient,
					Swell,
					Other
				FROM
				(
					SELECT 
						Event.MeterID,
						EventType.Name
					FROM
						Event JOIN
						EventType ON EventType.ID = Event.EventTypeID 
					WHERE
						CAST([StartTime] AS DATE) between @startDate AND @endDate
				)AS EventCode
				PIVOT
				(
					COUNT(Name)
					FOR Name IN (Interruption, Fault, Sag, Transient, Swell, Other)
				) AS PivotTable
				
			)AS Event_Count ON Event_Count.MeterID = Meter.ID

        WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
        ORDER BY Meter.Name
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- [selectMeterLocationsFaults] '04/07/2008' , '04/07/2014', 'jwalker'
-- =============================================
CREATE PROCEDURE [dbo].[selectMeterLocationsFaults]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @meterGroup AS int
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
		WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)

        order by [dbo].[Meter].[Name] asc

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- [selectMeterLocationsMaximumSwell] '08/22/2014' , '09/21/2014', 'jwalker'
-- =============================================

CREATE PROCEDURE [dbo].[selectMeterLocationsMaximumSwell]
    @EventDateFrom DATETIME,
    @EventDateTo DATETIME,
    @username AS NVARCHAR(4000)
AS
BEGIN

    SET NOCOUNT ON;
    
    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)

    SELECT
        Meter.ID,
        Meter.Name,
        MeterLocation.Longitude,
        MeterLocation.Latitude, 
        CAST(COALESCE((
            SELECT (10.0/9.0) * ((MAX([PerUnitMagnitude]) - 1.1)) * 100
            FROM
                Disturbance  JOIN
                Event ON Disturbance.EventID = Event.ID JOIN
                EventType ON EventType.ID = Event.EventTypeID AND EventType.Name = 'Swell'
            WHERE
                CAST(Event.StartTime AS DATE) BETWEEN @startDate AND @endDate AND
                Event.MeterID = Meter.ID AND
                PerUnitMagnitude IS NOT NULL AND
                PerUnitMagnitude >= 1.1
        ), 0) AS INT) AS Event_Count
    FROM
        Meter JOIN
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
    WHERE Meter.ID IN (SELECT * FROM authMeters(@username))
    ORDER BY Meter.Name

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, May 27, 2014>
-- Description: <Description, Selects Meter Identification Data, Event Count and Location for DateRange>
-- [selectMeterLocationsMinimumSags] '09/07/2014' , '09/07/2014', 'jwalker'
-- =============================================

CREATE PROCEDURE [dbo].[selectMeterLocationsMinimumSags]
    @EventDateFrom DATETIME,
    @EventDateTo DATETIME,
    @username AS NVARCHAR(4000)
AS
BEGIN

    SET NOCOUNT ON;
    
    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)

    SELECT
        Meter.ID,
        Meter.Name,
        MeterLocation.Longitude,
        MeterLocation.Latitude, 
        CAST(COALESCE((
            SELECT (10.0/8.0) * (0.9 - (MIN(PerUnitMagnitude))) * 100
            FROM
                Disturbance JOIN
                Event ON Disturbance.EventID = Event.ID JOIN
                EventType ON EventType.ID = Event.EventTypeID AND EventType.Name = 'Sag'
            WHERE
                CAST(Event.StartTime AS DATE) BETWEEN @startDate AND @endDate AND
                Event.MeterID = Meter.ID AND 
                PerUnitMagnitude IS NOT NULL AND PerUnitMagnitude <= 0.9
        ), 0) AS INT) AS Event_Count
    FROM
        Meter JOIN
        MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
    WHERE Meter.ID IN (SELECT * FROM authMeters(@username))
    ORDER BY Meter.Name

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 24, 2014>
-- Description: <Description, Selects Meter Identification Data, Alarm Count and Location for Day>
-- [selectMeterLocationsTrending] '08/19/2008','08/20/2008', 'external'
-- =============================================

CREATE PROCEDURE [dbo].[selectMeterLocationsTrending]
    @EventDateFrom DateTime,
    @EventDateTo DateTime,
    @meterGroup as int
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
			COALESCE(Alarm, 0) as Alarm,
			COALESCE(Offnormal, 0) As Offnormal,
			COALESCE(AlarmCount, 0) As AlarmCount
		FROM
			Meter JOIN
			MeterLocation ON Meter.MeterLocationID = MeterLocation.ID LEFT OUTER JOIN
			(
				SELECT 
					MeterID,
					Alarm + Offnormal as AlarmCount,
					Alarm,
					Offnormal
				FROM
					(
						SELECT
							Channel.MeterID,
							AlarmType.Name
						FROM
							AlarmType JOIN
							ChannelAlarmSummary ON ChannelAlarmSummary.AlarmTypeID = AlarmType.ID JOIN
							Channel ON Channel.ID = ChannelAlarmSummary.ChannelID
						WHERE ChannelAlarmSummary.Date BETWEEN @thedatefrom AND @thedateto
					
					) AS AlarmCodes
					PIVOT
					(
						COUNT(Name)
						FOR Name IN (Alarm, Offnormal)
					) AS PivotTable            
		) AS AlarmCount ON AlarmCount.MeterID = Meter.ID     
	   WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
       ORDER BY Meter.Name

END
GO

-- =============================================
-- Author:      <Author, William Ernest>
-- Create date: <Create Date, 8/4/2016>
-- Description: <Description, Selects Trending Data for a set of sites by Date>
-- [selectMeterLocationsTrendingData] '07/04/2008','08/05/2008', 'Voltage RMS', 'external'
-- =============================================
CREATE PROCEDURE [dbo].[selectMeterLocationsTrendingData]
    @EventDateFrom DATETIME,
    @EventDateTo DATETIME,
    @colorScaleName VARCHAR(200),
    @meterGroup AS int
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @thedatefrom AS DATE
    DECLARE @thedateto AS DATE

    SET @thedatefrom = CAST(@EventDateFrom AS DATE)
    SET @thedateto = CAST(@EventDateTo AS DATE)

	SELECT
		Meter.ID,
		Meter.Name,
		Data.Minimum AS Minimum,
		Data.Maximum AS Maximum,
		Data.Average AS Average,
		MeterLocation.Longitude,
		MeterLocation.Latitude
	FROM
		Meter LEFT OUTER JOIN
		(
            SELECT
                ContourChannel.MeterID AS MID,
				MIN(Minimum/COALESCE(ContourChannel.PerUnitValue, 1)) AS Minimum,
				MAX(Maximum/COALESCE(ContourChannel.PerUnitValue, 1)) AS Maximum,
				AVG(Average/COALESCE(ContourChannel.PerUnitValue, 1)) AS Average
			FROM
                ContourChannel JOIN
				DailyTrendingSummary ON  DailyTrendingSummary.ChannelID = ContourChannel.ChannelID
			WHERE Date >= @thedatefrom AND Date <= @thedateto AND ContourColorScaleName = @colorScaleName
			GROUP BY ContourChannel.MeterID
		) AS Data ON Data.MID = Meter.ID JOIN
		MeterLocation ON Meter.MeterLocationID = MeterLocation.ID
    WHERE Meter.ID IN (SELECT MeterID FROM MeterMeterGroup WHERE MeterGroupID = @meterGroup)
    ORDER BY Meter.Name
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Feb 25, 2015>
-- Description: <Description, Selects meters>
-- selectMeters 'External'
-- =============================================
CREATE PROCEDURE [dbo].[selectMeters]
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
-- selectSiteLinesDetailsByDate '2014-07-21', '140'

-- =============================================
CREATE PROCEDURE [dbo].[selectSiteLinesDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @localEventDate DATE = CAST(@EventDate AS DATE)
	DECLARE @localMeterID INT = CAST(@MeterID AS INT)

	; WITH cte AS
	(
		SELECT
			Event.LineID AS thelineid, 
			Event.ID AS theeventid, 
			EventType.Name AS theeventtype,
			CAST(Event.StartTime AS VARCHAR(26)) AS theinceptiontime,
			MeterLine.LineName + ' ' + [Line].[AssetKey] AS thelinename,
			Line.VoltageKV AS voltage,
			COALESCE(FaultSummary.FaultType, Phase.Name, '') AS thefaulttype,
			CASE WHEN FaultSummary.Distance = '-1E308' THEN 'NaN' ELSE COALESCE(CAST(CAST(FaultSummary.Distance AS DECIMAL(16, 4)) AS NVARCHAR(19)) + ' mi', '') END AS thecurrentdistance,
			dbo.EventHasImpactedComponents(Event.ID) AS pqiexists,
			Event.StartTime,
			Event.UpdatedBy,
			CASE EventType.Name
				WHEN 'Sag' THEN ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY Magnitude, Disturbance.StartTime, IsSelectedAlgorithm DESC, IsSuppressed, Inception)
                WHEN 'Interruption' THEN ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY Magnitude, Disturbance.StartTime, IsSelectedAlgorithm DESC, IsSuppressed, Inception)
				WHEN 'Swell' THEN ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY Magnitude DESC, Disturbance.StartTime, IsSelectedAlgorithm DESC, IsSuppressed, Inception)
				WHEN 'Fault' THEN ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY IsSelectedAlgorithm DESC, IsSuppressed, Inception)
				ELSE ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY Event.ID)
			END AS RowPriority
		FROM
			Event JOIN
			EventType ON Event.EventTypeID = EventType.ID LEFT OUTER JOIN
			Disturbance ON Disturbance.EventID = Event.ID LEFT OUTER JOIN
			FaultSummary ON FaultSummary.EventID = Event.ID  LEFT OUTER JOIN
			Phase ON Disturbance.PhaseID = Phase.ID JOIN
			Meter ON Event.MeterID = Meter.ID JOIN
			Line ON Event.LineID = Line.ID JOIN
			MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Line.ID
		WHERE
			CAST(Event.StartTime AS DATE) = @localEventDate AND
			Meter.ID = @localMeterID AND
            (Phase.ID IS NULL OR Phase.Name <> 'Worst')
	)
	SELECT
		thelineid,
		theeventid,
		theeventtype,
		theinceptiontime,
		thelinename,
		voltage,
		thefaulttype,
		thecurrentdistance,
		pqiexists,
		UpdatedBy
	FROM cte
	WHERE RowPriority = 1
	ORDER BY StartTime

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSiteLinesDisturbanceDetailsByDate '10/07/2014', '7'
-- =============================================
CREATE PROCEDURE [dbo].[selectSiteLinesDisturbanceDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @worstPhaseID INT = (SELECT ID FROM Phase WHERE Name = 'Worst')
	
	SELECT 
		Event.LineID AS thelineid, 
		Event.ID AS theeventid, 
		Disturbance.ID as disturbanceid,
		EventType.Name AS disturbancetype,
		Phase.Name AS phase,
        CASE Disturbance.PerUnitMagnitude
            WHEN -1E308 THEN 'NaN'
            ELSE CAST(CONVERT(DECIMAL(4,3), Disturbance.PerUnitMagnitude) AS VARCHAR(8))
        END AS magnitude,
        CASE Disturbance.DurationSeconds
            WHEN -1E308 THEN 'NaN'
            ELSE CAST(CONVERT(DECIMAL(10,3), Disturbance.DurationSeconds) AS VARCHAR(14))
        END AS duration,
		CAST(Disturbance.StartTime AS VARCHAR(26)) AS theinceptiontime,
        dbo.DateDiffTicks('1970-01-01', Disturbance.StartTime) / 10000.0 AS startmillis,
        dbo.DateDiffTicks('1970-01-01', Disturbance.EndTime) / 10000.0 AS endmillis,
		DisturbanceSeverity.SeverityCode,
		MeterLine.LineName + ' ' + [Line].[AssetKey] AS thelinename,
		Line.VoltageKV AS voltage
	FROM
		Event JOIN
		Disturbance ON Disturbance.EventID = Event.ID JOIN
        Disturbance WorstDisturbance ON
            Disturbance.EventID = WorstDisturbance.EventID AND
            Disturbance.PerUnitMagnitude = WorstDisturbance.PerUnitMagnitude AND
            Disturbance.DurationSeconds = WorstDisturbance.DurationSeconds JOIN
		EventType ON Disturbance.EventTypeID = EventType.ID JOIN
		Phase ON Disturbance.PhaseID = Phase.ID JOIN
		DisturbanceSeverity ON Disturbance.ID = DisturbanceSeverity.DisturbanceID JOIN
		Meter ON Meter.ID = @MeterID JOIN
		Line ON Event.LineID = Line.ID JOIN
		MeterLine ON MeterLine.MeterID = @MeterID AND MeterLine.LineID = Line.ID
	WHERE
		CAST(Disturbance.StartTime AS DATE) = @EventDate AND Event.MeterID = @MeterID AND
        WorstDisturbance.PhaseID = @worstPhaseID AND
        Disturbance.PhaseID <> @worstPhaseID
	ORDER BY
		Event.StartTime ASC
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
    @EventDate AS DATETIME,
    @MeterID AS NVARCHAR(MAX),
    @username AS NVARCHAR(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @thedate DATE = CAST(@EventDate AS DATE)
    DECLARE @MeterIDs TABLE (ID int);

    INSERT INTO @MeterIDs(ID)
    SELECT Value FROM dbo.String_to_int_table(@MeterID, ',')
    WHERE Value IN (SELECT * FROM authMeters(@username));

    SELECT 
        Meter.ID AS meterid, 
        Event.ID AS theeventid, 
        EventType.Name AS eventtype, 
        CAST(CAST(BreakerOperation.TripCoilEnergized AS TIME) AS NVARCHAR(100)) AS energized,
        BreakerOperation.BreakerNumber AS breakernumber,
        MeterLine.LineName AS linename,
        Phase.Name AS phasename,
        CAST(BreakerOperation.BreakerTiming AS DECIMAL(16,5)) AS timing,
        CAST(BreakerOperation.StatusTiming AS DECIMAL(16,5)) AS statustiming,
        BreakerOperation.BreakerSpeed AS speed,
        BreakerOperationType.Name AS operationtype 
    FROM
        BreakerOperation JOIN
        Event ON BreakerOperation.EventID = Event.ID JOIN
        EventType ON EventType.ID = Event.EventTypeID JOIN
        Meter ON Meter.ID = Event.MeterID JOIN
        Line ON Line.ID = Event.LineID JOIN
        MeterLine ON MeterLine.LineID = Event.LineID AND MeterLine.MeterID = Meter.ID JOIN
        BreakerOperationType ON BreakerOperation.BreakerOperationTypeID = BreakerOperationType.ID JOIN
        Phase ON BreakerOperation.PhaseID = Phase.ID
    WHERE
        Meter.ID IN (SELECT * FROM @MeterIDs) AND
        CAST(TripCoilEnergized AS DATE) = @thedate

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
    @EventDate AS DATETIME,
    @MeterID AS NVARCHAR(MAX),
    @username AS NVARCHAR(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @thedate DATE = CAST(@EventDate AS DATE)
    DECLARE @MeterIDs TABLE (ID INT);

    INSERT INTO @MeterIDs(ID)
    SELECT Value FROM dbo.String_to_int_table(@MeterID, ',')
    WHERE Value IN (SELECT * FROM authMeters(@username));

    DECLARE @TempTable TABLE (themeterid INT, thesite VARCHAR(100), thecount FLOAT, thename VARCHAR(100));

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT 
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        (
            SELECT COALESCE((
                SELECT CAST(CAST((GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) AS FLOAT) / NULLIF(CAST(expectedPoints AS FLOAT), 0) AS FLOAT) AS completenessPercentage
                FROM MeterDataQualitySummary
                WHERE
                    MeterID = Meter.ID AND
                    [Date] = @thedate
            )
        , 0)) AS thecount,
        'Completeness' as thename 
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT 
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        (
            SELECT COALESCE((
                SELECT CAST(NULLIF(CAST(expectedPoints AS FLOAT), 0) AS FLOAT) AS completenessPercentage
                FROM MeterDataQualitySummary
                WHERE 
                    MeterID = Meter.ID AND
                    [Date] = @thedate
            )
        , 0)) AS thecount,
        'Expected' AS thename 
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT 
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        (
            SELECT COALESCE((
                SELECT CAST(CAST((GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints + DuplicatePoints) AS FLOAT) / NULLIF(CAST(expectedPoints AS FLOAT), 0) AS FLOAT) AS completenessPercentage
                FROM MeterDataQualitySummary
                WHERE 
                    MeterID = Meter.ID AND
                    [Date] = @theDate
            )
        , 0)) AS thecount,
        'Received' AS thename 
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        (
            SELECT COALESCE((
                SELECT CAST(CAST((DuplicatePoints) AS FLOAT) / NULLIF(CAST(expectedPoints AS FLOAT), 0) AS FLOAT) AS completenessPercentage
                FROM MeterDataQualitySummary
                WHERE 
                    MeterID = Meter.ID AND
                    [Date] = @thedate
            )
        , 0)) AS thecount,
        'Duplicate' AS thename
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    DECLARE @composite TABLE (theeventid INT, themeterid INT, thesite VARCHAR(100), Expected FLOAT, Received FLOAT, Duplicate FLOAT, Completeness FLOAT);

    DECLARE @sitename VARCHAR(100)
    DECLARE @themeterid INT
    DECLARE @theeventid INT

    DECLARE site_cursor CURSOR FOR SELECT DISTINCT themeterid, thesite FROM @TempTable

    OPEN site_cursor

    FETCH NEXT FROM site_cursor INTO @themeterid , @sitename

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT @composite VALUES(
            (
                SELECT TOP 1 MeterDataQualitySummary.ID
                FROM MeterDataQualitySummary
                WHERE
                    MeterDataQualitySummary.MeterID = @themeterid AND
                    CAST([Date] as Date) = @thedate
            ),
            @themeterid,
            @sitename,
            (SELECT thecount * 100 FROM @TempTable WHERE thename = 'Expected' AND thesite = @sitename),
            (SELECT thecount * 100 FROM @TempTable WHERE thename = 'Received' AND thesite = @sitename),
            (SELECT thecount * 100 FROM @TempTable WHERE thename = 'Duplicate' AND thesite = @sitename),
            (SELECT thecount * 100 FROM @TempTable WHERE thename = 'Completeness' AND thesite = @sitename)
        )

        FETCH NEXT FROM site_cursor INTO @themeterid , @sitename
    END
 
    CLOSE site_cursor;
    DEALLOCATE site_cursor;

    SELECT * FROM @composite
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesCorrectnessDetailsByDate '07/19/2014', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,','External'
-- selectSitesCorrectnessDetailsByDate '09/03/2007', '108,109,86,87,110,118,13,17,167,168,77,78,79,80,70,46,169,185,186,40,6,170,88,89,52,192,15,16,91,92,93,94,95,96,97,28,98,99,100,101,102,171,57,172,125,173,63,19,104,174,55,105,106,107,103,111,112,113,60,53,160,114,115,116,117,68,12,58,54,8,59,18,20,9,193,119,120,81,164,121,175,156,157,176,177,11,122,123,21,65,1,41,39,23,51,127,128,129,130,165,76,131,132,133,134,135,136,137,14,124,45,47,73,64,2,138,139,140,141,142,143,82,83,62,50,144,145,56,30,42,146,147,148,149,150,151,32,152,126,74,67,10,66,22,178,179,29,180,48,181,153,154,155,194,24,43,34,4,69,37,158,26,182,36,159,161,162,163,71,166,25,31,44,49,72,61,75,187,188,189,190,191,3,84,85,7,195,90,183,27,196,184,35,33,197,198,199,200,201,38,5,','External'

-- =============================================
CREATE PROCEDURE [dbo].[selectSitesCorrectnessDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @thedate DATE = CAST(@EventDate AS DATE)
    DECLARE @MeterIDs TABLE (ID INT);

    INSERT INTO @MeterIDs(ID)
    SELECT Value
    FROM dbo.String_to_int_table(@MeterID, ',')
    WHERE Value IN (SELECT * FROM authMeters(@username));

    DECLARE @TempTable TABLE (themeterid INT, thesite VARCHAR(100), thecount FLOAT, thename VARCHAR(100));

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        (
            SELECT COALESCE(ROUND(CAST(SUM(LatchedPoints) AS FLOAT) / NULLIF(CAST(SUM(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) AS FLOAT), 0) * 100 , 0), 0) AS correctnessPercentage
            FROM MeterDataQualitySummary
            WHERE
                MeterID = Meter.ID AND
                [Date] = @thedate
        ) AS thecount,
        'Latched' as thename 
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT
        Meter.ID AS meterid,
        Meter.Name AS thesite,
        (
            SELECT COALESCE(ROUND(CAST(SUM(UnreasonablePoints) AS FLOAT) / NULLIF(CAST(SUM(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) AS FLOAT), 0) * 100 ,0), 0) AS correctnessPercentage
            FROM MeterDataQualitySummary
            WHERE
                MeterID = [dbo].[Meter].[ID] AND
                [Date] = @thedate
        ) AS thecount,
        'Unreasonable' AS thename 
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    INSERT INTO @TempTable(themeterid, thesite , thecount , thename)
    SELECT
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        (
            SELECT COALESCE(ROUND(CAST(SUM(NoncongruentPoints) AS FLOAT) / NULLIF(CAST(SUM(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints) AS FLOAT), 0) * 100 , 0), 0) AS correctnessPercentage
            FROM MeterDataQualitySummary
            WHERE
                MeterID = Meter.ID AND
                [Date] = @thedate
        ) AS thecount,
        'Noncongruent' AS thename
    FROM
        MeterDataQualitySummary JOIN
        Meter ON Meter.ID = MeterDataQualitySummary.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST([Date] AS DATE) = @thedate

    DECLARE @composite TABLE (theeventid INT, themeterid INT, thesite VARCHAR(100), Latched FLOAT, Unreasonable FLOAT, Noncongruent FLOAT, Correctness FLOAT);

    DECLARE @sitename VARCHAR(100)
    DECLARE @themeterid INT
    DECLARE @theeventid INT

    DECLARE site_cursor CURSOR FOR SELECT DISTINCT themeterid, thesite FROM @TempTable

    OPEN site_cursor

    FETCH NEXT FROM site_cursor INTO @themeterid, @sitename

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO @composite VALUES(
            (
                SELECT TOP 1 MeterDataQualitySummary.ID
                FROM MeterDataQualitySummary
                WHERE
                    MeterDataQualitySummary.MeterID = @themeterid AND
                    CAST([Date] AS DATE) = @theDate
            ),
            @themeterid,
            @sitename,
            (SELECT thecount FROM @TempTable WHERE thename = 'Latched' AND thesite = @sitename),
            (SELECT thecount FROM @TempTable WHERE thename = 'Unreasonable' AND thesite = @sitename),
            (SELECT thecount FROM @TempTable WHERE thename = 'Noncongruent' AND thesite = @sitename),
             (
                    SELECT 100.0 * CAST(GoodPoints AS FLOAT) / CAST(NULLIF(GoodPoints + LatchedPoints + UnreasonablePoints + NoncongruentPoints, 0) AS FLOAT) AS Correctness
                    FROM MeterDataQualitySummary
                    WHERE CAST([Date] AS DATE) = @theDate AND MeterID = @themeterid
             )
        )

        FETCH NEXT FROM site_cursor INTO @themeterid , @sitename
    END
 
    CLOSE site_cursor;
    DEALLOCATE site_cursor;

    SELECT * FROM @composite
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 23, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesDisturbancesDetailsByDate '07/19/2014', '1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,','External'
-- selectSitesDisturbancesDetailsByDate '07/19/2014', '0','External'
-- =============================================
CREATE PROCEDURE [dbo].[selectSitesDisturbancesDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate as DateTime,
    @MeterID as nvarchar(MAX),
    @username as nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @thedate DATE = CAST(@EventDate AS DATE)
    DECLARE  @MeterIDs TABLE (ID INT);

    INSERT INTO @MeterIDs(ID)
    SELECT Value
    FROM dbo.String_to_int_table(@MeterID, ',')
    WHERE Value IN (SELECT * FROM authMeters(@username));

    DECLARE @TempTable TABLE (themeterid INT, thesite VARCHAR(100), thecount INT, thename VARCHAR(100));

    INSERT INTO @TempTable (themeterid, thesite , thecount , thename)
    SELECT 
        Meter.ID AS meterid, 
        Meter.Name AS thesite, 
        DisturbanceSeverity.SeverityCode AS thecount, 
        SeverityCodes.SeverityCode AS thename 
    FROM
        DisturbanceSeverity JOIN
        ( Select 5 as SeverityCode UNION 
          SELECT 4 as SeverityCode UNION 
          SELECT 3 as SeverityCode UNION 
          SELECT 2 as SeverityCode UNION 
          SELECT 1 as SeverityCode UNION 
          SELECT 0 as SeverityCode
        ) AS SeverityCodes ON DisturbanceSeverity.SeverityCode = SeverityCodes.SeverityCode LEFT OUTER JOIN
        Disturbance ON Disturbance.ID = DisturbanceSeverity.DisturbanceID Join
        Event ON Event.ID = Disturbance.EventID JOIN
        Meter ON Meter.ID = Event.MeterID
    WHERE
        MeterID IN (SELECT * FROM @MeterIDs) AND
        CAST(Disturbance.StartTime AS DATE) = @thedate AND
        Disturbance.PhaseID = (SELECT ID FROM Phase WHERE Name = 'Worst')

    DECLARE @composite TABLE (theeventid INT, themeterid INT, thesite VARCHAR(100), [5] INT, [4] INT, [3] INT, [2] INT, [1] INT, [0] INT);

    DECLARE @sitename VARCHAR(100)
    DECLARE @themeterid INT
    DECLARE @theeventid INT

    DECLARE site_cursor CURSOR FOR SELECT DISTINCT themeterid, thesite FROM @TempTable

    OPEN site_cursor

    FETCH NEXT FROM site_cursor INTO @themeterid, @sitename

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO @composite VALUES(
            (
                SELECT TOP 1 Event.ID
                FROM Event
                WHERE
                    Event.MeterID = @themeterid AND
                    CAST([StartTime] AS DATE) = @theDate
            ),
            @themeterid,
            @sitename,
            (SELECT COUNT(thecount) FROM @TempTable WHERE thename = '5' AND thesite = @sitename),
            (SELECT COUNT(thecount) FROM @TempTable WHERE thename = '4' AND thesite = @sitename),
            (SELECT COUNT(thecount) FROM @TempTable WHERE thename = '3' AND thesite = @sitename),
            (SELECT COUNT(thecount) FROM @TempTable WHERE thename = '2' AND thesite = @sitename),
            (SELECT COUNT(thecount) FROM @TempTable WHERE thename = '1' AND thesite = @sitename),
            (SELECT COUNT(thecount) FROM @TempTable WHERE thename = '0' AND thesite = @sitename)
        )

        FETCH NEXT FROM site_cursor into @themeterid , @sitename
    END
 
    CLOSE site_cursor;
    DEALLOCATE site_cursor;

    SELECT * FROM @composite
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

    DECLARE @startDate DATE = CAST(@EventDate AS DATE)
	DECLARE @endDate DATE = DATEADD(DAY, 1, @startDate)
	
	DECLARE @PivotColumns NVARCHAR(MAX) = N''
	DECLARE @ReturnColumns NVARCHAR(MAX) = N''
	DECLARE @SQLStatement NVARCHAR(MAX) = N''

	SELECT @PivotColumns = @PivotColumns + '[' + COALESCE(CAST(t.Name as varchar(max)), '') + '],' 
	FROM (Select Name FROM EventType) AS t

	SELECT @ReturnColumns = @ReturnColumns + ' COALESCE([' + COALESCE(CAST(t.Name as varchar(max)), '') + '], 0) AS [' + COALESCE(CAST(t.Name as varchar(max)), '') + '],' 
	FROM (Select Name FROM EventType) AS t

	SET @SQLStatement =
	' SELECT EventID, MeterID, Site, ' + SUBSTRING(@ReturnColumns,0, LEN(@ReturnColumns)) +
	' FROM ( ' +
	'	SELECT ' +
	'		(SELECT TOP 1 ID FROM EVENT e WHERE e.MeterID = Event.MeterID) as EventID, Event.MeterID, COUNT(*) AS EventCount, EventType.Name, Meter.Name as Site ' +
	'		FROM Event JOIN '+
	'		EventType ON Event.EventTypeID = EventType.ID JOIN' +
	'		Meter ON Event.MeterID = Meter.ID' +
	'       WHERE ' +
    '			MeterID in (select * from authMeters(@username)) AND MeterID IN (SELECT * FROM String_To_Int_Table( @MeterID,  '','')) AND StartTime >= @startDate AND StartTime < @endDate  ' +
	'       GROUP BY Event.MeterID,Meter.Name,EventType.Name ' +
	'       ) as ed ' +
	' PIVOT( ' +
	'		SUM(ed.EventCount) ' +
	'		FOR ed.Name IN(' + SUBSTRING(@PivotColumns,0, LEN(@PivotColumns)) + ') ' +
	' ) as pvt ' +
	' ORDER BY MeterID '

	print @pivotcolumns
	print @returncolumns
	print @sqlstatement
	exec sp_executesql @SQLStatement, N'@username nvarchar(4000), @MeterID nvarchar(MAX), @startDate DATE, @endDate DATE ', @username = @username, @MeterID = @MeterID, @startDate = @startDate, @endDate = @endDate
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

    DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
    DECLARE @endDate DATE = CAST(@EventDateTo AS DATE)
    DECLARE  @MeterIDs TABLE (ID INT);

    INSERT INTO @MeterIDs(ID)
    SELECT Value
    FROM dbo.String_to_int_table(@MeterID, ',')
    WHERE Value IN (SELECT * FROM authMeters(@username));

    SELECT
        CAST(CAST(Event.StartTime AS DATETIME2(7)) AS VARCHAR(26)) AS starttime,
        CAST(CAST(Event.EndTime AS DATETIME2(7)) AS VARCHAR(26)) AS endtime,
        Event.ID AS eventid,
        Meter.ID AS meterid, 
        Meter.Name AS thesite,
        EventType.Name AS thename,
        MeterLine.LineName AS linename 
    FROM
        Event JOIN
        EventType ON EventType.ID = Event.EventTypeID JOIN
        Meter ON Meter.ID = Event.MeterID JOIN
        MeterLine ON MeterLine.MeterID = Meter.ID AND MeterLine.LineID = Event.LineID
    WHERE
        Meter.ID IN (SELECT * FROM @MeterIDs) AND
        CAST(StartTime AS DATE) BETWEEN @startDate AND @endDate
    ORDER BY StartTime

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
            CASE WHEN FaultSummary.Distance = '-1E308' THEN 'NaN' ELSE CAST(CAST(FaultSummary.Distance AS DECIMAL(16,2)) AS NVARCHAR(19)) END AS thecurrentdistance,
            ROW_NUMBER() OVER(PARTITION BY Event.ID ORDER BY FaultSummary.IsSuppressed, FaultSummary.IsSelectedAlgorithm DESC, FaultSummary.Inception) AS rk
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
-- Author:      <Author, William Ernest>
-- Create date: <Create Date, 8/4/2016>
-- Description: <Description, Selects Trending Data for a set of sites by Date>
-- selectSitesTrendingDataDetailsByDate '07/05/2008', 'Voltage RMS','199,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,200,201,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125', 'external' 
-- =============================================
CREATE PROCEDURE [dbo].[selectSitesTrendingDataDetailsByDate]
    -- Add the parameters for the stored procedure here
    @EventDate DateTime2,
    @colorScaleName VARCHAR(200),
    @MeterID AS nvarchar(MAX),
    @username AS nvarchar(4000)

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE  @MeterIDs TABLE (ID int);
    DECLARE  @ChannelID AS nvarchar(MAX);
    DECLARE  @Date as DateTime2;
    SET @Date = @EventDate;

    -- Create MeterIDs Table
    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    -- Trending Data
    SELECT
        Meter.ID as meterid,
        Meter.Name as Name,
        Channel.ID as channelid,
        DailyTrendingSummary.Date as date,
        MIN(Minimum/COALESCE(Channel.PerUnitValue,1)) as Minimum,
        MAX(Maximum/COALESCE(Channel.PerUnitValue,1)) as Maximum,
        AVG(Average/COALESCE(Channel.PerUnitValue,1)) as Average, 
        MeasurementCharacteristic.Name as characteristic,
        MeasurementType.Name as measurementtype,
        Phase.Name as phasename
    FROM
        DailyTrendingSummary JOIN 
        Channel ON DailyTrendingSummary.ChannelID = Channel.ID JOIN
        Meter ON Meter.ID = Channel.MeterID JOIN 
        MeasurementCharacteristic ON Channel.MeasurementCharacteristicID = MeasurementCharacteristic.ID JOIN
        MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID JOIN
        Phase ON Channel.PhaseID = Phase.ID
    WHERE Meter.ID IN (SELECT * FROM @MeterIDs) AND Channel.ID IN (SELECT ChannelID FROM ContourChannel WHERE ContourColorScaleName = @colorScaleName) AND Date = @Date
    GROUP BY Date, Meter.ID, Meter.Name, MeasurementCharacteristic.Name, MeasurementType.Name, Phase.Name, Channel.ID
    ORDER BY Date    
END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, July 29, 2014>
-- Description: <Description, Selects Events for a set of sites by Date>
-- selectSitesTrendingDetailsByDate '04/08/2008', '52','external'
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
        Channel.HarmonicGroup,
        SUM (ChannelAlarmSummary.AlarmPoints) as eventcount,
        @theDate as date
        
        from Channel
        
        join ChannelAlarmSummary on ChannelAlarmSummary.ChannelID = Channel.ID and Date = @theDate
        join Meter on Channel.MeterID = Meter.ID and [MeterID] in ( Select * from @MeterIDs)
        join [dbo].[AlarmType] on 
            [dbo].[AlarmType].[ID] = ChannelAlarmSummary.AlarmTypeID and
            ([dbo].[AlarmType].[Name] = 'OffNormal' or [dbo].[AlarmType].[Name] = 'Alarm')

        join [dbo].[MeasurementCharacteristic] on Channel.MeasurementCharacteristicID = [dbo].[MeasurementCharacteristic].[ID]
        join [dbo].[MeasurementType] on Channel.MeasurementTypeID =  [dbo].[MeasurementType].ID
        join [dbo].[Phase] on Channel.PhaseID = [dbo].[Phase].ID

        Group By Meter.ID , Channel.ID , Meter.Name , [dbo].[AlarmType].[Name], [MeasurementCharacteristic].[Name] , [MeasurementType].[Name] , [dbo].[Phase].[Name], Channel.HarmonicGroup
        Order By Meter.ID

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, Jun 26, 2015>
-- Description: <Description, Selects Trending Data for a MeterID by Date, MeasurementCharacteristic, MeqasurementType, and Phase for a day>
-- selectTrendingData '03/22/2008', 1 , 20 , 1 , 3

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
-- Author:      <Author, William Ernest>
-- Create date: <Create Date, Aug 4, 2016>
-- Description: <Description, Selects Trending Data for a ChannelID by Date for a day for purpose of creating smart alarms in openXDA/Historian>
-- [dbo].[selectTrendingDataByChannelByDate] '07/05/2008', '08/04/2008', 'Voltage RMS','199,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,200,201,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125', 'external' 
-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingDataByChannelByDate]
    @StartDate DateTime2,
    @EndDate DateTime2,
    @colorScale AS varchar(200),
    @MeterID AS nvarchar(MAX),
    @username AS nvarchar(4000)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE  @MeterIDs TABLE (ID int);
    DECLARE  @ChannelID AS nvarchar(MAX);
    DECLARE  @BeginDate DateTime2;
    DECLARE  @StopDate DateTime2;

    SET @BeginDate = @StartDate;
    SET @StopDate = @EndDate;

    -- Create MeterIDs Table
    INSERT INTO @MeterIDs(ID) SELECT Value FROM dbo.String_to_int_table(@MeterID, ',') where Value in (select * from authMeters(@username));

    -- Trending Data
    SELECT
        Date,
        MIN(Minimum/COALESCE(Channel.PerUnitValue, 1)) as Minimum,
        MAX(Maximum/COALESCE(Channel.PerUnitValue,1)) as Maximum,
        AVG(Average/COALESCE(Channel.PerUnitValue,1)) as Average
    FROM
        DailyTrendingSummary JOIN 
        Channel ON DailyTrendingSummary.ChannelID = Channel.ID JOIN
        Meter ON Meter.ID = Channel.MeterID 
    WHERE Meter.ID IN (SELECT * FROM @MeterIDs) AND Channel.ID IN (SELECT ChannelID FROM ContourChannel WHERE ContourChannel.ContourColorScaleName = @colorScale) AND Date >= @BeginDate AND Date <= @StopDate
    GROUP BY Date
    ORDER BY Date
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
-- Description: <Description, Selects Trending Data for a ChannelID by Date for a day>
-- selectTrendingDataByChannelIDDate2 '9/15/2015', 988
-- =============================================
CREATE PROCEDURE [dbo].[selectTrendingDataByChannelIDDate2]
    @EventDate DateTime2,
    @ChannelID int
AS
BEGIN
    SET NOCOUNT ON;

    -- Trending Data
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
        TrendingData.Time AS thedate,
        TrendingData.Minimum AS theminimum,
        TrendingData.Maximum AS themaximum,
        TrendingData.Average AS theaverage
    FROM
        TrendingData
    WHERE
        TrendingData.ChannelID = @ChannelID
    ORDER BY TrendingData.Time
    
    -- Alarm Data
    DECLARE @alarmHigh FLOAT
    DECLARE @alarmLow FLOAT

    SELECT
        @EventDate AS thedatefrom,
        DATEADD(DAY, 1, @EventDate) AS thedateto,
        CASE
            WHEN AlarmRangeLimit.PerUnit <> 0 AND Channel.PerUnitValue IS NOT NULL THEN AlarmRangeLimit.High * PerUnitValue
            ELSE AlarmRangeLimit.High
        END AS alarmlimithigh,
        CASE
            WHEN AlarmRangeLimit.PerUnit <> 0 AND Channel.PerUnitValue IS NOT NULL THEN AlarmRangeLimit.Low * PerUnitValue
            ELSE AlarmRangeLimit.Low
        END AS alarmlimitlow
    FROM
        AlarmRangeLimit JOIN
        Channel ON AlarmRangeLimit.ChannelID = Channel.ID
    WHERE
        AlarmRangeLimit.AlarmTypeID = (SELECT ID FROM AlarmType WHERE Name = 'Alarm') AND
        AlarmRangeLimit.ChannelID = @ChannelID

    -- Off-normal data
    DECLARE @dayOfWeek INT = DATEPART(DW, @EventDate) - 1
    DECLARE @hourOfWeek INT = @dayOfWeek * 24

    ; WITH HourlyIndex AS
    (
        SELECT @hourOfWeek AS HourOfWeek
        UNION ALL
        SELECT HourOfWeek + 1
        FROM HourlyIndex
        WHERE (HourOfWeek + 1) < @hourOfWeek + 24
    )
    SELECT
        DATEADD(HOUR, HourlyIndex.HourOfWeek - @hourOfWeek, @EventDate) AS thedatefrom,
        DATEADD(HOUR, HourlyIndex.HourOfWeek - @hourOfWeek + 1, @EventDate) AS thedateto,
        HourOfWeekLimit.High AS offlimithigh,
        HourOfWeekLimit.Low AS offlimitlow
    FROM
        HourlyIndex LEFT OUTER JOIN
        HourOfWeekLimit ON HourOfWeekLimit.HourOfWeek = HourlyIndex.HourOfWeek
    WHERE
        HourOfWeekLimit.ChannelID IS NULL OR
        HourOfWeekLimit.ChannelID = @ChannelID
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
-- [selectTrendingForMeterIDByDateRange] '03/12/2008', '03/12/2014', '52', 'External'
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

DECLARE @startDate DATE = CAST(@EventDateFrom AS DATE)
DECLARE @endDate DATE = DATEADD(DAY, 1, CAST(@EventDateTo AS DATE))

SELECT AlarmDate as thedate, COALESCE(OffNormal,0) as offnormal, COALESCE(Alarm,0) as alarm
FROM(
	SELECT Date AS AlarmDate, AlarmType.Name, SUM(AlarmPoints) as AlarmPoints
	FROM ChannelAlarmSummary JOIN
		 Channel ON ChannelAlarmSummary.ChannelID = Channel.ID JOIN
		 AlarmType ON AlarmType.ID = ChannelAlarmSummary.AlarmTypeID
    WHERE MeterID in (select * from authMeters(@username)) AND MeterID IN (SELECT * FROM String_To_Int_Table(@MeterID, ',')) AND Date >= @startDate AND Date < @endDate
	GROUP BY Date, AlarmType.Name
) AS table1
PIVOT(
	SUM(table1.AlarmPoints)
	FOR table1.Name IN(Alarm, OffNormal)
) as pvt

END
GO

-- =============================================
-- Author:      <Author, Jeff Walker>
-- Create date: <Create Date, June 19, 2015>
-- Description: <Description, Selects Events for a MeterID by Date for date range>
-- selectTrendingForMeterIDsByDate '05/20/2008', '06/19/2015', '32,33,10,11,34,42,91,92,1,2,3,4,93,109,110,94,12,13,116,15,16,17,18,19,20,21,22,23,24,25,26,95,96,49,97,28,98,29,30,31,27,35,36,37,84,38,39,40,41,117,43,44,5,88,45,99,80,81,100,101,46,47,51,52,53,54,89,55,56,57,58,59,60,61,48,62,63,64,65,66,67,6,7,68,69,70,71,72,73,74,75,76,50,102,103,104,105,77,78,79,118,82,106,83,85,86,87,90,111,112,113,114,115,8,9,119,14,107,120,108,121,122,123,124,125,', 'external'
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
            SELECT MeterID, AlarmTypeID, Sum(AlarmPoints) AS AlarmCount FROM ChannelAlarmSummary
            join Channel on ChannelAlarmSummary.ChannelID = Channel.ID
            join Meter on Channel.MeterID = Meter.ID
            where MeterID in (Select * from @MeterIDs)
            and Date between @EventDateFrom and @EventDateTo
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

CREATE FUNCTION [dbo].[authMeters](@username varchar(100))
RETURNS TABLE 
AS 
RETURN
(
Select distinct MeterID
from UserMeter
where UserName = @username
)
GO
