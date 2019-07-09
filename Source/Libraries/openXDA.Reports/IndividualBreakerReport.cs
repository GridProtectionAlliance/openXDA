//******************************************************************************************************
//  IndividualBreakerReport.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  07/02/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Web.Model;
using openHistorian.XDALink;
using openXDA.Model;
using Root.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using ChartSeries = System.Windows.Forms.DataVisualization.Charting.Series;
using log4net;
using GSF;
using GSF.Data;

namespace openXDA.Reports
{
    public class IndividualBreakerReport : Root.Reports.Report
    {
        #region [ Members ]
        public class Point {
            public DateTime Time;
            public double Value;
        }
        // Constants
        private const double PageMarginMillimeters = 25.4D;             // 1-inch margin
        private const double PageWidthMillimeters = 8.5D * 25.4D;       // 8.5 inch width
        private const double PageHeightMillimeters = 11.0D * 25.4D;     // 11 inch height

        private const double FooterHeightMillimeters = (10.0D / 72.0D) * 25.4D;
        private const double SpacingMillimeters = 6.0D;

        private const string TitleText = "Individual Breaker Report";

        private const string timingQuery =
        @"
                    SELECT
	                    BreakerOperation.TripCoilEnergized as Time,
	                    BreakerOperation.BreakerNumber,
	                    MeterLine.LineName,
	                    Phase.Name as Phase,
	                    BreakerOperation.BreakerTiming as WaveformTiming,
	                    BreakerOperation.StatusTiming as StatusTiming,
	                    MaximoBreaker.BreakerSpeed,
	                    MaximoBreaker.BreakerSpeed * 1.12 as SpeedBandwidth,
	                    MaximoBreaker.BreakerSpeed * 0.12 as Bandwidth,
	                    BreakerOperationType.Name as OperationTiming,
	                    MaximoBreaker.Manufacturer,
	                    MaximoBreaker.SerialNum,
	                    MaximoBreaker.MfrYear,
	                    MaximoBreaker.ModelNum,
	                    MaximoBreaker.InterruptCurrentRating,
	                    MaximoBreaker.ContinuousAmpRating,
	                    MIN(ROUND(FaultSummary.PrefaultCurrent, 0)) as PrefaultCurrent,
	                    MAX(ROUND(FaultSummary.CurrentMagnitude, 0)) as MaxCurrent
                    FROM
	                    BreakerOperation JOIN
	                    BreakerOperationType ON BreakerOperation.BreakerOperationTypeID = BreakerOperationType.ID JOIN
	                    Phase ON Phase.ID = BreakerOperation.PhaseID JOIN
	                    Event ON BreakerOperation.EventID = Event.ID JOIN
	                    MeterLINE ON Event.MeterID = MeterLine.MeterID AND Event.LineID = MeterLine.LineID JOIN
	                    MaximoBreaker ON BreakerOperation.BreakerNumber = SUBSTRING(MaximoBreaker.BreakerNum, PATINDEX('%[^0]%', MaximoBreaker.BreakerNum + '.'), LEN(MaximoBreaker.BreakerNum)) LEFT JOIN
	                    FaultSummary ON Event.ID = FaultSummary.EventID AND FaultSummary.IsSelectedAlgorithm = 1
                    WHERE
	                    BreakerOperation.BreakerNumber = {0} AND
                        CAST(BreakerOperation.TripCoilEnergized as Date) BETWEEN {1} AND {2}
                    GROUP BY
	                    BreakerOperation.TripCoilEnergized,
	                    BreakerOperation.BreakerNumber,
	                    MeterLine.LineName,
	                    Phase.Name,
	                    BreakerOperation.BreakerTiming,
	                    BreakerOperation.StatusTiming,
	                    MaximoBreaker.BreakerSpeed,
	                    MaximoBreaker.BreakerSpeed * 1.12,
	                    MaximoBreaker.BreakerSpeed * 0.12,
	                    BreakerOperationType.Name,
	                    MaximoBreaker.Manufacturer,
	                    MaximoBreaker.SerialNum,
	                    MaximoBreaker.MfrYear,
	                    MaximoBreaker.ModelNum,
	                    MaximoBreaker.InterruptCurrentRating,
	                    MaximoBreaker.ContinuousAmpRating
                    ORDER BY
                        Time
                ";

