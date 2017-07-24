﻿//******************************************************************************************************
//  FaultLocationOperation.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  08/04/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.MeterDataTableAdapters;
using FaultData.DataResources;
using FaultData.DataSets;
using GSF.Collections;
using log4net;
using EventKey = System.Tuple<int, System.DateTime, System.DateTime>;
using GSF.Web.Model;

namespace FaultData.DataOperations
{
    public class FaultLocationOperation : DataOperationBase<MeterDataSet>
    {
        #region [ Members ]

        // Nested Types
        private class FaultSummarizer
        {
            #region [ Members ]

            // Fields
            public readonly FaultLocationData.FaultGroupDataTable FaultGroupTable;
            public readonly FaultLocationData.FaultSegmentDataTable FaultSegmentTable;
            public readonly FaultLocationData.FaultCurveDataTable FaultCurveTable;
            public readonly FaultLocationData.FaultSummaryDataTable FaultSummaryTable;

            public MeterDataSet MeterDataSet;
            public DataGroup DataGroup;
            public FaultGroup FaultGroup;

            private List<Tuple<EventKey, FaultLocationData.FaultGroupRow>> m_faultGroupList;
            private List<Tuple<EventKey, FaultLocationData.FaultSegmentRow>> m_faultSegmentList;
            private List<Tuple<EventKey, FaultLocationData.FaultCurveRow>> m_faultCurveList;
            private List<Tuple<EventKey, FaultLocationData.FaultSummaryRow>> m_faultSummaryList;

            private double m_systemFrequency;

            #endregion

            #region [ Constructors ]

            public FaultSummarizer(double systemFrequency)
            {
                FaultGroupTable = new FaultLocationData.FaultGroupDataTable();
                FaultSegmentTable = new FaultLocationData.FaultSegmentDataTable();
                FaultCurveTable = new FaultLocationData.FaultCurveDataTable();
                FaultSummaryTable = new FaultLocationData.FaultSummaryDataTable();

                m_faultGroupList = new List<Tuple<Tuple<int, DateTime, DateTime>, FaultLocationData.FaultGroupRow>>();
                m_faultSegmentList = new List<Tuple<EventKey, FaultLocationData.FaultSegmentRow>>();
                m_faultCurveList = new List<Tuple<EventKey, FaultLocationData.FaultCurveRow>>();
                m_faultSummaryList = new List<Tuple<EventKey, FaultLocationData.FaultSummaryRow>>();

                m_systemFrequency = systemFrequency;
            }

            #endregion

            #region [ Methods ]

            public void SummarizeFault()
            {
                FaultLocationData.FaultSegmentRow faultSegment;
                FaultLocationData.FaultCurveRow faultCurveRow;

                EventKey eventKey;

                int faultNumber;

                eventKey = Tuple.Create(DataGroup.Line.ID, DataGroup.StartTime, DataGroup.EndTime);
                faultNumber = 1;

                // Create a fault group row for the whole group of faults
                if (FaultGroup.FaultDetectionLogicResult != false || FaultGroup.FaultValidationLogicResult != false)
                    m_faultGroupList.Add(Tuple.Create(eventKey, CreateFaultGroupRow(FaultGroup)));

                foreach (Fault fault in FaultGroup.Faults)
                {
                    // Create a fault segment for the fault itself
                    faultSegment = CreateFaultSegment(fault);

                    if ((object)faultSegment != null)
                        m_faultSegmentList.Add(Tuple.Create(eventKey, faultSegment));

                    // Create fault segments for each fault type found within the fault
                    foreach (Fault.Segment segment in fault.Segments)
                    {
                        faultSegment = CreateFaultSegment(segment);

                        if ((object)faultSegment != null)
                            m_faultSegmentList.Add(Tuple.Create(eventKey, faultSegment));
                    }

                    // Create the fault summary rows for this fault
                    foreach (FaultLocationData.FaultSummaryRow faultSummaryRow in CreateFaultSummaryRows(fault, faultNumber))
                        m_faultSummaryList.Add(Tuple.Create(eventKey, faultSummaryRow));

                    // Increment the fault number
                    faultNumber++;
                }

                // Generate fault curves for each algorithm used to analyze the fault
                if (FaultGroup.Faults.Any())
                {
                    for (int i = 0; i < FaultGroup.Faults[0].Curves.Count; i++)
                    {
                        faultCurveRow = CreateFaultCurveRow(i);
                        m_faultCurveList.Add(Tuple.Create(eventKey, faultCurveRow));
                    }
                }
            }

