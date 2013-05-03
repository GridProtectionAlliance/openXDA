//*********************************************************************************************************************
// XmlFaultResultsWriter.cs
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
//  -------------------------------------------------------------------------------------------------------------------
//  03/06/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//*********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GSF.IO;

namespace FaultAlgorithms
{
    /// <summary>
    /// Writes the results of fault analysis to an XML file.
    /// </summary>
    public class XmlFaultResultsWriter : IFaultResultsWriter
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
            // Configuration information does not get written to XML results file
        }

        /// <summary>
        /// Writes the results to the output source.
        /// </summary>
        /// <param name="disturbanceRecorder">The device that collected the disturbance data.</param>
        /// <param name="disturbanceFiles">Information about the data files collected during the disturbance.</param>
        /// <param name="lineDataSets">The data sets used for analysis to determine fault location.</param>
        public void WriteResults(Device disturbanceRecorder, ICollection<DisturbanceFile> disturbanceFiles, ICollection<Tuple<Line, FaultLocationDataSet>> lineDataSets)
        {
            const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

            XElement sourceFiles;

            Line lineDefinition;
            FaultLocationDataSet faultDataSet;

            long[] times;
            int firstFaultCycleIndex;
            CycleData firstFaultCycle;
            DateTime firstFaultCycleTime;

            int faultCalculationIndex;
            CycleData faultCalculationCycle;
            double iaFault;
            double ibFault;
            double icFault;
            double vaFault;
            double vbFault;
            double vcFault;

            XDocument resultsDocument;
            XElement results;
            string resultsDirectory;
            string resultsFileName;
            string resultsFilePath;

            // Create XML element to contain
            // fault information for each line
            results =
                new XElement("results",
                    new XElement("meterStationName", disturbanceRecorder.StationName)
                );

            // Add information about source files to the results
            sourceFiles = new XElement("disturbanceFiles");

            foreach (DisturbanceFile disturbanceFile in disturbanceFiles)
                sourceFiles.Add(new XElement("sourceFilePath", disturbanceFile.SourcePath));

            results.Add(sourceFiles);

            // Add information about each faulted line to the results
            foreach (Tuple<Line, FaultLocationDataSet> lineDataSet in lineDataSets)
            {
                lineDefinition = lineDataSet.Item1;
                faultDataSet = lineDataSet.Item2;

                // Get the timestamp of the first fault cycle
                times = faultDataSet.Voltages.AN.Times;
                firstFaultCycleIndex = faultDataSet.FaultedCycles.First();
                firstFaultCycle = faultDataSet.Cycles[firstFaultCycleIndex];
                firstFaultCycleTime = new DateTime(times[firstFaultCycle.StartIndex]);

                // Get the fault calculation cycle
                faultCalculationIndex = faultDataSet.FaultCalculationCycle;

                if (faultCalculationIndex >= 0 && faultCalculationIndex < faultDataSet.Cycles.Count)
                {
                    faultCalculationCycle = faultDataSet.Cycles[faultCalculationIndex];

                    // Get values for current and voltage during fault
                    iaFault = faultCalculationCycle.AN.I.RMS;
                    ibFault = faultCalculationCycle.BN.I.RMS;
                    icFault = faultCalculationCycle.CN.I.RMS;
                    vaFault = faultCalculationCycle.AN.V.RMS;
                    vbFault = faultCalculationCycle.BN.V.RMS;
                    vcFault = faultCalculationCycle.CN.V.RMS;
                }
                else
                {
                    // Initialize to default values since
                    // we don't know the actual fault values
                    iaFault = 0.0D;
                    ibFault = 0.0D;
                    icFault = 0.0D;
                    vaFault = 0.0D;
                    vbFault = 0.0D;
                    vcFault = 0.0D;
                }

                // Add fault information for the current line to the results element
                results.Add(
                    new XElement("line",
                        new XElement("name", lineDefinition.Name),
                        new XElement("length", faultDataSet.LineDistance),
                        new XElement("faultType", faultDataSet.FaultType.ToString()),
                        new XElement("faultDistance", faultDataSet.FaultDistance),
                        new XElement("cyclesOfData", faultDataSet.Cycles.Count),
                        new XElement("faultCycles", faultDataSet.FaultCycleCount),
                        new XElement("faultCalculationCycle", faultDataSet.FaultCalculationCycle),
                        new XElement("firstFaultCycleTime", firstFaultCycleTime.ToString(dateTimeFormat)),
                        new XElement("iaFault", iaFault),
                        new XElement("ibFault", ibFault),
                        new XElement("icFault", icFault),
                        new XElement("vaFault", vaFault),
                        new XElement("vbFault", vbFault),
                        new XElement("vcFault", vcFault),
                        new XElement("iaMax", faultDataSet.Cycles.Max(cycle => cycle.AN.I.RMS)),
                        new XElement("ibMax", faultDataSet.Cycles.Max(cycle => cycle.BN.I.RMS)),
                        new XElement("icMax", faultDataSet.Cycles.Max(cycle => cycle.CN.I.RMS)),
                        new XElement("vaMin", faultDataSet.Cycles.Min(cycle => cycle.AN.V.RMS)),
                        new XElement("vbMin", faultDataSet.Cycles.Min(cycle => cycle.BN.V.RMS)),
                        new XElement("vcMin", faultDataSet.Cycles.Min(cycle => cycle.CN.V.RMS))
                    ));
            }

            // Create the XML document
            resultsDocument =
                new XDocument(
                    new XElement("openFLE", results)
                );

            // Get the directory name to place the results file in
            if (!m_parameters.TryGetValue("resultsDirectory", out resultsDirectory))
                resultsDirectory = "Results";

            resultsDirectory = FilePath.GetAbsolutePath(resultsDirectory);

            if (!Directory.Exists(resultsDirectory))
                Directory.CreateDirectory(resultsDirectory);

            // Create file path based on drop file name and avoid file name collisions
            resultsFileName = string.Format("{0}.xml", FilePath.GetFileNameWithoutExtension(disturbanceFiles.First().DestinationPath));
            resultsFilePath = Path.Combine(resultsDirectory, resultsFileName);

            for (int i = 2; File.Exists(resultsFilePath); i++)
            {
                resultsFileName = string.Format("{0}({1:D2}).xml", FilePath.GetFileNameWithoutExtension(disturbanceFiles.First().DestinationPath), i);
                resultsFilePath = Path.Combine(FilePath.GetAbsolutePath(resultsDirectory), resultsFileName);
            }

            resultsDocument.Save(resultsFilePath);
        }

        #endregion
    }
}
