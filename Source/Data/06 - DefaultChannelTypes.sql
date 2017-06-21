INSERT INTO MeasurementType(Name, Description) VALUES ('Voltage', 'Voltage')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Current', 'Current')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Power', 'Power')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Energy', 'Energy')
GO

INSERT INTO MeasurementType(Name, Description) VALUES ('Digital', 'Digital')
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

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('BreakerStatus', 'Breaker Status', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TCE', 'Trip Coil Energized', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Q', 'Reactive power (VAR)', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PIVLIntgPos', 'Value of active power integrated over time (Energy - watt-hours) in the positive direction (toward load).', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('QIVLIntgPos', 'Value of reactive power integrated over time (Energy - watt-hours) in the positive direction (toward load).', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Peak', 'Peak value', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FlkrMagAvg', 'Flicker average RMS value', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('EvenTHD', 'Even harmonic distortion (%)', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('OddTHD', 'Odd harmonic distortion (%)', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FormFactor', 'Form factor', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('ArithSum', 'Arithmetic sum', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('S0S1', 'Zero sequence component unbalance (%)', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('S2S1', 'Negative sequence component unbalance (%)', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TIF', 'TIF', 0)
GO

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('DF', 'Displacement Factor - Cosine of the phase angle between fundamental frequency voltage and current phasors.', 0)
GO


INSERT INTO Phase(Name, Description) VALUES('AN', 'A-phase to neutral')
GO

INSERT INTO Phase(Name, Description) VALUES('BN', 'B-phase to neutral')
GO

INSERT INTO Phase(Name, Description) VALUES('CN', 'C-phase to neutral')
GO

INSERT INTO Phase(Name, Description) VALUES('AB', 'A-phase to B-phase')
GO

INSERT INTO Phase(Name, Description) VALUES('BC', 'B-phase to C-phase')
GO

INSERT INTO Phase(Name, Description) VALUES('CA', 'C-phase to A-phase')
GO

INSERT INTO Phase(Name, Description) VALUES('RES', 'Residual')
GO

INSERT INTO Phase(Name, Description) VALUES('None', 'No phase')
GO

INSERT INTO Phase(Name, Description) VALUES('Worst', 'Worst value of all phases')
GO

INSERT INTO Phase(Name, Description) VALUES('LineToNeutralAverage', 'Average of line-to-neutral values')
GO

INSERT INTO Phase(Name, Description) VALUES('LineToLineAverage', 'Average of line-to-line values')
GO
