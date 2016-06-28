USE [openXDA]
GO

ALTER FUNCTION HasImpactedComponents
(
    @disturbanceID INT
)
RETURNS INT
AS BEGIN
    DECLARE @hasImpactedComponents INT
    DECLARE @facilityID INT
    DECLARE @magnitude FLOAT
    DECLARE @duration FLOAT
    
    SELECT
        @facilityID = FacilityID,
        @magnitude = PerUnitMagnitude,
        @duration = DurationSeconds
    FROM
        Disturbance JOIN
        Event ON Disturbance.EventID = Event.ID JOIN
        MeterFacility ON MeterFacility.MeterID = Event.MeterID
    WHERE Disturbance.ID = @disturbanceID
    
    ; WITH EPRITolerancePoint AS
    (
        SELECT
            ROW_NUMBER() OVER(PARTITION BY TestCurve.TestCurveID ORDER BY Y, X) AS RowNumber,
            ComponentID,
            TestCurve.TestCurveID,
            Y AS Magnitude,
            X AS Duration
        FROM
            IndustrialPQ.dbo.TestCurvePoint JOIN
            IndustrialPQ.dbo.TestCurve ON TestCurvePoint.TestCurveID = TestCurve.TestCurveID
    ),
    EPRIToleranceSegment AS
    (
        SELECT
            P1.RowNumber,
            P1.ComponentID,
            P1.TestCurveID,
            CASE WHEN @duration < P1.Duration THEN 0
                 WHEN @magnitude < P1.Magnitude THEN 1
                 WHEN @duration > P2.Duration THEN 0
                 WHEN P1.Duration = P2.Duration THEN 0
                 WHEN @duration <= (((P2.Magnitude - P1.Magnitude) / (P2.Duration - P1.Duration)) * (@duration - P1.Duration) + P1.Magnitude) THEN 1
                 ELSE 0
            END AS IsUnder,
            CASE WHEN P1.Duration > P2.Duration THEN 1
                 ELSE 0
            END AS IsBackwards
        FROM
            EPRITolerancePoint P1 LEFT OUTER JOIN
            EPRITolerancePoint P2 ON P1.TestCurveID = P2.TestCurveID AND P1.RowNumber = P2.RowNumber - 1
        WHERE
            P1.Duration <> P2.Duration
    ),
    EPRIToleranceCurve AS
    (
        SELECT
            ComponentID,
            TestCurveID,
            IsUnder
        FROM
        (
            SELECT
                ROW_NUMBER() OVER(PARTITION BY TestCurveID ORDER BY RowNumber) AS RowNumber,
                ComponentID,
                TestCurveID,
                1 - IsBackwards AS IsUnder
            FROM
                EPRIToleranceSegment
            WHERE
                IsUnder <> 0
        ) AS Temp
        WHERE
            RowNumber = 1
    ),
    USERTolerancePoint AS
    (
        SELECT
            ROW_NUMBER() OVER(PARTITION BY TestCurve.TestCurveID ORDER BY Y, X) AS RowNumber,
            ComponentID,
            TestCurve.TestCurveID,
            Y AS Magnitude,
            X AS Duration
        FROM
            UserIndustrialPQ.dbo.TestCurvePoint JOIN
            UserIndustrialPQ.dbo.TestCurve ON TestCurvePoint.TestCurveID = TestCurve.TestCurveID
    ),
    USERToleranceSegment AS
    (
        SELECT
            P1.RowNumber,
            P1.ComponentID,
            P1.TestCurveID,
            CASE WHEN @duration < P1.Duration THEN 0
                 WHEN @magnitude < P1.Magnitude THEN 1
                 WHEN @duration > P2.Duration THEN 0
                 WHEN P1.Duration = P2.Duration THEN 0
                 WHEN @duration <= (((P2.Magnitude - P1.Magnitude) / (P2.Duration - P1.Duration)) * (@duration - P1.Duration) + P1.Magnitude) THEN 1
                 ELSE 0
            END AS IsUnder,
            CASE WHEN P1.Duration > P2.Duration THEN 1
                 ELSE 0
            END AS IsBackwards
        FROM
            USERTolerancePoint P1 LEFT OUTER JOIN
            USERTolerancePoint P2 ON P1.TestCurveID = P2.TestCurveID AND P1.RowNumber = P2.RowNumber - 1
        WHERE
            P1.Duration <> P2.Duration
    ),
    USERToleranceCurve AS
    (
        SELECT
            ComponentID,
            TestCurveID,
            IsUnder
        FROM
        (
            SELECT
                ROW_NUMBER() OVER(PARTITION BY TestCurveID ORDER BY RowNumber) AS RowNumber,
                ComponentID,
                TestCurveID,
                1 - IsBackwards AS IsUnder
            FROM
                USERToleranceSegment
            WHERE
                IsUnder <> 0
        ) AS Temp
        WHERE
            RowNumber = 1
    ),
    FacilityCurve AS
    (
        SELECT
            CurveID,
            CurveDB,
            SectionTypeName,
            Title AS SectionTitle,
            Rank AS SectionRank,
            Content AS SectionContent
        FROM
            UserIndustrialPQ.dbo.FacilityAudit JOIN
            UserIndustrialPQ.dbo.AuditSection ON AuditSection.FacilityAuditID = FacilityAudit.FacilityAuditID JOIN
            UserIndustrialPQ.dbo.SectionType ON AuditSection.SectionTypeID = SectionType.SectionTypeID JOIN
            UserIndustrialPQ.dbo.AuditCurve ON AuditCurve.AuditSectionID = AuditSection.AuditSectionID
        WHERE
            AuditCurve.CurveType = 'TOLERANCE' AND
            FacilityAudit.FacilityID = @facilityID
    ),
    ImpactedComponentID AS
    (
        SELECT EPRIToleranceCurve.ComponentID
        FROM
            EPRIToleranceCurve JOIN
            FacilityCurve ON FacilityCurve.CurveID = EPRIToleranceCurve.TestCurveID
        WHERE
            CurveDB = 'EPRI' AND
            IsUnder <> 0
        UNION
        SELECT USERToleranceCurve.ComponentID
        FROM
            USERToleranceCurve JOIN
            FacilityCurve ON FacilityCurve.CurveID = USERToleranceCurve.TestCurveID
        WHERE
            CurveDB = 'USER' AND
            IsUnder <> 0
    )
    SELECT @hasImpactedComponents = CASE WHEN EXISTS (SELECT * FROM ImpactedComponentID) THEN 1 ELSE 0 END
    
    RETURN @hasImpactedComponents
