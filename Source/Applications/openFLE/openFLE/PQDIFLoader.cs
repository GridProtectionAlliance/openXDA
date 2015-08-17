//*********************************************************************************************************************
// PQDIFLoader.cs
// Version 1.1 and subsequent releases
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
// --------------------------------------------------------------------------------------------------------------------
//
// Version 1.0
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  05/23/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaultAlgorithms;
using GSF;
using GSF.PQDIF.Logical;

namespace openFLE
{
    /// <summary>
    /// Defines functions needed to load PQDIF data sets for openFLE.
    /// </summary>
    internal static class PQDIFLoader
    {
        /// <summary>
        /// Populate known voltage and current data from PQDIF file.
        /// </summary>
        /// <param name="faultDataSet">Fault data set to be populated.</param>
        /// <param name="settings">Source parameters.</param>
        public static void PopulateDataSet(FaultLocationDataSet faultDataSet, Dictionary<string, string> settings)
        {
            string fileName;

            List<ObservationRecord> observationRecords;
            ObservationRecord faultObservation;
            ChannelInstance[] anChannels;
            ChannelInstance[] bnChannels;
            ChannelInstance[] cnChannels;

            if (!settings.TryGetValue("fileName", out fileName) || !File.Exists(fileName))
                throw new ArgumentException("Parameters must define a valid \"fileName\" setting.");

            // Parse PQDif File Data
            using (LogicalParser logicalParser = new LogicalParser(fileName))
            {
                observationRecords = new List<ObservationRecord>();
                logicalParser.Open();

                while (logicalParser.HasNextObservationRecord())
                    observationRecords.Add(logicalParser.NextObservationRecord());
            }

            // Get the first observation record that contains six wave forms.
            // Assume that this observation record will has both voltage and
            // current wave forms for each of the three phases
            faultObservation = observationRecords.FirstOrDefault(observation => observation.ChannelInstances.Count(channel => channel.Definition.QuantityTypeID == QuantityType.WaveForm) >= 6);

            if ((object)faultObservation != null)
            {
                // Get the voltage and current wave forms for each of the six phase types
                IEnumerable<ChannelInstance> waveForms = faultObservation.ChannelInstances.Where(channel => channel.Definition.QuantityTypeID == QuantityType.WaveForm).ToList();

                anChannels = waveForms.Where(channel => channel.Definition.Phase == Phase.AN).ToArray();
                bnChannels = waveForms.Where(channel => channel.Definition.Phase == Phase.BN).ToArray();
                cnChannels = waveForms.Where(channel => channel.Definition.Phase == Phase.CN).ToArray();

                // Attempt to fill in fault data for each of the three phase types
                FillFaultData(faultDataSet.Voltages.AN, faultDataSet.Currents.AN, anChannels);
                FillFaultData(faultDataSet.Voltages.BN, faultDataSet.Currents.BN, bnChannels);
                FillFaultData(faultDataSet.Voltages.CN, faultDataSet.Currents.CN, cnChannels);

                // Set the frequency in the data set to the nominal frequency of the system
                faultDataSet.Frequency = faultObservation.Settings.NominalFrequency;
            }
        }

        // Fills in fault data from an array of PQDIF channels which contains
        // exactly one voltage channel and exactly one current channel.
        private static void FillFaultData(MeasurementData voltageData, MeasurementData currentData, ChannelInstance[] channels)
        {
            ChannelInstance voltageChannel;
            ChannelInstance currentChannel;

            SeriesInstance voltageTimeSeries;
            SeriesInstance currentTimeSeries;
            SeriesInstance voltageDataSeries;
            SeriesInstance currentDataSeries;

            // Split the collection of wave forms into separate voltage and current wave forms
            voltageChannel = channels.SingleOrDefault(channel => channel.Definition.QuantityMeasured == QuantityMeasured.Voltage);
            currentChannel = channels.SingleOrDefault(channel => channel.Definition.QuantityMeasured == QuantityMeasured.Current);

            if ((object)voltageChannel != null && (object)currentChannel != null)
            {
                // Get series instances for time, voltage, and current
                voltageTimeSeries = voltageChannel.SeriesInstances.Single(series => series.Definition.ValueTypeID == SeriesValueType.Time);
                currentTimeSeries = currentChannel.SeriesInstances.Single(series => series.Definition.ValueTypeID == SeriesValueType.Time);
                voltageDataSeries = voltageChannel.SeriesInstances.Single(series => series.Definition.ValueTypeID == SeriesValueType.Val);
                currentDataSeries = currentChannel.SeriesInstances.Single(series => series.Definition.ValueTypeID == SeriesValueType.Val);

                // Fill in fault data for each of the three series obtained above
                voltageData.Times = ToTicks(voltageTimeSeries);
                currentData.Times = ToTicks(currentTimeSeries);
                voltageData.Measurements = voltageDataSeries.OriginalValues.Select(Convert.ToDouble).ToArray();
                currentData.Measurements = currentDataSeries.OriginalValues.Select(Convert.ToDouble).ToArray();
            }
        }

        // Takes a PQDIF time series and converts it to an array of 64-bit integer values in ticks.
        private static long[] ToTicks(SeriesInstance timeSeries)
        {
            ObservationRecord observation = timeSeries.Channel.ObservationRecord;
            long startTime;
            double nominalFrequency;

            if (timeSeries.Definition.QuantityUnits == QuantityUnits.Seconds)
            {
                startTime = observation.StartTime.Ticks;

                // The time series contains values in seconds,
                // relative to the start time of the observation record
                return timeSeries.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(Ticks.FromSeconds)
                    .Select(timeOffset => startTime + (long)timeOffset)
                    .ToArray();
            }
            else if (timeSeries.Definition.QuantityUnits == QuantityUnits.Timestamp)
            {
                // The time series contains values in absolute time
                return timeSeries.OriginalValues
                    .Cast<DateTime>()
                    .Select(time => time.Ticks)
                    .ToArray();
            }
            else if (timeSeries.Definition.QuantityUnits == QuantityUnits.Cycles)
            {
                startTime = observation.StartTime.Ticks;
                nominalFrequency = observation.Settings.NominalFrequency;

                // TODO: Is this correct (dividing by nominal frequency)?
                // The time series contains values in cycles,
                // relative to the start time of the observation record
                return timeSeries.OriginalValues
                    .Select(Convert.ToDouble)
                    .Select(cycles => cycles / nominalFrequency)
                    .Select(Ticks.FromSeconds)
                    .Select(timeOffset => startTime + (long)timeOffset)
                    .ToArray();
            }

            // The time series does not contain timestamps...?
            return null;
        }
    }
}
