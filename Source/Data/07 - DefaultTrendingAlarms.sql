INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 360, NULL, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'AngleFund'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'AvgImbal'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 30, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'FlkrPLT'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'FlkrPST'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 70, 50, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'Frequency'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 500, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'Instantaneous'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1.4, 0, 0, 1
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'RMS'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SNeg'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SpectraHGroup'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SPos'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SZero'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'TotalTHD'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 50, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'TotalTHDRMS'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 360, NULL, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'AngleFund'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10, 1, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'CrestFactor'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'HRMS'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'IHRMS'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 200, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'Instantaneous'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 60000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'IT'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 20000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'RMS'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 20000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'RMSPeakDemand'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SNeg'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SpectraHGroup'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 50000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SPos'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SZero'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'TDD'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'TID'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 10000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'TIDRMS'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1000000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'P'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1000000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PDemand'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PF'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PFDemand'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1000000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PPeakDemand'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'QDemand'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'QFund'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1000000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'S'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 100000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'SDemand'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Digital' AND MChar.Name = 'BreakerStatus'
GO

INSERT INTO DefaultDataQualityRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Digital' AND MChar.Name = 'TCE'
GO
