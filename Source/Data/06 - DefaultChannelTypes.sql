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

INSERT INTO Phase(Name, Description) VALUES('NG', 'Neutral to ground')
GO

INSERT INTO Phase(Name, Description) VALUES('None', 'No phase')
GO

INSERT INTO Phase(Name, Description) VALUES('Worst', 'Worst value of all phases')
GO

INSERT INTO Phase(Name, Description) VALUES('LineToNeutralAverage', 'Average of line-to-neutral values')
GO

INSERT INTO Phase(Name, Description) VALUES('LineToLineAverage', 'Average of line-to-line values')
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average RMS Voltage Phase A', 'Average RMS Voltage Phase A', (SELECT ID FROM Unit WHERE Name = 'Volts'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS'), (SELECT ID FROM Phase WHERE Name = 'AN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average RMS Voltage Phase B', 'Average RMS Voltage Phase B', (SELECT ID FROM Unit WHERE Name = 'Volts'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS'), (SELECT ID FROM Phase WHERE Name = 'BN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average RMS Voltage Phase C', 'Average RMS Voltage Phase C', (SELECT ID FROM Unit WHERE Name = 'Volts'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS'), (SELECT ID FROM Phase WHERE Name = 'CN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Voltage Unbalanace(S2/S1)', 'Average Voltage Unbalanace(S2/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Voltage Unbalanace(S0/S1)', 'Average Voltage Unbalanace(S0/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Pst Flicker Phase A', 'Pst Flicker Phase A', (SELECT ID FROM Unit WHERE Name = 'Per Unit'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST'), (SELECT ID FROM Phase WHERE Name = 'AN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Pst Flicker Phase B', 'Pst Flicker Phase B', (SELECT ID FROM Unit WHERE Name = 'Per Unit'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST'), (SELECT ID FROM Phase WHERE Name = 'BN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Pst Flicker Phase C', 'Pst Flicker Phase C', (SELECT ID FROM Unit WHERE Name = 'Per Unit'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST'), (SELECT ID FROM Phase WHERE Name = 'CN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Vthd Phase A', 'Average Vthd Phase A', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'AN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Vthd Phase B', 'Average Vthd Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'BN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Vthd Phase C', 'Average Vthd Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'CN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H3 Phase A', 'Average V H3 Phase A', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H3 Phase B', 'Average V H3 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H3 Phase C', 'Average V H3 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H5 Phase A', 'Average V H5 Phase A', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H5 Phase B', 'Average V H5 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H5 Phase C', 'Average V H5 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H7 Phase A', 'Average V H7 Phase A', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H7 Phase B', 'Average V H7 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H7 Phase C', 'Average V H7 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H11 Phase A', 'Average V H11 Phase A', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H11 Phase B', 'Average V H11 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H11 Phase C', 'Average V H11 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H13 Phase A', 'Average V H13 Phase A', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H13 Phase B', 'Average V H13 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average V H13 Phase C', 'Average V H13 Phase C', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Current Unbalanace(S2/S1)', 'Average Current Unbalanace(S2/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Current Unbalanace(S0/S1)', 'Average Current Unbalanace(S0/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Ithd Phase A', 'Average Ithd Phase A', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'AN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Ithd Phase B', 'Average Ithd Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'BN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Ithd Phase C', 'Average Ithd Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'CN'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Ithd Phase N', 'Average Ithd Phase N', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'NG'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Ithd Phase RES', 'Average Ithd Phase RES', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'), (SELECT ID FROM Phase WHERE Name = 'RES'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H3 Phase A', 'Average I H3 Phase A', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H3 Phase B', 'Average I H3 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H3 Phase C', 'Average I H3 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H3 Phase N', 'Average I H3 Phase N', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'NG'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H3 Phase RES', 'Average I H3 Phase RES', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'RES'), 3)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H5 Phase A', 'Average I H5 Phase A', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H5 Phase B', 'Average I H5 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H5 Phase C', 'Average I H5 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H5 Phase N', 'Average I H5 Phase N', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'NG'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H5 Phase RES', 'Average I H5 Phase RES', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'RES'), 5)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H7 Phase A', 'Average I H7 Phase A', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H7 Phase B', 'Average I H7 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H7 Phase C', 'Average I H7 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H7 Phase N', 'Average I H7 Phase N', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'NG'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H7 Phase RES', 'Average I H7 Phase RES', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'RES'), 7)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H11 Phase A', 'Average I H11 Phase A', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H11 Phase B', 'Average I H11 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H11 Phase C', 'Average I H11 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H11 Phase N', 'Average I H11 Phase N', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'NG'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H11 Phase RES', 'Average I H11 Phase RES', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'RES'), 11)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H13 Phase A', 'Average I H13 Phase A', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'AN'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H13 Phase B', 'Average I H13 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'BN'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H13 Phase C', 'Average I H13 Phase C', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'CN'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H13 Phase N', 'Average I H13 Phase N', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'NG'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average I H13 Phase RES', 'Average I H13 Phase RES', (SELECT ID FROM Unit WHERE Name = 'Amps'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'), (SELECT ID FROM Phase WHERE Name = 'RES'), 13)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('P', 'P', (SELECT ID FROM Unit WHERE Name = 'KW'), (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'P'), (SELECT ID FROM Phase WHERE Name = 'LineToNeutralAverage'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Q', 'Q', (SELECT ID FROM Unit WHERE Name = 'KVAR'), (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Q'), (SELECT ID FROM Phase WHERE Name = 'LineToNeutralAverage'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('S', 'S', (SELECT ID FROM Unit WHERE Name = 'KVA'), (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S'), (SELECT ID FROM Phase WHERE Name = 'LineToNeutralAverage'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('PF', 'PF', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PF'), (SELECT ID FROM Phase WHERE Name = 'LineToNeutralAverage'), 0)
GO