        private const string testTimingQuery = @"
            SELECT 
                * 
            FROM 
                IndividualBreakerQuery  
            WHERE
	            BreakerNumber = {0} AND
                Cast(Time as Date) BETWEEN {1} AND {2} 
            ORDER BY Time
        ";

        private const string infoQuery =
        @"
            SELECT DISTINCT
	            MaximoBreaker.BreakerNum as [Breaker Number],
	            MeterLine.LineName as [Line Name],
	            MaximoBreaker.BreakerSpeed as [Breaker Mfr Speed],
	            MaximoBreaker.BreakerSpeed * 1.12 as [Speed Bandwidth],
	            MaximoBreaker.BreakerSpeed * 0.12 as [12% Speed Bandwidth],
	            MaximoBreaker.Manufacturer,
	            MaximoBreaker.SerialNum as [Serial Number],
	            MaximoBreaker.MfrYear as [Mfr Year],
	            MaximoBreaker.ModelNum as [Model Number],
	            MaximoBreaker.InterruptCurrentRating as [Interrupt Current Rating (A)],
	            MaximoBreaker.ContinuousAmpRating as [Continuous Amp Rating (A)]
            FROM
	            MaximoBreaker LEFT JOIN
	            BreakerChannel ON SUBSTRING(MaximoBreaker.BreakerNum, PATINDEX('%[^0]%', MaximoBreaker.BreakerNum + '.'), LEN(MaximoBreaker.BreakerNum)) = BreakerChannel.BreakerNumber LEFT JOIN
	            Channel ON BreakerChannel.ChannelID = Channel.ID LEFT JOIN
	            MeterLINE ON Channel.MeterID = MeterLine.MeterID AND Channel.LineID = MeterLine.LineID
                ";

        private const string testInfoQuery = @"
            SELECT 
                * 
            FROM 
                MaximoBreakerInfo 
            WHERE
	            [Breaker Number] = {0} 
        ";

        #endregion

        #region [ Properties ]

        public string BreakerID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public FontDef FontDefinition { get; set; }
        public DataTable TimingDataTable { get; set; }
        public DataTable InfoDataTable { get; set; }
        public List<Point> TimingPoints { get; set; }
        public double SpeedBandwidth { get; set; }
        public double Speed { get; set; }
        #endregion

        #region [ Constructors ]

        public IndividualBreakerReport( string breakerId, DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            BreakerID = breakerId;
            FontDefinition = new FontDef(this, "Helvetica");

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings")) {
#if DEBUG
                TimingDataTable = connection.RetrieveData(testTimingQuery, breakerId, startTime, endTime);
#else
                TimingDataTable = connection.RetrieveData(timingQuery, breakerId, startTime, endTime);
#endif

                TimingPoints = TimingDataTable.Select().Select(x => new Point() { Time = DateTime.Parse(x["Time"].ToString()), Value = double.Parse(x["WaveformTiming"].ToString()) }).OrderBy(x => x.Time).ToList();

#if DEBUG
                InfoDataTable = connection.RetrieveData(testInfoQuery, breakerId, startTime, endTime);
#else
                InfoDataTable = connection.RetrieveData(infoQuery, breakerId, startTime, endTime);
#endif
                Speed = double.Parse(InfoDataTable.Select().FirstOrDefault()?["Breaker Mfr Speed"].ToString() ?? "0");
                SpeedBandwidth = double.Parse(InfoDataTable.Select().FirstOrDefault()?["Speed Bandwidth"].ToString() ?? "0");

            }
        }

#endregion

#region [ Methods ]

