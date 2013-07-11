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
using System.Data.SqlClient;
using System.Linq;
using FaultAlgorithms;
using GSF;

namespace SqlServerTedWriter
{
    /// <summary>
    /// Writes the results of fault analysis to SQL Server Transient Event Database.
    /// </summary>
    public class SqlServerTedWriter : IFaultResultsWriter
    {
        #region [ Members ]

        // Fields
        private Dictionary<string, string> m_parameters;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Parameters used to configure the results writer.
        /// </summary>
        public Dictionary<string, string> Parameters
        {
            get
            {
                return m_parameters;
            }
            set
            {
                m_parameters = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Writes configuration information to the output source.
        /// </summary>
        /// <param name="deviceConfigurations">Configuration information to be written to the output source.</param>
        public void WriteConfiguration(ICollection<Device> deviceConfigurations)
        {
            string connectionString;
            int deviceID;

            if (!m_parameters.TryGetValue("connectionString", out connectionString))
                connectionString = m_parameters.JoinKeyValuePairs();

            using (SqlConnection connection = new SqlConnection(connectionString))
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
        public void WriteResults(Device disturbanceRecorder, ICollection<DisturbanceFile> disturbanceFiles, ICollection<Tuple<Line, FaultLocationDataSet>> lineDataSets)
        {
            string connectionString;
            int deviceID;
            int fileGroupID;

            if (!m_parameters.TryGetValue("connectionString", out connectionString))
                connectionString = m_parameters.JoinKeyValuePairs();

            using (SqlConnection connection = new SqlConnection(connectionString))
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

            long[] times;
            int firstFaultCycleIndex;
            CycleData firstFaultCycle;
            DateTime firstFaultCycleTime = DateTime.MinValue;

            CycleData faultCalculationCycle;
            double iaFault = -1.0D;
            double ibFault = -1.0D;
            double icFault = -1.0D;
            double vaFault = -1.0D;
            double vbFault = -1.0D;
            double vcFault = -1.0D;

            // Get the timestamp of the first fault cycle
            times = faultDataSet.Voltages.AN.Times;

            if (faultDataSet.FaultedCycles.Count > 0)
            {
                firstFaultCycleIndex = faultDataSet.FaultedCycles.First();

                if (firstFaultCycleIndex >= 0 && firstFaultCycleIndex < faultDataSet.Cycles.Count)
                {
                    firstFaultCycle = faultDataSet.Cycles[firstFaultCycleIndex];
                    firstFaultCycleTime = new DateTime(times[firstFaultCycle.StartIndex]);
                }
            }

            // Get voltage and current values during the fault
            if (faultDataSet.FaultCalculationCycle >= 0 && faultDataSet.FaultCalculationCycle < faultDataSet.Cycles.Count)
            {
                faultCalculationCycle = faultDataSet.Cycles[faultDataSet.FaultCalculationCycle];
                iaFault = faultCalculationCycle.AN.I.RMS;
                ibFault = faultCalculationCycle.BN.I.RMS;
                icFault = faultCalculationCycle.CN.I.RMS;
                vaFault = faultCalculationCycle.AN.V.RMS;
                vbFault = faultCalculationCycle.BN.V.RMS;
                vcFault = faultCalculationCycle.CN.V.RMS;
            }

            command.CommandText = "SELECT ID FROM Line WHERE FLEID = @fleID";
            command.Parameters.AddWithValue("@fleID", line.ID);
            lineID = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters.Clear();

            command.CommandText = "INSERT INTO LineDisturbance (LineID, FileGroupID, FaultType, FaultDistance, CyclesOfData, " +
                                  "FaultCycles, FaultCalculationCycle, FirstFaultCycleTime, IAFault, IBFault, ICFault, VAFault, " +
                                  "VBFault, VCFault, IAMax, IBMax, ICMax, VAMin, VBMin, VCMin) VALUES (@lineID, @fileGroupID, " +
                                  "@faultType, @faultDistance, @cyclesOfData, @faultCycles, @faultCalculationCycle, " +
                                  "@firstFaultCycleTime, @iaFault, @ibFault, @icFault, @vaFault, @vbFault, @vcFault, @iaMax, " +
                                  "@ibMax, @icMax, @vaMin, @vbMin, @vcMin)";

            command.Parameters.AddWithValue("@lineID", lineID);
            command.Parameters.AddWithValue("@fileGroupID", fileGroupID);
            command.Parameters.AddWithValue("@faultType", faultDataSet.FaultType.ToString());
            command.Parameters.AddWithValue("@faultDistance", faultDataSet.FaultDistance);
            command.Parameters.AddWithValue("@cyclesOfData", faultDataSet.Cycles.Count);
            command.Parameters.AddWithValue("@faultCycles", faultDataSet.FaultCycleCount);
            command.Parameters.AddWithValue("@faultCalculationCycle", faultDataSet.FaultCalculationCycle);
            command.Parameters.AddWithValue("@firstFaultCycleTime", firstFaultCycleTime);
            command.Parameters.AddWithValue("@iaFault", iaFault);
            command.Parameters.AddWithValue("@ibFault", ibFault);
            command.Parameters.AddWithValue("@icFault", icFault);
            command.Parameters.AddWithValue("@vaFault", vaFault);
            command.Parameters.AddWithValue("@vbFault", vbFault);
            command.Parameters.AddWithValue("@vcFault", vcFault);
            command.Parameters.AddWithValue("@iaMax", faultDataSet.Cycles.Max(cycle => cycle.AN.I.RMS));
            command.Parameters.AddWithValue("@ibMax", faultDataSet.Cycles.Max(cycle => cycle.BN.I.RMS));
            command.Parameters.AddWithValue("@icMax", faultDataSet.Cycles.Max(cycle => cycle.CN.I.RMS));
            command.Parameters.AddWithValue("@vaMin", faultDataSet.Cycles.Min(cycle => cycle.AN.V.RMS));
            command.Parameters.AddWithValue("@vbMin", faultDataSet.Cycles.Min(cycle => cycle.BN.V.RMS));
            command.Parameters.AddWithValue("@vcMin", faultDataSet.Cycles.Min(cycle => cycle.CN.V.RMS));
            command.ExecuteNonQuery();
            command.Parameters.Clear();
        }

        #endregion
    }
}
