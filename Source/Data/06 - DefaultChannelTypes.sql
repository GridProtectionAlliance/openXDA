INSERT INTO ValueListGroup (Name, Description) VALUES ('Unit', 'List of Units associated with Channels')
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'V', 'V',1)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'A', 'A',2)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), '%', '%',3)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'Pst', 'Pst',4)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'Plt', 'Plt',5)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'Degree', 'Degree',6)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'Hz', 'Hz',7)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'VA', 'VA',8)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'W', 'W',9)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'VAR', 'VAR',10)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'None', 'None',11)
GO
INSERT INTO ValueList (GroupID, Value, AltValue, SortOrder) VALUES ((SELECT ID FROM ValueListGroup WHERE Name ='Unit'), 'Unknown', 'Unknown',12)
GO

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
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('ArithSum', 'Arithmetic sum', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('AvgImbal', 'Imbalance by max deviation from average', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('BreakerStatus', 'Breaker Status', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('CrestFactor', 'Crest factor', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('DF', 'Displacement Factor - Cosine of the phase angle between fundamental frequency voltage and current phasors.', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('DFArith', 'Displacement Factor arithmetic sum', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('DFVector', 'Displacement Factor vector', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('EvenTHD', 'Even harmonic distortion (%)', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FlkrMagAvg', 'Flicker average RMS value', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FlkrPLT', 'Long term flicker', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FlkrPST', 'Short term flicker', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('FormFactor', 'Form factor', 0)
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
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('KFactor', 'Transformer K factor', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('None', 'None', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('OddTHD', 'Odd harmonic distortion (%)', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('P', 'Power', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PDemand', 'Active power for a demand interval', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Peak', 'Peak value', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PF', 'True power factor', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PFArith', 'Power factor arithmatic sum', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PFDemand', 'True power factor for a demand interval', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PFVector', 'Power factor vector', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PHarmonic', 'Active power harmonic', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PIntg', 'Active power integrated over time', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PIVLIntgPos', 'Value of active power integrated over time (Energy - watt-hours) in the positive direction (toward load).', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('PLTSlide', 'Sliding PLT', 0)
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
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('S0S1', 'Zero sequence component unbalance (%)', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('S2S1', 'Negative sequence component unbalance (%)', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SArith', 'Apparent power arithmetic sum', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SArithFund', 'Apparent power arithmetic sum at fundamental frequency', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SDemand', 'Apparent power for a demand interval', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SIntgFund', 'Apparent power integrated over time at fundamental frequency', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SNeg', 'Negative sequence component', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Spectra', 'Spectra', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SpectraHGroup', 'Spectra by harmonic group index', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SpectraIGroup', 'Spectra by interharmonic group index', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SPos', 'Positive sequence component', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SVector', 'Apparent power Vector', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SVectorFund', 'Apparent power Vector at fundamental frequency', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('SZero', 'Negative sequence component', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TCE', 'Trip Coil Energized', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TDD', 'Total demand distortion', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TID', 'Total interharmonic distortion', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TIDRMS', 'Total interharmonic distortion normalized to RMS', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TIF', 'TIF', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TotalTHD', 'Total harmonic distortion', 1)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('TotalTHDRMS', 'Total THD normalized to RMS', 1)
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
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('Q', 'Reactive power (VAR)', 0)
GO
INSERT INTO MeasurementCharacteristic(Name, Description, Display) VALUES ('QIVLIntgPos', 'Value of reactive power integrated over time (Energy - watt-hours) in the positive direction (toward load).', 0)
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

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('RMS Voltage', 'RMS Voltage', 'Volts', (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Voltage Unbalance(S2/S1)', 'Voltage Unbalance(S2/S1)', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Voltage Unbalance(S0/S1)', 'Voltage Unbalance(S0/S1)', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Flicker Pst', 'Short Term Flicker Perceptibility (Pst)', 'Per Unit', (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Voltage THD', 'Voltage THD', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Voltage Harmonic', 'Voltage Harmonic', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Voltage'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Current Unbalance(S2/S1)', 'Current Unbalance(S2/S1)', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Current Unbalance(S0/S1)', 'Current Unbalance(S0/S1)', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Current THD', 'Current THD', 'Amps', (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Current Harmonic', 'Current Harmonic', 'Amps', (SELECT ID FROM MeasurementType WHERE Name = 'Current'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Active Power', 'Active Power', 'KW', (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'P'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Reactive Power', 'Reactive Power', 'KVAR', (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Q'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Apparent Power', 'Apparent Power', 'KVA', (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S'))
GO

INSERT INTO PQMeasurement(Name, Description, Unit, MeasurementTypeID, MeasurementCharacteristicID) VALUES('Power Factor', 'Power Factor', 'Percent', (SELECT ID FROM MeasurementType WHERE Name = 'Power'), (SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PF'))
GO

-- Channel Groupings
INSERT INTO ChannelGroup(Name, Description) VALUES ('Voltage', 'Voltage')
GO

INSERT INTO ChannelGroup(Name, Description) VALUES ('Current', 'Current')
GO

INSERT INTO ChannelGroup(Name, Description) VALUES ('Power', 'Power')
GO

INSERT INTO ChannelGroup(Name, Description) VALUES ('Energy', 'Energy')
GO

INSERT INTO ChannelGroup(Name, Description) VALUES ('CapBank', 'CapBank')
GO

INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS'),'V RMS', 'V')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'),'V TotalTHD', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPST'),'V FlkrPST', 'Pst')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FlkrPLT'),'V FlkrPLT', 'Plt')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'),'V SpectraHGroup', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AngleFund'),'V AngleFund', 'Degree')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AvgImbal'),'V AvgImbal', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Frequency'),'V Frequency', 'Hz')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'HRMS'),'V HRMS', 'V')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'None'),'V None', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SNeg'),'V SNeg', 'V')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SPos'),'V SPos', 'V')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SZero'),'V SZero', 'V')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'),'V S0S1', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'),'V S2S1', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PLTSlide'),'V PLTSlide', 'Plt')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'RMS'),'I RMS', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TotalTHD'),'I TotalTHD', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraHGroup'),'I SpectraHGroup', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AngleFund'),'I AngleFund', 'Degree')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AvgImbal'),'I AvgImbal', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'HRMS'),'I HRMS', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SNeg'),'I SNeg', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SPos'),'I SPos', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SZero'),'I SZero', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S0S1'),'I S0S1', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S2S1'),'I S2S1', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Current'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'KFactor'),'I KFactor', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Energy'),(SELECT ID FROM MeasurementType WHERE Name = 'Energy'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PIntg'),'PIntg', 'VA')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Energy'),(SELECT ID FROM MeasurementType WHERE Name = 'Energy'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'QIntg'),'QIntg', 'VA')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Energy'),(SELECT ID FROM MeasurementType WHERE Name = 'Energy'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SIntgFund'),'SIntgFund', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'AngleFund'),'AngleFund', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'P'),'P', 'W')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PDemand'),'PDemand', 'W')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PF'),'PF', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PFDemand'),'PFDemand', 'None')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'QDemand'),'QDemand', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'QFund'),'QFund', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'S'),'S', 'VA')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SDemand'),'SDemand', 'VA')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Q'),'Q', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'DF'),'DF', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'DFArith'),'DFArith', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'DFVector'),'DFVector', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PFVector'),'PFVector', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PFArith'),'PFArith', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'PHarmonic'),'PHarmonic', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SVector'),'SVector', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SVectorFund'),'SVectorFund', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SArith'),'SArith', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'Power'),(SELECT ID FROM MeasurementType WHERE Name = 'Power'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SArithFund'),'SArithFund', 'VAR')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'CrestFactor'),'V CrestFactor', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Peak'),'V Peak', 'V')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'EvenTHD'),'V EvenTHD', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'OddTHD'),'V OddTHD', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FormFactor'),'V FormFactor', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'ArithSum'),'V ArithSum', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TIF'),'V TIF', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Spectra'),'V Spectra', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Voltage'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraIGroup'),'V SpectraIGroup', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'CrestFactor'),'I CrestFactor', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'IHRMS'),'IHRMS', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'IT'),'I IT', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'TID'),'I TID', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Peak'),'I Peak', 'A')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'EvenTHD'),'I EvenTHD', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'OddTHD'),'I OddTHD', '%')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'FormFactor'),'I FormFactor', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'ArithSum'),'I ArithSum', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'Spectra'),'I Spectra', 'Unknown')
GO
INSERT INTO ChannelGroupType (ChannelGroupID, MeasurementTypeID, MeasurementCharacteristicID, DisplayName, Unit) VALUES ((SELECT ID FROM ChannelGroup WHERE Name = 'CapBank'),(SELECT ID FROM MeasurementType WHERE Name = 'Current'),(SELECT ID FROM MeasurementCharacteristic WHERE Name = 'SpectraIGroup'),'I SpectraIGroup', 'Unknown')
GO