            public void FillTables(DbAdapterContainer dbAdapterContainer)
            {
                MeterData.EventDataTable eventTable;
                Dictionary<EventKey, MeterData.EventRow> eventLookup;
                MeterData.EventRow eventRow;

                eventTable = dbAdapterContainer.GetAdapter<EventTableAdapter>().GetDataByFileGroup(MeterDataSet.FileGroup.ID);

                eventLookup = eventTable
                    .Where(evt => evt.MeterID == MeterDataSet.Meter.ID)
                    .GroupBy(CreateEventKey)
                    .ToDictionary(grouping => grouping.Key, grouping =>
                    {
                        if (grouping.Count() > 1)
                            Log.Warn($"Duplicate event found for meter {MeterDataSet.Meter.AssetKey}: {string.Join(", ", grouping.Select(evt => evt.ID))}");

                        return grouping.First();
                    });

                foreach (Tuple<EventKey, FaultLocationData.FaultGroupRow> faultGroup in m_faultGroupList)
                {
                    if (eventLookup.TryGetValue(faultGroup.Item1, out eventRow))
                    {
                        faultGroup.Item2.EventID = eventRow.ID;
                        FaultGroupTable.AddFaultGroupRow(faultGroup.Item2);
                    }
                }

                foreach (Tuple<EventKey, FaultLocationData.FaultSegmentRow> faultSegment in m_faultSegmentList)
                {
                    if (eventLookup.TryGetValue(faultSegment.Item1, out eventRow))
                    {
                        faultSegment.Item2.EventID = eventRow.ID;
                        FaultSegmentTable.AddFaultSegmentRow(faultSegment.Item2);
                    }
                }

                foreach (Tuple<EventKey, FaultLocationData.FaultCurveRow> faultCurve in m_faultCurveList)
                {
                    if (eventLookup.TryGetValue(faultCurve.Item1, out eventRow))
                    {
                        faultCurve.Item2.EventID = eventRow.ID;
                        FaultCurveTable.AddFaultCurveRow(faultCurve.Item2);
                    }
                }

                foreach (Tuple<EventKey, FaultLocationData.FaultSummaryRow> faultSummary in m_faultSummaryList)
                {
                    if (eventLookup.TryGetValue(faultSummary.Item1, out eventRow))
                    {
                        faultSummary.Item2.EventID = eventRow.ID;
                        FaultSummaryTable.AddFaultSummaryRow(faultSummary.Item2);
                    }
                }
            }

            private FaultLocationData.FaultGroupRow CreateFaultGroupRow(FaultGroup faultGroup)
            {
                FaultLocationData.FaultGroupRow faultGroupRow = FaultGroupTable.NewFaultGroupRow();

                if (faultGroup.FaultDetectionLogicResult.HasValue)
                    faultGroupRow.FaultDetectionLogicResult = faultGroup.FaultDetectionLogicResult.GetValueOrDefault() ? 1 : 0;

                faultGroupRow.DefaultFaultDetectionLogicResult = faultGroup.DefaultFaultDetectionLogicResult ? 1 : 0;
                faultGroupRow.FaultValidationLogicResult = faultGroup.FaultValidationLogicResult ? 1 : 0;

                return faultGroupRow;
            }

            private FaultLocationData.FaultSegmentRow CreateFaultSegment(Fault fault)
            {
                SegmentType segmentType;
                FaultLocationData.FaultSegmentRow faultSegment;

                // ReSharper disable once InconsistentlySynchronizedField
                if (s_segmentTypeLookup.TryGetValue("Fault", out segmentType))
                {
                    faultSegment = FaultSegmentTable.NewFaultSegmentRow();
                    faultSegment.SegmentTypeID = segmentType.ID;
                    faultSegment.StartTime = fault.InceptionTime;
                    faultSegment.EndTime = fault.ClearingTime;
                    faultSegment.StartSample = fault.StartSample;
                    faultSegment.EndSample = fault.EndSample;

                    return faultSegment;
                }

                return null;
            }

