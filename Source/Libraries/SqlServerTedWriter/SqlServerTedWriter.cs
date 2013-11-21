//******************************************************************************************************
//  SqlServerTedWriter.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/02/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FaultAlgorithms;
using GSF;
using GSF.Configuration;

namespace SqlServerTedWriter
{
    /// <summary>
    /// Writes the results of fault analysis to SQL Server Transient Event Database.
    /// </summary>
    public class SqlServerTedWriter : FaultResultsWriterBase
    {
        private string m_connectionString;

        /// <summary>
        /// Writes configuration information to the output source.
        /// </summary>
        /// <param name="deviceConfigurations">Configuration information to be written to the output source.</param>
        public override void WriteConfiguration(ICollection<Device> deviceConfigurations)
        {
            int deviceID;

            using (SqlConnection connection = new SqlConnection(m_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();

                foreach (Device deviceConfiguration in deviceConfigurations)
                {
                    deviceID = AddOrUpdate(deviceConfiguration, command);

                    foreach (Line lineConfiguration in deviceConfiguration.Lines)
                        AddOrUpdate(lineConfiguration, deviceID, command);
                }
            }
        }

        /// <summary>
        /// Writes the results to the output source.
        /// </summary>
        /// <param name="disturbanceRecorder">The device that collected the disturbance data.</param>
        /// <param name="disturbanceFiles">Information about the data files collected during the disturbance.</param>
        /// <param name="lineDataSets">The data sets used for analysis to determine fault location.</param>
        public override void WriteResults(Device disturbanceRecorder, ICollection<DisturbanceFile> disturbanceFiles, ICollection<Tuple<Line, FaultLocationDataSet>> lineDataSets)
        {
            int deviceID;
            int fileGroupID;

            using (SqlConnection connection = new SqlConnection(m_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    command.Transaction = transaction;

                    try
                    {
                        // Get the ID of the device that captured the fault data
                        command.CommandText = "SELECT ID FROM Device WHERE FLEID = @fleID";
                        command.Parameters.AddWithValue("@fleID", disturbanceRecorder.ID);
                        deviceID = Convert.ToInt32(command.ExecuteScalar());
                        command.Parameters.Clear();

                        // Create a group for the disturbance files
                        command.CommandText = "INSERT INTO FileGroup (DeviceID) VALUES (@deviceID)";
                        command.Parameters.AddWithValue("@deviceID", deviceID);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        // Get the identifier for the file group
                        command.CommandText = "SELECT @@IDENTITY";
                        fileGroupID = Convert.ToInt32(command.ExecuteScalar());

                        // Insert disturbance file information into the database
                        foreach (DisturbanceFile disturbanceFile in disturbanceFiles)
                        {
                            command.CommandText = "INSERT INTO DisturbanceFile (FileGroupID, SourcePath, DestinationPath, FileSize, " +
                                                  "CreationTime, LastWriteTime, LastAccessTime, FileWatcherStartTime, FileWatcherEndTime, " +
                                                  "FLEStartTime, FLEEndTime) VALUES (@fileGroupID, @sourcePath, @destinationPath, @fileSize, " +
                                                  "@creationTime, @lastWriteTime, @lastAccessTime, @fileWatcherStartTime, @fileWatcherEndTime, " +
                                                  "@fleStartTime, @fleEndTime)";

                            command.Parameters.AddWithValue("@fileGroupID", fileGroupID);
                            command.Parameters.AddWithValue("@sourcePath", disturbanceFile.SourcePath);
                            command.Parameters.AddWithValue("@destinationPath", disturbanceFile.DestinationPath);
                            command.Parameters.AddWithValue("@fileSize", disturbanceFile.FileSize);
                            command.Parameters.AddWithValue("@creationTime", disturbanceFile.CreationTime);
                            command.Parameters.AddWithValue("@lastWriteTime", disturbanceFile.LastWriteTime);
                            command.Parameters.AddWithValue("@lastAccessTime", disturbanceFile.LastAccessTime);
                            command.Parameters.AddWithValue("@fileWatcherStartTime", disturbanceFile.FileWatcherTimeStarted);
                            command.Parameters.AddWithValue("@fileWatcherEndTime", disturbanceFile.FileWatcherTimeProcessed);
                            command.Parameters.AddWithValue("@fleStartTime", disturbanceFile.FLETimeStarted);
                            command.Parameters.AddWithValue("@fleEndTime", disturbanceFile.FLETimeProcessed);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        // Insert information about the faults into the database
                        foreach (Tuple<Line, FaultLocationDataSet> lineDataSet in lineDataSets)
                            InsertFaultData(lineDataSet.Item1, lineDataSet.Item2, command, fileGroupID);

                        transaction.Commit();
                    }
                    catch (Exception commitException)
                    {
                        try
                        {
                            // Commit failed - attempt to rollback
                            transaction.Rollback();
                        }
                        catch (Exception rollbackException)
                        {
                            // Throw exception to indicate that rollback failed
                            throw new InvalidOperationException(string.Format("Unable to rollback database transaction after failure to commit: {0}", rollbackException.Message), rollbackException);
                        }

                        // Throw exception to indicate commit failed
                        throw new InvalidOperationException(string.Format("Unable to commit database transaction due to exception: {0}", commitException.Message), commitException);
                    }
                }
            }
        }

        private int AddOrUpdate(Device device, SqlCommand command)
        {
            int deviceCount;
            int deviceID;

            // Determine if the device already exists
            command.CommandText = "SELECT COUNT(*) FROM Device WHERE FLEID = @fleID";
            command.Parameters.AddWithValue("@fleID", device.ID);
            deviceCount = Convert.ToInt32(command.ExecuteScalar());

            if (deviceCount > 0)
            {
                // Get device ID
                command.CommandText = "SELECT ID FROM Device WHERE FLEID = @fleID";
                deviceID = Convert.ToInt32(command.ExecuteScalar());
                command.Parameters.Clear();

                // Update device
                command.CommandText = "UPDATE Device SET FLEID = @fleID, Make = @make, Model = @model, StationID = @stationID, StationName = @stationName WHERE ID = @deviceID";
                command.Parameters.AddWithValue("@fleID", device.ID);
                command.Parameters.AddWithValue("@make", device.Make);
                command.Parameters.AddWithValue("@model", device.Model);
                command.Parameters.AddWithValue("@stationID", device.StationID);
                command.Parameters.AddWithValue("@stationName", device.StationName);
                command.Parameters.AddWithValue("@deviceID", deviceID);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            else
            {
                command.Parameters.Clear();

                // Insert device into Device table
                command.CommandText = "INSERT INTO Device (FLEID, Make, Model, StationID, StationName) VALUES (@fleID, @make, @model, @stationID, @stationName)";
                command.Parameters.AddWithValue("@fleID", device.ID);
                command.Parameters.AddWithValue("@make", device.Make);
                command.Parameters.AddWithValue("@model", device.Model);
                command.Parameters.AddWithValue("@stationID", device.StationID);
                command.Parameters.AddWithValue("@stationName", device.StationName);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                // Get device ID
                command.CommandText = "SELECT @@IDENTITY";
                deviceID = Convert.ToInt32(command.ExecuteScalar());
            }

            return deviceID;
        }

        private int AddOrUpdate(Line line, int deviceID, SqlCommand command)
        {
            int lineCount;
            int lineID;

            // Determine if the device already exists
            command.CommandText = "SELECT COUNT(*) FROM Line WHERE FLEID = @fleID";
            command.Parameters.AddWithValue("@fleID", line.ID);
            lineCount = Convert.ToInt32(command.ExecuteScalar());

            if (lineCount > 0)
            {
                // Get line ID
                command.CommandText = "SELECT ID FROM Line WHERE FLEID = @fleID";
                lineID = Convert.ToInt32(command.ExecuteScalar());
                command.Parameters.Clear();

                // Update device
                command.CommandText = "UPDATE Line SET DeviceID = @deviceID, FLEID = @fleID, Name = @name, Voltage = @voltage, Rating50F = @rating50F, Length = @length, " +
                                      "EndStationID = @endStationID, EndStationName = @endStationName, R1 = @r1, X1 = @x1, R0 = @r0, X0 = @x0 WHERE ID = @lineID";

                command.Parameters.AddWithValue("@deviceID", deviceID);
                command.Parameters.AddWithValue("@fleID", line.ID);
                command.Parameters.AddWithValue("@name", line.Name);
                command.Parameters.AddWithValue("@voltage", line.Voltage);
                command.Parameters.AddWithValue("@rating50F", line.Rating50F);
                command.Parameters.AddWithValue("@length", line.Length);
                command.Parameters.AddWithValue("@endStationID", line.EndStationID);
                command.Parameters.AddWithValue("@endStationName", line.EndStationName);
                command.Parameters.AddWithValue("@r1", line.R1);
                command.Parameters.AddWithValue("@x1", line.X1);
                command.Parameters.AddWithValue("@r0", line.R0);
                command.Parameters.AddWithValue("@x0", line.X0);
                command.Parameters.AddWithValue("@lineID", lineID);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            else
            {
                command.Parameters.Clear();

                // Insert line into Line table
                command.CommandText = "INSERT INTO Line (DeviceID, FLEID, Name, Voltage, Rating50F, Length, EndStationID, EndStationName, R1, X1, R0, X0) " +
                                      "VALUES (@deviceID, @fleID, @name, @voltage, @rating50F, @length, @endStationID, @endStationName, @r1, @x1, @r0, @x0)";

                command.Parameters.AddWithValue("@deviceID", deviceID);
                command.Parameters.AddWithValue("@fleID", line.ID);
                command.Parameters.AddWithValue("@name", line.Name);
                command.Parameters.AddWithValue("@voltage", line.Voltage);
                command.Parameters.AddWithValue("@rating50F", line.Rating50F);
                command.Parameters.AddWithValue("@length", line.Length);
                command.Parameters.AddWithValue("@endStationID", line.EndStationID);
                command.Parameters.AddWithValue("@endStationName", line.EndStationName);
                command.Parameters.AddWithValue("@r1", line.R1);
                command.Parameters.AddWithValue("@x1", line.X1);
                command.Parameters.AddWithValue("@r0", line.R0);
                command.Parameters.AddWithValue("@x0", line.X0);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                // Get device ID
                command.CommandText = "SELECT @@IDENTITY";
                lineID = Convert.ToInt32(command.ExecuteScalar());
            }

            return lineID;
        }

        private void InsertFaultData(Line line, FaultLocationDataSet faultDataSet, SqlCommand command, int fileGroupID)
        {
            int lineID;
            int lineDisturbanceID;

            int largestCurrentIndex;
            double faultDistance;

            // Get largest current index and median fault distance for that cycle
            largestCurrentIndex = faultDataSet.Cycles.GetLargestCurrentIndex();

            faultDistance = faultDataSet.FaultDistances.Values
                .Select(distances => distances[largestCurrentIndex])
                .OrderBy(distance => distance)
                .ToArray()[faultDataSet.FaultDistances.Count / 2];

            // Get ID of the line associated with this fault data
            command.CommandText = "SELECT ID FROM Line WHERE FLEID = @fleID";
            command.Parameters.AddWithValue("@fleID", line.ID);
            lineID = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters.Clear();

            // Insert a disturbance record for this line
            command.CommandText = "INSERT INTO LineDisturbance (LineID, FileGroupID, FaultType, LargestCurrentIndex, " +
                                  "FaultDistance, IAMax, IBMax, ICMax, VAMin, VBMin, VCMin) VALUES (@lineID, @fileGroupID, " +
                                  "@faultType, @largestCurrentIndex, @faultDistance, @iaMax, @ibMax, @icMax, @vaMin, @vbMin, @vcMin)";

            command.Parameters.AddWithValue("@lineID", lineID);
            command.Parameters.AddWithValue("@fileGroupID", fileGroupID);
            command.Parameters.AddWithValue("@faultType", faultDataSet.FaultType.ToString());
            command.Parameters.AddWithValue("@largestCurrentIndex", largestCurrentIndex);
            command.Parameters.AddWithValue("@faultDistance", faultDistance);
            command.Parameters.AddWithValue("@iaMax", faultDataSet.Cycles.Max(cycle => cycle.AN.I.RMS));
            command.Parameters.AddWithValue("@ibMax", faultDataSet.Cycles.Max(cycle => cycle.BN.I.RMS));
            command.Parameters.AddWithValue("@icMax", faultDataSet.Cycles.Max(cycle => cycle.CN.I.RMS));
            command.Parameters.AddWithValue("@vaMin", faultDataSet.Cycles.Min(cycle => cycle.AN.V.RMS));
            command.Parameters.AddWithValue("@vbMin", faultDataSet.Cycles.Min(cycle => cycle.BN.V.RMS));
            command.Parameters.AddWithValue("@vcMin", faultDataSet.Cycles.Min(cycle => cycle.CN.V.RMS));
            command.ExecuteNonQuery();
            command.Parameters.Clear();

            // Get the ID of the disturbance record
            command.CommandText = "SELECT @@IDENTITY";
            lineDisturbanceID = Convert.ToInt32(command.ExecuteScalar());

            // Insert data for each cycle into the database
            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
                InsertCycleData(faultDataSet, command, lineDisturbanceID, i);
        }

        private void InsertCycleData(FaultLocationDataSet faultDataSet, SqlCommand command, int lineDisturbanceID, int cycleIndex)
        {
            int cycleID;
            CycleData cycle;
            ComplexNumber[] sequenceVoltages;
            ComplexNumber[] sequenceCurrents;

            // Get the cycle data and sequence
            // components from the fault data set
            cycle = faultDataSet.Cycles[cycleIndex];
            sequenceVoltages = CycleData.CalculateSequenceComponents(cycle.AN.V, cycle.BN.V, cycle.CN.V);
            sequenceCurrents = CycleData.CalculateSequenceComponents(cycle.AN.I, cycle.BN.I, cycle.CN.I);

            // Insert the cycle data into the database
            command.CommandText = "INSERT INTO Cycle (LineDisturbanceID, TimeStart, CycleIndex," +
                                  "ANVoltagePeak, ANVoltageRMS, ANVoltagePhase, ANCurrentPeak, ANCurrentRMS, ANCurrentPhase, " +
                                  "BNVoltagePeak, BNVoltageRMS, BNVoltagePhase, BNCurrentPeak, BNCurrentRMS, BNCurrentPhase, " +
                                  "CNVoltagePeak, CNVoltageRMS, CNVoltagePhase, CNCurrentPeak, CNCurrentRMS, CNCurrentPhase, " +
                                  "PositiveVoltageMagnitude, PositiveVoltageAngle, PositiveCurrentMagnitude, PositiveCurrentAngle, " +
                                  "NegativeVoltageMagnitude, NegativeVoltageAngle, NegativeCurrentMagnitude, NegativeCurrentAngle, " +
                                  "ZeroVoltageMagnitude, ZeroVoltageAngle, ZeroCurrentMagnitude, ZeroCurrentAngle) " +
                                  "VALUES (@lineDisturbanceID, @timeStart, @cycleIndex, " +
                                  "@anVoltagePeak, @anVoltageRMS, @anVoltagePhase, @anCurrentPeak, @anCurrentRMS, @anCurrentPhase, " +
                                  "@bnVoltagePeak, @bnVoltageRMS, @bnVoltagePhase, @bnCurrentPeak, @bnCurrentRMS, @bnCurrentPhase, " +
                                  "@cnVoltagePeak, @cnVoltageRMS, @cnVoltagePhase, @cnCurrentPeak, @cnCurrentRMS, @cnCurrentPhase, " +
                                  "@positiveVoltageMagnitude, @positiveVoltageAngle, @positiveCurrentMagnitude, @positiveCurrentAngle, " +
                                  "@negativeVoltageMagnitude, @negativeVoltageAngle, @negativeCurrentMagnitude, @negativeCurrentAngle, " +
                                  "@zeroVoltageMagnitude, @zeroVoltageAngle, @zeroCurrentMagnitude, @zeroCurrentAngle)";

            command.Parameters.AddWithValue("@lineDisturbanceID", lineDisturbanceID);
            command.Parameters.AddWithValue("@timeStart", cycle.StartTime);
            command.Parameters.AddWithValue("@cycleIndex", cycleIndex);
            command.Parameters.AddWithValue("@anVoltagePeak", cycle.AN.V.Peak);
            command.Parameters.AddWithValue("@anVoltageRMS", cycle.AN.V.RMS);
            command.Parameters.AddWithValue("@anVoltagePhase", cycle.AN.V.Phase.ToDegrees());
            command.Parameters.AddWithValue("@anCurrentPeak", cycle.AN.I.Peak);
            command.Parameters.AddWithValue("@anCurrentRMS", cycle.AN.I.RMS);
            command.Parameters.AddWithValue("@anCurrentPhase", cycle.AN.I.Phase.ToDegrees());
            command.Parameters.AddWithValue("@bnVoltagePeak", cycle.BN.V.Peak);
            command.Parameters.AddWithValue("@bnVoltageRMS", cycle.BN.V.RMS);
            command.Parameters.AddWithValue("@bnVoltagePhase", cycle.BN.V.Phase.ToDegrees());
            command.Parameters.AddWithValue("@bnCurrentPeak", cycle.BN.I.Peak);
            command.Parameters.AddWithValue("@bnCurrentRMS", cycle.BN.I.RMS);
            command.Parameters.AddWithValue("@bnCurrentPhase", cycle.BN.I.Phase.ToDegrees());
            command.Parameters.AddWithValue("@cnVoltagePeak", cycle.CN.V.Peak);
            command.Parameters.AddWithValue("@cnVoltageRMS", cycle.CN.V.RMS);
            command.Parameters.AddWithValue("@cnVoltagePhase", cycle.CN.V.Phase.ToDegrees());
            command.Parameters.AddWithValue("@cnCurrentPeak", cycle.CN.I.Peak);
            command.Parameters.AddWithValue("@cnCurrentRMS", cycle.CN.I.RMS);
            command.Parameters.AddWithValue("@cnCurrentPhase", cycle.CN.I.Phase.ToDegrees());
            command.Parameters.AddWithValue("@positiveVoltageMagnitude", sequenceVoltages[0].Magnitude);
            command.Parameters.AddWithValue("@positiveVoltageAngle", sequenceVoltages[0].Angle.ToDegrees());
            command.Parameters.AddWithValue("@negativeVoltageMagnitude", sequenceVoltages[1].Magnitude);
            command.Parameters.AddWithValue("@negativeVoltageAngle", sequenceVoltages[1].Angle.ToDegrees());
            command.Parameters.AddWithValue("@zeroVoltageMagnitude", sequenceVoltages[2].Magnitude);
            command.Parameters.AddWithValue("@zeroVoltageAngle", sequenceVoltages[2].Angle.ToDegrees());
            command.Parameters.AddWithValue("@positiveCurrentMagnitude", sequenceCurrents[0].Magnitude);
            command.Parameters.AddWithValue("@positiveCurrentAngle", sequenceCurrents[0].Angle.ToDegrees());
            command.Parameters.AddWithValue("@negativeCurrentMagnitude", sequenceCurrents[1].Magnitude);
            command.Parameters.AddWithValue("@negativeCurrentAngle", sequenceCurrents[1].Angle.ToDegrees());
            command.Parameters.AddWithValue("@zeroCurrentMagnitude", sequenceCurrents[2].Magnitude);
            command.Parameters.AddWithValue("@zeroCurrentAngle", sequenceCurrents[2].Angle.ToDegrees());

            command.ExecuteNonQuery();
            command.Parameters.Clear();

            // Get the ID of the cycle data that was entered
            command.CommandText = "SELECT @@IDENTITY";
            cycleID = Convert.ToInt32(command.ExecuteScalar());

            // Insert fault distances calculated for this cycle
            InsertFaultDistances(faultDataSet, command, cycleIndex, cycleID);
        }

        private void InsertFaultDistances(FaultLocationDataSet faultDataSet, SqlCommand command, int cycleIndex, int cycleID)
        {
            IDbDataParameter algorithmParameter = command.CreateParameter();
            IDbDataParameter distanceParameter = command.CreateParameter();
            double distance;

            command.CommandText = "INSERT INTO FaultDistance (CycleID, Algorithm, Distance) VALUES (@cycleID, @algorithm, @distance)";
            command.Parameters.AddWithValue("@cycleID", cycleID);
            command.Parameters.Add(algorithmParameter);
            command.Parameters.Add(distanceParameter);

            algorithmParameter.ParameterName = "@algorithm";
            distanceParameter.ParameterName = "@distance";

            foreach (KeyValuePair<string, double[]> faultDistances in faultDataSet.FaultDistances)
            {
                distance = faultDistances.Value[cycleIndex];

                if (!double.IsNaN(distance))
                {
                    algorithmParameter.Value = faultDistances.Key;
                    distanceParameter.Value = distance;
                    command.ExecuteNonQuery();
                }
            }

            command.Parameters.Clear();
        }

        /// <summary>
        /// Loads saved <see cref="FaultResultsWriterBase"/> settings from the config file if the <see cref="P:GSF.Adapters.Adapter.PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException"><see cref="P:GSF.Adapters.Adapter.SettingsCategory"/> has a value of null or empty string.</exception>
        public override void LoadSettings()
        {
            base.LoadSettings();

            if (PersistSettings)
            {
                // Load settings from the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[SettingsCategory];
                settings.Add("ConnectionString", @"Data Source=localhost\SQLEXPRESS; Initial Catalog=openFLE; Integrated Security=SSPI", "Connection parameters required to connect to the SQL Server database.");
                m_connectionString = settings["ConnectionString"].Value;
            }
        }

        /// <summary>
        /// Saves <see cref="FaultResultsWriterBase"/> settings to the config file if the <see cref="P:GSF.Adapters.Adapter.PersistSettings"/> property is set to true.
        /// </summary>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException"><see cref="P:GSF.Adapters.Adapter.SettingsCategory"/> has a value of null or empty string.</exception>
        public override void SaveSettings()
        {
            base.SaveSettings();

            if (PersistSettings)
            {
                // Load settings from the specified category.
                ConfigurationFile config = ConfigurationFile.Current;
                CategorizedSettingsElementCollection settings = config.Settings[SettingsCategory];
                settings["ConnectionString", true].Update(m_connectionString);
                config.Save();
            }
        }
    }
}