-- Default Asset Types
INSERT INTO AssetType (ID, Name, Description) VALUES (1,'Line','Transmission Line')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (2,'Bus','Bus')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (3,'Breaker','Breaker')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (4,'CapacitorBank','Bank of Capacitors')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (5,'LineSegment','Segment of a Transmission Line')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (6,'Transformer','Transformer')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (7,'CapacitorBankRelay','Relay for a Capacitor Bank')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (8,'DER','DER governed by IEEE Standard 1547-2018')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (9,'StationAux','Station Auxiliary')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (10,'StationBattery','Station Battery')
GO
INSERT INTO AssetType (ID, Name, Description) VALUES (11,'Generation','Generation')
GO

-- Default Asset Connections
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-LineSegment','No measurements are passed across this connection',1,'SELECT 0','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Line','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Breaker','only Voltages and Breaker Status are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name IN (''Voltage'', ''Digital'') THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-(Single)Breaker','Currents, Voltage and Breaker Status are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name IN (''Voltage'', ''Current'', ''Digital'') THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 1')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-(Double)Breaker','only Voltage are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-CapBank','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Relay','only Voltages are passed across this connection.',0,'SELECT 0','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Relay-CapBank','This Connection is for the Cap Bank Analytic',0,'SELECT 0','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Transformer-(Single)Breaker','Currents, Voltage and Breaker Status are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name IN (''Voltage'', ''Current'', ''Digital'') THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 1')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('CapBank-(Single)Breaker','Currents, Voltage and Breaker Status are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name IN (''Voltage'', ''Current'', ''Digital'') THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 1')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-Transformer','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-Transformer','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-CapBank','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('DER-(Single)Breaker','Currents, Voltage and Breaker Status are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name IN (''Voltage'', ''Current'', ''Digital'') THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 1')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Line-DER','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('DER-Transformer','only Voltages are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT 0')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Transformer-(Double)Breaker','only Voltage are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}')
GO
INSERT INTO AssetRelationshipType ( Name, Description, BiDirectional, JumpConnection, PassThrough)
	VALUES ('Bus-(Double)Breaker','only Voltage are passed across this connection.',1,'SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}','SELECT CASE WHEN MeasurementType.Name = ''Voltage'' THEN 1 ELSE 0 END FROM Channel JOIN MeasurementType ON Channel.MeasurementTypeID = MeasurementType.ID WHERE Channel.ID = {ChannelID}')