        public byte[] createPDF()
        {
            try
            {
                GenerateReport();

                using (MemoryStream stream = new MemoryStream())
                {
                    this.formatter.Create(this, stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex) {
                Log.Error(ex.ToString(), ex);
                return null;
            }
        }

        private void GenerateReport()
        {
            CreatePage();
            double verticalMillimeters = InsertHeader();

            verticalMillimeters = CreateInfoTable(verticalMillimeters);
            verticalMillimeters = CreateTimingTable(verticalMillimeters);

            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                CreatePage();
                verticalMillimeters = InsertHeader();
            }

            Chart chart = GenerateLineChart();
            verticalMillimeters += 75;

            if(chart != null)
                page_Cur.AddMM(PageMarginMillimeters, verticalMillimeters, new RepImageMM(ChartToImage(chart), PageWidthMillimeters - 2 * PageMarginMillimeters, 75));

            foreach (Page page in enum_Page)
            {
                InsertFooter(page);
            }
        }

        private double CreateInfoTable(double verticalMillimeters)
        {

            if (InfoDataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText(verticalMillimeters, $"   No Breaker Information available.");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) =>
                {
                    verticalMillimeters += tlm.rCurY_MM;
                    verticalMillimeters = NextTablePage(verticalMillimeters, ea);
                };

                new TlmColumnMM(tlm, "Breaker Info", (PageWidthMillimeters - 2 * PageMarginMillimeters) * .5);
                new TlmColumnMM(tlm, "Value", (PageWidthMillimeters - 2 * PageMarginMillimeters) * .5);

                DataRow dataRow = InfoDataTable.Select().First();
                foreach (DataColumn column in InfoDataTable.Columns)
                {
                        tlm.NewRow();
                        tlm.Add(0, new RepString(textProp, column.ColumnName));
                        tlm.Add(1, new RepString(textProp, dataRow[column.ColumnName].ToString()));
                }

                tlm.Commit();

                verticalMillimeters += tlm.rCurY_MM + 10;
            }

            return verticalMillimeters;
        }

        private double CreateTimingTable(double verticalMillimeters)
        {

            string[] notInfo = { "Time", "Phase", "WaveformTiming", "StatusTiming", "OperationTiming" };

            if (TimingDataTable.Rows.Count == 0)
            {
                verticalMillimeters += InsertItalicText(verticalMillimeters, $"   No Breaker Operations during {StartTime.ToString("MM/dd/yyyy")} - {EndTime.ToString("MM/dd/yyyy")}");
                return verticalMillimeters;
            }

            FontProp headerProp = new FontProp(FontDefinition, 0);
            headerProp.rSizePoint = 10.0D;
            using (TableLayoutManager tlm = new TableLayoutManager(headerProp))
            {
                FontProp textProp = new FontProp(FontDefinition, 0);
                textProp.rSizePoint = 8.0D;
                tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
                tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
                tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
                tlm.eNewContainer += (oSender, ea) =>
                {
                    verticalMillimeters += tlm.rCurY_MM;
                    verticalMillimeters = NextTablePage(verticalMillimeters, ea);
                };


                // define columns
                foreach (string column in notInfo) {
                    new TlmColumnMM(tlm, column, (PageWidthMillimeters - 2 * PageMarginMillimeters)/ notInfo.Count());
                }


                foreach(DataRow row in TimingDataTable.Rows)
                {
                    tlm.NewRow();
                    int index = 0;
                    foreach (string column in notInfo) {
                        if(column == "Time")
                            tlm.Add(index++, new RepString(textProp, DateTime.Parse(row[column].ToString()).ToString("MM/dd/yyyy HH:mm:ss")));
                        else
                            tlm.Add(index++, new RepString(textProp, row[column].ToString()));
                    }
                }

                tlm.Commit();

                verticalMillimeters += tlm.rCurY_MM + 10;
            }

            return verticalMillimeters;
        }

        // Creates a page and sets width and height to standard 8.5x11 inches.
        private Page CreatePage()
        {
            Page page = new Page(this);
            page.rWidthMM = PageWidthMillimeters;
            page.rHeightMM = PageHeightMillimeters;
            return page;
        }

