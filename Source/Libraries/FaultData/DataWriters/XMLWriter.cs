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
using System.Linq;
using System.Xml.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using GSF.Units;
using CycleDataTableAdapter = FaultData.Database.MeterDataTableAdapters.CycleDataTableAdapter;
using Line = FaultData.Database.Line;

namespace FaultData.DataWriters
{
    /// <summary>
    /// Writes the results of fault analysis to an XML file.
    /// </summary>
    public class XMLWriter
    {
        #region [ Members ]

        // Nested Types
        private class FaultRecordInfo
        {
            public FileGroup FileGroup;
            public Meter Meter;
            public Line Line;

            public MeterData.EventRow Event;
            public MeterData.CycleDataRow CycleData;
            public List<FaultSegment> FaultSegments;
            public List<FaultLocationData.FaultCurveRow> FaultCurves;
        }

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";
        const string DoubleFormat = "0.####";

        // Fields
        private string m_connectionString;
        private double m_maxFaultDistanceMultiplier;
        private double m_minFaultDistanceMultiplier;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="XMLWriter"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database containing fault location data.</param>
        public XMLWriter(string connectionString)
        {
            m_connectionString = connectionString;
        }

        #endregion

        #region [ Properties ]

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

        public void WriteResults(int eventID, string resultsFilePath)
        {
            FaultRecordInfo faultRecordInfo = new FaultRecordInfo();

            using (FileInfoDataContext fileInfo = new FileInfoDataContext(m_connectionString))
            using (MeterInfoDataContext meterInfo = new MeterInfoDataContext(m_connectionString))
            using (FaultLocationInfoDataContext faultLocationInfo = new FaultLocationInfoDataContext(m_connectionString))
            using (EventTableAdapter eventAdapter = new EventTableAdapter())
            using (CycleDataTableAdapter cycleDataAdapter = new CycleDataTableAdapter())
            using (FaultCurveTableAdapter faultCurveAdapter = new FaultCurveTableAdapter())
            {
                eventAdapter.Connection.ConnectionString = m_connectionString;
                cycleDataAdapter.Connection.ConnectionString = m_connectionString;
                faultCurveAdapter.Connection.ConnectionString = m_connectionString;

                faultRecordInfo.Event = eventAdapter.GetDataByID(eventID).First();

                faultRecordInfo.FileGroup = fileInfo.FileGroups.Single(fg => faultRecordInfo.Event.FileGroupID == fg.ID);
                faultRecordInfo.Meter = meterInfo.Meters.Single(m => faultRecordInfo.Event.MeterID == m.ID);
                faultRecordInfo.Line = meterInfo.Lines.Single(l => faultRecordInfo.Event.LineID == l.ID);

                faultRecordInfo.CycleData = cycleDataAdapter.GetDataBy(eventID).Single();
                faultRecordInfo.FaultSegments = faultLocationInfo.FaultSegments.Where(segment => segment.EventID == eventID).ToList();
                faultRecordInfo.FaultCurves = faultCurveAdapter.GetDataBy(eventID).ToList();

                WriteResults(faultRecordInfo, resultsFilePath);
            }
        }

        private void WriteResults(FaultRecordInfo faultRecordInfo, string resultsFilePath)
        {
            XDocument resultsDocument;
            XElement results;

            VICycleDataGroup viCycleDataGroup = ToCycleDataSet(faultRecordInfo.CycleData);

            results =
                new XElement("results",
                    new XElement("meter", faultRecordInfo.Meter.Name),
                    new XElement("disturbanceFiles", GetPathElements(faultRecordInfo)),
                    new XElement("line",
                        new XElement("name", faultRecordInfo.Line.Name),
                        new XElement("length", faultRecordInfo.Line.Length.ToString(DoubleFormat))
                    ),
                    new XElement("prefault",
                        new XElement("time", viCycleDataGroup.VA.RMS[0].Time.ToString(DateTimeFormat)),
                        GetCycleElements(viCycleDataGroup, 0)
                    ),
                    GetFaultElements(faultRecordInfo, viCycleDataGroup)
                );

            // Create the XML document
            resultsDocument =
                new XDocument(
                    new XElement("openFLE", results)
                );

            resultsDocument.Save(resultsFilePath);
        }

        private List<XElement> GetFaultElements(FaultRecordInfo faultRecordInfo, VICycleDataGroup viCycleDataGroup)
        {
            List<List<FaultSegment>> faults = faultRecordInfo.FaultSegments
                .Aggregate(new List<List<FaultSegment>>(), (list, segment) =>
                {
                    if (segment.SegmentType.Name == "Prefault" || segment.SegmentType.Name == "Postfault")
                    {
                        list.Add(new List<FaultSegment>());
                    }
                    else
                    {
                        if (list.Count == 0)
                            list.Add(new List<FaultSegment>());

                        list.Last().Add(segment);
                    }

                    return list;
                });

            List<DataSeries> faultCurves = faultRecordInfo.FaultCurves
                .Select(ToDataSeries)
                .ToList();

            return faults
                .Where(fault => fault.Count > 0)
                .Select(fault => GetFaultElement(faultRecordInfo.Line, viCycleDataGroup, faultCurves, fault))
                .Where(element => (object)element != null)
                .ToList();
        }