GO

-- Add Connection between AssetTypes and AssetRelationshipTypes for SystemCenter UI to reduce potential for Config issues

INSERT INTO AssetRelationshipTypeAssetType (AssetRelationshipTypeID, AssetTypeID ) VALUES
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-(Double)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Line'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-LineSegment'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-CapBank'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Relay'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Relay-CapBank'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBankRelay')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Transformer-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Transformer')),
	((SELECT ID FROM AssetRelationshipType where Name = 'CapBank-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Transformer'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-Transformer'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-CapBank'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'DER-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'DER')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-DER'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'DER-Transformer'),(SELECT ID FROM AssetType WHERE Name = 'DER')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-(Double)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Transformer-(Double)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Transformer'))
GO

INSERT INTO AssetRelationshipTypeAssetType (AssetRelationshipTypeID, AssetTypeID ) VALUES
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-(Double)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Line'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-LineSegment'),(SELECT ID FROM AssetType WHERE Name = 'LineSegment')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-CapBank'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Relay'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBankRelay')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Relay-CapBank'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Transformer-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'CapBank-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-Transformer'),(SELECT ID FROM AssetType WHERE Name = 'Transformer')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-Transformer'),(SELECT ID FROM AssetType WHERE Name = 'Transformer')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-CapBank'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM AssetRelationshipType where Name = 'DER-(Single)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Line-DER'),(SELECT ID FROM AssetType WHERE Name = 'DER')),
	((SELECT ID FROM AssetRelationshipType where Name = 'DER-Transformer'),(SELECT ID FROM AssetType WHERE Name = 'Transformer')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Bus-(Double)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM AssetRelationshipType where Name = 'Transformer-(Double)Breaker'),(SELECT ID FROM AssetType WHERE Name = 'Breaker'))

