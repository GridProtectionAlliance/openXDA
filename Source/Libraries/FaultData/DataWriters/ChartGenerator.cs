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
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using FaultData.DataAnalysis;
using GSF.Data;
using GSF.Data.Model;
using log4net;
using openXDA.Model;
using DataPoint = FaultData.DataAnalysis.DataPoint;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace FaultData.DataWriters
{
    public class ChartGenerator
    {
        #region [ Members ]

        // Fields
        private AdoDataConnection m_connection;
        private int m_eventID;

        private Lazy<VIDataGroup> m_viDataGroup;
        private Lazy<VICycleDataGroup> m_viCycleDataGroup;
        private Lazy<Dictionary<string, DataSeries>> m_faultCurveLookup;
        private Dictionary<string, Lazy<DataSeries>> m_seriesLookup;

        #endregion

        #region [ Constructors ]

        public ChartGenerator(AdoDataConnection connection, int eventID)
        {
            m_connection = connection;
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

        #endregion

        #region [ Methods ]

        public Chart GenerateChart(string title, List<string> keys, List<string> names, DateTime startTime, DateTime endTime)
        {
            List<DataSeries> dataSeriesList;

            Chart chart;
            ChartArea area;
            Series series;

            DateTime dataStart;
            DateTime dataEnd;
            DateTime topOfMinute;
            double seconds;

            if (keys.Count == 0)
                return null;

            dataSeriesList = keys.Select(GetDataSeries).ToList();

            // Snap startTime and endTime
            // to the data's time boundaries
            dataStart = ToDateTime(0);
            dataEnd = ToDateTime(dataSeriesList[0].DataPoints.Count - 1);

            if (startTime < dataStart)
                startTime = dataStart;

            if (endTime > dataEnd)
                endTime = dataEnd;

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
                if ((object)dataSeriesList[i] == null)
                    continue;

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
            DataSeries faultCurve;

            if (m_seriesLookup.TryGetValue(key, out lazySeries))
                return lazySeries.Value;

            if (m_faultCurveLookup.Value.TryGetValue(key, out faultCurve))
                return faultCurve;

            return null;
        }

        private VIDataGroup GetVIDataGroup()
        {
            TableOperations<Meter> meterTable = new TableOperations<Meter>(m_connection);
            TableOperations<Event> eventTable = new TableOperations<Event>(m_connection);

            Event evt = eventTable.QueryRecordWhere("ID = {0}", m_eventID);
            Meter meter = meterTable.QueryRecordWhere("ID = {0}", evt.MeterID);
            byte[] timeDomainData = m_connection.ExecuteScalar<byte[]>("SELECT TimeDomainData FROM EventData WHERE ID = {0}", evt.EventDataID);

            meter.ConnectionFactory = () => new AdoDataConnection(m_connection.Connection, m_connection.AdapterType, false);

            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(meter, timeDomainData);
            return new VIDataGroup(dataGroup);
        }

        private VICycleDataGroup GetVICycleDataGroup()
        {
            double frequency = m_connection.ExecuteScalar<double?>("SELECT Value FROM Setting WHERE Name = 'SystemFrequency'") ?? 60.0D;
            return Transform.ToVICycleDataGroup(GetVIDataGroup(), frequency);
        }

        private Dictionary<string, DataSeries> GetFaultCurveLookup()
        {
            TableOperations<FaultCurve> faultCurveTable = new TableOperations<FaultCurve>(m_connection);

            Func<FaultCurve, DataSeries> toDataSeries = faultCurve =>
            {
                DataGroup dataGroup = new DataGroup();
                dataGroup.FromData(faultCurve.Data);
                return dataGroup[0];
            };

            return faultCurveTable
                .QueryRecordsWhere("EventID = {0}", m_eventID)
                .GroupBy(faultCurve => faultCurve.Algorithm)
                .ToDictionary(grouping => grouping.Key, grouping =>
                {
                    if (grouping.Count() > 1)
                        Log.Warn($"Duplicate fault curve ({grouping.Key}) found for event {grouping.First().EventID}");

                    return toDataSeries(grouping.First());
                });
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof(ChartGenerator));

        // Static Methods
        public static Stream ConvertToChartImageStream(AdoDataConnection connection, XElement chartElement)
        {
            ChartGenerator chartGenerator;

            Lazy<DataRow> faultSummary;
            Lazy<double> systemFrequency;
            DateTime inception;
            DateTime clearing;

            int width;
            int height;
            double prefaultCycles;
            double postfaultCycles;

            string title;
            List<string> keys;
            List<string> names;
            DateTime startTime;
            DateTime endTime;

            int eventID;
            int faultID;

            // Read parameters from the XML data and set up defaults
            eventID = Convert.ToInt32((string)chartElement.Attribute("eventID") ?? "-1");
            faultID = Convert.ToInt32((string)chartElement.Attribute("faultID") ?? "-1");
            prefaultCycles = Convert.ToDouble((string)chartElement.Attribute("prefaultCycles") ?? "NaN");
            postfaultCycles = Convert.ToDouble((string)chartElement.Attribute("postfaultCycles") ?? "NaN");

            title = (string)chartElement.Attribute("yAxisTitle");
            keys = GetKeys(chartElement);
            names = GetNames(chartElement);

            width = Convert.ToInt32((string)chartElement.Attribute("width"));
            height = Convert.ToInt32((string)chartElement.Attribute("height"));

            startTime = DateTime.MinValue;
            endTime = DateTime.MaxValue;

            faultSummary = new Lazy<DataRow>(() => connection.RetrieveData("SELECT * FROM FaultSummary WHERE ID = {0}", faultID).Select().FirstOrDefault());
            systemFrequency = new Lazy<double>(() => connection.ExecuteScalar(60.0D, "SELECT Value FROM Setting WHERE Name = 'SystemFrequency'"));

            // If prefaultCycles is specified and we have a fault summary we can use,
            // we can determine the start time of the chart based on fault inception
            if (!double.IsNaN(prefaultCycles) && (object)faultSummary.Value != null)
            {
                inception = faultSummary.Value.ConvertField<DateTime>("Inception");
                startTime = inception.AddSeconds(-prefaultCycles / systemFrequency.Value);
            }

            // If postfaultCycles is specified and we have a fault summary we can use,
            // we can determine the start time of the chart based on fault clearing
            if (!double.IsNaN(postfaultCycles) && (object)faultSummary.Value != null)
            {
                inception = faultSummary.Value.ConvertField<DateTime>("Inception");
                clearing = inception.AddSeconds(faultSummary.Value.ConvertField<double>("DurationSeconds"));
                endTime = clearing.AddSeconds(postfaultCycles / systemFrequency.Value);
            }

            // Create the chart generator to generate the chart
            chartGenerator = new ChartGenerator(connection, eventID);

            using (Chart chart = chartGenerator.GenerateChart(title, keys, names, startTime, endTime))
            {
                // Set the chart size based on the specified width and height;
                // this allows us to dynamically change font sizes and line
                // widths before converting the chart to an image
                SetChartSize(chart, width, height);

                // Determine if either the minimum or maximum of the y-axis is specified explicitly
                if ((object)chartElement.Attribute("yAxisMaximum") != null)
                    chart.ChartAreas[0].AxisY.Maximum = Convert.ToDouble((string)chartElement.Attribute("yAxisMaximum"));

                if ((object)chartElement.Attribute("yAxisMinimum") != null)
                    chart.ChartAreas[0].AxisY.Minimum = Convert.ToDouble((string)chartElement.Attribute("yAxisMinimum"));

                // If the calculation cycle is to be highlighted, determine whether the highlight should be in the range of a single index or a full cycle.
                // If we have a fault summary we can use, apply the appropriate highlight based on the calculation cycle
                if (string.Equals((string)chartElement.Attribute("highlightCalculation"), "index", StringComparison.OrdinalIgnoreCase))
                {
                    if ((object)faultSummary.Value != null)
                    {
                        int calculationCycle = faultSummary.Value.ConvertField<int>("CalculationCycle");
                        DateTime calculationTime = chartGenerator.ToDateTime(calculationCycle);
                        double calculationPosition = chart.ChartAreas[0].AxisX.Minimum + (calculationTime - startTime).TotalSeconds;
                        chart.ChartAreas[0].CursorX.Position = calculationPosition;
                    }
                }
                else if (string.Equals((string)chartElement.Attribute("highlightCalculation"), "cycle", StringComparison.OrdinalIgnoreCase))
                {
                    if ((object)faultSummary.Value != null)
                    {
                        int calculationCycle = faultSummary.Value.ConvertField<int>("CalculationCycle");
                        DateTime calculationTime = chartGenerator.ToDateTime(calculationCycle);
                        double calculationPosition = chart.ChartAreas[0].AxisX.Minimum + (calculationTime - startTime).TotalSeconds;
                        chart.ChartAreas[0].CursorX.SelectionStart = calculationPosition;
                        chart.ChartAreas[0].CursorX.SelectionEnd = calculationPosition + 1.0D / 60.0D;
                    }
                }

                // Convert the generated chart to an image
                return ConvertToImageStream(chart, ChartImageFormat.Png);
            }
        }

        public static List<string> GetKeys(XElement chartElement)
        {
            return chartElement
                .Elements()
                .Select(childElement => (string)childElement.Attribute("key"))
                .ToList();
        }

        public static List<string> GetNames(XElement chartElement)
        {
            return chartElement
                .Elements()
                .Select(childElement => (string)childElement)
                .ToList();
        }

        public static void SetChartSize(Chart chart, int width, int height)
        {
            int fontSize = (int)Math.Round(height / 37.0D);
            int borderWidth = (int)Math.Round(height / 480.0D);

            chart.Width = width;
            chart.Height = height;

            chart.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = fontSize;
            chart.ChartAreas[0].AxisY.LabelAutoFitMaxFontSize = fontSize;
            chart.ChartAreas[0].AxisX.LabelAutoFitMinFontSize = fontSize;
            chart.ChartAreas[0].AxisY.LabelAutoFitMinFontSize = fontSize;
            chart.ChartAreas[0].AxisX.TitleFont = new Font(chart.ChartAreas[0].AxisX.TitleFont.FontFamily, fontSize);
            chart.ChartAreas[0].AxisY.TitleFont = new Font(chart.ChartAreas[0].AxisY.TitleFont.FontFamily, fontSize);
            chart.Legends[0].Font = new Font(chart.Legends[0].Font.FontFamily, fontSize, FontStyle.Regular);

            foreach (Series series in chart.Series)
                series.BorderWidth = borderWidth;
        }

        public static Stream ConvertToImageStream(Chart chart, ChartImageFormat format)
        {
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, format);
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}