        // Inserts the page footer onto the given page, which includes the time of report generation as well as the page number.
        private double InsertFooter(Page page)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 12.0D;
            page.AddMM(PageWidthMillimeters - PageMarginMillimeters - font.rGetTextWidthMM(page.iPageNo.ToString()), PageHeightMillimeters - 15, new RepString(font, page.iPageNo.ToString()));
            return font.rSizeMM;
        }

        // Inserts the page footer onto the given page, which includes the time of report generation as well as the page number.
        private double InsertHeader()
        {
            string ReportIdentifier = "Individual Breaker Report - " + StartTime.ToString("MM/dd/yyyy") + " - " + EndTime.ToString("MM/dd/yyyy")  + "";

            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 12.0D;

            FontProp meterNameFont = new FontProp(FontDefinition, 0.0D);
            meterNameFont.rSizePoint = 12.0D;

            int height = 6;

            double reportIdentifierHorizontalPosition = PageWidthMillimeters / 2 - font.rGetTextWidthMM(ReportIdentifier) / 2;

            page_Cur.AddMM(reportIdentifierHorizontalPosition, height, new RepString(font, ReportIdentifier));
            page_Cur.AddMM(0, 10, new RepRectMM(new BrushProp(this, Color.Black), PageWidthMillimeters, 0.1D));

            return 20;
        }


        // Inserts the given text as a section header (16-pt, bold).
        private double InsertItalicText(double verticalMillimeters, string text)
        {
            FontProp font = new FontProp(FontDefinition, 0.0D);
            font.rSizePoint = 14.0D;
            font.bBold = false;
            font.bItalic = true;
            page_Cur.AddMM(PageMarginMillimeters, verticalMillimeters, new RepString(font, text));
            return font.rSizeMM + 5;
        }

        private double NextTablePage( double verticalMillimeters, TlmBase.NewContainerEventArgs ea)
        {
            if (verticalMillimeters > (PageHeightMillimeters * 0.75))
            {
                CreatePage();
                verticalMillimeters = InsertHeader();
            }

            ea.container.rHeightMM = PageHeightMillimeters - verticalMillimeters - PageMarginMillimeters;
            page_Cur.AddMM(PageMarginMillimeters, verticalMillimeters, ea.container);

            return verticalMillimeters;
        }

        private Chart GenerateLineChart()
        {
            Chart chart;

            if (TimingPoints.Count == 0)
            {
                return null;
            }

            ChartArea area;
            ChartSeries series;
            area = new ChartArea();

            // style x axis
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.IsLabelAutoFit = false;
            //area.AxisX.LabelAutoFitMinFontSize = 20;
            //area.AxisX.LabelStyle.Format = "MM/dd/yyyy";
            area.AxisX.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 20);
            area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            area.AxisX.Maximum = TimingPoints.Count - 1;
            area.AxisX.Minimum = 0;
            area.AxisX.Interval = 1;
            area.AxisX.MajorTickMark.Interval = 1;
            area.AxisX.TextOrientation = TextOrientation.Rotated90;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.LabelStyle.IsEndLabelVisible = true;

            // style y axis
            //area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.Maximum = (Math.Ceiling(SpeedBandwidth) > Math.Ceiling(TimingPoints.Select(x => x.Value).Max()) ? Math.Ceiling(SpeedBandwidth) : Math.Ceiling(TimingPoints.Select(x => x.Value).Max()));
            area.AxisY.Minimum = 0;
            area.AxisY.Interval = 1;
            area.AxisY.LabelAutoFitMinFontSize = 20;
            area.AxisY.LabelStyle.Format = "0.00";

            //create chart
            chart = new Chart();
            chart.Width = 1200;
            chart.Height = 500;
            chart.Name = "Waveform Timing";
            
            Legend legend = new Legend("Legend");
            legend.Font = new Font(FontFamily.GenericSansSerif, 15);
            chart.Legends.Add(legend);
            chart.ChartAreas.Add(area);


            series = new ChartSeries("Waveform Timing");
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;
            series.Color = Color.Black;
            series.IsValueShownAsLabel = true;
            
            int index = 0;
            foreach (var point in TimingPoints)
            {
                area.AxisX.CustomLabels.Add(index, index +.9, point.Time.ToString("MM/dd/yyyy"));
                DataPoint dataPoint = new DataPoint(index++, point.Value);
     
                series.Points.Add(dataPoint);
            }

            chart.Series.Add(series);

            chart.Series.Add(MakeLimitSeries(SpeedBandwidth, Color.Red, "12% Bandwidth", 0, TimingPoints.Count-1));
            chart.Series.Add(MakeLimitSeries(Speed, Color.Orange, "MFR Speed", 0, TimingPoints.Count-1));

            return chart;

        }

        private ChartSeries MakeLimitSeries(double value, Color color, string name, int start, int end)
        {
            ChartSeries cs = new ChartSeries(name);
            cs.ChartType = SeriesChartType.FastLine;
            cs.BorderWidth = 1;

            cs.Color = color;
            cs.Points.AddXY(start, value);
            cs.Points.AddXY(end, value);
            return cs;
        }

        private Stream ChartToImage(Chart chart)
        {
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, ChartImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }

#endregion

#region [ Static ]

        private static readonly ILog Log = LogManager.GetLogger(typeof(PQReport));

#endregion
    }
}
