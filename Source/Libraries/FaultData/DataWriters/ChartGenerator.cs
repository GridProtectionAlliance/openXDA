//******************************************************************************************************
//  ChartGenerator.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/12/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using FaultData.DataAnalysis;
using FaultData.Database;
using FaultData.Database.FaultLocationDataTableAdapters;
using FaultData.Database.MeterDataTableAdapters;
using CycleDataTableAdapter = FaultData.Database.MeterDataTableAdapters.CycleDataTableAdapter;
using DataPoint = FaultData.DataAnalysis.DataPoint;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace FaultData.DataWriters
{
    public class ChartGenerator
    {
        #region [ Members ]

        // Fields
        private string m_lengthUnits;

        private DbAdapterContainer m_dbAdapterContainer;
        private int m_eventID;

        private Lazy<VIDataGroup> m_viDataGroup;
        private Lazy<VICycleDataGroup> m_viCycleDataGroup;
        private Lazy<Dictionary<string, DataSeries>> m_faultCurveLookup;
        private Dictionary<string, Lazy<DataSeries>> m_seriesLookup;

        #endregion

        #region [ Constructors ]

        public ChartGenerator(DbAdapterContainer dbAdapterContainer, int eventID)
        {
            m_lengthUnits = "miles";

            m_dbAdapterContainer = dbAdapterContainer;
            m_eventID = eventID;

            m_viDataGroup = new Lazy<VIDataGroup>(GetVIDataGroup);
            m_viCycleDataGroup = new Lazy<VICycleDataGroup>(GetVICycleDataGroup);
            m_faultCurveLookup = new Lazy<Dictionary<string, DataSeries>>(GetFaultCurveLookup);
            m_seriesLookup = new Dictionary<string, Lazy<DataSeries>>(StringComparer.OrdinalIgnoreCase);

            InitializeSeriesLookup();
        }

        #endregion

        #region [ Properties ]

        public int EventID
        {
            get
            {
                return m_eventID;
            }
        }

        public string LengthUnits
        {
            get
            {
                return m_lengthUnits;
            }
            set
            {
                m_lengthUnits = value;
            }
        }

        #endregion

        #region [ Methods ]

        public Chart GenerateChart(string title, List<string> keys, List<string> names, DateTime startTime, DateTime endTime)
        {
            List<DataSeries> dataSeriesList;

            Chart chart;
            ChartArea area;
            Series series;

            DateTime topOfMinute;
            double seconds;

            if (keys.Count == 0)
                return null;

            dataSeriesList = keys.Select(GetDataSeries).ToList();

            // Align startTime with top of the minute
            topOfMinute = startTime.AddTicks(-(startTime.Ticks % TimeSpan.TicksPerMinute));

            area = new ChartArea();
            area.AxisX.Title = string.Format("Seconds since {0:MM/dd/yyyy HH:mm}", startTime);
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Format = "0.000";
            area.AxisY.Title = title;
            area.AxisY.LabelStyle.Format = "0";
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart = new Chart();
            chart.Legends.Add(new Legend());
            chart.ChartAreas.Add(area);

            for (int i = 0; i < dataSeriesList.Count; i++)
            {
                series = new Series(names[i]);
                series.ChartType = SeriesChartType.FastLine;
                series.BorderWidth = 5;

                foreach (DataPoint dataPoint in dataSeriesList[i].DataPoints)
                {
                    if (dataPoint.Time < startTime)
                        continue;

                    if (dataPoint.Time > endTime)
                        break;

                    if (double.IsNaN(dataPoint.Value))
                        continue;

                    seconds = (dataPoint.Time - topOfMinute).TotalSeconds;
                    series.Points.AddXY(seconds, dataPoint.Value);
                }

                chart.Series.Add(series);
            }

            area.AxisX.Maximum = (endTime - topOfMinute).TotalSeconds;
            area.AxisX.Minimum = (startTime - topOfMinute).TotalSeconds;

            return chart;
        }

        public DateTime ToDateTime(int index)
        {
            DateTime dateTime;

            dateTime = m_seriesLookup
                .Where(kvp => kvp.Value.IsValueCreated)
                .Select(kvp => kvp.Value.Value[index].Time)
                .FirstOrDefault();

            if (dateTime != default(DateTime))
                return dateTime;

            if (m_faultCurveLookup.IsValueCreated)
                return m_faultCurveLookup.Value.First().Value[index].Time;

            return m_viDataGroup.Value.VA[index].Time;
        }

        private void InitializeSeriesLookup()
        {
            m_seriesLookup.Add("VA", new Lazy<DataSeries>(() => m_viDataGroup.Value.VA));
            m_seriesLookup.Add("VB", new Lazy<DataSeries>(() => m_viDataGroup.Value.VB));
            m_seriesLookup.Add("VC", new Lazy<DataSeries>(() => m_viDataGroup.Value.VC));
            m_seriesLookup.Add("IA", new Lazy<DataSeries>(() => m_viDataGroup.Value.IA));
            m_seriesLookup.Add("IB", new Lazy<DataSeries>(() => m_viDataGroup.Value.IB));
            m_seriesLookup.Add("IC", new Lazy<DataSeries>(() => m_viDataGroup.Value.IC));
            m_seriesLookup.Add("IR", new Lazy<DataSeries>(() => m_viDataGroup.Value.IR));

            m_seriesLookup.Add("VA RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.VA.RMS));
            m_seriesLookup.Add("VB RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.VB.RMS));
            m_seriesLookup.Add("VC RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.VC.RMS));
            m_seriesLookup.Add("IA RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.IA.RMS));
            m_seriesLookup.Add("IB RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.IB.RMS));
            m_seriesLookup.Add("IC RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.IC.RMS));
            m_seriesLookup.Add("IR RMS", new Lazy<DataSeries>(() => m_viCycleDataGroup.Value.IR.RMS));
        }

        private DataSeries GetDataSeries(string key)
        {
            Lazy<DataSeries> lazySeries;

            if (!m_seriesLookup.TryGetValue(key, out lazySeries))
                return m_faultCurveLookup.Value[key];

            return lazySeries.Value;
        }

        private VIDataGroup GetVIDataGroup()
        {
            MeterInfoDataContext meterInfo = m_dbAdapterContainer.GetAdapter<MeterInfoDataContext>();
            EventTableAdapter eventAdapter = m_dbAdapterContainer.GetAdapter<EventTableAdapter>();
            MeterData.EventRow eventRow = eventAdapter.GetDataByID(m_eventID)[0];
            Meter meter = meterInfo.Meters.Single(m => m.ID == eventRow.MeterID);
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, eventRow.Data);
            return new VIDataGroup(dataGroup);
        }

        private VICycleDataGroup GetVICycleDataGroup()
        {
            CycleDataTableAdapter cycleDataAdapter = m_dbAdapterContainer.GetAdapter<CycleDataTableAdapter>();
            MeterData.CycleDataRow cycleDataRow = cycleDataAdapter.GetDataBy(m_eventID)[0];
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(cycleDataRow.Data);
            return new VICycleDataGroup(dataGroup);
        }

        private Dictionary<string, DataSeries> GetFaultCurveLookup()
        {
            FaultCurveTableAdapter faultCurveAdapter = m_dbAdapterContainer.GetAdapter<FaultCurveTableAdapter>();
            FaultLocationData.FaultCurveDataTable faultCurveTable = faultCurveAdapter.GetDataBy(m_eventID);

            Func<FaultLocationData.FaultCurveRow, DataSeries> toDataSeries = faultCurve =>
            {
                DataGroup dataGroup = new DataGroup();
                dataGroup.FromData(faultCurve.Data);
                return dataGroup[0];
            };

            return faultCurveTable.ToDictionary(faultCurve => faultCurve.Algorithm, toDataSeries);
        }

        #endregion
    }
}