        private XElement GetFaultElement(Line line, VICycleDataGroup viCycleDataGroup, List<DataSeries> faultCurves, List<FaultSegment> fault)
        {
            DateTime startTime = fault.First().StartTime;
            DateTime endTime = fault.Last().EndTime;
            int startSample = fault.First().StartSample;
            int endSample = fault.Last().EndSample;
            double duration = (endTime - startTime).TotalSeconds;
            VICycleDataGroup subGroup = viCycleDataGroup.ToSubSet(startSample, endSample);

            List<double> validDistances = faultCurves
                .Select(series => series.ToSubSeries(startSample, endSample))
                .SelectMany(series => series.DataPoints)
                .Select(dataPoint => dataPoint.Value)
                .Where(value => value >= MinFaultDistanceMultiplier * line.Length)
                .Where(value => value <= MaxFaultDistanceMultiplier * line.Length)
                .OrderBy(value => value)
                .ToList();

            if (!validDistances.Any())
            {
                validDistances = faultCurves
                    .Select(series => series.ToSubSeries(startSample, endSample))
                    .SelectMany(series => series.DataPoints)
                    .Select(dataPoint => dataPoint.Value)
                    .OrderBy(value => value)
                    .ToList();
            }

            return new XElement("fault",
                new XElement("distance", validDistances[validDistances.Count / 2].ToString(DoubleFormat)),
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
                GetSegmentElements(fault),
                new XElement("largestCurrent", GetCycleElements(subGroup, GetLargestCurrentCycleIndex(subGroup))),
                new XElement("smallestVoltage", GetCycleElements(subGroup, GetSmallestVoltageCycleIndex(subGroup)))
            );
        }

        private List<XElement> GetSegmentElements(List<FaultSegment> segments)
        {
            return segments
                .Select(GetSegmentElement)
                .ToList();
        }

        private XElement GetSegmentElement(FaultSegment segment)
        {
            double duration = (segment.EndTime - segment.StartTime).TotalSeconds;

            return new XElement("segment",
                new XElement("type", segment.SegmentType.Name),
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

        private static List<XElement> GetPathElements(FaultRecordInfo faultRecordInfo)
        {
            List<XElement> pathElements = faultRecordInfo.FileGroup.DataFiles
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

        private int GetLargestCurrentCycleIndex(VICycleDataGroup viCycleDataGroup)
        {
            double largestCurrent = viCycleDataGroup.IA.RMS[0].Value + viCycleDataGroup.IB.RMS[0].Value + viCycleDataGroup.IC.RMS[0].Value;
            int largestCurrentIndex = 0;

            double iCurrent;

            for (int i = 1; i < viCycleDataGroup.IA.ToDataGroup().Samples; i++)
            {
                iCurrent = viCycleDataGroup.IA.RMS[i].Value + viCycleDataGroup.IB.RMS[i].Value + viCycleDataGroup.IC.RMS[i].Value;

                if (iCurrent > largestCurrent)
                {
                    largestCurrent = iCurrent;
                    largestCurrentIndex = i;
                }
            }

            return largestCurrentIndex;
        }

        private int GetSmallestVoltageCycleIndex(VICycleDataGroup viCycleDataGroup)
        {
            double smallestVoltage = viCycleDataGroup.VA.RMS[0].Value + viCycleDataGroup.VB.RMS[0].Value + viCycleDataGroup.VC.RMS[0].Value;
            int smallestVoltageIndex = 0;

            double iVoltage;

            for (int i = 1; i < viCycleDataGroup.VA.ToDataGroup().Samples; i++)
            {
                iVoltage = viCycleDataGroup.VA.RMS[i].Value + viCycleDataGroup.VB.RMS[i].Value + viCycleDataGroup.VC.RMS[i].Value;

                if (iVoltage > smallestVoltage)
                {
                    smallestVoltage = iVoltage;
                    smallestVoltageIndex = i;
                }
            }

            return smallestVoltageIndex;
        }

        private VICycleDataGroup ToCycleDataSet(MeterData.CycleDataRow cycleData)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(cycleData.Data);
            return new VICycleDataGroup(dataGroup);
        }

        private DataSeries ToDataSeries(FaultLocationData.FaultCurveRow faultCurve)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(faultCurve.Data);
            return dataGroup[0];
        }

        #endregion
    }
}