GO

-- EPRI CapBank Analytics As Defined in EPRI Documentation --
INSERT INTO CBDataError (ID, Description) VALUES
	(0, 'No Error'),
	(11, 'Error: One or more input parameters are incorrect.'),
	(12, 'Error: One or more waveforms are missing or data lengths or all waveforms are not the same or data length of each waveform is less than 24 cycles'),
	(13, 'Error: Non-uniform sampling time or sample rates of waveforms do not match those specified in the input parameters')
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
	(8,'Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long'),
	(1002,'Failed closing: one pole does not close, while the other two close and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(1020,'Failed closing: one pole does not close, while the other two close and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(1021,'Failed closing: one pole does not close, while the other two close and Blown fuses detected'),
	(1022,'Failed closing: one pole does not close, while the other two close and Missing tap voltage, blown resistors, or other unhealthy condition'),
	(1008,'Failed closing: one pole does not close, while the other two close and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long.'),
	(100802,'Failed closing: one pole does not close, while the other two close and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions.'),
	(100820,'Failed closing: one pole does not close, while the other two close and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Some fuseless units are shorted, or fused units shorted and fuses failed to clear.'),
	(100821,'Failed closing: one pole does not close, while the other two close and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Blown fuses detected.'),
	(100822,'Failed closing: one pole does not close, while the other two close and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Missing tap voltage, blown resistors, or other unhealthy condition.'),
	(1102,'Failed opening: one pole does not open, while the other two open and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(1120,'Failed opening: one pole does not open, while the other two open and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(1121,'Failed opening: one pole does not open, while the other two open and Blown fuses detected'),
	(1122,'Failed opening: one pole does not open, while the other two open and Missing tap voltage, blown resistors, or other unhealthy condition'),
	(1202,'The longest time between two closing or opening poles is more than 2 cycles and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(1220,'The longest time between two closing or opening poles is more than 2 cycles and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(1221,'The longest time between two closing or opening poles is more than 2 cycles and Blown fuses detected'),
	(1222,'The longest time between two closing or opening poles is more than 2 cycles and Missing tap voltage, blown resistors, or other unhealthy condition'),
	(1204,'The longest time between two closing or opening poles is more than 2 cycles and Restrike or reignition without failed opening.'),
	(120402,'The longest time between two closing or opening poles is more than 2 cycles and Restrike or reignition without failed opening. and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions.'),
	(120420,'The longest time between two closing or opening poles is more than 2 cycles and Restrike or reignition without failed opening. and Some fuseless units are shorted, or fused units shorted and fuses failed to clear.'),
	(120421,'The longest time between two closing or opening poles is more than 2 cycles and Restrike or reignition without failed opening. and Blown fuses detected.'),
	(120422,'The longest time between two closing or opening poles is more than 2 cycles and Restrike or reignition without failed opening. and Missing tap voltage, blown resistors, or other unhealthy condition.'),
	(1208,'The longest time between two closing or opening poles is more than 2 cycles and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long.'),
	(120802,'The longest time between two closing or opening poles is more than 2 cycles and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions.'),
	(120820,'The longest time between two closing or opening poles is more than 2 cycles and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Some fuseless units are shorted, or fused units shorted and fuses failed to clear.'),
	(120821,'The longest time between two closing or opening poles is more than 2 cycles and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Blown fuses detected.'),
	(120822,'The longest time between two closing or opening poles is more than 2 cycles and Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Missing tap voltage, blown resistors, or other unhealthy condition.'),
	(302,'Failed opening, a pole never opens or failed opening during next step opening and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(2003,'Failed opening, a pole never opens or failed opening during next step opening and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(2103,'Failed opening, a pole never opens or failed opening during next step opening and Blown fuses detected'),
	(2203,'Failed opening, a pole never opens or failed opening during next step opening and Missing tap voltage, blown resistors, or other unhealthy condition'),
	(402,'Restrike or reignition without failed opening and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(2004,'Restrike or reignition without failed opening and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(2104,'Restrike or reignition without failed opening and Blown fuses detected'),
	(2204,'Restrike or reignition without failed opening and Missing tap voltage, blown resistors, or other unhealthy condition'),
	(502,'Restrike or reignition and failed opening and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(2005,'Restrike or reignition and failed opening and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(2105,'Restrike or reignition and failed opening and Blown fuses detected'),
	(2205,'Restrike or reignition and failed opening and Missing tap voltage, blown resistors, or other unhealthy condition'),
	(802,'Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Some capacitor elements or units are shorted, blown fuses, i.e., unhealthy cap bank and/or relay conditions'),
	(2008,'Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Some fuseless units are shorted, or fused units shorted and fuses failed to clear'),
	(2108,'Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Blown fuses detected'),
	(2208,'Pre-insertion switch has abnormality: switched-in without pre-insertion or never switched-out (stuck) or duration is too short or too long and Missing tap voltage, blown resistors, or other unhealthy condition')
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
	(3,'Mid-rack tap voltages are missing'),
	(-1,'Unavailable')
GO

/* Other and Test event types are always allowed since those apply to all asset types in general */
INSERT INTO EventTypeAssetType (EventTypeID,AssetTypeID) VALUES
	((SELECT ID FROM EventType WHERE Name = 'Fault'),(SELECT ID FROM AssetType WHERE Name = 'Transformer')),
	((SELECT ID FROM EventType WHERE Name = 'Fault'),(SELECT ID FROM AssetType WHERE Name = 'Line')),
	((SELECT ID FROM EventType WHERE Name = 'RecloseIntoFault'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM EventType WHERE Name = 'BreakerOpen'),(SELECT ID FROM AssetType WHERE Name = 'Breaker')),
	((SELECT ID FROM EventType WHERE Name = 'Interruption'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM EventType WHERE Name = 'Sag'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM EventType WHERE Name = 'Swell'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM EventType WHERE Name = 'Transient'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM EventType WHERE Name = 'Interruption'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM EventType WHERE Name = 'Sag'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM EventType WHERE Name = 'Swell'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM EventType WHERE Name = 'Transient'),(SELECT ID FROM AssetType WHERE Name = 'CapacitorBank')),
	((SELECT ID FROM EventType WHERE Name = 'Snapshot'),(SELECT ID FROM AssetType WHERE Name = 'Bus')),
	((SELECT ID FROM EventType WHERE Name = 'Snapshot'),(SELECT ID FROM AssetType WHERE Name = 'Breaker'))
GO