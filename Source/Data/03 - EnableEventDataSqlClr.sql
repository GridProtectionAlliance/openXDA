EXEC sp_configure 'clr enabled', 1
GO

RECONFIGURE
GO

CREATE FUNCTION GetEventData(@eventID INT)
RETURNS TABLE
(
    SeriesID INT,
    Characteristic NVARCHAR(200),
    Time DATETIME2,
    Value FLOAT
)
AS EXTERNAL NAME [openXDA.SqlClr].[openXDA.SqlClr.XDAFunctions].GetEventData;
GO

CREATE FUNCTION GetCycleData(@eventID INT)
RETURNS TABLE
(
    SeriesID INT,
    Time DATETIME2,
    Value FLOAT
)
AS EXTERNAL NAME [openXDA.SqlClr].[openXDA.SqlClr.XDAFunctions].GetCycleData;
GO

CREATE FUNCTION GetFaultData(@eventID INT)
RETURNS TABLE
(
    Algorithm NVARCHAR(80),
    Time DATETIME2,
    Value FLOAT
)
AS EXTERNAL NAME [openXDA.SqlClr].[openXDA.SqlClr.XDAFunctions].GetFaultData;
GO