            private FaultLocationData.FaultSegmentRow CreateFaultSegment(Fault.Segment segment)
            {
                string segmentTypeName;
                SegmentType segmentType;
                FaultLocationData.FaultSegmentRow faultSegment;

                segmentTypeName = string.Format("{0} Fault", segment.FaultType).Replace("ABC", "3-Phase");

                // ReSharper disable once InconsistentlySynchronizedField
                if (s_segmentTypeLookup.TryGetValue(segmentTypeName, out segmentType))
                {
                    faultSegment = FaultSegmentTable.NewFaultSegmentRow();
                    faultSegment.SegmentTypeID = segmentType.ID;
                    faultSegment.StartTime = segment.StartTime;
                    faultSegment.EndTime = segment.EndTime;
                    faultSegment.StartSample = segment.StartSample;
                    faultSegment.EndSample = segment.EndSample;

                    return faultSegment;
                }

                return null;
            }

            private IEnumerable<FaultLocationData.FaultSummaryRow> CreateFaultSummaryRows(Fault fault, int faultNumber)
            {
                FaultLocationData.FaultSummaryRow faultSummaryRow;
                double durationSeconds;

                foreach (Fault.Summary summary in fault.Summaries)
                {
                    // Calculate the duration of the fault in seconds
                    durationSeconds = fault.Duration.TotalSeconds;

                    // Create the fault summary record to be written to the database
                    faultSummaryRow = FaultSummaryTable.NewFaultSummaryRow();
                    faultSummaryRow.Algorithm = summary.DistanceAlgorithm;
                    faultSummaryRow.FaultNumber = faultNumber;
                    faultSummaryRow.CalculationCycle = fault.CalculationCycle;
                    faultSummaryRow.Distance = ToDbFloat(summary.Distance);
                    faultSummaryRow.CurrentMagnitude = ToDbFloat(fault.CurrentMagnitude);
                    faultSummaryRow.CurrentLag = ToDbFloat(fault.CurrentLag);
                    faultSummaryRow.PrefaultCurrent = ToDbFloat(fault.PrefaultCurrent);
                    faultSummaryRow.PostfaultCurrent = ToDbFloat(fault.PostfaultCurrent);
                    faultSummaryRow.Inception = fault.InceptionTime;
                    faultSummaryRow.DurationSeconds = durationSeconds;
                    faultSummaryRow.DurationCycles = durationSeconds * m_systemFrequency;
                    faultSummaryRow.FaultType = fault.Type.ToString();
                    faultSummaryRow.IsSelectedAlgorithm = summary.IsSelectedAlgorithm ? 1 : 0;
                    faultSummaryRow.IsValid = summary.IsValid ? 1 : 0;
                    faultSummaryRow.IsSuppressed = fault.IsSuppressed ? 1 : 0;

                    yield return faultSummaryRow;
                }
            }

            private FaultLocationData.FaultCurveRow CreateFaultCurveRow(int curveIndex)
            {
                FaultLocationData.FaultCurveRow faultCurveRow;
                DataSeries series;
                int startSample;
                int endSample;

                // Create the fault curve record to be written to the database
                faultCurveRow = FaultCurveTable.NewFaultCurveRow();
                faultCurveRow.Algorithm = FaultGroup.Faults[0].Curves[curveIndex].Algorithm;

                series = DataGroup.DataSeries[0].Multiply(double.NaN);

                foreach (Fault fault in FaultGroup.Faults)
                {
                    startSample = fault.StartSample;
                    endSample = startSample + fault.Curves.Min(curve => curve.Series.DataPoints.Count) - 1;

                    for (int sample = fault.StartSample; sample <= endSample; sample++)
                        series[sample].Value = fault.Curves[curveIndex].Series[sample - fault.StartSample].Value;
                }

                faultCurveRow.Data = Serialize(series);

                return faultCurveRow;
            }

            private double ToDbFloat(double value)
            {
                const double Invalid = -1.0e308D;

                if (double.IsNaN(value) || double.IsInfinity(value))
                    return Invalid;

                return value;
            }

            private byte[] Serialize(DataSeries series)
            {
                DataGroup group = new DataGroup();
                group.Add(series);
                return group.ToData();
            }

            private EventKey CreateEventKey(MeterData.EventRow eventRow)
            {
                return Tuple.Create(eventRow.LineID, eventRow.StartTime, eventRow.EndTime);
            }

            #endregion
        }

        // Fields
        private DbAdapterContainer m_dbAdapterContainer;
        private FaultSummarizer m_faultSummarizer;
        private double m_systemFrequency;

        #endregion

        #region [ Properties ]

        [Setting]
        public double SystemFrequency
        {
            get
            {
                return m_systemFrequency;
            }
            set
            {
                m_systemFrequency = value;
            }
        }

        #endregion

        #region [ Methods ]

