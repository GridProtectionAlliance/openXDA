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
//  --------------------------------------------------------------------------------------------------- 
//  Portions of this work are derived from "openFLE" which is an Electric Power Research Institute, Inc.
//  (EPRI) copyrighted open source software product released under the BSD license.  openFLE carries
//  the following copyright notice: Version 1.0 - Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC.
//  All rights reserved.
//  ---------------------------------------------------------------------------------------------------
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
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.IO;
using GSF.Units;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an XML file.
    /// </summary>
    public class XMLWriter : IDataWriter
    {
        #region [ Members ]

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";
        const string DoubleFormat = "0.####";

        // Fields
        private string m_resultsPath;

        private double m_maxVoltage;
        private double m_maxCurrent;
        private double m_lowVoltageThreshold;
        private double m_maxLowVoltageCurrent;
        private double m_maxTimeOffset;
        private double m_minTimeOffset;

        private double m_residualCurrentTrigger;
        private double m_phaseCurrentTrigger;
        private double m_prefaultTrigger;
        private double m_faultSuppressionTrigger;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;

        #endregion

        #region [ Properties ]

        public string ResultsPath
        {
            get
            {
                return m_resultsPath;
            }
            set
            {
                m_resultsPath = value;
            }
        }

        public double MaxVoltage
        {
            get
            {
                return m_maxVoltage;
            }
            set
            {
                m_maxVoltage = value;
            }
        }

        public double MaxCurrent
        {
            get
            {
                return m_maxCurrent;
            }
            set
            {
                m_maxCurrent = value;
            }
        }

        public double LowVoltageThreshold
        {
            get
            {
                return m_lowVoltageThreshold;
            }
            set
            {
                m_lowVoltageThreshold = value;
            }
        }

        public double MaxLowVoltageCurrent
        {
            get
            {
                return m_maxLowVoltageCurrent;
            }
            set
            {
                m_maxLowVoltageCurrent = value;
            }
        }

        public double MaxTimeOffset
        {
            get
            {
                return m_maxTimeOffset;
            }
            set
            {
                m_maxTimeOffset = value;
            }
        }

        public double MinTimeOffset
        {
            get
            {
                return m_minTimeOffset;
            }
            set
            {
                m_minTimeOffset = value;
            }
        }

        public double ResidualCurrentTrigger
        {
            get
            {
                return m_residualCurrentTrigger;
            }
            set
            {
                m_residualCurrentTrigger = value;
            }
        }

        public double PhaseCurrentTrigger
        {
            get
            {
                return m_phaseCurrentTrigger;
            }
            set
            {
                m_phaseCurrentTrigger = value;
            }
        }

        public double PrefaultTrigger
        {
            get
            {
                return m_prefaultTrigger;
            }
            set
            {
                m_prefaultTrigger = value;
            }
        }

        public double FaultSuppressionTrigger
        {
            get
            {
                return m_faultSuppressionTrigger;
            }
            set
            {
                m_faultSuppressionTrigger = value;
            }
        }

        public double MaxFaultDistanceMultiplier
        {
            get
            {
                return m_maxFaultDistanceMultiplier;
            }
            set
            {
                m_maxFaultDistanceMultiplier = value;
            }
        }

        public double MinFaultDistanceMultiplier
        {
            get
            {
                return m_minFaultDistanceMultiplier;
            }
            set
            {
                m_minFaultDistanceMultiplier = value;
            }
        }

        #endregion

        #region [ Methods ]

        public void WriteResults(DbAdapterContainer dbAdapterContainer, MeterDataSet meterDataSet)
        {
            CycleDataResource cycleDataResource;
            FaultDataResource faultDataResource;
            FaultDataResource.Factory faultDataResourceFactory;

            DataGroup dataGroup;
            VICycleDataGroup viCycleDataGroup;
            List<Fault> faults;

            string rootFileName;
            string fileName;

            faultDataResourceFactory = new FaultDataResource.Factory()
            {
                DbAdapterContainer = dbAdapterContainer,
                MaxVoltage = m_maxVoltage,
                MaxCurrent = m_maxCurrent,
                LowVoltageThreshold = m_lowVoltageThreshold,
                MaxLowVoltageCurrent = m_maxLowVoltageCurrent,
                MaxTimeOffset = m_maxTimeOffset,
                MinTimeOffset = m_minTimeOffset,
                ResidualCurrentTrigger = m_residualCurrentTrigger,
                PhaseCurrentTrigger = m_phaseCurrentTrigger,
                PrefaultTrigger = m_prefaultTrigger,
                FaultSuppressionTrigger = m_faultSuppressionTrigger,
                MaxFaultDistanceMultiplier = m_maxFaultDistanceMultiplier,
                MinFaultDistanceMultiplier = m_minFaultDistanceMultiplier
            };

            cycleDataResource = meterDataSet.GetResource<CycleDataResource>();
            faultDataResource = meterDataSet.GetResource(faultDataResourceFactory.Create);

            if (!Directory.Exists(m_resultsPath))
                Directory.CreateDirectory(m_resultsPath);

            for (int i = 0; i < cycleDataResource.DataGroups.Count; i++)
            {
                dataGroup = cycleDataResource.DataGroups[i];

                if (faultDataResource.FaultLookup.TryGetValue(dataGroup, out faults))
                {
                    rootFileName = FilePath.GetFileNameWithoutExtension(meterDataSet.FilePath);
                    fileName = string.Format("{0},{1:000},Line{2}.xml", rootFileName, i, dataGroup.Line.AssetKey);

                    viCycleDataGroup = cycleDataResource.VICycleDataGroups[i];
                    WriteResults(meterDataSet, dataGroup, viCycleDataGroup, faults, Path.Combine(m_resultsPath, fileName));
                }
            }
        }

        private void WriteResults(MeterDataSet meterDataSet, DataGroup dataGroup, VICycleDataGroup viCycleDataGroup, List<Fault> faults, string resultsFilePath)
        {
            XDocument resultsDocument;
            XElement results;
            string lineName;

            lineName = meterDataSet.Meter.MeterLines
                .Where(ml => ml.LineID == dataGroup.Line.ID)
                .Select(ml => ml.LineName)
                .FirstOrDefault() ?? dataGroup.Line.AssetKey;

            results =
                new XElement("results",
                    new XElement("meter", meterDataSet.Meter.Name),
                    new XElement("disturbanceFiles", GetPathElements(meterDataSet.FileGroup)),
                    new XElement("line",
                        new XElement("name", lineName),
                        new XElement("length", dataGroup.Line.Length.ToString(DoubleFormat))
                    ),
                    new XElement("prefault",
                        new XElement("time", dataGroup.StartTime.ToString(DateTimeFormat)),
                        GetCycleElements(viCycleDataGroup, 0)
                    ),
                    GetFaultElements(faults)
                );

            // Create the XML document
            resultsDocument =
                new XDocument(
                    new XElement("openFLE", results)
                );

            resultsDocument.Save(resultsFilePath);
        }

        private List<XElement> GetFaultElements(List<Fault> faults)
        {
            return faults
                .Select(GetFaultElement)
                .ToList();
        }

        private XElement GetFaultElement(Fault fault)
        {
            DateTime startTime = fault.Info.InceptionTime;
            DateTime endTime = fault.Info.ClearingTime;
            int startSample = fault.Info.StartSample;
            int endSample = fault.Info.EndSample;
            double duration = fault.Info.Duration.TotalSeconds;

            return new XElement("fault",
                new XElement("type", fault.Info.Type),
                new XElement("distance", fault.Info.Distance),
                new XElement("algorithm", fault.Info.DistanceAlgorithm),
                new XElement("calculationCycle", fault.Info.CalculationCycle),
                new XElement("faultCurrent", fault.Info.CurrentMagnitude),
                new XElement("inception",
                    new XElement("time", startTime.ToString(DateTimeFormat)),
                    new XElement("sample", startSample)
                ),
                new XElement("clearing",
                    new XElement("time", endTime.ToString(DateTimeFormat)),
                    new XElement("sample", endSample)
                ),
                new XElement("duration",
                    new XElement("seconds", duration.ToString(DoubleFormat)),
                    new XElement("cycles", (duration * Frequency).ToString(DoubleFormat))
                ),
                GetSegmentElements(fault)
            );
        }

        private List<XElement> GetSegmentElements(Fault fault)
        {
            return fault.Segments
                .Select(GetSegmentElement)
                .ToList();
        }

        private XElement GetSegmentElement(Fault.Segment segment)
        {
            double duration = (segment.EndTime - segment.StartTime).TotalSeconds;

            return new XElement("segment",
                new XElement("type", segment.FaultType),
                new XElement("start",
                    new XElement("time", segment.StartTime.ToString(DateTimeFormat)),
                    new XElement("index", segment.StartSample)),
                new XElement("end",
                    new XElement("time", segment.EndTime.ToString(DateTimeFormat)),
                    new XElement("index", segment.EndSample)),
                new XElement("duration",
                    new XElement("seconds", duration.ToString(DoubleFormat)),
                    new XElement("cycles", (duration * Frequency).ToString(DoubleFormat))));
        }

        private static List<XElement> GetPathElements(FileGroup fileGroup)
        {
            List<XElement> pathElements = fileGroup.DataFiles
                .Select(dataFile => new XElement("path", dataFile.FilePath))
                .ToList();

            return pathElements;
        }

        private List<XElement> GetCycleElements(VICycleDataGroup viCycleDataGroup, int cycleIndex)
        {
            return new List<XElement>()
            {
                GetCycleElement("VA", viCycleDataGroup.VA, cycleIndex),
                GetCycleElement("VB", viCycleDataGroup.VB, cycleIndex),
                GetCycleElement("VC", viCycleDataGroup.VC, cycleIndex),
                GetCycleElement("IA", viCycleDataGroup.IA, cycleIndex),
                GetCycleElement("IB", viCycleDataGroup.IB, cycleIndex),
                GetCycleElement("IC", viCycleDataGroup.IC, cycleIndex)
            };
        }

        private XElement GetCycleElement(string name, CycleDataGroup cycleDataGroup, int cycleIndex)
        {
            return new XElement(name,
                new XElement("RMSMagnitude", cycleDataGroup.RMS[cycleIndex].Value.ToString(DoubleFormat)),
                new XElement("PhaseAngle", new Angle(cycleDataGroup.Phase[cycleIndex].Value).ToDegrees().ToString(DoubleFormat)));
        }

        #endregion
    }
}
