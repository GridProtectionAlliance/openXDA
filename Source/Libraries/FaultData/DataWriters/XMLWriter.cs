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
            public FaultLocationData.CycleDataRow CycleData;
            public List<FaultSegment> FaultSegments;
            public List<FaultLocationData.FaultCurveRow> FaultCurves;
        }

        // Constants
        // TODO: Hardcoded frequency
        private const double Frequency = 60.0D;

        // Fields
        private string m_connectionString;

        #endregion

        #region [ Constructors ]

        public XMLWriter(string connectionString)
        {
            m_connectionString = connectionString;
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

                faultRecordInfo.Event = eventAdapter.GetDataBy(eventID).First();

                faultRecordInfo.FileGroup = fileInfo.FileGroups.Single(fg => faultRecordInfo.Event.FileGroupID == fg.ID);
                faultRecordInfo.Meter = meterInfo.Meters.Single(m => faultRecordInfo.Event.MeterID == m.ID);
                faultRecordInfo.Line = meterInfo.Lines.Single(l => faultRecordInfo.Event.LineID == l.ID);

                faultRecordInfo.CycleData = cycleDataAdapter.GetDataBy(eventID).Single();
                faultRecordInfo.FaultSegments = faultLocationInfo.FaultSegments.Where(segment => segment.EventID == eventID).ToList();
                faultRecordInfo.FaultCurves = faultCurveAdapter.GetDataBy(eventID).Where(curve => curve.EventID == eventID).ToList();

                WriteResults(faultRecordInfo, resultsFilePath);
            }
        }

        private void WriteResults(FaultRecordInfo faultRecordInfo, string resultsFilePath)
        {
            const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

            XDocument resultsDocument;
            XElement results;

            VICycleDataSet viCycleDataSet = ToCycleDataSet(faultRecordInfo.CycleData);

            results =
                new XElement("results",
                    new XElement("meter", faultRecordInfo.Meter.Name),
                    new XElement("disturbanceFiles", GetPathElements(faultRecordInfo)),
                    new XElement("line",
                        new XElement("name", faultRecordInfo.Line.Name),
                        new XElement("length", faultRecordInfo.Line.Length)
                    ),
                    new XElement("prefault",
                        new XElement("time", viCycleDataSet.VA.RMS[0].Time.ToString(DateTimeFormat)),
                        GetCycleElements(viCycleDataSet, 0)
                    ),
                    GetFaultElements(faultRecordInfo, viCycleDataSet, DateTimeFormat)
                );

            // Create the XML document
            resultsDocument =
                new XDocument(
                    new XElement("openFLE", results)
                );

            resultsDocument.Save(resultsFilePath);
        }

        private List<XElement> GetFaultElements(FaultRecordInfo faultRecordInfo, VICycleDataSet viCycleDataSet, string dateTimeFormat)
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
                .Select(fault => GetFaultElement(faultRecordInfo.Line, viCycleDataSet, faultCurves, fault, dateTimeFormat))
                .Where(element => (object)element != null)
                .ToList();
        }

        private XElement GetFaultElement(Line line, VICycleDataSet viCycleDataSet, List<DataSeries> faultCurves, List<FaultSegment> fault, string dateTimeFormat)
        {
            DateTime startTime = fault.First().StartTime;
            DateTime endTime = fault.Last().EndTime;
            int startSample = fault.First().StartSample;
            int endSample = fault.Last().EndSample;
            double duration = (endTime - startTime).TotalSeconds;
            VICycleDataSet subSet = viCycleDataSet.ToSubSet(startSample, endSample);

            List<double> validDistances = faultCurves
                .Select(series => series.ToSubSeries(startSample, endSample))
                .SelectMany(series => series.DataPoints)
                .Select(dataPoint => dataPoint.Value)
                .Where(value => value >= -0.1D * line.Length)
                .Where(value => value <= 1.25D * line.Length)
                .OrderBy(value => value)
                .ToList();

            if (!validDistances.Any())
                return null;

            return new XElement("fault",
                new XElement("distance", validDistances[validDistances.Count / 2]),
                new XElement("inception",
                    new XElement("time", startTime.ToString(dateTimeFormat)),
                    new XElement("sample", startSample)
                ),
                new XElement("clearing",
                    new XElement("time", endTime.ToString(dateTimeFormat)),
                    new XElement("sample", endSample)
                ),
                new XElement("duration",
                    new XElement("seconds", duration),
                    new XElement("cycles", duration * Frequency)
                ),
                GetSegmentElements(fault, dateTimeFormat),
                new XElement("largestCurrent", GetCycleElements(subSet, GetLargestCurrentCycleIndex(subSet))),
                new XElement("smallestVoltage", GetCycleElements(subSet, GetSmallestVoltageCycleIndex(subSet)))
            );
        }

        private List<XElement> GetSegmentElements(List<FaultSegment> segments, string dateTimeFormat)
        {
            return segments
                .Select(segment => GetSegmentElement(segment, dateTimeFormat))
                .ToList();
        }

        private XElement GetSegmentElement(FaultSegment segment, string dateTimeFormat)
        {
            double duration = (segment.EndTime - segment.StartTime).TotalSeconds;

            return new XElement("segment",
                new XElement("type", segment.SegmentType.Name),
                new XElement("start",
                    new XElement("time", segment.StartTime.ToString(dateTimeFormat)),
                    new XElement("index", segment.StartSample)),
                new XElement("end",
                    new XElement("time", segment.EndTime.ToString(dateTimeFormat)),
                    new XElement("index", segment.EndSample)),
                new XElement("duration",
                    new XElement("seconds", duration),
                    new XElement("cycles", duration * Frequency)));
        }

        private static List<XElement> GetPathElements(FaultRecordInfo faultRecordInfo)
        {
            List<XElement> pathElements = faultRecordInfo.FileGroup.DataFiles
                .Select(dataFile => new XElement("path", dataFile.FilePath))
                .ToList();

            return pathElements;
        }

        private List<XElement> GetCycleElements(VICycleDataSet viCycleDataSet, int cycleIndex)
        {
            return new List<XElement>()
            {
                GetCycleElement("VA", viCycleDataSet.VA, cycleIndex),
                GetCycleElement("VB", viCycleDataSet.VB, cycleIndex),
                GetCycleElement("VC", viCycleDataSet.VC, cycleIndex),
                GetCycleElement("IA", viCycleDataSet.IA, cycleIndex),
                GetCycleElement("IB", viCycleDataSet.IB, cycleIndex),
                GetCycleElement("IC", viCycleDataSet.IC, cycleIndex)
            };
        }

        private XElement GetCycleElement(string name, CycleDataGroup cycleDataGroup, int cycleIndex)
        {
            return new XElement(name,
                new XElement("RMS", cycleDataGroup.RMS[cycleIndex].Value),
                new XElement("Phase", cycleDataGroup.Phase[cycleIndex].Value),
                new XElement("Peak", cycleDataGroup.Peak[cycleIndex].Value));
        }

        private int GetLargestCurrentCycleIndex(VICycleDataSet viCycleDataSet)
        {
            double largestCurrent = viCycleDataSet.IA.RMS[0].Value + viCycleDataSet.IB.RMS[0].Value + viCycleDataSet.IC.RMS[0].Value;
            int largestCurrentIndex = 0;

            double iCurrent;

            for (int i = 1; i < viCycleDataSet.IA.ToDataGroup().Samples; i++)
            {
                iCurrent = viCycleDataSet.IA.RMS[i].Value + viCycleDataSet.IB.RMS[i].Value + viCycleDataSet.IC.RMS[i].Value;

                if (iCurrent > largestCurrent)
                {
                    largestCurrent = iCurrent;
                    largestCurrentIndex = i;
                }
            }

            return largestCurrentIndex;
        }

        private int GetSmallestVoltageCycleIndex(VICycleDataSet viCycleDataSet)
        {
            double smallestVoltage = viCycleDataSet.VA.RMS[0].Value + viCycleDataSet.VB.RMS[0].Value + viCycleDataSet.VC.RMS[0].Value;
            int smallestVoltageIndex = 0;

            double iVoltage;

            for (int i = 1; i < viCycleDataSet.VA.ToDataGroup().Samples; i++)
            {
                iVoltage = viCycleDataSet.VA.RMS[i].Value + viCycleDataSet.VB.RMS[i].Value + viCycleDataSet.VC.RMS[i].Value;

                if (iVoltage > smallestVoltage)
                {
                    smallestVoltage = iVoltage;
                    smallestVoltageIndex = i;
                }
            }

            return smallestVoltageIndex;
        }

        private VICycleDataSet ToCycleDataSet(FaultLocationData.CycleDataRow cycleData)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(cycleData.Data);
            return new VICycleDataSet(dataGroup);
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
