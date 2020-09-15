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

INSERT INTO MeasurementType(Name, Description) VALUES ('Analog', 'Analog')
GO


INSERT INTO MeasurementType(Name, Description) VALUES ('TripCoilCurrent', 'Relay Trip Coil Energization Current')
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

INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Trigger - RMS', 'RMS Trigger', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Trigger - Impulse', 'Impulse Trigger', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Trigger - THD', 'THD Trigger', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Trigger - Ubal', 'Unbalance Trigger', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Trigger - I', 'Instantanous Current Trigger', 0)
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

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Voltage Unbalance(S2/S1)', 'Average Voltage Unbalance(S2/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Voltage Unbalance(S0/S1)', 'Average Voltage Unbalance(S0/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
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

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Current Unbalance(S2/S1)', 'Average Current Unbalance(S2/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
GO

INSERT INTO PQMeasurement(Name, Description, UnitID, MeasurementTypeID, MeasurementCharacteristicID, PhaseID, HarmonicGroup) VALUES('Average Current Unbalance(S0/S1)', 'Average Current Unbalance(S0/S1)', (SELECT ID FROM Unit WHERE Name = 'Percent'), (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'), (SELECT ID FROM Phase WHERE Name = 'None'), 0)
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

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average RMS Voltage Phase A'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average RMS Voltage Phase B'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average RMS Voltage Phase C'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Pst Flicker Phase A'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Pst Flicker Phase B'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Pst Flicker Phase C'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Vthd Phase A'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Vthd Phase B'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Vthd Phase C'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Voltage Unbalance(S2/S1)'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Voltage Unbalance(S0/S1)'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Current Unbalance(S2/S1)'), 5)
GO

INSERT INTO StepChangeMeasurement (PQMeasurementID, Setting) VALUES ((SELECT ID FROM PQMeasurement WHERE Name = 'Average Current Unbalance(S0/S1)'), 5)
GO

-- Default Asset Types
INSERT INTO AssetType (Name, Description) VALUES ('Line','Transmission Line')
GO
INSERT INTO AssetType (Name, Description) VALUES ('Bus','Bus')
GO
INSERT INTO AssetType (Name, Description) VALUES ('Breaker','Breaker')
GO
INSERT INTO AssetType (Name, Description) VALUES ('CapacitorBank','Bank of Capacitors')
GO
INSERT INTO AssetType (Name, Description) VALUES ('LineSegment','Segment of a Transmission Line')
GO
INSERT INTO AssetType (Name, Description) VALUES ('Transformer','Transformer')
GO
INSERT INTO AssetType (Name, Description) VALUES ('CapacitorBankRelay','Relay for a Capacitor Bank')
GO

-- Default Asset Connections
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-LineSegment','No measurements are passed across this connection',1,'SELECT 0','SELECT 0')
GO

INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Line','only Voltages are passed across this connection.',1,'SELECT (CASE WHEN {0} = 1 THEN 1 ELSE 0 END)','SELECT 0')
GO

INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Breaker','only Voltages and Breaker Status are passed across this connection.',1,'SELECT (CASE WHEN {0} = 1 OR {0} = 5 THEN 1 ELSE 0 END)','SELECT 0')
GO

INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-(Single)Breaker','Currents, Voltage and Breaker Status are passed across this connection.',1,'SELECT (CASE WHEN {0} = 1 OR {0} = 5 OR {0} = 2 THEN 1 ELSE 0 END)','SELECT 1')
GO

INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-(Double)Breaker','only Voltage are passed across this connection.',1,'SELECT (CASE WHEN {0} = 1 THEN 1 ELSE 0 END)','SELECT (CASE WHEN {0} = 1 THEN 1 ELSE 0 END)')
GO

INSERT INTO CBDataError (ID, Description) VALUES
	(0, 'No Error'),
	(11, 'Error: One or more input parameters are incorrect.'),
	(12, 'Error: One or more waveforms are missing or data lengths or all waveforms are not the same or data length of each waveform is less than 24 cycles'),
	(13, 'Error: Non-uniform sampling time or sample rates of waveforms do not match those specified in the input parameters')
GO

INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-CapBank','only Voltages are passed across this connection.',1,'SELECT (CASE WHEN {0} = 1 THEN 1 ELSE 0 END)','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Relay','only Voltages are passed across this connection.',0,'SELECT 0','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Relay-CapBank','This Connection is for the Cap Bank Analytic',0,'SELECT 0','SELECT 0')
GO

INSERT INTO CBOperation (ID, Description) VALUES
	(-1, 'Cannot be determined (inconclusive) or not determined because of data error'),
	(101,'Opening operation: final opening. After de-energizing, the number cap bank is zero.'),
	(102, 'Opening operation: next step opening. After de-energizing, the number of cap bank is reduced by one with at least one remaining in service.'),
	(201, 'Closing operation: first closing. Initially there is no cap bank in service. After the operation, the number of cap bank in service is one.'),
	(202, 'Closing operation: next step closing. After the energizing operation, the number of cap bank increases by one.'),
	(-101,'No switching operation. Current at the beginning and end are near zero, no cap.bank is in service'),
	(-102, 'No switching operation. Current at the beginning and end are about the same,but not zero. There can be one or more cap. banks in service'),
	(-103,'No switching operation. There is at least one cap. bank in service. Current at the end is higher than that the beginning more than the tolerance, indicating shorted units or restrike/reignition conditions.'),
	(-200,'Not a capacitor switching operation, because a voltage sag/swell below 0.85 andabove 1.08 pu is detected')
GO

INSERT INTO CBStatus (ID, Description) VALUES
	(-1,'Data error'),
	(0,'All pole closing, normal closing, normal opening'),
	(10,'Failed closing: one pole does not close, while the other two close'),
	(11,'Missing pole, this is a follow-on of failed closing. Two poles open, while one has zero current all the time'),
	(12,'The longest time between two closing or opening poles is more than 2 cycles'),
	(2,'Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(20,'Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(21,'Blown fuses detected'),
	(22,'Missing tap voltage, blown resistors, or other unhealthy condition'),
	(3,'Failed opening, a pole never opens or failed opening during next step opening'),
	(4,'Restrike or reignition without failed opening'),
	(5,'Restrike or reignition and failed opening'),
	(6,'Voltage waveform may have a sag, swell, or other non-sinusoidal features. Current waveforms may have sporadic transients such as those due to loose connections'),
	(7,'No switching operations. (1) Current at the beginning and end are near zero. No cap. bank is in service. (2) Current at the beginning and end are not zero but correspond to that of energized banks. One or more cap banks are in service'),
	(8,'Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long')
GO

INSERT INTO CBRestrikeType (ID, Description) VALUES
	(0,'No restrike'),
	(10,'Possible restrike or reignition on this phase'),
	(20,'Possible restrike or reignition on the other one or two phases of this bank, but not on this phase'),
	(31,'Reignition on this phase, with current gap less than 0.25 cycle'),
	(32,'Restrike on this phase, with a current gap of longer than 0.25 cycle'),
	(41,'Reignition on this phase, with voltage polarity reversal'),
	(42,'Restrike on this phase, with voltage polarity reversal')
GO

INSERT INTO CBSwitchingCondition (ID, Description) VALUES
	(0,'Normal'),
	(1,'One transient present, switched-in without pre-insertion or never switched-out (stuck)'),
	(2,'duration too short (less than 1 cycle) or long (more than 12 cycles)'),
	(3,'Unknown or undetermined')
GO

INSERT INTO CBBankHealth (ID, Description) VALUES
	(0,'Capacitor bank and relay are healthy'),
	(1,'Some fuseless units are shorted, or fused units shorted because fuses failed to clear'),
	(2,'Some fused units have blown fuses'),
	(3,'Mid-rack tap voltages are missing')
GO