INSERT INTO MeasurementType(Name, Description) VALUES ('Voltage', 'Voltage')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Current', 'Current')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Power', 'Power')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Energy', 'Energy')
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('AngleFund', 'Power angle at fundamental frequency', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('AvgImbal', 'Imbalance by max deviation from average', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('CrestFactor', 'Crest factor', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FlkrPLT', 'Long term flicker', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FlkrPST', 'Short term flicker', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Frequency', 'Frequency', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('HRMS', 'Harmonic RMS', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('IHRMS', 'Interharmonic RMS', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Instantaneous', 'Instantaneous', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('IT', 'IT', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('None', 'None', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('P', 'Power', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PDemand', 'Active power for a demand interval', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PF', 'True power factor', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PFDemand', 'True power factor for a demand interval', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PIntg', 'Active power integrated over time', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PPeakDemand', 'Peak active power demand', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('QDemand', 'Reactive power for a demand interval', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('QFund', 'Reactive power at fundamental frequency', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('QIntg', 'Reactive power integrated over time', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('RMS', 'RMS', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('RMSPeakDemand', 'Peak demand current', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('S', 'Apparent power', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SDemand', 'Apparent power for a demand interval', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SNeg', 'Negative sequence component', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SpectraHGroup', 'Spectra by harmonic group index', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SPos', 'Positive sequence component', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SZero', 'Negative sequence component', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TDD', 'Total demand distortion', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TID', 'Total interharmonic distortion', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TIDRMS', 'Total interharmonic distortion normalized to RMS', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TotalTHD', 'Total harmonic distortion', 1)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TotalTHDRMS', 'Total THD normalized to RMS', 1)
GO

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
SELECT MType.ID, MChar.ID, 1.4, 0, 0, 0
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

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 8, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'AvgImbal'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 0.8, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'FlkrPLT'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'FlkrPST'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 60.2, 59.8, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'Frequency'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1.1, 0.9, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'Instantaneous'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1.1, 0.9, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'RMS'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SNeg'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 5, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SpectraHGroup'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 10000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SPos'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 25, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'SZero'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 5, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'TotalTHD'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 5, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Voltage' AND MChar.Name = 'TotalTHDRMS'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1.6, 1.3, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'CrestFactor'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 25, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'HRMS'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 10, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'IHRMS'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 900, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'Instantaneous'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 900, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'RMS'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 700, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'RMSPeakDemand'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 100, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SNeg'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 5, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SpectraHGroup'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SPos'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 25, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'SZero'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 10, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'TDD'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 2, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'TID'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 20, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Current' AND MChar.Name = 'TIDRMS'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 3000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'P'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 10000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PDemand'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1, 0.9, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PF'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1, 0.9, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PFDemand'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 20000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'PPeakDemand'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 2000000, -2000000, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'QDemand'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 500000, -500000, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'QFund'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 4000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'S'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 11000000, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Power' AND MChar.Name = 'SDemand'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Digital' AND MChar.Name = 'BreakerStatus'
GO

INSERT INTO DefaultAlarmRangeLimit(MeasurementTypeID, MeasurementCharacteristicID, AlarmTypeID, Severity, High, Low, RangeInclusive, PerUnit)
SELECT MType.ID, MChar.ID, 5, 900, 1, 0, 0, 0
FROM MeasurementType MType CROSS JOIN MeasurementCharacteristic MChar
WHERE MType.Name = 'Digital' AND MChar.Name = 'TCE'
GO
