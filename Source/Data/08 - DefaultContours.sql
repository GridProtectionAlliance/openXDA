INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Voltage RMS', 1.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 1 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'RMS' AND Phase.Name IN ('AN', 'BN', 'CN', 'AB', 'BC', 'CA')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 1 ContourColorScaleID,    -1 Value, 0xAAFF0000 Color,  1 OrderID UNION ALL
SELECT 1 ContourColorScaleID,     0 Value, 0xAAFF0000 Color,  2 OrderID UNION ALL
SELECT 1 ContourColorScaleID,     0 Value, 0xAAFFFF00 Color,  3 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 0.225 Value, 0xAAFFFF00 Color,  4 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 0.225 Value, 0xAA00FF00 Color,  5 OrderID UNION ALL
SELECT 1 ContourColorScaleID,  0.45 Value, 0xAA00FF00 Color,  6 OrderID UNION ALL
SELECT 1 ContourColorScaleID,  0.45 Value, 0xAA00FFFF Color,  7 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 0.675 Value, 0xAA00FFFF Color,  8 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 0.675 Value, 0xAA0000FF Color,  9 OrderID UNION ALL
SELECT 1 ContourColorScaleID,   0.9 Value, 0xAA0000FF Color, 10 OrderID UNION ALL
SELECT 1 ContourColorScaleID,   0.9 Value, 0x66444444 Color, 11 OrderID UNION ALL
SELECT 1 ContourColorScaleID,   1.1 Value, 0x66444444 Color, 12 OrderID UNION ALL
SELECT 1 ContourColorScaleID,   1.1 Value, 0xAA0000FF Color, 13 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 1.325 Value, 0xAA0000FF Color, 14 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 1.325 Value, 0xAA00FFFF Color, 15 OrderID UNION ALL
SELECT 1 ContourColorScaleID,  1.55 Value, 0xAA00FFFF Color, 16 OrderID UNION ALL
SELECT 1 ContourColorScaleID,  1.55 Value, 0xAA00FF00 Color, 17 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 1.775 Value, 0xAA00FF00 Color, 18 OrderID UNION ALL
SELECT 1 ContourColorScaleID, 1.775 Value, 0xAAFFFF00 Color, 19 OrderID UNION ALL
SELECT 1 ContourColorScaleID,     2 Value, 0xAAFFFF00 Color, 20 OrderID UNION ALL
SELECT 1 ContourColorScaleID,     2 Value, 0xAAFF0000 Color, 21 OrderID UNION ALL
SELECT 1 ContourColorScaleID,     3 Value, 0xAAFF0000 Color, 22 OrderID
GO

INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Voltage THD (L-G)', 0.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 2 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'TotalTHD' AND Phase.Name IN ('AN', 'BN', 'CN')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 2 ContourColorScaleID, 0 Value, 0x66444444 Color,  1 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 1 Value, 0x66444444 Color,  2 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 1 Value, 0xAA0000FF Color,  3 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 2 Value, 0xAA0000FF Color,  4 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 2 Value, 0xAA00FFFF Color,  5 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 3 Value, 0xAA00FFFF Color,  6 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 3 Value, 0xAA00FF00 Color,  7 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 4 Value, 0xAA00FF00 Color,  8 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 4 Value, 0xAAFFFF00 Color,  9 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 5 Value, 0xAAFFFF00 Color, 10 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 5 Value, 0xAAFF0000 Color, 11 OrderID UNION ALL
SELECT 2 ContourColorScaleID, 6 Value, 0xAAFF0000 Color, 12 OrderID
GO

INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Voltage THD (AN)', 0.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 3 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'TotalTHD' AND Phase.Name IN ('AN')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 3 ContourColorScaleID, 0 Value, 0x66444444 Color,  1 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 1 Value, 0x66444444 Color,  2 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 1 Value, 0xAA0000FF Color,  3 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 2 Value, 0xAA0000FF Color,  4 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 2 Value, 0xAA00FFFF Color,  5 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 3 Value, 0xAA00FFFF Color,  6 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 3 Value, 0xAA00FF00 Color,  7 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 4 Value, 0xAA00FF00 Color,  8 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 4 Value, 0xAAFFFF00 Color,  9 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 5 Value, 0xAAFFFF00 Color, 10 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 5 Value, 0xAAFF0000 Color, 11 OrderID UNION ALL
SELECT 3 ContourColorScaleID, 6 Value, 0xAAFF0000 Color, 12 OrderID
GO

INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Voltage THD (BN)', 0.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 4 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'TotalTHD' AND Phase.Name IN ('BN')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 4 ContourColorScaleID, 0 Value, 0x66444444 Color,  1 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 1 Value, 0x66444444 Color,  2 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 1 Value, 0xAA0000FF Color,  3 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 2 Value, 0xAA0000FF Color,  4 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 2 Value, 0xAA00FFFF Color,  5 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 3 Value, 0xAA00FFFF Color,  6 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 3 Value, 0xAA00FF00 Color,  7 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 4 Value, 0xAA00FF00 Color,  8 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 4 Value, 0xAAFFFF00 Color,  9 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 5 Value, 0xAAFFFF00 Color, 10 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 5 Value, 0xAAFF0000 Color, 11 OrderID UNION ALL
SELECT 4 ContourColorScaleID, 6 Value, 0xAAFF0000 Color, 12 OrderID
GO

INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Voltage THD (CN)', 0.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 5 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'TotalTHD' AND Phase.Name IN ('CN')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 5 ContourColorScaleID, 0 Value, 0x66444444 Color,  1 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 1 Value, 0x66444444 Color,  2 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 1 Value, 0xAA0000FF Color,  3 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 2 Value, 0xAA0000FF Color,  4 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 2 Value, 0xAA00FFFF Color,  5 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 3 Value, 0xAA00FFFF Color,  6 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 3 Value, 0xAA00FF00 Color,  7 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 4 Value, 0xAA00FF00 Color,  8 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 4 Value, 0xAAFFFF00 Color,  9 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 5 Value, 0xAAFFFF00 Color, 10 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 5 Value, 0xAAFF0000 Color, 11 OrderID UNION ALL
SELECT 5 ContourColorScaleID, 6 Value, 0xAAFF0000 Color, 12 OrderID
GO

INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Flicker PST', 0.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 6 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'FlkrPST' AND Phase.Name IN ('AN', 'BN', 'CN')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 6 ContourColorScaleID,   0 Value, 0x66444444 Color,  1 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.2 Value, 0x66444444 Color,  2 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.2 Value, 0xAA0000FF Color,  3 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.4 Value, 0xAA0000FF Color,  4 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.4 Value, 0xAA00FFFF Color,  5 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.6 Value, 0xAA00FFFF Color,  6 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.6 Value, 0xAA00FF00 Color,  7 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.8 Value, 0xAA00FF00 Color,  8 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 0.8 Value, 0xAAFFFF00 Color,  9 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 1.0 Value, 0xAAFFFF00 Color, 10 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 1.0 Value, 0xAAFF0000 Color, 11 OrderID UNION ALL
SELECT 6 ContourColorScaleID, 1.2 Value, 0xAAFF0000 Color, 12 OrderID
GO

INSERT INTO ContourColorScale(Name, NominalValue)
VALUES('Flicker PLT', 0.0)
GO

INSERT INTO ContourChannelType(ContourColorScaleID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID)
SELECT 7 ContourColorScaleID, MeasurementType.ID MeasurementTypeID, MeasurementCharacteristic.ID MeasurementCharacteristicID, Phase.ID PhaseID
FROM MeasurementType CROSS JOIN MeasurementCharacteristic CROSS JOIN Phase
WHERE MeasurementType.Name = 'Voltage' AND MeasurementCharacteristic.Name = 'FlkrPST' AND Phase.Name IN ('AN', 'BN', 'CN')
GO

INSERT INTO ContourColorScalePoint(ContourColorScaleID, Value, Color, OrderID)
SELECT 7 ContourColorScaleID,    0 Value, 0x66444444 Color,  1 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.16 Value, 0x66444444 Color,  2 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.16 Value, 0xAA0000FF Color,  3 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.32 Value, 0xAA0000FF Color,  4 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.32 Value, 0xAA00FFFF Color,  5 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.48 Value, 0xAA00FFFF Color,  6 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.48 Value, 0xAA00FF00 Color,  7 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.64 Value, 0xAA00FF00 Color,  8 OrderID UNION ALL
SELECT 7 ContourColorScaleID, 0.64 Value, 0xAAFFFF00 Color,  9 OrderID UNION ALL
SELECT 7 ContourColorScaleID,  0.8 Value, 0xAAFFFF00 Color, 10 OrderID UNION ALL
SELECT 7 ContourColorScaleID,  0.8 Value, 0xAAFF0000 Color, 11 OrderID UNION ALL
SELECT 7 ContourColorScaleID,  1.0 Value, 0xAAFF0000 Color, 12 OrderID
GO
