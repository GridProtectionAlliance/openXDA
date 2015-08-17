//*********************************************************************************************************************
// CycleDataSet.cs
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
//  06/14/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GSF;
using GSF.NumericalAnalysis;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a collection of all the cycles extracted from a given data set.
    /// </summary>
    public class CycleDataSet : IEnumerable<CycleData>
    {
        #region [ Members ]

        // Fields
        private List<CycleData> m_cycles;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="CycleDataSet"/> class.
        /// </summary>
        public CycleDataSet()
        {
            m_cycles = new List<CycleData>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CycleDataSet"/> class.
        /// </summary>
        /// <param name="frequency">The frequency of the measured system, in Hz.</param>
        /// <param name="voltageDataSet">The data set containing voltage data points.</param>
        /// <param name="currentDataSet">The data set containing current data points.</param>
        public CycleDataSet(double frequency, MeasurementDataSet voltageDataSet, MeasurementDataSet currentDataSet)
        {
            Populate(frequency, voltageDataSet, currentDataSet);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the data structure containing a
        /// full cycle of data at the given index.
        /// </summary>
        /// <param name="i">The index of the cycle.</param>
        /// <returns>The cycle of data at the given index.</returns>
        public CycleData this[int i]
        {
            get
            {
                return m_cycles[i];
            }
            set
            {
                while(i >= m_cycles.Count)
                    m_cycles.Add(null);

                m_cycles[i] = value;
            }
        }

        /// <summary>
        /// Gets the size of the cycle data set.
        /// </summary>
        public int Count
        {
            get
            {
                return m_cycles.Count;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Populates the cycle data set by calculating cycle
        /// data based on the given measurement data sets.
        /// </summary>
        /// <param name="frequency">The frequency of the measured system, in Hz.</param>
        /// <param name="voltageDataSet">Data set containing voltage waveform measurements.</param>
        /// <param name="currentDataSet">Data set containing current waveform measurements.</param>
        public void Populate(double frequency, MeasurementDataSet voltageDataSet, MeasurementDataSet currentDataSet)
        {
            List<MeasurementData> measurementDataList;
            int sampleRateDivisor;
            int numberOfCycles;

            measurementDataList = new List<MeasurementData>()
            {
                voltageDataSet.AN, voltageDataSet.BN, voltageDataSet.CN,
                currentDataSet.AN, currentDataSet.BN, currentDataSet.CN
            };

            sampleRateDivisor = measurementDataList
                .Select(measurementData => measurementData.SampleRate)
                .GreatestCommonDenominator();

            numberOfCycles = measurementDataList
                .Select(measurementData => (measurementData.Measurements.Length - measurementData.SampleRate + 1) / (measurementData.SampleRate / sampleRateDivisor))
                .Min();

            for (int i = 0; i < numberOfCycles; i++)
                m_cycles.Add(new CycleData(i, sampleRateDivisor, frequency, voltageDataSet, currentDataSet));
        }

        /// <summary>
        /// Returns the index of the cycle with the largest total current.
        /// </summary>
        /// <returns>The index of the cycle with the largest total current.</returns>
        public int GetLargestCurrentIndex()
        {
            int index = 0;
            int bestFaultIndex = -1;
            double largestCurrent = 0.0D;

            foreach (CycleData cycle in m_cycles)
            {
                double totalCurrent = cycle.AN.I.RMS + cycle.BN.I.RMS + cycle.CN.I.RMS;

                if (totalCurrent > largestCurrent)
                {
                    bestFaultIndex = index;
                    largestCurrent = totalCurrent;
                }

                index++;
            }

            return bestFaultIndex;
        }

        /// <summary>
        /// Clears the cycle data set so that it can be repopulated.
        /// </summary>
        public void Clear()
        {
            m_cycles.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of cycles.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object that can be used to iterate through the collection.</returns>
        public IEnumerator<CycleData> GetEnumerator()
        {
            foreach (CycleData cycle in m_cycles)
            {
                yield return cycle;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of cycles.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region [ Static ]

        // Static Methods

        /// <summary>
        /// Exports the given <see cref="CycleDataSet"/> to a CSV file.
        /// </summary>
        /// <param name="fileName">The name of the CSV file.</param>
        /// <param name="cycles">The cycle data set to be exported.</param>
        public static void ExportToCSV(string fileName, CycleDataSet cycles)
        {
            const string Header =
                "AN V RMS,AN V Phase,AN V Peak," +
                "BN V RMS,BN V Phase,BN V Peak," +
                "CN V RMS,CN V Phase,CN V Peak," +
                "Pos V Magnitude,Pos V Angle," +
                "Neg V Magnitude,Neg V Angle," +
                "Zero V Magnitude,Zero V Angle," +
                "AN I RMS,AN I Phase,AN I Peak," +
                "BN I RMS,BN I Phase,BN I Peak," +
                "CN I RMS,CN I Phase,CN I Peak," +
                "Pos I Magnitude,Pos I Angle," +
                "Neg I Magnitude,Neg I Angle," +
                "Zero I Magnitude,Zero I Angle";

            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write data to the file
                    foreach (CycleData cycleData in cycles.m_cycles)
                        fileWriter.WriteLine(ToCSV(cycleData));
                }
            }
        }

        // Converts the cycle data to a row of CSV data.
        private static string ToCSV(CycleData cycleData)
        {
            ComplexNumber[] vSeq = CycleData.CalculateSequenceComponents(cycleData.AN.V, cycleData.BN.V, cycleData.CN.V);
            ComplexNumber[] iSeq = CycleData.CalculateSequenceComponents(cycleData.AN.I, cycleData.BN.I, cycleData.CN.I);

            string vCsv = string.Format("{0},{1},{2}", ToCSV(cycleData.AN.V), ToCSV(cycleData.BN.V), ToCSV(cycleData.CN.V));
            string vSeqCsv = string.Format("{0},{1},{2}", ToCSV(vSeq[1]), ToCSV(vSeq[2]), ToCSV(vSeq[0]));
            string iCsv = string.Format("{0},{1},{2}", ToCSV(cycleData.AN.I), ToCSV(cycleData.BN.I), ToCSV(cycleData.CN.I));
            string iSeqCsv = string.Format("{0},{1},{2}", ToCSV(iSeq[1]), ToCSV(iSeq[2]), ToCSV(iSeq[0]));

            return string.Format("{0},{1},{2},{3}", vCsv, vSeqCsv, iCsv, iSeqCsv);
        }

        // Converts the cycle to CSV data.
        private static string ToCSV(Cycle cycle)
        {
            return string.Format("{0},{1},{2}", cycle.RMS, cycle.Phase.ToDegrees(), cycle.Peak);
        }

        // Converts the sequence component to CSV data.
        private static string ToCSV(ComplexNumber sequenceComponent)
        {
            return string.Format("{0},{1}", sequenceComponent.Magnitude, sequenceComponent.Angle.ToDegrees());
        }

        #endregion
    }
}