        public override void Prepare(DbAdapterContainer dbAdapterContainer)
        {
            if ((object)s_segmentTypeLookup == null)
            {
                lock (SegmentTypeLookupLock)
                {
                    if ((object)s_segmentTypeLookup == null)
                        InitializeSegmentTypeLookup(dbAdapterContainer);
                }
            }

            m_dbAdapterContainer = dbAdapterContainer;
        }

        public override void Prepare(DataContext dataContext)
        {
        }


        public override void Execute(MeterDataSet meterDataSet)
        {
            FaultDataResource faultDataResource = meterDataSet.GetResource(() => new FaultDataResource(m_dbAdapterContainer));
            CycleDataResource cycleDataResource = CycleDataResource.GetResource(meterDataSet, m_dbAdapterContainer);
            FaultGroup faultGroup;

            Log.Info("Executing operation to load fault location data into the database...");

            m_faultSummarizer = new FaultSummarizer(m_systemFrequency);
            m_faultSummarizer.MeterDataSet = meterDataSet;

            foreach (DataGroup dataGroup in cycleDataResource.DataGroups)
            {
                if (!faultDataResource.FaultLookup.TryGetValue(dataGroup, out faultGroup))
                    continue;

                // Generate summary rows for the fault to be entered into the database
                m_faultSummarizer.DataGroup = dataGroup;
                m_faultSummarizer.FaultGroup = faultGroup;
                m_faultSummarizer.SummarizeFault();
            }
        }

        public override void Load(DbAdapterContainer dbAdapterContainer)
        {
            FaultLocationData.FaultGroupDataTable faultGroupTable;
            FaultLocationData.FaultSegmentDataTable faultSegmentTable;
            FaultLocationData.FaultCurveDataTable faultCurveTable;
            FaultLocationData.FaultSummaryDataTable faultSummaryTable;

            m_faultSummarizer.FillTables(dbAdapterContainer);

            faultGroupTable = m_faultSummarizer.FaultGroupTable;
            faultSegmentTable = m_faultSummarizer.FaultSegmentTable;
            faultCurveTable = m_faultSummarizer.FaultCurveTable;
            faultSummaryTable = m_faultSummarizer.FaultSummaryTable;

            if (faultSegmentTable.Count == 0 && faultCurveTable.Count == 0 && faultSummaryTable.Count == 0)
                return;

            Log.Info("Loading fault data into the database...");

            BulkLoader bulkLoader = new BulkLoader();
            bulkLoader.Connection = dbAdapterContainer.Connection;
            bulkLoader.CommandTimeout = dbAdapterContainer.CommandTimeout;
            bulkLoader.Load(faultGroupTable);
            bulkLoader.Load(faultSegmentTable);
            bulkLoader.Load(faultCurveTable);
            bulkLoader.Load(faultSummaryTable);

            Log.Info(string.Format("Loaded {0} faults into the database.", faultSummaryTable.Count));
        }

        private void InitializeSegmentTypeLookup(DbAdapterContainer dbAdapterContainer)
        {
            Func<string, SegmentType> segmentTypeFactory;
            FaultLocationInfoDataContext faultLocationInfo;

            faultLocationInfo = dbAdapterContainer.GetAdapter<FaultLocationInfoDataContext>();

            segmentTypeFactory = name =>
            {
                SegmentType segmentType = new SegmentType()
                {
                    Name = name,
                    Description = name
                };

                faultLocationInfo.SegmentTypes.InsertOnSubmit(segmentType);

                return segmentType;
            };

            s_segmentTypeLookup = faultLocationInfo.SegmentTypes
                .GroupBy(segmentType => segmentType.Name)
                .ToDictionary(grouping => grouping.Key, grouping =>
                {
                    if (grouping.Count() > 1)
                        Log.Warn($"Duplicate segment type found: {grouping.Key}");

                    return grouping.First();
                });

            s_segmentTypeLookup.GetOrAdd("Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("AN Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("BN Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("CN Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("AB Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("BC Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("CA Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("ABG Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("BCG Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("CAG Fault", segmentTypeFactory);
            s_segmentTypeLookup.GetOrAdd("3-Phase Fault", segmentTypeFactory);

            faultLocationInfo.SubmitChanges();
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly object SegmentTypeLookupLock = new object();
        private static Dictionary<string, SegmentType> s_segmentTypeLookup;
        private static readonly ILog Log = LogManager.GetLogger(typeof(FaultLocationOperation));

        #endregion
    }
}