END
GO

ALTER PROCEDURE GetPQIFacility
    @facilityID INT
AS BEGIN
    SELECT
        Facility.FacilityName,
        Facility.FacilityVoltage,
        Facility.UtilitySupplyVoltage,
        Address.Address1,
        Address.Address2,
        Address.City,
        Address.StateOrProvince,
        Address.PostalCode,
        Address.Country,
        Company.CompanyName,
        Company.Industry
    FROM
        UserIndustrialPQ.dbo.Facility JOIN
        UserIndustrialPQ.dbo.Address ON Facility.AddressID = Address.AddressID JOIN
        UserIndustrialPQ.dbo.Company ON Address.CompanyID = Company.CompanyID
    WHERE
        Facility.FacilityID = @facilityID
END
GO

ALTER PROCEDURE GetImpactedComponents
    @facilityID INT,
    @magnitude FLOAT,
    @duration FLOAT
AS BEGIN
    WITH EPRITolerancePoint AS
    (
        SELECT
            ROW_NUMBER() OVER(PARTITION BY TestCurve.TestCurveID ORDER BY Y, X) AS RowNumber,
            ComponentID,
            TestCurve.TestCurveID,
            Y AS Magnitude,
            X AS Duration
        FROM
            IndustrialPQ.dbo.TestCurvePoint JOIN
            IndustrialPQ.dbo.TestCurve ON TestCurvePoint.TestCurveID = TestCurve.TestCurveID
    ),
    EPRIToleranceSegment AS
    (
        SELECT
            P1.RowNumber,
            P1.ComponentID,
            P1.TestCurveID,
            CASE WHEN @duration < P1.Duration THEN 0
                 WHEN @magnitude < P1.Magnitude THEN 1
                 WHEN @duration > P2.Duration THEN 0
                 WHEN P1.Duration = P2.Duration THEN 0
                 WHEN @duration <= (((P2.Magnitude - P1.Magnitude) / (P2.Duration - P1.Duration)) * (@duration - P1.Duration) + P1.Magnitude) THEN 1
                 ELSE 0
            END AS IsUnder,
            CASE WHEN P1.Duration > P2.Duration THEN 1
                 ELSE 0
            END AS IsBackwards
        FROM
            EPRITolerancePoint P1 LEFT OUTER JOIN
            EPRITolerancePoint P2 ON P1.TestCurveID = P2.TestCurveID AND P1.RowNumber = P2.RowNumber - 1
        WHERE
            P1.Duration <> P2.Duration
    ),
    EPRIToleranceCurve AS
    (
        SELECT
            ComponentID,
            TestCurveID,
            IsUnder
        FROM
        (
            SELECT
                ROW_NUMBER() OVER(PARTITION BY TestCurveID ORDER BY RowNumber) AS RowNumber,
                ComponentID,
                TestCurveID,
                1 - IsBackwards AS IsUnder
            FROM
                EPRIToleranceSegment
            WHERE
                IsUnder <> 0
        ) AS Temp
        WHERE
            RowNumber = 1
    ),
    USERTolerancePoint AS
    (
        SELECT
            ROW_NUMBER() OVER(PARTITION BY TestCurve.TestCurveID ORDER BY Y, X) AS RowNumber,
            ComponentID,
            TestCurve.TestCurveID,
            Y AS Magnitude,
            X AS Duration
        FROM
            UserIndustrialPQ.dbo.TestCurvePoint JOIN
            UserIndustrialPQ.dbo.TestCurve ON TestCurvePoint.TestCurveID = TestCurve.TestCurveID
    ),
    USERToleranceSegment AS
    (
        SELECT
            P1.RowNumber,
            P1.ComponentID,
            P1.TestCurveID,
            CASE WHEN @duration < P1.Duration THEN 0
                 WHEN @magnitude < P1.Magnitude THEN 1
                 WHEN @duration > P2.Duration THEN 0
                 WHEN P1.Duration = P2.Duration THEN 0
                 WHEN @duration <= (((P2.Magnitude - P1.Magnitude) / (P2.Duration - P1.Duration)) * (@duration - P1.Duration) + P1.Magnitude) THEN 1
                 ELSE 0
            END AS IsUnder,
            CASE WHEN P1.Duration > P2.Duration THEN 1
                 ELSE 0
            END AS IsBackwards
        FROM
            USERTolerancePoint P1 LEFT OUTER JOIN
            USERTolerancePoint P2 ON P1.TestCurveID = P2.TestCurveID AND P1.RowNumber = P2.RowNumber - 1
        WHERE
            P1.Duration <> P2.Duration
    ),
    USERToleranceCurve AS
    (
        SELECT
            ComponentID,
            TestCurveID,
            IsUnder
        FROM
        (
            SELECT
                ROW_NUMBER() OVER(PARTITION BY TestCurveID ORDER BY RowNumber) AS RowNumber,
                ComponentID,
                TestCurveID,
                1 - IsBackwards AS IsUnder
            FROM
                USERToleranceSegment
            WHERE
                IsUnder <> 0
        ) AS Temp
        WHERE
            RowNumber = 1
    ),
    FacilityCurve AS
    (
        SELECT
            CurveID,
            CurveDB,
            Facility.FacilityName AS Facility,
            Area.Title AS Area,
            Equipment.Title AS SectionTitle,
            Equipment.Rank AS SectionRank
        FROM
            UserIndustrialPQ.dbo.FacilityAudit JOIN
            UserIndustrialPQ.dbo.Facility ON FacilityAudit.FacilityID = Facility.FacilityID JOIN
            UserIndustrialPQ.dbo.AuditSection Equipment ON Equipment.FacilityAuditID = FacilityAudit.FacilityAuditID JOIN
            UserIndustrialPQ.dbo.AuditSectionTree ON AuditSectionTree.AuditSectionChildID = Equipment.AuditSectionID JOIN
            UserIndustrialPQ.dbo.AuditSection Area ON AuditSectionTree.AuditSectionParentID = Area.AuditSectionID JOIN
            UserIndustrialPQ.dbo.AuditCurve ON AuditCurve.AuditSectionID = Equipment.AuditSectionID
        WHERE
            AuditCurve.CurveType = 'TOLERANCE' AND
            FacilityAudit.FacilityID = @facilityID
    )
    SELECT
        FacilityCurve.Facility,
        FacilityCurve.Area,
        FacilityCurve.SectionTitle,
        FacilityCurve.SectionRank,
        Component.ComponentModel,
        Manufacturer.ManufacturerName,
        Series.SeriesName,
        ComponentType.ComponentTypeName
    FROM
        IndustrialPQ.dbo.Component JOIN
        IndustrialPQ.dbo.Series ON Component.SeriesID = Series.SeriesID JOIN
        IndustrialPQ.dbo.Manufacturer ON Series.ManufacturerID = Manufacturer.ManufacturerID JOIN
        IndustrialPQ.dbo.ComponentType ON Component.ComponentTypeID = ComponentType.ComponentTypeID JOIN
        EPRIToleranceCurve ON EPRIToleranceCurve.ComponentID = Component.ComponentID JOIN
        FacilityCurve ON FacilityCurve.CurveID = EPRIToleranceCurve.TestCurveID
    WHERE
        CurveDB = 'EPRI' AND
        IsUnder <> 0
    UNION
    SELECT
        FacilityCurve.Facility,
        FacilityCurve.Area,
        FacilityCurve.SectionTitle,
        FacilityCurve.SectionRank,
        Component.ComponentModel,
        Manufacturer.ManufacturerName,
        Series.SeriesName,
        ComponentType.ComponentTypeName
    FROM
        UserIndustrialPQ.dbo.Component JOIN
        UserIndustrialPQ.dbo.Series ON Component.SeriesID = Series.SeriesID JOIN
        UserIndustrialPQ.dbo.Manufacturer ON Series.ManufacturerID = Manufacturer.ManufacturerID JOIN
        UserIndustrialPQ.dbo.ComponentType ON Component.ComponentTypeID = ComponentType.ComponentTypeID JOIN
        USERToleranceCurve ON USERToleranceCurve.ComponentID = Component.ComponentID JOIN
        FacilityCurve ON FacilityCurve.CurveID = USERToleranceCurve.TestCurveID
    WHERE
        CurveDB = 'USER' AND
        IsUnder <> 0
END
GO