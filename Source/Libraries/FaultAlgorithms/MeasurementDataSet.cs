//*********************************************************************************************************************
// MeasurementDataSet.cs
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
using System.IO;
using System.Linq;
using GSF;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a set of 3-phase line-to-neutral and line-to-line time-domain power data.
    /// </summary>
    public class MeasurementDataSet
    {
        #region [ Members ]

        // Constants

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

        // Fields

        /// <summary>
        /// Line-to-neutral A-phase data.
        /// </summary>
        public MeasurementData AN;

        /// <summary>
        /// Line-to-neutral B-phase data.
        /// </summary>
        public MeasurementData BN;

        /// <summary>
        /// Line-to-neutral C-phase data.
        /// </summary>
        public MeasurementData CN;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="MeasurementDataSet"/>.
        /// </summary>
        public MeasurementDataSet()
        {
            AN = new MeasurementData();
            BN = new MeasurementData();
            CN = new MeasurementData();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Uses system frequency to calculate the sample rate for each set
        /// of <see cref="MeasurementData"/> in this measurement data set.
        /// </summary>
        /// <param name="frequency">The frequency of the measured system, in Hz.</param>
        public void CalculateSampleRates(double frequency)
        {
            CalculateSampleRate(frequency, AN);
            CalculateSampleRate(frequency, BN);
            CalculateSampleRate(frequency, CN);
        }

        /// <summary>
        /// Explicitly sets the sample rate for each set of
        /// <see cref="MeasurementData"/> in this measurement data set.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        public void SetSampleRate(int sampleRate)
        {
            AN.SampleRate = sampleRate;
            BN.SampleRate = sampleRate;
            CN.SampleRate = sampleRate;
        }

        /// <summary>
        /// Writes all voltage measurement data to a CSV file. 
        /// </summary>
        /// <param name="fileName">Export file name.</param>
        public void ExportVoltageDataToCSV(string fileName)
        {
            const string Header = "Time,AN,BN,CN,AB,BC,CA";

            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write the data to the file
                    for (int i = 0; i < AN.Times.Length; i++)
                    {
                        string time = new DateTime(AN.Times[i]).ToString(DateTimeFormat);

                        double an = AN.Measurements[i];
                        double bn = BN.Measurements[i];
                        double cn = CN.Measurements[i];

                        fileWriter.Write("{0},{1},{2},{3},", time, an, bn, cn);
                        fileWriter.WriteLine("{0},{1},{2}", an - bn, bn - cn, cn - an);
                    }
                }
            }
        }

        /// <summary>
        /// Writes all current measurement data to a CSV file. 
        /// </summary>
        /// <param name="fileName">Export file name.</param>
        public void ExportCurrentDataToCSV(string fileName)
        {
            const string Header = "Time,AN,BN,CN";

            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write the data to the file
                    for (int i = 0; i < AN.Times.Length; i++)
                    {
                        string time = new DateTime(AN.Times[i]).ToString(DateTimeFormat);

                        double an = AN.Measurements[i];
                        double bn = BN.Measurements[i];
                        double cn = CN.Measurements[i];

                        fileWriter.WriteLine("{0},{1},{2},{3}", time, an, bn, cn);
                    }
                }
            }
        }

        private void CalculateSampleRate(double frequency, MeasurementData measurementData)
        {
            long[] times;
            long startTicks;
            long endTicks;
            double cycles;

            // Get the collection of measurement timestamps
            times = measurementData.Times;

            // Determine the start and end time of the data set
            startTicks = times[0];
            endTicks = times[times.Length - 1];

            // Determine the number of cycles in the file,
            // based on the system frequency
            cycles = frequency * Ticks.ToSeconds(endTicks - startTicks);

            // Calculate the number of samples per cycle
            measurementData.SampleRate = (int)Math.Round(times.Length / cycles);
        }

        #endregion

        #region [ Static ]

        // Static Methods

        /// <summary>
        /// Writes all measurement data to a CSV file. 
        /// </summary>
        /// <param name="fileName">Export file name.</param>
        /// <param name="voltageData">The voltage measurement data to be written to the file.</param>
        /// <param name="currentData">The current measurement data to be written to the file.</param>
        public static void ExportToCSV(string fileName, MeasurementDataSet voltageData, MeasurementDataSet currentData)
        {
            const string Header = "Time,AN V,BN V,CN V,AB V,BC V,CA V,AN I,BN I,CN I";

            using (FileStream fileStream = File.Create(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write the data to the file
                    for (int i = 0; i < voltageData.AN.Times.Length; i++)
                    {
                        string time = new DateTime(voltageData.AN.Times[i]).ToString(DateTimeFormat);

                        double vAN = voltageData.AN.Measurements[i];
                        double vBN = voltageData.BN.Measurements[i];
                        double vCN = voltageData.CN.Measurements[i];

                        double iAN = currentData.AN.Measurements[i];
                        double iBN = currentData.BN.Measurements[i];
                        double iCN = currentData.CN.Measurements[i];

                        fileWriter.Write("{0},{1},{2},{3},", time, vAN, vBN, vCN);
                        fileWriter.Write("{0},{1},{2},", vAN - vBN, vBN - vCN, vCN - vAN);
                        fileWriter.WriteLine("{0},{1},{2}", iAN, iBN, iCN);
                    }
                }
            }
        }

        #endregion
    }
}